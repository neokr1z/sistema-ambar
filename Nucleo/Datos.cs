using System.Text.Json;
using MySqlConnector;
using System.Security.Cryptography;

namespace AMBAR.Semana4;

internal static class Db
{
    private static string? connectionString;
    public static MySqlConnection Connect()
    {
        if (connectionString is null)
        {
            var file = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            using var json = JsonDocument.Parse(File.ReadAllText(file));
            connectionString = json.RootElement.GetProperty("ConnectionString").GetString()
                ?? throw new InvalidOperationException("La cadena de conexión está vacía.");
        }
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            // La base de AMBAR usa utf8mb4_unicode_ci. La collation predeterminada
            // del servidor MySQL 8 puede ser distinta y romper comparaciones SQL.
            using var cmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci", connection);
            cmd.ExecuteNonQuery();
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }

    public static MySqlCommand Sql(MySqlConnection c, string sql, MySqlTransaction? trx = null)
        => new(sql, c, trx);

    public static void Param(MySqlCommand c, string name, object? value)
        => c.Parameters.AddWithValue(name, value ?? DBNull.Value);

    public static void Audit(MySqlConnection conn, MySqlTransaction? tx, int userId,
        string type, string module, string description, string? record = null)
    {
        using var c = Sql(conn, """
            INSERT INTO Bitacora(Id_usuario,tipo_evento,modulo_afectado,descripcion,id_registro_afectado)
            VALUES (@id,@tipo,@modulo,@detalle,@registro)
            """, tx);
        Param(c, "@id", userId); Param(c, "@tipo", type);
        Param(c, "@modulo", module); Param(c, "@detalle", description);
        Param(c, "@registro", record);
        c.ExecuteNonQuery();
    }
}

internal sealed record Sesion(int Id, string Username, string Role, bool RequiresPasswordChange)
{
    public bool IsAdmin => Role.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
    public bool IsSecretary => Role.Equals("Secretaria", StringComparison.OrdinalIgnoreCase);
    public bool IsTeacher => Role.Equals("Docente", StringComparison.OrdinalIgnoreCase);
    // Administrador y Secretaría inscriben, reinscriben, consultan y modifican alumnos.
    public bool PuedeGestionarAlumnos => IsAdmin || IsSecretary;
}

internal static class Auth
{
    private const int MaxIntentos = 5;
    private const int MinutosBloqueo = 15;

    public static Sesion? Login(string username, string password)
    {
        using var c = Db.Connect();
        using var cmd = Db.Sql(c, """
            SELECT u.Id_usuario,u.nombre_usuario,u.password_hash,r.nombre,u.debe_cambiar_password,
                   COALESCE(CEIL(TIMESTAMPDIFF(SECOND,NOW(),u.bloqueado_hasta)/60),0)
            FROM Usuario u INNER JOIN Rol r ON r.id_rol=u.Id_rol
            WHERE u.nombre_usuario=@nombre AND u.estatus='ACTIVO'
            """);
        Db.Param(cmd,"@nombre", username);
        int id, minutosBloqueo;
        string stored, role, user;
        bool change;
        using (var reader = cmd.ExecuteReader())
        {
            if (!reader.Read()) return null;
            id = reader.GetInt32(0);
            user = reader.GetString(1);
            stored = reader.GetString(2);
            role = reader.GetString(3);
            change = reader.GetBoolean(4);
            minutosBloqueo = Convert.ToInt32(reader.GetValue(5));
        }
        // Con la cuenta bloqueada ni siquiera se revisa la contraseña.
        if (minutosBloqueo > 0)
            throw new UnauthorizedAccessException(
                $"Cuenta bloqueada por demasiados intentos fallidos. Intenta de nuevo en {minutosBloqueo} minuto(s).");

        if (!PasswordHasher.Verify(password, stored))
        {
            RegistrarFallo(c, id);
            return null;
        }
        var session = new Sesion(id, user, role, change);
        try { Identidad.RequireActive(c, null, session); } catch (UnauthorizedAccessException) { return null; }
        using (var ok = Db.Sql(c, "UPDATE Usuario SET intentos_fallidos=0,bloqueado_hasta=NULL WHERE Id_usuario=@id"))
        {
            Db.Param(ok, "@id", id);
            ok.ExecuteNonQuery();
        }
        Db.Audit(c, null, id, "ACCESO", "AUTENTICACION", "Inicio de sesión correcto");
        return session;
    }

    private static void RegistrarFallo(MySqlConnection c, int id)
    {
        // MySQL evalúa las asignaciones de izquierda a derecha: la segunda ya ve el contador actualizado.
        // Si un bloqueo anterior ya venció, el conteo vuelve a empezar en 1.
        using (var upd = Db.Sql(c, """
            UPDATE Usuario
            SET intentos_fallidos = IF(bloqueado_hasta IS NOT NULL AND bloqueado_hasta<=NOW(),1,intentos_fallidos+1),
                bloqueado_hasta   = IF(intentos_fallidos>=@max,DATE_ADD(NOW(),INTERVAL @minutos MINUTE),NULL)
            WHERE Id_usuario=@id
            """))
        {
            Db.Param(upd, "@id", id);
            Db.Param(upd, "@max", MaxIntentos);
            Db.Param(upd, "@minutos", MinutosBloqueo);
            upd.ExecuteNonQuery();
        }
        Db.Audit(c, null, id, "ACCESO_FALLIDO", "AUTENTICACION", "Contraseña incorrecta");
    }
}

internal static class PasswordHasher
{
    private const int Iterations = 250000;
    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] bytes = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations,HashAlgorithmName.SHA256,32);
        return $"AMBAR:{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(bytes)}";
    }

    public static bool Verify(string password, string hash)
    {
        try
        {
            var p = hash.Split(':');
            if (p.Length != 4 || p[0] != "AMBAR") return false;
            int iterations = int.Parse(p[1]);
            if (iterations < 100000 || iterations > 1000000) return false;
            byte[] salt = Convert.FromBase64String(p[2]);
            byte[] expected = Convert.FromBase64String(p[3]);
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password,salt,iterations,HashAlgorithmName.SHA256,expected.Length);
            return CryptographicOperations.FixedTimeEquals(actual,expected);
        }
        catch (Exception) { return false; }
    }
}

