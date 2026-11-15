namespace AMBAR.Semana4;

// Único lugar donde la aplicación decide cuál es el periodo escolar vigente.
// Se toma de Periodo_Escolar (estatus ACTIVO, el más reciente) y se guarda en memoria.
// Al cambiar de semestre basta activar el nuevo periodo en la BD; no hay que editar código.
internal static class PeriodoActual
{
    private static string? nombre;

    public static string Nombre => nombre ??= Consultar();

    public static void Reiniciar() => nombre = null;

    private static string Consultar()
    {
        using var c = Db.Connect();
        using var cmd = Db.Sql(c, """
            SELECT nombre_periodo FROM Periodo_Escolar
            WHERE estatus='ACTIVO' ORDER BY fecha_inicio DESC LIMIT 1
            """);
        return cmd.ExecuteScalar() as string
            ?? throw new InvalidOperationException("No hay un periodo escolar ACTIVO. Actívalo en la tabla Periodo_Escolar.");
    }
}
