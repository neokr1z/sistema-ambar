using System.Data;
namespace AMBAR.Semana4;

internal sealed record ListaGrupo(string Grupo, string Periodo, DataTable Alumnos);
internal static class RepositorioListas
{
    public static ListaGrupo Obtener(Sesion actor, int grupo, int? asignacion = null)
    {
        if(!actor.CanSeeStudents && !actor.IsTeacher)
            throw new UnauthorizedAccessException("No tienes permiso para consultar listas de grupo.");
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,actor);
        if(actor.IsTeacher)
        {
            using var permiso=Db.Sql(c,"""
                SELECT COUNT(*) FROM Asignacion_Docente ad
                JOIN Usuario u ON u.ID_docente=ad.ID_docente
                WHERE u.Id_usuario=@actor AND ad.ID_grupo=@grupo AND ad.Id_asignacion=@asignacion
                """);
            Db.Param(permiso,"@actor",actor.Id);Db.Param(permiso,"@grupo",grupo);Db.Param(permiso,"@asignacion",asignacion);
            if(Convert.ToInt32(permiso.ExecuteScalar())!=1)
                throw new UnauthorizedAccessException("Sólo puedes generar listas de tus materias y grupos asignados.");
        }
        string nombre,periodo;
        using(var info=Db.Sql(c,"SELECT g.nombre_grupo,p.nombre_periodo FROM Grupo g JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo WHERE g.ID_grupo=@grupo"))
        {
            Db.Param(info,"@grupo",grupo);using var r=info.ExecuteReader();
            if(!r.Read())throw new ArgumentException("El grupo ya no existe.");
            nombre=r.GetString(0);periodo=r.GetString(1);
        }
        using var cmd=Db.Sql(c,"""
            SELECT DISTINCT a.Numero_control AS Control,
                CONCAT_WS(' ',a.apellido_paterno,a.apellido_materno,a.nombre) AS Alumno,
                a.estatus AS Estatus
            FROM Inscripcion i JOIN Alumno a ON a.Numero_control=i.Numero_control
            WHERE i.ID_grupo=@grupo AND UPPER(i.estatus)='INSCRITO' AND UPPER(a.estatus)='INSCRITO'
            ORDER BY Alumno,Control
            """);
        Db.Param(cmd,"@grupo",grupo);using var rd=cmd.ExecuteReader();var data=new DataTable();data.Load(rd);
        return new(nombre,periodo,data);
    }
}
