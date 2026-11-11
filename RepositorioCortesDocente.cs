using System.Data;
using MySqlConnector;

namespace AMBAR.Semana4;

// Cortes independientes de Calificacion (unidades históricas): admiten tanto
// alumnos con expediente como reinscritos mínimos, sin duplicar personas.
internal static class RepositorioCortesDocente
{
    public static readonly int[] Semanas={5,9,13,17};

    private static void Autorizar(MySqlConnection c,MySqlTransaction? tx,Sesion actor,
        int asignacion,bool bloquear=false)
    {
        if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
        Identidad.RequireActive(c,tx,actor);
        using var cmd=Db.Sql(c,"""
            SELECT ad.Id_asignacion
            FROM Asignacion_Docente ad
            JOIN Usuario u ON u.ID_docente=ad.ID_docente
            JOIN Docente d ON d.ID_docente=ad.ID_docente
            JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            JOIN Materia m ON m.ID_Materia=ad.ID_Materia
            WHERE ad.Id_asignacion=@asignacion AND u.Id_usuario=@actor
              AND UPPER(d.estatus)='ACTIVO'
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(m.estatus)='ACTIVO'
            """+(bloquear?" FOR UPDATE":""),tx);
        Db.Param(cmd,"@asignacion",asignacion);
        Db.Param(cmd,"@actor",actor.Id);
        if(cmd.ExecuteScalar() is null)
            throw new UnauthorizedAccessException("No tienes acceso a esta materia o grupo.");
    }

    private static (DateTime Inicio,DateTime Fin) Periodo(MySqlConnection c,
        MySqlTransaction? tx,int asignacion)
    {
        using var cmd=Db.Sql(c,"""
            SELECT p.fecha_inicio,p.fecha_fin FROM Asignacion_Docente a
            JOIN Grupo g ON g.ID_grupo=a.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE a.Id_asignacion=@asignacion
            """,tx);
        Db.Param(cmd,"@asignacion",asignacion);
        using var r=cmd.ExecuteReader();
        if(!r.Read())throw new ArgumentException("No se encontró el periodo escolar.");
        return (r.GetDateTime(0).Date,r.GetDateTime(1).Date);
    }

    public static (int SemanaActual,DateTime Cierre) Calendario(Sesion actor,int asignacion)
    {
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        var (inicio,_) = Periodo(c,null,asignacion);
        using var fecha=Db.Sql(c,"SELECT CURDATE()");
        DateTime hoy=Convert.ToDateTime(fecha.ExecuteScalar()).Date;
        int semana=hoy<inicio?0:((hoy-inicio).Days/7)+1;
        return (semana,inicio.AddDays(16*7));
    }

    private static DataTable Lista(MySqlConnection c,MySqlTransaction? tx,int asignacion,int semana)
    {
        using var cmd=Db.Sql(c,"""
            SELECT inscritos.Control,inscritos.Alumno,c.calificacion AS Nota
            FROM (
                SELECT a.Numero_control AS Control,
                    CONCAT_WS(' ',a.apellido_paterno,a.apellido_materno,a.nombre) AS Alumno
                FROM Asignacion_Docente ad
                JOIN Inscripcion i ON i.ID_grupo=ad.ID_grupo
                JOIN Alumno a ON a.Numero_control=i.Numero_control
                WHERE ad.Id_asignacion=@asignacion AND UPPER(i.estatus)='INSCRITO'
                  AND UPPER(a.estatus)='INSCRITO'
                UNION
                SELECT r.numero_control,
                    CONCAT_WS(' ',r.apellido_paterno,r.apellido_materno,r.nombre)
                FROM Asignacion_Docente ad
                JOIN Reinscripcion r ON r.ID_grupo=ad.ID_grupo
                WHERE ad.Id_asignacion=@asignacion AND UPPER(r.estatus)='INSCRITO'
            ) inscritos
            LEFT JOIN Calificacion_Corte c
              ON c.numero_control=inscritos.Control
             AND c.Id_asignacion=@asignacion AND c.semana=@semana
            ORDER BY inscritos.Alumno,inscritos.Control
            """,tx);
        Db.Param(cmd,"@asignacion",asignacion);Db.Param(cmd,"@semana",semana);
        using var rd=cmd.ExecuteReader();var dt=new DataTable();dt.Load(rd);return dt;
    }

    public static DataTable Obtener(Sesion actor,int asignacion,int semana)
    {
        if(!Semanas.Contains(semana))throw new ArgumentException("Corte inválido.");
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        return Lista(c,null,asignacion,semana);
    }

    // Se ejecuta al abrir las calificaciones en o después de semana 17:
    // no requiere que un usuario permanezca conectado ni un servicio de fondo.
    public static int CerrarAutomaticamente(Sesion actor,int asignacion)
    {
        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Autorizar(c,tx,actor,asignacion,true);
        var (inicio,_) = Periodo(c,tx,asignacion);
        using(var fecha=Db.Sql(c,"SELECT CURDATE()",tx))
        {
            var hoy=Convert.ToDateTime(fecha.ExecuteScalar()).Date;
            if(hoy<inicio.AddDays(16*7))return 0;
        }
        int cerrados=0;
        var alumnos=Lista(c,tx,asignacion,5).Rows.Cast<DataRow>()
            .Select(x=>Convert.ToString(x["Control"])!).ToArray();
        foreach(var control in alumnos)
        {
            // Truncar a dos decimales, NUNCA redondear.
            using var cmd=Db.Sql(c,"""
                INSERT INTO Calificacion_Corte
                   (numero_control,Id_asignacion,semana,calificacion)
                SELECT @control,@asignacion,17,
                       IF(TRUNCATE(SUM(calificacion)/3,2)<70,0,TRUNCATE(SUM(calificacion)/3,2))
                FROM Calificacion_Corte
                WHERE numero_control=@control AND Id_asignacion=@asignacion
                  AND semana IN (5,9,13)
                  AND NOT EXISTS (
                      SELECT 1 FROM Calificacion_Corte f17
                      WHERE f17.numero_control=@control
                        AND f17.Id_asignacion=@asignacion AND f17.semana=17)
                HAVING COUNT(DISTINCT semana)=3
                """,tx);
            Db.Param(cmd,"@control",control);Db.Param(cmd,"@asignacion",asignacion);
            if(cmd.ExecuteNonQuery()>0)cerrados++;
        }
        if(cerrados>0)
            Db.Audit(c,tx,actor.Id,"CIERRE_CALIFICACIONES","DOCENCIA",
                $"Se calcularon {cerrados} calificaciones definitivas de semana 17.",asignacion.ToString());
        tx.Commit();
        return cerrados;
    }


    public static DataTable Historial(Sesion actor,int asignacion,int semana=0)
    {
        if(semana!=0 && !Semanas.Contains(semana))
            throw new ArgumentException("Semana no válida.");
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        using var cmd=Db.Sql(c,"""
            SELECT c.numero_control AS Control,
                COALESCE(CONCAT_WS(' ',a.apellido_paterno,a.apellido_materno,a.nombre),
                         CONCAT_WS(' ',r.apellido_paterno,r.apellido_materno,r.nombre),
                         'SIN EXPEDIENTE ACTIVO') AS Alumno,
                c.semana AS Semana,
                c.calificacion AS Calificación,
                c.fecha_actualizacion AS Actualización
            FROM Calificacion_Corte c
            LEFT JOIN Alumno a ON a.Numero_control=c.numero_control
            LEFT JOIN Reinscripcion r ON r.numero_control=c.numero_control
            WHERE c.Id_asignacion=@asignacion AND (@semana=0 OR c.semana=@semana)
            ORDER BY Alumno,c.semana
            """);
        Db.Param(cmd,"@asignacion",asignacion);Db.Param(cmd,"@semana",semana);
        using var rd=cmd.ExecuteReader();var table=new DataTable();table.Load(rd);return table;
    }

    public static DataTable Cambios(Sesion actor,int asignacion,int semana=0)
    {
        if(semana!=0 && !Semanas.Contains(semana))
            throw new ArgumentException("Semana no válida.");
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        using var cmd=Db.Sql(c,"""
            SELECT b.fecha_hora AS Fecha,b.id_registro_afectado AS Control,
                   b.tipo_evento AS Acción,b.descripcion AS Detalle
            FROM Bitacora b
            WHERE b.modulo_afectado='DOCENCIA'
              AND b.tipo_evento IN ('CAPTURA_CORTE','CIERRE_CALIFICACIONES')
              AND (
                    (b.tipo_evento='CAPTURA_CORTE'
                     AND b.descripcion LIKE CONCAT('Asignación ',@asignacion,'; semana ',IF(@semana=0,'%',@semana),';%'))
                 OR (b.tipo_evento='CIERRE_CALIFICACIONES'
                     AND b.id_registro_afectado=CAST(@asignacion AS CHAR)
                     AND @semana IN (0,17))
                  )
            ORDER BY b.fecha_hora DESC,b.Id_bitacora DESC
            """);
        Db.Param(cmd,"@asignacion",asignacion);Db.Param(cmd,"@semana",semana);
        using var rd=cmd.ExecuteReader();var table=new DataTable();table.Load(rd);return table;
    }

    public static void Guardar(Sesion actor,int asignacion,int semana,IReadOnlyList<NotaCambio> cambios)
    {
        if(semana is not (5 or 9 or 13))
            throw new ArgumentException("La semana 17 se calcula automáticamente y no admite captura.");
        if(cambios.Count==0)throw new ArgumentException("No hay cambios para guardar.");
        if(cambios.Select(x=>x.Control).Distinct().Count()!=cambios.Count)
            throw new ArgumentException("La lista contiene números de control repetidos.");
        foreach(var x in cambios)
            if(x.Valor<0 || x.Valor>100 || decimal.Truncate(x.Valor*100)!=x.Valor*100)
                throw new ArgumentException("Captura valores entre 0 y 100, con máximo dos decimales.");

        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Autorizar(c,tx,actor,asignacion,true);
        var (inicio,fin)=Periodo(c,tx,asignacion);
        using(var fecha=Db.Sql(c,"SELECT CURDATE()",tx))
        {
            DateTime hoy=Convert.ToDateTime(fecha.ExecuteScalar()).Date;
            if(hoy<inicio.AddDays((semana-1)*7))
                throw new ArgumentException($"La captura de semana {semana} todavía no está habilitada.");
            if(hoy>=inicio.AddDays(16*7) || hoy>fin)
                throw new ArgumentException("Llegó la semana 17: el periodo de captura está cerrado.");
        }

        var listado=Lista(c,tx,asignacion,semana).Rows.Cast<DataRow>()
            .Select(x=>Convert.ToString(x["Control"])!).ToHashSet();
        if(semana is 9 or 13)
        {
            int anterior=semana==9?5:9;
            var previo=Lista(c,tx,asignacion,anterior);
            foreach(DataRow fila in previo.Rows)
                if(fila.IsNull("Nota"))
                    throw new ArgumentException(
                        $"No puedes iniciar semana {semana}: falta completar semana {anterior} "+
                        $"del alumno {fila["Control"]}. Debes capturar en orden a todo el grupo.");
        }
        foreach(var cambio in cambios)
        {
            if(!listado.Contains(cambio.Control))
                throw new ArgumentException($"El alumno {cambio.Control} no pertenece al grupo activo.");
            using(var prev=Db.Sql(c,"""
                SELECT COUNT(DISTINCT semana) FROM Calificacion_Corte
                WHERE numero_control=@control AND Id_asignacion=@asignacion
                  AND semana IN (5,9,13) AND semana<@semana
                """,tx))
            {
                Db.Param(prev,"@control",cambio.Control);
                Db.Param(prev,"@asignacion",asignacion);
                Db.Param(prev,"@semana",semana);
                int previos=Convert.ToInt32(prev.ExecuteScalar());
                int requeridos=semana==5?0:semana==9?1:2;
                if(previos!=requeridos)
                    throw new ArgumentException($"No puedes capturar semana {semana} de {cambio.Control}: falta un corte anterior.");
            }

            decimal? actual=null;
            using(var q=Db.Sql(c,"""
                SELECT calificacion FROM Calificacion_Corte
                WHERE numero_control=@control AND Id_asignacion=@asignacion AND semana=@semana
                FOR UPDATE
                """,tx))
            {
                Db.Param(q,"@control",cambio.Control);Db.Param(q,"@asignacion",asignacion);
                Db.Param(q,"@semana",semana);
                var val=q.ExecuteScalar();if(val is not null)actual=Convert.ToDecimal(val);
            }
            if(actual!=cambio.Original)
                throw new ArgumentException("La calificación cambió desde otra sesión; recarga antes de guardar.");
            using(var q=Db.Sql(c,"""
                SELECT COUNT(*) FROM Calificacion_Corte
                WHERE numero_control=@control AND Id_asignacion=@asignacion AND semana=17
                """,tx))
            {
                Db.Param(q,"@control",cambio.Control);Db.Param(q,"@asignacion",asignacion);
                if(Convert.ToInt32(q.ExecuteScalar())>0)
                    throw new ArgumentException("Esta materia ya tiene calificación final definitiva.");
            }

            // Menores a 70 se almacenan como 0. Se conserva la precisión válida.
            decimal nota=cambio.Valor<70?0:cambio.Valor;
            using(var q=Db.Sql(c,"""
                INSERT INTO Calificacion_Corte
                    (numero_control,Id_asignacion,semana,calificacion)
                VALUES (@control,@asignacion,@semana,@nota)
                ON DUPLICATE KEY UPDATE calificacion=VALUES(calificacion)
                """,tx))
            {
                Db.Param(q,"@control",cambio.Control);Db.Param(q,"@asignacion",asignacion);
                Db.Param(q,"@semana",semana);Db.Param(q,"@nota",nota);
                q.ExecuteNonQuery();
            }
            Db.Audit(c,tx,actor.Id,"CAPTURA_CORTE","DOCENCIA",
                $"Asignación {asignacion}; semana {semana}; control {cambio.Control}; anterior "+
                (actual?.ToString("0.##")??"SIN CAPTURA")+
                $"; valor recibido {cambio.Valor:0.##}; guardado {nota:0.##}.",
                cambio.Control);
        }
        tx.Commit();
    }
}
