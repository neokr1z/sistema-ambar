using System.Data;
using System.Text.RegularExpressions;
using MySqlConnector;

namespace AMBAR.Semana4;

// Módulo de demostración autónomo: el número de control indica generación, no identidad verificada.
internal sealed record ReinscripcionDemo(string Control, string Nombre, string Paterno, string Materno,
    int Anio, int Semestre, int Grupo, string GrupoNombre, string Turno);

internal sealed record AlumnoParaReinscripcion(string Nombre,string Paterno,string Materno,
    string Curp,DateTime Nacimiento,string Correo,string Emergencia);

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

    public static void ValidarSemestre(string numero,int semestre)
    {
        var (anio,_) = Inferir(numero);
        if ((anio==2026 && semestre!=2) || (anio==2025 && semestre is not (2 or 3)))
            throw new ArgumentException("Semestre de destino incompatible con la generación. 2026: solo 2.º; 2025: 2.º o 3.º.");
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

    public static AlumnoParaReinscripcion? BuscarAlumno(Sesion actor,string numero)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT nombre,apellido_paterno,apellido_materno,curp,
                   fecha_nacimiento,correo_institucional,telefono_emergencia
            FROM Alumno WHERE Numero_control=@control
            """);
        Db.Param(cmd,"@control",numero.Trim());
        using var r=cmd.ExecuteReader();
        return r.Read()?new AlumnoParaReinscripcion(r.GetString(0),r.GetString(1),r.GetString(2),
            r.GetString(3),r.GetDateTime(4),r.GetString(5),r.GetString(6)):null;
    }

    public static void VerificarDisponible(Sesion actor,string numero)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        VerificarDisponible(c,null,numero);
    }

    private static void VerificarDisponible(MySqlConnection c,MySqlTransaction? tx,string numero)
    {
        VerificarNoPrimerSemestre(c,tx,numero);
        using var cmd=Db.Sql(c,"""
            SELECT COUNT(*) FROM Inscripcion i
            JOIN Grupo g ON g.ID_grupo=i.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE i.Numero_control=@control AND UPPER(i.estatus)='INSCRITO'
              AND p.nombre_periodo=@periodo
            """,tx);
        Db.Param(cmd,"@control",numero);Db.Param(cmd,"@periodo",Periodo);
        if(Convert.ToInt32(cmd.ExecuteScalar())>0)
            throw new ArgumentException("Este número de control ya tiene una inscripción activa en agosto–diciembre 2026.");
        using var viejo=Db.Sql(c,"SELECT COUNT(*) FROM Reinscripcion_Demostracion WHERE numero_control=@control AND UPPER(estatus)='INSCRITO'",tx);
        Db.Param(viejo,"@control",numero);
        if(Convert.ToInt32(viejo.ExecuteScalar())>0)
            throw new ArgumentException("Este número ya figura reinscrito en los registros anteriores. No se permite duplicarlo.");
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

    // Crea un expediente cuando no existe, o reutiliza el existente.
    // La inscripción y el expediente se confirman juntos, sin inventar CURP o fechas.
    public static void Guardar(Sesion actor,string numero,string nombre,string paterno,string materno,
        int grupo,int semestre,string curp,DateTime nacimiento,string correo,string emergencia)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        numero=numero.Trim();
        ValidarSemestre(numero,semestre);
        nombre=Validation.Name(nombre,"nombre",60);
        paterno=Validation.Name(paterno,"apellido paterno");
        materno=Validation.Name(materno,"apellido materno");
        curp=curp.Trim().ToUpperInvariant();
        Validation.CurpIdentity(curp,nombre,paterno,materno,nacimiento.Date);
        correo=Contacto.Email(correo,"Correo institucional");
        if(!Validation.InstitutionalEmail(correo))
            throw new ArgumentException("El correo institucional debe pertenecer a @acapulco.tecnm.mx.");
        // La validación de dominio es local; NO se envía correo.
        CorreoDominio.Current.RequireAsync(correo,"Correo institucional").GetAwaiter().GetResult();
        emergencia=Contacto.Phone(emergencia,"Teléfono de emergencia");

        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c,tx,actor);
        using(var gate=Db.Sql(c,"SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE",tx))
            if(gate.ExecuteScalar() is null)
                throw new InvalidOperationException("Ejecuta el script 06 para crear Control_Identidad.");
        VerificarDisponible(c,tx,numero);

        bool registrado;
        using(var cmd=Db.Sql(c,"""
            SELECT nombre,apellido_paterno,apellido_materno,curp,fecha_nacimiento,
                   correo_institucional,telefono_emergencia
            FROM Alumno WHERE Numero_control=@control FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@control",numero);
            using var r=cmd.ExecuteReader();
            registrado=r.Read();
            if(registrado)
            {
                // Un número de control existente no debe asignarse a otra identidad.
                bool mismo=string.Equals(r.GetString(0),nombre,StringComparison.OrdinalIgnoreCase)
                    && string.Equals(r.GetString(1),paterno,StringComparison.OrdinalIgnoreCase)
                    && string.Equals(r.GetString(2),materno,StringComparison.OrdinalIgnoreCase)
                    && string.Equals(r.GetString(3),curp,StringComparison.OrdinalIgnoreCase)
                    && r.GetDateTime(4).Date==nacimiento.Date
                    && string.Equals(r.GetString(5),correo,StringComparison.OrdinalIgnoreCase)
                    && r.GetString(6)==emergencia;
                if(!mismo)
                    throw new ArgumentException("El número de control ya pertenece a otro expediente o los datos no coinciden. No se crearán alumnos duplicados.");
            }
        }
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
            if(value is null)throw new ArgumentException("Grupo inválido para el semestre seleccionado.");
            capacidad=Convert.ToInt32(value);
        }
        using(var cmd=Db.Sql(c,"""
            SELECT (SELECT COUNT(*) FROM Inscripcion
                    WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
                 +(SELECT COUNT(*) FROM Reinscripcion_Demostracion
                    WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
            """,tx))
        {
            Db.Param(cmd,"@grupo",grupo);
            if(Convert.ToInt32(cmd.ExecuteScalar())>=capacidad)
                throw new ArgumentException("El grupo ya no tiene lugares disponibles.");
        }
        if(!registrado)
        {
            int plan;
            using(var cmd=Db.Sql(c,"""
                SELECT p.id_plan FROM Plan_Estudio p
                JOIN Carrera ca ON ca.clave_carrera=p.clave_carrera
                JOIN Grupo g ON g.ID_Carrera=ca.ID_Carrera
                WHERE g.ID_grupo=@grupo AND UPPER(p.estatus)='ACTIVO'
                ORDER BY p.id_plan LIMIT 1
                """,tx))
            {
                Db.Param(cmd,"@grupo",grupo);
                var value=cmd.ExecuteScalar();
                if(value is null)throw new ArgumentException("No hay plan de estudios activo para este grupo.");
                plan=Convert.ToInt32(value);
            }
            using(var check=Db.Sql(c,"SELECT COUNT(*) FROM Alumno WHERE curp=@curp OR correo_institucional=@correo",tx))
            {
                Db.Param(check,"@curp",curp);Db.Param(check,"@correo",correo);
                if(Convert.ToInt32(check.ExecuteScalar())>0)
                    throw new ArgumentException("La CURP o el correo institucional ya está registrado con otro número de control.");
            }
            // La ficha administrativa indica origen por reinscripción, NO admisión nueva.
            using(var asp=Db.Sql(c,"""
                INSERT INTO Aspirante(numero_ficha,curp,promedio_preparatoria,nombre_preparatoria,
                    resultado_examen,estatus)
                VALUES(@ficha,@curp,NULL,NULL,'NO_APLICA','REINSCRIPCION')
                """,tx))
            {
                Db.Param(asp,"@ficha","REINS-"+numero);Db.Param(asp,"@curp",curp);
                asp.ExecuteNonQuery();
            }
            int idAspirante;
            using(var id=Db.Sql(c,"SELECT LAST_INSERT_ID()",tx))
                idAspirante=Convert.ToInt32(id.ExecuteScalar());
            using(var nuevo=Db.Sql(c,"""
                INSERT INTO Alumno(Numero_control,ID_Aspirante,ID_Plan,curp,nombre,
                    apellido_paterno,apellido_materno,fecha_nacimiento,
                    telefono_emergencia,correo_institucional,estatus)
                VALUES(@control,@asp,@plan,@curp,@nombre,@paterno,@materno,@nacimiento,
                       @emergencia,@correo,'INSCRITO')
                """,tx))
            {
                Db.Param(nuevo,"@control",numero);Db.Param(nuevo,"@asp",idAspirante);
                Db.Param(nuevo,"@plan",plan);Db.Param(nuevo,"@curp",curp);
                Db.Param(nuevo,"@nombre",nombre);Db.Param(nuevo,"@paterno",paterno);
                Db.Param(nuevo,"@materno",materno);Db.Param(nuevo,"@nacimiento",nacimiento.Date);
                Db.Param(nuevo,"@emergencia",emergencia);Db.Param(nuevo,"@correo",correo);
                nuevo.ExecuteNonQuery();
            }
        }
        using(var ins=Db.Sql(c,"""
            INSERT INTO Inscripcion(Numero_control,ID_grupo,tipo_inscripcion,estatus)
            VALUES(@control,@grupo,'REINSCRIPCION','INSCRITO')
            """,tx))
        {
            Db.Param(ins,"@control",numero);Db.Param(ins,"@grupo",grupo);
            ins.ExecuteNonQuery();
        }
        Db.Audit(c,tx,actor.Id,"REINSCRIPCION","ALUMNOS",
            registrado?"Reinscripción de alumno existente":"Alta de expediente y reinscripción",numero);
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
