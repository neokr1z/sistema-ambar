using System.Data;
using MySqlConnector;

namespace AMBAR.Semana4;

internal sealed record ResumenReinscripcion(string Control, string Alumno, string Carrera,
    int SemestreAnterior, int SemestreDestino, string PeriodoAnterior, string Situacion,
    int TotalMaterias, int Aprobadas, int Calificadas, bool PuedeReinscribir, DataTable Calificaciones);

internal static class RepositorioReinscripcion
{
    internal const string PeriodoDestino = "Agosto-Diciembre 2026";

    private static void Permiso(MySqlConnection c, MySqlTransaction? tx, Sesion actor)
    {
        if (!actor.CanSeeStudents) throw new UnauthorizedAccessException("No tienes permiso para reinscribir.");
        Identidad.RequireActive(c,tx,actor);
    }

    // La información del semestre cursado se obtiene de un historial explícito:
    // jamás se infiere que quien esté en 1.º haya cursado 2.º.
    public static ResumenReinscripcion Consultar(Sesion actor,string numero)
    {
        numero=Validation.Mandatory(numero,"número de control");
        using var c=Db.Connect();
        Permiso(c,null,actor);
        string nombre,carrera,estado;int origen;string periodo;
        using (var cmd=Db.Sql(c,"""
            SELECT CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno),
              ca.nombre_carrera,a.estatus,h.semestre_cursado,h.periodo_origen
            FROM Alumno a
            JOIN Plan_Estudio pl ON pl.id_plan=a.ID_Plan
            JOIN Carrera ca ON ca.clave_carrera=pl.clave_carrera
            LEFT JOIN Historial_Semestre_Reinscripcion h ON h.numero_control=a.Numero_control
            WHERE a.Numero_control=@control
            """))
        {
            Db.Param(cmd,"@control",numero);
            using var r=cmd.ExecuteReader();
            if(!r.Read())throw new ArgumentException("No existe un alumno con ese número de control.");
            nombre=r.GetString(0);carrera=r.GetString(1);estado=r.GetString(2);
            if(r.IsDBNull(3))
                throw new ArgumentException("El alumno no tiene un historial de 2.º o 4.º semestre del periodo anterior. No se puede asumir que aprobó materias; registra primero sus antecedentes académicos.");
            origen=r.GetInt32(3);periodo=r.GetString(4);
        }
        var notas=new DataTable();
        using(var cmd=Db.Sql(c,"""
            SELECT m.nombre AS Materia, f.calificacion AS Calificación,
              CASE WHEN f.calificacion IS NULL THEN 'PENDIENTE'
                   WHEN f.calificacion<70 THEN 'REPROBADA' ELSE 'APROBADA' END AS Resultado
            FROM Materia_Reinscripcion m
            LEFT JOIN Calificacion_Final_Reinscripcion f
              ON f.id_materia_reinscripcion=m.id_materia_reinscripcion
              AND f.numero_control=@control AND f.periodo_origen=@periodo
            WHERE m.semestre_origen=@origen ORDER BY m.nombre
            """))
        {
            Db.Param(cmd,"@control",numero);Db.Param(cmd,"@periodo",periodo);Db.Param(cmd,"@origen",origen);
            using var r=cmd.ExecuteReader();notas.Load(r);
        }
        int total=notas.Rows.Count,calificadas=0,aprobadas=0;
        foreach(DataRow row in notas.Rows)
        {
            if(row["Calificación"]==DBNull.Value)continue;
            calificadas++;
            if(Convert.ToDecimal(row["Calificación"])>=70)aprobadas++;
        }
        bool already=YaInscrito(c,null,numero);
        bool elegible=origen is 2 or 4 && total>0 && total==calificadas && calificadas==aprobadas
            && estado.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase) && !already;
        string situacion=already?"Ya existe inscripción activa en agosto–diciembre 2026":
            !estado.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase)?"Alumno sin estatus INSCRITO":
            total==0?"Sin catálogo de materias":
            total!=calificadas?"Faltan calificaciones finales":
            calificadas!=aprobadas?"Tiene materias con calificación inferior a 70":"Apto para reinscripción";
        return new(numero,nombre,carrera,origen,origen+1,periodo,situacion,
            total,aprobadas,calificadas,elegible,notas);
    }

    private static bool YaInscrito(MySqlConnection c,MySqlTransaction? tx,string control)
    {
        using var cmd=Db.Sql(c,"""
            SELECT COUNT(*) FROM Inscripcion i
            JOIN Grupo g ON g.ID_grupo=i.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE i.Numero_control=@control AND UPPER(i.estatus)='INSCRITO'
              AND p.nombre_periodo=@periodo
            """,tx);
        Db.Param(cmd,"@control",control);Db.Param(cmd,"@periodo",PeriodoDestino);
        return Convert.ToInt32(cmd.ExecuteScalar())>0;
    }

    public static List<Choice> Grupos(Sesion actor, int destino)
    {
        if(destino is not (3 or 5))return new();
        using var c=Db.Connect();Permiso(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT g.ID_grupo,CONCAT(g.nombre_grupo,' · ',COALESCE(g.turno,'SIN TURNO'),
                ' · ',GREATEST(0,g.Capacidad-COUNT(i.Id_inscripcion)),' lugares') AS etiqueta
            FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            LEFT JOIN Inscripcion i ON i.ID_grupo=g.ID_grupo AND UPPER(i.estatus)='INSCRITO'
            WHERE s.numero_semestre=@destino AND p.nombre_periodo=@periodo
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO'
            GROUP BY g.ID_grupo,g.nombre_grupo,g.turno,g.Capacidad
            HAVING COUNT(i.Id_inscripcion)<g.Capacidad
            ORDER BY g.turno,g.nombre_grupo
            """);
        Db.Param(cmd,"@destino",destino);Db.Param(cmd,"@periodo",PeriodoDestino);
        var grupos=new List<Choice>();using var r=cmd.ExecuteReader();
        while(r.Read())grupos.Add(new Choice(r.GetInt32(0),r.GetString(1)));
        return grupos;
    }

    public static void Guardar(Sesion actor,string numero,int idGrupo)
    {
        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Permiso(c,tx,actor);
        // Bloqueo compartido de identidad para serializar inscripciones de la aplicación.
        using(var lockCmd=Db.Sql(c,"SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE",tx))
            if(lockCmd.ExecuteScalar() is null)throw new InvalidOperationException("Falta Control_Identidad. Ejecuta la migración 06.");
        // Bloquear alumno y revalidar todos los requisitos DENTRO de la transacción.
        string estatus,periodoOrigen;int origen;
        using(var cmd=Db.Sql(c,"""
            SELECT a.estatus,h.semestre_cursado,h.periodo_origen
            FROM Alumno a JOIN Historial_Semestre_Reinscripcion h
              ON h.numero_control=a.Numero_control
            WHERE a.Numero_control=@control FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@control",numero);using var r=cmd.ExecuteReader();
            if(!r.Read())throw new ArgumentException("Alumno sin historial académico para reinscripción.");
            estatus=r.GetString(0);origen=r.GetInt32(1);periodoOrigen=r.GetString(2);
        }
        if(estatus.ToUpperInvariant()!="INSCRITO" || origen is not (2 or 4))
            throw new ArgumentException("El alumno no reúne los requisitos de reinscripción.");
        if(YaInscrito(c,tx,numero))throw new ArgumentException("El alumno ya tiene una inscripción activa en este periodo.");
        int total,aprobadas;
        using(var cmd=Db.Sql(c,"""
            SELECT COUNT(*) AS total,
              COUNT(CASE WHEN f.calificacion>=70 THEN 1 END) AS aprobadas
            FROM Materia_Reinscripcion m
            LEFT JOIN Calificacion_Final_Reinscripcion f
              ON f.id_materia_reinscripcion=m.id_materia_reinscripcion
              AND f.numero_control=@control AND f.periodo_origen=@origenPeriodo
            WHERE m.semestre_origen=@origen
            """,tx))
        {
            Db.Param(cmd,"@control",numero);Db.Param(cmd,"@origenPeriodo",periodoOrigen);Db.Param(cmd,"@origen",origen);
            using var r=cmd.ExecuteReader();r.Read();
            total=r.GetInt32(0);aprobadas=r.GetInt32(1);
        }
        if(total==0 || total!=aprobadas)
            throw new ArgumentException("Debe aprobar todas las materias del semestre anterior con 70 o más. Revisa las calificaciones faltantes y reprobadas.");
        int capacity;
        using(var cmd=Db.Sql(c,"""
            SELECT g.Capacidad FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            JOIN Carrera ca ON ca.ID_Carrera=g.ID_Carrera
            JOIN Plan_Estudio pl ON pl.clave_carrera=ca.clave_carrera
            JOIN Alumno a ON a.ID_Plan=pl.id_plan
            WHERE g.ID_grupo=@grupo AND a.Numero_control=@control
              AND s.numero_semestre=@destino AND p.nombre_periodo=@periodo
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO'
            FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@grupo",idGrupo);Db.Param(cmd,"@control",numero);
            Db.Param(cmd,"@destino",origen+1);Db.Param(cmd,"@periodo",PeriodoDestino);
            object? value=cmd.ExecuteScalar();
            if(value is null)throw new ArgumentException("Grupo inválido para el semestre siguiente, carrera o periodo.");
            capacity=Convert.ToInt32(value);
        }
        using(var cmd=Db.Sql(c,"SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO'",tx))
        {
            Db.Param(cmd,"@grupo",idGrupo);
            if(Convert.ToInt32(cmd.ExecuteScalar())>=capacity)
                throw new ArgumentException("El grupo ya no tiene lugares disponibles.");
        }
        using(var cmd=Db.Sql(c,"""
            INSERT INTO Inscripcion(Numero_control,ID_grupo,tipo_inscripcion,estatus)
            VALUES(@control,@grupo,'REINSCRIPCION','INSCRITO')
            """,tx))
        {
            Db.Param(cmd,"@control",numero);Db.Param(cmd,"@grupo",idGrupo);cmd.ExecuteNonQuery();
        }
        Db.Audit(c,tx,actor.Id,"REINSCRIPCION","ALUMNOS",$"Reinscripción de semestre {origen} a {origen+1} en {PeriodoDestino}",numero);
        tx.Commit();
    }
}
