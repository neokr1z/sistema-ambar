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
        var list=new List<Asignacion>();using var r=cmd.ExecuteReader();
        while(r.Read())list.Add(new(r.GetInt32(0),r.GetInt32(1),r.GetString(2),r.GetInt32(3),r.GetString(4),r.GetInt32(5),r.GetString(6)));
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
    public static List<Choice> Units(Sesion actor,int assignment,bool activeOnly=true)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT ue.ID_Unidad,CONCAT('Unidad ',ue.numero_unidad,' · ',ue.descripcion)
            FROM Unidad_Evaluacion ue JOIN Asignacion_Docente ad ON ad.ID_Materia=ue.ID_Materia
            WHERE ad.Id_asignacion=@id AND (@active=0 OR UPPER(ue.estatus)='ACTIVO') ORDER BY ue.numero_unidad
            """);Db.Param(cmd,"@id",assignment);Db.Param(cmd,"@active",activeOnly?1:0);
        var list=new List<Choice>();using var r=cmd.ExecuteReader();while(r.Read())list.Add(new(r.GetInt32(0),r.GetString(1)));return list;
    }
    public static List<Choice> Windows(Sesion actor,int assignment)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT pc.id_periodo_captura,pc.nombre_captura FROM Periodo_Captura pc
            JOIN Grupo g ON g.ID_Periodo=pc.id_periodo JOIN Asignacion_Docente ad ON ad.ID_grupo=g.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE ad.Id_asignacion=@id AND UPPER(pc.estatus) IN ('ACTIVO','ABIERTO')
              AND CURDATE() BETWEEN pc.fecha_inicio AND pc.fecha_limite
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO' ORDER BY pc.fecha_limite,pc.id_periodo_captura
            """);Db.Param(cmd,"@id",assignment);
        var list=new List<Choice>();using var r=cmd.ExecuteReader();while(r.Read())list.Add(new(r.GetInt32(0),r.GetString(1)));return list;
    }
    public static DataTable Roster(Sesion actor,int assignment,int unit=0)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT DISTINCT a.Numero_control AS Control,CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS Alumno,
                cal.calificacion AS Nota FROM Asignacion_Docente ad
            JOIN Inscripcion i ON i.ID_grupo=ad.ID_grupo JOIN Alumno a ON a.Numero_control=i.Numero_control
            LEFT JOIN Calificacion cal ON cal.Id_asignacion=ad.Id_asignacion AND cal.numero_control=a.Numero_control AND cal.ID_Unidad=@unit
            WHERE ad.Id_asignacion=@id AND UPPER(i.estatus)='INSCRITO' AND UPPER(a.estatus)='INSCRITO'
            ORDER BY Alumno,Control
            """);Db.Param(cmd,"@id",assignment);Db.Param(cmd,"@unit",unit);return Table(cmd);
    }
    public static DataTable Schedule(Sesion actor,int assignment)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT dia AS Día,hora_inicio AS Inicio,hora_fin AS Fin,aula AS Aula FROM Horario
            WHERE Id_asignacion=@id ORDER BY FIELD(dia,'Lunes','Martes','Miércoles','Miercoles','Jueves','Viernes','Sábado','Sabado'),hora_inicio
            """);Db.Param(cmd,"@id",assignment);return Table(cmd);
    }
    public static void Save(Sesion actor,int assignment,int unit,int window,IReadOnlyList<NotaCambio> changes)
    {
        if(changes.Count==0)throw new ArgumentException("No hay calificaciones nuevas o modificadas para guardar.");
        if(changes.Select(x=>x.Control).Distinct().Count()!=changes.Count)throw new ArgumentException("Hay alumnos repetidos en la captura.");
        foreach(var x in changes)if(x.Valor<0 || x.Valor>100 || decimal.Round(x.Valor,2)!=x.Valor)
            throw new ArgumentException("Usa calificaciones de 0 a 100, con hasta dos decimales.");
        using var c=Db.Connect();using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Own(c,tx,actor,assignment,true);
        using(var gate=Db.Sql(c,"""
            SELECT pc.id_periodo_captura FROM Periodo_Captura pc JOIN Grupo g ON g.ID_Periodo=pc.id_periodo
            JOIN Asignacion_Docente ad ON ad.ID_grupo=g.ID_grupo
            JOIN Unidad_Evaluacion ue ON ue.ID_Materia=ad.ID_Materia
            WHERE ad.Id_asignacion=@id AND ue.ID_Unidad=@unit AND UPPER(ue.estatus)='ACTIVO'
              AND pc.id_periodo_captura=@window AND UPPER(pc.estatus) IN ('ACTIVO','ABIERTO')
              AND CURDATE() BETWEEN pc.fecha_inicio AND pc.fecha_limite FOR UPDATE
            """,tx))
        {
            Db.Param(gate,"@id",assignment);Db.Param(gate,"@unit",unit);Db.Param(gate,"@window",window);
            if(gate.ExecuteScalar() is null)throw new ArgumentException("La unidad no corresponde a la materia o el periodo de captura está cerrado.");
        }
        foreach(var change in changes)
        {
            using(var roster=Db.Sql(c,"""
                SELECT a.Numero_control FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
                JOIN Asignacion_Docente ad ON ad.ID_grupo=i.ID_grupo
                WHERE ad.Id_asignacion=@id AND a.Numero_control=@control
                  AND UPPER(i.estatus)='INSCRITO' AND UPPER(a.estatus)='INSCRITO' FOR UPDATE
                """,tx))
            {
                Db.Param(roster,"@id",assignment);Db.Param(roster,"@control",change.Control);
                if(roster.ExecuteScalar() is null)throw new ArgumentException("Un alumno ya no está inscrito en este grupo. Recarga la lista.");
            }
            int? gradeId=null;decimal? current=null;
            using(var previous=Db.Sql(c,"SELECT Id_calificacion,calificacion FROM Calificacion WHERE Id_asignacion=@id AND ID_Unidad=@unit AND numero_control=@control FOR UPDATE",tx))
            {
                Db.Param(previous,"@id",assignment);Db.Param(previous,"@unit",unit);Db.Param(previous,"@control",change.Control);
                using var r=previous.ExecuteReader();if(r.Read()){gradeId=r.GetInt32(0);current=r.GetDecimal(1);}
            }
            if(current!=change.Original)throw new ArgumentException("Otra captura modificó una calificación. Recarga los datos antes de guardar.");
            using(var save=Db.Sql(c,gradeId.HasValue?
                "UPDATE Calificacion SET calificacion=@value,fecha_registro=CURRENT_TIMESTAMP WHERE Id_calificacion=@grade":
                "INSERT INTO Calificacion(numero_control,Id_asignacion,ID_Unidad,calificacion) VALUES(@control,@id,@unit,@value)",tx))
            {
                Db.Param(save,"@grade",gradeId);Db.Param(save,"@value",change.Valor);Db.Param(save,"@control",change.Control);
                Db.Param(save,"@id",assignment);Db.Param(save,"@unit",unit);save.ExecuteNonQuery();
                if(!gradeId.HasValue)gradeId=Convert.ToInt32(save.LastInsertedId);
            }
            using(var history=Db.Sql(c,"""
                INSERT INTO Registro_Captura(id_calificacion,id_periodo_captura,id_usuario,valor_reportado,observaciones)
                VALUES(@grade,@window,@actor,@value,@note)
                """,tx))
            {
                Db.Param(history,"@grade",gradeId);Db.Param(history,"@window",window);Db.Param(history,"@actor",actor.Id);
                Db.Param(history,"@value",change.Valor);Db.Param(history,"@note",current.HasValue?$"Cambio de {current:0.##} a {change.Valor:0.##}":"Primera captura");history.ExecuteNonQuery();
            }
        }
        Db.Audit(c,tx,actor.Id,"CAPTURA_CALIFICACIONES","CALIFICACIONES",$"Se guardaron {changes.Count} calificaciones. Unidad {unit}.",assignment.ToString());
        tx.Commit();
    }
    public static DataTable History(Sesion actor,int assignment,int unit=0)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT cal.numero_control AS Control,CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS Alumno,
                ue.numero_unidad AS Unidad,cal.calificacion AS Calificación,cal.fecha_registro AS Actualización
            FROM Calificacion cal JOIN Alumno a ON a.Numero_control=cal.numero_control
            JOIN Unidad_Evaluacion ue ON ue.ID_Unidad=cal.ID_Unidad
            WHERE cal.Id_asignacion=@id AND (@unit=0 OR cal.ID_Unidad=@unit) ORDER BY Alumno,Unidad
            """);Db.Param(cmd,"@id",assignment);Db.Param(cmd,"@unit",unit);return Table(cmd);
    }
    public static DataTable Changes(Sesion actor,int assignment,int unit=0)
    {
        using var c=Db.Connect();Own(c,null,actor,assignment);
        using var cmd=Db.Sql(c,"""
            SELECT rc.fecha_registro AS Fecha,cal.numero_control AS Control,
                CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS Alumno,ue.numero_unidad AS Unidad,
                rc.valor_reportado AS Calificación,pc.nombre_captura AS Captura,rc.observaciones AS Detalle
            FROM Registro_Captura rc JOIN Calificacion cal ON cal.Id_calificacion=rc.id_calificacion
            JOIN Alumno a ON a.Numero_control=cal.numero_control JOIN Unidad_Evaluacion ue ON ue.ID_Unidad=cal.ID_Unidad
            JOIN Periodo_Captura pc ON pc.id_periodo_captura=rc.id_periodo_captura
            WHERE cal.Id_asignacion=@id AND (@unit=0 OR cal.ID_Unidad=@unit)
            ORDER BY rc.fecha_registro DESC,rc.id_registro_captura DESC
            """);Db.Param(cmd,"@id",assignment);Db.Param(cmd,"@unit",unit);return Table(cmd);
    }
    private static DataTable Table(MySqlCommand cmd){using var r=cmd.ExecuteReader();var table=new DataTable();table.Load(r);return table;}
}
