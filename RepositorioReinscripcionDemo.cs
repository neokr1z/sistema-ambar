using System.Data;
using System.Text.RegularExpressions;
using MySqlConnector;

namespace AMBAR.Semana4;

// Módulo de demostración autónomo: el número de control indica generación, no identidad verificada.
internal sealed record ReinscripcionDemo(string Control, string Nombre, string Paterno, string Materno,
    int Anio, int Semestre, int Grupo, string GrupoNombre, string Turno);

internal static class RepositorioReinscripcionDemo
{
    public const string Periodo="Agosto-Diciembre 2026";

    public static (int Anio,int Semestre) Inferir(string numero)
    {
        numero=numero.Trim();
        if(!Regex.IsMatch(numero,@"\A[0-9]{10}\z"))
            throw new ArgumentException("El número de control debe contener exactamente 10 dígitos.");
        return numero[..4] switch
        {
            "2025"=>(2025,3),
            "2026"=>(2026,2),
            _=>throw new ArgumentException("Para agosto-diciembre 2026 solo se permiten generaciones 2026 (2.º) y 2025 (3.º).")
        };
    }

    // Impide reinscribir a quien acaba de inscribirse a primer semestre en el periodo actual.
    public static void VerificarNoPrimerSemestre(Sesion actor,string numero)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        VerificarNoPrimerSemestre(c,null,numero);
    }

    private static void VerificarNoPrimerSemestre(MySqlConnection c,MySqlTransaction? tx,string numero)
    {
        using var cmd=Db.Sql(c,"""
            SELECT COUNT(*) FROM Inscripcion i
            JOIN Grupo g ON g.ID_grupo=i.ID_grupo
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            WHERE i.Numero_control=@control
              AND UPPER(i.estatus)='INSCRITO'
              AND s.numero_semestre=1
              AND pe.nombre_periodo=@periodo
            """,tx);
        Db.Param(cmd,"@control",numero);Db.Param(cmd,"@periodo",Periodo);
        if(Convert.ToInt32(cmd.ExecuteScalar())>0)
            throw new ArgumentException("El alumno ya está inscrito en primer semestre durante agosto–diciembre 2026; no puede reinscribirse en el mismo periodo.");
    }

    public static ReinscripcionDemo? Existente(Sesion actor,string numero)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT r.nombre,r.apellido_paterno,r.apellido_materno,r.anio_ingreso,
              r.semestre_destino,r.ID_grupo,g.nombre_grupo,COALESCE(g.turno,'')
            FROM Reinscripcion_Demostracion r JOIN Grupo g ON g.ID_grupo=r.ID_grupo
            WHERE r.numero_control=@control
            """);
        Db.Param(cmd,"@control",numero.Trim());
        using var rd=cmd.ExecuteReader();
        if(!rd.Read())return null;
        return new(numero.Trim(),rd.GetString(0),rd.GetString(1),rd.GetString(2),
            rd.GetInt32(3),rd.GetInt32(4),rd.GetInt32(5),rd.GetString(6),rd.GetString(7));
    }

    public static List<Choice> Grupos(Sesion actor,int semestre)
    {
        if(semestre is not (2 or 3))throw new ArgumentException("Semestre inválido.");
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT g.ID_grupo,CONCAT(g.nombre_grupo,' · ',COALESCE(g.turno,'SIN TURNO'),
              ' · ',GREATEST(0,g.Capacidad-COUNT(DISTINCT r.numero_control)-COUNT(DISTINCT i.Numero_control)),' lugares')
            FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            LEFT JOIN Reinscripcion_Demostracion r ON r.ID_grupo=g.ID_grupo AND UPPER(r.estatus)='INSCRITO'
            LEFT JOIN Inscripcion i ON i.ID_grupo=g.ID_grupo AND UPPER(i.estatus)='INSCRITO'
            WHERE s.numero_semestre=@semestre AND p.nombre_periodo=@periodo
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO'
            GROUP BY g.ID_grupo,g.nombre_grupo,g.turno,g.Capacidad
            HAVING COUNT(DISTINCT r.numero_control)+COUNT(DISTINCT i.Numero_control)<g.Capacidad
            ORDER BY g.turno,g.nombre_grupo
            """);
        Db.Param(cmd,"@semestre",semestre);Db.Param(cmd,"@periodo",Periodo);
        var items=new List<Choice>();using var reader=cmd.ExecuteReader();
        while(reader.Read())items.Add(new Choice(reader.GetInt32(0),reader.GetString(1)));
        return items;
    }

    public static void Guardar(Sesion actor,string numero,string nombre,string paterno,string materno,int grupo)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        numero=numero.Trim();
        var (anio,semestre)=Inferir(numero);
        nombre=Validation.Name(nombre,"nombre",60);
        paterno=Validation.Name(paterno,"apellido paterno");
        materno=Validation.Name(materno,"apellido materno");
        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c,tx,actor);
        VerificarNoPrimerSemestre(c,tx,numero);
        using(var gate=Db.Sql(c,"SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE",tx))
            if(gate.ExecuteScalar() is null)throw new InvalidOperationException("Ejecuta el script de migración 06.");
        using(var existe=Db.Sql(c,"""
            SELECT COUNT(*) FROM Reinscripcion_Demostracion WHERE numero_control=@control
            """,tx))
        {
            Db.Param(existe,"@control",numero);
            if(Convert.ToInt32(existe.ExecuteScalar())>0)
                throw new ArgumentException("Este número de control ya está reinscrito en la demostración.");
        }
        // Se conserva intacto cualquier expediente previo en Alumno.
        // Esta tabla independiente solo guarda una prueba del número validado.
        int capacidad;
        using(var cmd=Db.Sql(c,"""
            SELECT g.Capacidad FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE g.ID_grupo=@grupo AND s.numero_semestre=@semestre
              AND p.nombre_periodo=@periodo AND UPPER(g.Estatus)='ACTIVO'
              AND UPPER(p.estatus)='ACTIVO' FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@grupo",grupo);Db.Param(cmd,"@semestre",semestre);Db.Param(cmd,"@periodo",Periodo);
            var value=cmd.ExecuteScalar();
            if(value is null)throw new ArgumentException("El grupo no pertenece al semestre correspondiente.");
            capacidad=Convert.ToInt32(value);
        }
        using(var cmd=Db.Sql(c,"""
            SELECT (SELECT COUNT(*) FROM Reinscripcion_Demostracion
                    WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
                 + (SELECT COUNT(*) FROM Inscripcion
                    WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
            """,tx))
        {
            Db.Param(cmd,"@grupo",grupo);
            if(Convert.ToInt32(cmd.ExecuteScalar())>=capacidad)
                throw new ArgumentException("El grupo no tiene lugares disponibles.");
        }
        using(var cmd=Db.Sql(c,"""
            INSERT INTO Reinscripcion_Demostracion
              (numero_control,nombre,apellido_paterno,apellido_materno,anio_ingreso,semestre_destino,ID_grupo)
            VALUES(@control,@nombre,@paterno,@materno,@anio,@semestre,@grupo)
            """,tx))
        {
            Db.Param(cmd,"@control",numero);Db.Param(cmd,"@nombre",nombre);
            Db.Param(cmd,"@paterno",paterno);Db.Param(cmd,"@materno",materno);
            Db.Param(cmd,"@anio",anio);Db.Param(cmd,"@semestre",semestre);Db.Param(cmd,"@grupo",grupo);
            cmd.ExecuteNonQuery();
        }
        Db.Audit(c,tx,actor.Id,"REINSCRIPCION_DEMO","ALUMNOS","Registro de demostración por generación (no verifica historial académico)",numero);
        tx.Commit();
    }

    public static DataTable Lista(Sesion actor,int grupo)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT numero_control AS 'No. control',
              CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno) AS 'Alumno'
            FROM Reinscripcion_Demostracion
            WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO'
            ORDER BY numero_control
            """);
        Db.Param(cmd,"@grupo",grupo);
        var data=new DataTable();using var reader=cmd.ExecuteReader();data.Load(reader);return data;
    }
}
