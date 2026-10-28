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
        return connection;
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
    public bool CanSeeStudents => IsAdmin || IsSecretary;
}

internal static class Auth
{
    public static Sesion? Login(string username, string password)
    {
        using var c = Db.Connect();
        using var cmd = Db.Sql(c, """
            SELECT u.Id_usuario,u.nombre_usuario,u.password_hash,r.nombre,u.debe_cambiar_password
            FROM Usuario u INNER JOIN Rol r ON r.id_rol=u.Id_rol
            WHERE u.nombre_usuario=@nombre AND UPPER(u.estatus)='ACTIVO'
            """);
        Db.Param(cmd,"@nombre", username);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        int id = reader.GetInt32(0);
        string stored = reader.GetString(2);
        string role = reader.GetString(3);
        string user = reader.GetString(1);
        bool change = reader.GetBoolean(4);
        reader.Close();
        if (!PasswordHasher.Verify(password, stored)) return null;
        var session = new Sesion(id, user, role, change);
        try { Identidad.RequireActive(c, null, session); } catch (UnauthorizedAccessException) { return null; }
        Db.Audit(c, null, id, "ACCESO", "AUTENTICACION", "Inicio de sesión correcto");
        return session;
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

