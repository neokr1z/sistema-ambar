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

    // El cierre NO depende del reloj; solo del guardado de semana 17.
    private static bool Cerrada(MySqlConnection c,MySqlTransaction? tx,int asignacion)
    {
        using var cmd=Db.Sql(c,"SELECT COUNT(*) FROM Calificacion_Corte WHERE Id_asignacion=@a AND semana=17",tx);
        Db.Param(cmd,"@a",asignacion);
        return Convert.ToInt32(cmd.ExecuteScalar())>0;
    }

    public static bool EstaCerrada(Sesion actor,int asignacion)
    {
        using var c=Db.Connect();
        Autorizar(c,null,actor,asignacion);
        return Cerrada(c,null,asignacion);
    }

    public static (bool Habilitada,string Aviso) Estado(Sesion actor,int asignacion,int semana)
    {
        if(!Semanas.Contains(semana))throw new ArgumentException("Semana inválida.");
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        if(Cerrada(c,null,asignacion))
            return (false,"MATERIA CERRADA: ya se registró la semana 17. No se pueden modificar calificaciones.");
        var alumnos=Lista(c,null,asignacion,semana);
        if(alumnos.Rows.Count==0)return (false,"Este grupo no tiene alumnos activos.");
        if(semana==5)return (true,"Semana 5 disponible. Las correcciones se permiten hasta el cierre.");
        int anterior=semana==9?5:semana==13?9:13;
        var previos=Lista(c,null,asignacion,anterior);
        foreach(DataRow fila in previos.Rows)
            if(fila.IsNull("Nota"))
                return (false,$"Primero termina semana {anterior} de todo el grupo. Falta: {fila["Control"]}.");
        return semana==17
            ? (true,"Semana 17: captura a TODOS los alumnos y confirma el cierre definitivo. Después podrás imprimir la lista final.")
            : (true,$"Semana {semana} disponible; ya está completa la semana {anterior}.");
    }

    public static DataTable Finales(Sesion actor,int asignacion)
    {
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        if(!Cerrada(c,null,asignacion))
            throw new ArgumentException("Debes guardar la semana 17 antes de imprimir las calificaciones finales.");
        using var cmd=Db.Sql(c,"""
            SELECT n.numero_control AS Control,
                   COALESCE(
                       MAX(CASE WHEN a.Numero_control IS NOT NULL
                           THEN CONCAT_WS(' ',a.apellido_paterno,a.apellido_materno,a.nombre) END),
                       MAX(CASE WHEN r.numero_control IS NOT NULL
                           THEN CONCAT_WS(' ',r.apellido_paterno,r.apellido_materno,r.nombre) END),
                       'SIN EXPEDIENTE') AS Alumno,
                   MAX(CASE WHEN n.semana=5 THEN n.calificacion END) AS Semana5,
                   MAX(CASE WHEN n.semana=9 THEN n.calificacion END) AS Semana9,
                   MAX(CASE WHEN n.semana=13 THEN n.calificacion END) AS Semana13,
                   MAX(CASE WHEN n.semana=17 THEN n.calificacion END) AS Semana17
            FROM Calificacion_Corte n
            LEFT JOIN Alumno a ON a.Numero_control=n.numero_control
            LEFT JOIN Reinscripcion r ON r.numero_control=n.numero_control
            WHERE n.Id_asignacion=@a
            GROUP BY n.numero_control
            ORDER BY Alumno,n.numero_control
            """);
        Db.Param(cmd,"@a",asignacion);
        var result=new DataTable();
        using(var rd=cmd.ExecuteReader())result.Load(rd);
        result.Columns.Add("Final",typeof(decimal));
        foreach(DataRow fila in result.Rows)
        {
            if(fila.IsNull("Semana5")||fila.IsNull("Semana9")||
               fila.IsNull("Semana13")||fila.IsNull("Semana17"))continue;
            decimal total=Convert.ToDecimal(fila["Semana5"])+
                Convert.ToDecimal(fila["Semana9"])+
                Convert.ToDecimal(fila["Semana13"])+
                Convert.ToDecimal(fila["Semana17"]);
            decimal promedio=decimal.Truncate(total/4m);
            fila["Final"]=promedio<70m?0m:promedio;
        }
        return result;
    }

    public static DataTable Historial(Sesion actor,int asignacion,int semana=0)
    {
        if(semana!=0 && !Semanas.Contains(semana))
            throw new ArgumentException("Semana no válida.");
        using var c=Db.Connect();Autorizar(c,null,actor,asignacion);
        using var cmd=Db.Sql(c,"""
            SELECT c.numero_control AS Control,
                CASE WHEN a.Numero_control IS NOT NULL
                     THEN CONCAT_WS(' ',a.apellido_paterno,a.apellido_materno,a.nombre)
                     WHEN r.numero_control IS NOT NULL
                     THEN CONCAT_WS(' ',r.apellido_paterno,r.apellido_materno,r.nombre)
                     ELSE 'SIN EXPEDIENTE ACTIVO' END AS Alumno,
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
        if(!Semanas.Contains(semana))throw new ArgumentException("Semana no válida.");
        if(cambios.Count==0)throw new ArgumentException("No hay calificaciones nuevas para guardar.");
        if(cambios.Select(x=>x.Control).Distinct().Count()!=cambios.Count)
            throw new ArgumentException("La lista tiene números de control repetidos.");
        foreach(var x in cambios)
            if(x.Valor<0 || x.Valor>100 || decimal.Truncate(x.Valor)!=x.Valor)
                throw new ArgumentException("Escribe calificaciones enteras de 0 a 100.");

        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        // Bloquear asignación para serializar capturas y cierre concurrentes.
        Autorizar(c,tx,actor,asignacion,true);
        if(Cerrada(c,tx,asignacion))
            throw new ArgumentException("Semana 17 guardada: todas las calificaciones están cerradas definitivamente.");

        var filas=Lista(c,tx,asignacion,semana);
        var alumnos=filas.Rows.Cast<DataRow>()
            .Select(x=>Convert.ToString(x["Control"])!).ToHashSet();
        if(alumnos.Count==0)
            throw new ArgumentException("Este grupo no tiene alumnos activos.");
        if(semana==17 && (cambios.Count!=alumnos.Count || cambios.Any(x=>x.Original.HasValue)))
            throw new ArgumentException("Para cerrar captura la semana 17 de TODOS los alumnos juntos.");

        if(semana is 9 or 13 or 17)
        {
            int anterior=semana==9?5:semana==13?9:13;
            var previos=Lista(c,tx,asignacion,anterior);
            foreach(DataRow fila in previos.Rows)
                if(fila.IsNull("Nota"))
                    throw new ArgumentException(
                        $"Primero termina semana {anterior} de todo el grupo; falta el alumno {fila["Control"]}.");
        }
        foreach(var cambio in cambios)
        {
            if(!alumnos.Contains(cambio.Control))
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
                int requeridos=semana==5?0:semana==9?1:semana==13?2:3;
                if(Convert.ToInt32(prev.ExecuteScalar())!=requeridos)
                    throw new ArgumentException($"No se permiten saltos entre semanas; faltan cortes de {cambio.Control}.");
            }

            decimal? actual=null;
            using(var q=Db.Sql(c,"""
                SELECT calificacion FROM Calificacion_Corte
                WHERE numero_control=@control AND Id_asignacion=@asignacion AND semana=@semana
                FOR UPDATE
                """,tx))
            {
                Db.Param(q,"@control",cambio.Control);
                Db.Param(q,"@asignacion",asignacion);
                Db.Param(q,"@semana",semana);
                var val=q.ExecuteScalar();
                if(val is not null)actual=Convert.ToDecimal(val);
            }
            if(actual!=cambio.Original)
                throw new ArgumentException("Las calificaciones cambiaron en otra sesión; recarga la lista.");
            decimal nota=cambio.Valor<70m?0m:cambio.Valor;
            using(var cmd=Db.Sql(c,"""
                INSERT INTO Calificacion_Corte
                    (numero_control,Id_asignacion,semana,calificacion)
                VALUES (@control,@asignacion,@semana,@nota)
                ON DUPLICATE KEY UPDATE calificacion=VALUES(calificacion)
                """,tx))
            {
                Db.Param(cmd,"@control",cambio.Control);
                Db.Param(cmd,"@asignacion",asignacion);
                Db.Param(cmd,"@semana",semana);
                Db.Param(cmd,"@nota",nota);
                cmd.ExecuteNonQuery();
            }
            Db.Audit(c,tx,actor.Id,"CAPTURA_CORTE","DOCENCIA",
                $"Asignación {asignacion}; semana {semana}; control {cambio.Control}; anterior "+
                (actual?.ToString("0.##")??"SIN CAPTURA")+
                $"; valor recibido {cambio.Valor:0.##}; guardado {nota:0.##}.",
                cambio.Control);
        }
        if(semana==17)
            Db.Audit(c,tx,actor.Id,"CIERRE_CALIFICACIONES","DOCENCIA",
                $"Semana 17 capturada para {alumnos.Count} alumnos. Cierre definitivo de los cuatro cortes.",
                asignacion.ToString());
        tx.Commit();
    }
}
