namespace AMBAR.Semana4;

internal sealed record RegistroPostal(string Colonia, string Localidad, string Municipio, string Estado);

internal static class CatalogoPostal
{
    public static List<RegistroPostal> Buscar(string codigoPostal)
    {
        if (!Validation.Postal(codigoPostal))
            return new List<RegistroPostal>();
        var result = new List<RegistroPostal>();
        using var c = Db.Connect();
        using var cmd = Db.Sql(c, """
            SELECT colonia, localidad, municipio, estado
            FROM Catalogo_Postal WHERE codigo_postal=@cp ORDER BY colonia
            """);
        Db.Param(cmd, "@cp", codigoPostal);
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
            result.Add(new RegistroPostal(rd.GetString(0),rd.GetString(1),rd.GetString(2),rd.GetString(3)));
        return result;
    }

    public static void Validar(string cp, string colonia, string localidad, string municipio, string estado)
    {
        var matches = Buscar(cp);
        if (!matches.Any(x =>
            string.Equals(x.Colonia,colonia,StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Localidad,localidad,StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Municipio,municipio,StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Estado,estado,StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("La colonia, localidad, municipio y estado no corresponden al código postal registrado en el catálogo SEPOMEX.");
    }
}
