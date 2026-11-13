using MySqlConnector;

namespace AMBAR.Semana4;

internal static class Identidad
{
    public static void RequireActive(MySqlConnection c, MySqlTransaction? tx, Sesion actor, bool adminOnly = false)
    {
        using var check = Db.Sql(c, """
            SELECT COUNT(*) FROM Usuario u JOIN Rol r ON r.id_rol=u.Id_rol
            LEFT JOIN Docente d ON d.ID_docente=u.ID_docente
            LEFT JOIN Personal_Administrativo p ON p.num_administrativo=u.num_administrativo
            WHERE u.Id_usuario=@id AND UPPER(u.estatus)='ACTIVO' AND r.nombre=@role
              AND (u.ID_docente IS NULL OR UPPER(d.estatus)='ACTIVO')
              AND (u.num_administrativo IS NULL OR UPPER(p.estatus)='ACTIVO')
            """, tx);
        Db.Param(check, "@id", actor.Id); Db.Param(check, "@role", actor.Role);
        if ((adminOnly && !actor.IsAdmin) || Convert.ToInt32(check.ExecuteScalar()) != 1)
            throw new UnauthorizedAccessException("La sesión ya no está autorizada. Cierra sesión e ingresa nuevamente.");
    }

    // Una fila compartida serializa altas/ediciones entre tablas, no sólo dentro de cada módulo.
    // Se adquiere antes de bloquear expedientes o grupos. La transacción libera el bloqueo.
    public static void Check(MySqlConnection c, MySqlTransaction tx, Sesion actor,
        string first, string paternal, string maternal, string curp,
        string control = "", int aspirantId = 0, string rfc = "")
    {
        using (var gate = Db.Sql(c, "SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE", tx))
            if (gate.ExecuteScalar() is null) throw new InvalidOperationException("Ejecuta la migración 06 antes de capturar datos.");
        RequireActive(c, tx, actor);
        using var cmd = Db.Sql(c, """
            SELECT COUNT(*) FROM (
                SELECT nombre,apellido_paterno,apellido_materno,curp,NULL AS RFC
                FROM Alumno WHERE Numero_control<>@control
                UNION ALL SELECT nombre,apellido_paterno,apellido_materno,curp,RFC FROM Docente
                UNION ALL SELECT nombre,apellido_paterno,apellido_materno,curp,RFC FROM Personal_Administrativo
            ) personas
            WHERE UPPER(TRIM(curp))=@curp
               OR REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)), '[[:space:]]+', ' ')
                   COLLATE utf8mb4_unicode_ci = @nombre COLLATE utf8mb4_unicode_ci
               OR (@rfc<>'' AND UPPER(TRIM(RFC))=@rfc)
            """, tx);
        Db.Param(cmd, "@control", control); Db.Param(cmd, "@curp", curp);
        Db.Param(cmd, "@nombre", $"{first} {paternal} {maternal}"); Db.Param(cmd, "@rfc", rfc);
        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            throw new ArgumentException("Ya existe una persona con ese nombre completo, CURP o RFC, incluso si está dada de baja.");
        using var asp = Db.Sql(c, "SELECT COUNT(*) FROM Aspirante WHERE UPPER(TRIM(curp))=@curp AND ID_Aspirante<>@id", tx);
        Db.Param(asp, "@curp", curp); Db.Param(asp, "@id", aspirantId);
        if (Convert.ToInt32(asp.ExecuteScalar()) > 0)
            throw new ArgumentException("La CURP ya pertenece a otra ficha de aspirante.");
    }
}
