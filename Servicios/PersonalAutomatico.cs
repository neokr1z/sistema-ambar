using System.Globalization;
using System.Text;
using MySqlConnector;

namespace AMBAR.Semana4;

internal static class PersonalAutomatico
{
    public static string EmailBase(string first, string paternal, string maternal)
    {
        string Letters(string text) => new string(text.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetter(c)).ToArray());
        string name = Letters(Validation.Name(first, "Nombre", 60));
        string ap = Letters(Validation.Name(paternal, "Apellido paterno"));
        string am = Letters(Validation.Name(maternal, "Apellido materno"));
        return $"{name}{char.ToUpperInvariant(ap[0])}{char.ToUpperInvariant(am[0])}@acapulco.tecnm.mx";
    }
    // Invocar después de Identidad.Check, con el bloqueo global de altas adquirido.
    public static string AvailableEmail(MySqlConnection c, MySqlTransaction tx, string proposed)
    {
        var parts=proposed.Split('@');
        for(int n=1;n<100000;n++)
        {
            string suffix=n==1?"":n.ToString(CultureInfo.InvariantCulture);
            string local=parts[0][..Math.Min(parts[0].Length,64-suffix.Length)];
            string value=local+suffix+"@"+parts[1];
            using var cmd=Db.Sql(c,"""
                SELECT (SELECT COUNT(*) FROM Docente WHERE correo_institucional=@email)
                     + (SELECT COUNT(*) FROM Personal_Administrativo WHERE correo_institucional=@email)
                     + (SELECT COUNT(*) FROM Alumno WHERE correo_institucional=@email)
                """,tx);
            Db.Param(cmd,"@email",value);
            if(Convert.ToInt32(cmd.ExecuteScalar())==0)return value;
        }
        throw new InvalidOperationException("No fue posible generar un correo disponible.");
    }
    public static string NextNumber(MySqlConnection c, MySqlTransaction tx)
    {
        using var cmd=Db.Sql(c,"""
            SELECT COALESCE(MAX(CAST(SUBSTRING(num_administrativo,4) AS UNSIGNED)),0)+1
            FROM Personal_Administrativo WHERE num_administrativo REGEXP '^ADM[0-9]+$'
            """,tx);
        long n=Convert.ToInt64(cmd.ExecuteScalar());
        if(n>999999999)throw new InvalidOperationException("Se agotó la numeración administrativa.");
        return $"ADM{n:D6}";
    }
}
