using System.Data;
using MySqlConnector;

namespace AMBAR.Semana4;

internal sealed record Asignacion(int Id,int MateriaId,string Materia,int GrupoId,string Grupo,int PeriodoId,string Periodo)
{
    public override string ToString()=>$"{Materia} · {Grupo} · {Periodo}";
}
internal sealed record NotaCambio(string Control,decimal Valor,decimal? Original);

internal static class RepositorioDocente
{
    private static void Teacher(MySqlConnection c,MySqlTransaction? tx,Sesion actor)
    {
        if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
        Identidad.RequireActive(c,tx,actor);
    }
    public static List<Asignacion> Assignments(Sesion actor)
    {
        using var c=Db.Connect();Teacher(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT ad.Id_asignacion,m.ID_Materia,m.nombre_materia,g.ID_grupo,g.nombre_grupo,p.ID_Periodo,p.nombre_periodo
            FROM Asignacion_Docente ad JOIN Usuario u ON u.ID_docente=ad.ID_docente
            JOIN Materia m ON m.ID_Materia=ad.ID_Materia JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo WHERE u.Id_usuario=@actor
            ORDER BY p.fecha_inicio DESC,m.nombre_materia,g.nombre_grupo
            """);Db.Param(cmd,"@actor",actor.Id);
        var list=new List<Asignacion>();
        using(var r=cmd.ExecuteReader())
            while(r.Read())
            {
                list.Add(new(r.GetInt32(0),r.GetInt32(1),r.GetString(2),
                    r.GetInt32(3),r.GetString(4),r.GetInt32(5),r.GetString(6)));
            }
        return list;
    }
    private static void Own(MySqlConnection c,MySqlTransaction? tx,Sesion actor,int assignment,bool write=false)
    {
        Teacher(c,tx,actor);
        using var cmd=Db.Sql(c,"""
            SELECT ad.Id_asignacion FROM Asignacion_Docente ad JOIN Usuario u ON u.ID_docente=ad.ID_docente
            JOIN Grupo g ON g.ID_grupo=ad.ID_grupo JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            JOIN Materia m ON m.ID_Materia=ad.ID_Materia
            WHERE u.Id_usuario=@actor AND ad.Id_asignacion=@id
              AND (@write=0 OR (UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO' AND UPPER(m.estatus)='ACTIVO'))
            """+(write?" FOR UPDATE":""),tx);
        Db.Param(cmd,"@actor",actor.Id);Db.Param(cmd,"@id",assignment);Db.Param(cmd,"@write",write?1:0);
        if(cmd.ExecuteScalar() is null)throw new UnauthorizedAccessException("La asignación no pertenece a tu cuenta o ya no está habilitada.");
    }
    public static DataTable Schedule(Sesion actor,int assignment)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT dia AS Día,hora_inicio AS Inicio,hora_fin AS Fin,aula AS Aula FROM Horario
            WHERE Id_asignacion=@id ORDER BY FIELD(dia,'Lunes','Martes','Miércoles','Miercoles','Jueves','Viernes','Sábado','Sabado'),hora_inicio
            """);Db.Param(cmd,"@id",assignment);return Table(cmd);
    }
    private static DataTable Table(MySqlCommand cmd){using var r=cmd.ExecuteReader();var table=new DataTable();table.Load(r);return table;}
}
