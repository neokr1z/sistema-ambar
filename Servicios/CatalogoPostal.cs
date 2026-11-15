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
            result.Add(new RegistroPostal(rd.GetString(0), rd.GetString(1), rd.GetString(2), rd.GetString(3)));
        return result;
    }

    // Devuelve la clave del asentamiento específico, no sólo el código postal:
    // un código postal puede contener varias colonias.
    public static long ObtenerId(string cp, string colonia, string localidad, string municipio, string estado)
    {
        if (!Validation.Postal(cp))
            throw new ArgumentException("Código postal inválido.");
        using var c = Db.Connect();
        using var cmd = Db.Sql(c, """
            SELECT id FROM Catalogo_Postal
            WHERE codigo_postal=@cp AND colonia=@col AND localidad=@loc
              AND municipio=@mun AND estado=@estado
            LIMIT 1
            """);
        Db.Param(cmd, "@cp", cp);
        Db.Param(cmd, "@col", colonia);
        Db.Param(cmd, "@loc", localidad);
        Db.Param(cmd, "@mun", municipio);
        Db.Param(cmd, "@estado", estado);
        object? id = cmd.ExecuteScalar();
        if (id is null || id == DBNull.Value)
            throw new ArgumentException("El domicilio no corresponde a un registro del catálogo postal SEPOMEX.");
        return Convert.ToInt64(id);
    }

    public static void Validar(string cp, string colonia, string localidad, string municipio, string estado)
        => _ = ObtenerId(cp, colonia, localidad, municipio, estado);
}
