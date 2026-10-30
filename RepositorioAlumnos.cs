using MySqlConnector;
using System.Data;

namespace AMBAR.Semana4;

internal static class RepositorioAlumnos
{
    private static void CheckNss(MySqlConnection c, MySqlTransaction tx, StudentData d)
    {
        if(d.Nss.Length==0)return;
        using var cmd=Db.Sql(c,"SELECT COUNT(*) FROM Alumno WHERE nss=@nss AND Numero_control<>@control",tx);
        Db.Param(cmd,"@nss",d.Nss);Db.Param(cmd,"@control",d.Control);
        if(Convert.ToInt32(cmd.ExecuteScalar())>0)
            throw new ArgumentException("Ese NSS ya está registrado en otro alumno. Verifica la constancia del IMSS.");
    }

    public static List<Choice> Plans()
    {
        var list=new List<Choice>();
        using var c=Db.Connect();
        using var cmd=Db.Sql(c,"""
            SELECT p.id_plan,CONCAT(c.nombre_carrera,' · ',p.nombre_plan,' (',p.version,')')
            FROM Plan_Estudio p INNER JOIN Carrera c ON c.clave_carrera=p.clave_carrera
            WHERE UPPER(p.estatus)='ACTIVO' AND UPPER(c.estatus)='ACTIVO'
            ORDER BY c.nombre_carrera,p.nombre_plan
            """);
        using var r=cmd.ExecuteReader();
        while(r.Read())list.Add(new Choice(r.GetInt32(0),r.GetString(1)));
        return list;
    }

    public static List<Choice> Groups(int planId, int? periodId = null)
    {
        var list=new List<Choice>();
        using var c=Db.Connect();
        using var cmd=Db.Sql(c,"""
            SELECT g.ID_grupo,CONCAT(g.nombre_grupo,' · ',pe.nombre_periodo,
                ' (',g.Capacidad-COUNT(i.Id_inscripcion),' lugares disponibles)') AS etiqueta
            FROM Grupo g
            INNER JOIN Carrera ca ON ca.ID_Carrera=g.ID_Carrera
            INNER JOIN Plan_Estudio pl ON pl.clave_carrera=ca.clave_carrera
            INNER JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre AND s.numero_semestre=1
            INNER JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            LEFT JOIN Inscripcion i ON i.ID_grupo=g.ID_grupo AND UPPER(i.estatus)='INSCRITO'
            WHERE pl.id_plan=@plan AND g.habilitado_fichas=1 AND pe.nombre_periodo='Agosto-Diciembre 2026' AND (@period IS NULL OR g.ID_Periodo=@period) AND UPPER(g.Estatus)='ACTIVO' AND UPPER(pe.estatus)='ACTIVO'
            GROUP BY g.ID_grupo,g.nombre_grupo,pe.nombre_periodo,g.Capacidad
            HAVING COUNT(i.Id_inscripcion)<g.Capacidad
            ORDER BY g.nombre_grupo
            """);
        Db.Param(cmd,"@plan",planId); Db.Param(cmd,"@period",periodId);
        using var r=cmd.ExecuteReader();
        while(r.Read())list.Add(new Choice(r.GetInt32(0),r.GetString(1)));
        return list;
    }

    public static AspirantInfo? Aspirant(string ficha)
    {
        using var c=Db.Connect();
        using var cmd=Db.Sql(c,"""
            SELECT ID_Aspirante,curp,resultado_examen,estatus,periodo_fichas
            FROM Aspirante WHERE numero_ficha=@ficha
            """);
        Db.Param(cmd,"@ficha",ficha.Trim());
        using var r=cmd.ExecuteReader();
        if(!r.Read())return null;
        return new AspirantInfo(r.GetInt32(0),r.IsDBNull(1)?"":r.GetString(1),r.GetString(2),r.GetString(3),r.IsDBNull(4)?null:r.GetInt32(4));
    }

    private static bool Eligible(string result,string status)
    {
        string res=result.Trim().ToUpperInvariant();
        string state=status.Trim().ToUpperInvariant();
        return (res=="APROBADO" || res=="ACEPTADO" || res=="ACREDITADO") &&
            (state=="ACTIVO" || state=="ACEPTADO" || state=="APROBADO");
    }

    public static string Enroll(StudentData d,Sesion session) => EnrollAsync(d,session).GetAwaiter().GetResult();
    public static async Task<string> EnrollAsync(StudentData d,Sesion session)
    {
        if(!session.CanSeeStudents)throw new UnauthorizedAccessException("El rol no puede inscribir alumnos.");
        if(d.AspirantId<=0 || d.PlanId<=0 || d.GroupId<=0)
            throw new ArgumentException("Primero busca una ficha válida, selecciona el plan y un grupo con cupo.");
        d.Validate();
        await CorreoDominio.Current.RequireAsync(d.Email,"Correo institucional").ConfigureAwait(false);
        if(d.PersonalEmail.Length>0) await CorreoDominio.Current.RequireAsync(d.PersonalEmail,"Correo personal").ConfigureAwait(false);
        CatalogoPostal.Validar(d.Postal,d.Neighborhood,d.Locality,d.Municipality,d.State);
        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Identidad.Check(c,tx,session,d.First,d.Paternal,d.Maternal,d.Curp,aspirantId:d.AspirantId);
        CheckNss(c,tx,d);
        // Los bloqueos de aspirante y grupo impiden que dos secretarias dupliquen una inscripción.
        string result,status,curp; int? period;
        using(var lockAsp=Db.Sql(c,"SELECT resultado_examen,estatus,curp,periodo_fichas FROM Aspirante WHERE ID_Aspirante=@id FOR UPDATE",tx))
        {
            Db.Param(lockAsp,"@id",d.AspirantId);
            using var reader=lockAsp.ExecuteReader();
            if(!reader.Read())throw new ArgumentException("La ficha del aspirante ya no existe.");
            result=reader.GetString(0); status=reader.GetString(1);curp=reader.IsDBNull(2)?"":reader.GetString(2);
            period=reader.IsDBNull(3)?null:reader.GetInt32(3);
        }
        if(!Eligible(result,status))throw new ArgumentException("El aspirante no tiene admisión aprobada/aceptada.");
        if(!(curp.Length==0 && period.HasValue) && !string.Equals(d.Curp,curp,StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("La CURP no coincide con la ficha del aspirante.");
        int existing;
        using(var x=Db.Sql(c,"SELECT COUNT(*) FROM Alumno WHERE ID_Aspirante=@a",tx))
        {Db.Param(x,"@a",d.AspirantId);existing=Convert.ToInt32(x.ExecuteScalar());}
        if(existing>0)throw new ArgumentException("Este aspirante ya fue dado de alta.");
        int capacity;
        using(var lockGroup=Db.Sql(c,"""
            SELECT g.Capacidad FROM Grupo g
            INNER JOIN Carrera ca ON ca.ID_Carrera=g.ID_Carrera
            INNER JOIN Plan_Estudio p ON p.clave_carrera=ca.clave_carrera
            INNER JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            INNER JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            WHERE g.ID_grupo=@grupo AND p.id_plan=@plan AND s.numero_semestre=1
                AND UPPER(g.Estatus)='ACTIVO' AND UPPER(pe.estatus)='ACTIVO'
                AND (@period IS NULL OR (g.ID_Periodo=@period AND g.habilitado_fichas=1))
            FOR UPDATE
            """,tx))
        {
            Db.Param(lockGroup,"@period",period);
            Db.Param(lockGroup,"@grupo",d.GroupId);Db.Param(lockGroup,"@plan",d.PlanId);
            object? size=lockGroup.ExecuteScalar();
            if(size is null)throw new ArgumentException("El grupo no está vigente o no corresponde al plan seleccionado.");
            capacity=Convert.ToInt32(size);
        }
        int occupied;
        using(var check=Db.Sql(c,"SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=@g AND UPPER(estatus)='INSCRITO'",tx))
        {Db.Param(check,"@g",d.GroupId);occupied=Convert.ToInt32(check.ExecuteScalar());}
        if(occupied>=capacity)throw new ArgumentException("El grupo ya no tiene lugares disponibles.");
        // Identificador de demostración; en producción debe adecuarse a la política institucional.
        d.Control=DateTime.Today.Year.ToString()+d.AspirantId.ToString("D6");
        if(string.IsNullOrWhiteSpace(d.Email))
            throw new ArgumentException("Capture el correo institucional. AMBAR no crea cuentas de correo reales.");
        using(var add=Db.Sql(c,"""
            INSERT INTO Alumno(Numero_control,ID_Aspirante,ID_Plan,curp,nombre,apellido_paterno,
                apellido_materno,fecha_nacimiento,nss,telefono,telefono_emergencia,alergias,
                correo_institucional,correo_propio,estatus)
            VALUES (@control,@asp,@plan,@curp,@nombre,@ap,@am,@birth,@nss,@tel,@emerg,@aler,@email,@personal,'INSCRITO')
            """,tx))
        {
            StudentParams(add,d);
            Db.Param(add,"@control",d.Control);Db.Param(add,"@asp",d.AspirantId);Db.Param(add,"@plan",d.PlanId);
            add.ExecuteNonQuery();
        }
        using(var address=Db.Sql(c,"""
            INSERT INTO Domicilio(numero_control,calle,numero_exterior,numero_interior,colonia,
                localidad,municipio,estado,codigo_postal)
            VALUES (@control,@street,@ext,@int,@col,@local,@mun,@state,@postal)
            """,tx))
        {
            AddressParams(address,d);Db.Param(address,"@control",d.Control);address.ExecuteNonQuery();
        }
        using(var ins=Db.Sql(c,"""
            INSERT INTO Inscripcion(Numero_control,ID_grupo,tipo_inscripcion,estatus)
            VALUES (@control,@group,'NUEVO_INGRESO','INSCRITO')
            """,tx))
        {Db.Param(ins,"@control",d.Control);Db.Param(ins,"@group",d.GroupId);ins.ExecuteNonQuery();}
        using(var asp=Db.Sql(c,"UPDATE Aspirante SET estatus='INSCRITO',curp=@curp WHERE ID_Aspirante=@id",tx))
        {Db.Param(asp,"@curp",d.Curp);Db.Param(asp,"@id",d.AspirantId);asp.ExecuteNonQuery();}
        Db.Audit(c,tx,session.Id,"INSCRIPCION","ALUMNOS","Inscripción de nuevo ingreso",d.Control);
        tx.Commit();
        return d.Control;
    }

    public static DataTable ListStudents(Sesion session,string filter)
    {
        if(!session.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        var t=new DataTable();
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,session);
        using var cmd=Db.Sql(c,"""
            SELECT a.Numero_control AS 'No. control',
                CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS 'Nombre completo',
                a.curp AS 'CURP',ca.nombre_carrera AS 'Carrera',a.correo_institucional AS 'Correo',
                a.estatus AS 'Estatus'
            FROM Alumno a
            INNER JOIN Plan_Estudio p ON p.id_plan=a.ID_Plan
            INNER JOIN Carrera ca ON ca.clave_carrera=p.clave_carrera
            WHERE (@admin=1 OR UPPER(a.estatus)<>'OCULTO')
                AND (@filter='' OR a.Numero_control LIKE @like OR a.nombre LIKE @like
                    OR a.apellido_paterno LIKE @like OR a.curp LIKE @like)
            ORDER BY a.apellido_paterno,a.apellido_materno,a.nombre
            """);
        Db.Param(cmd,"@admin",session.IsAdmin?1:0);
        Db.Param(cmd,"@filter",filter.Trim()); Db.Param(cmd,"@like","%"+filter.Trim()+"%");
        using var reader=cmd.ExecuteReader();
        t.Load(reader);
        return t;
    }

    public static StudentData? Detail(Sesion session,string control)
    {
        if(!session.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,session);
        using var cmd=Db.Sql(c,"""
            SELECT a.Numero_control,a.ID_Aspirante,a.ID_Plan,a.curp,a.nombre,a.apellido_paterno,
                a.apellido_materno,a.fecha_nacimiento,a.nss,a.telefono,a.telefono_emergencia,
                a.alergias,a.correo_institucional,a.correo_propio,a.estatus,
                d.calle,d.numero_exterior,d.numero_interior,d.colonia,d.localidad,d.municipio,d.estado,d.codigo_postal,
                ca.nombre_carrera,p.nombre_plan,g.nombre_grupo,pe.nombre_periodo,
                asp.numero_ficha,i.fecha_inscripcion
            FROM Alumno a
            INNER JOIN Aspirante asp ON asp.ID_Aspirante=a.ID_Aspirante
            INNER JOIN Plan_Estudio p ON p.id_plan=a.ID_Plan
            INNER JOIN Carrera ca ON ca.clave_carrera=p.clave_carrera
            LEFT JOIN Domicilio d ON d.numero_control=a.Numero_control
            LEFT JOIN Inscripcion i ON i.Id_inscripcion=(SELECT MAX(i2.Id_inscripcion)
                FROM Inscripcion i2 WHERE i2.Numero_control=a.Numero_control)
            LEFT JOIN Grupo g ON g.ID_grupo=i.ID_grupo
            LEFT JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            WHERE a.Numero_control=@control AND (@admin=1 OR UPPER(a.estatus)<>'OCULTO')
            """);
        Db.Param(cmd,"@control",control);Db.Param(cmd,"@admin",session.IsAdmin?1:0);
        using var r=cmd.ExecuteReader();
        if(!r.Read())return null;
        string Get(int i)=>r.IsDBNull(i)?"":r.GetString(i);
        return new StudentData {
            Control=Get(0),AspirantId=r.GetInt32(1),PlanId=r.GetInt32(2),Curp=Get(3),
            First=Get(4),Paternal=Get(5),Maternal=Get(6),Birth=r.GetDateTime(7),
            Nss=Get(8),Phone=Get(9),Emergency=Get(10),Allergies=Get(11),Email=Get(12),
            PersonalEmail=Get(13),Status=Get(14),Street=Get(15),Exterior=Get(16),Interior=Get(17),
            Neighborhood=Get(18),Locality=Get(19),Municipality=Get(20),State=Get(21),Postal=Get(22),
            Career=Get(23),Plan=Get(24),Group=Get(25),Period=Get(26),ApplicantNumber=Get(27),
            EnrollmentDate=r.IsDBNull(28)?null:r.GetDateTime(28)
        };
    }

    private static void StudentParams(MySqlCommand cmd,StudentData d)
    {
        Db.Param(cmd,"@curp",d.Curp);Db.Param(cmd,"@nombre",d.First);
        Db.Param(cmd,"@ap",d.Paternal);Db.Param(cmd,"@am",d.Maternal);
        Db.Param(cmd,"@birth",d.Birth.Date);
        Db.Param(cmd,"@nss",d.Nss.Length>0?d.Nss:null);
        Db.Param(cmd,"@tel",d.Phone.Length>0?d.Phone:null);
        Db.Param(cmd,"@emerg",d.Emergency);
        Db.Param(cmd,"@aler",d.Allergies.Length>0?d.Allergies:null);
        Db.Param(cmd,"@email",d.Email);
        Db.Param(cmd,"@personal",d.PersonalEmail.Length>0?d.PersonalEmail:null);
    }

    private static void AddressParams(MySqlCommand cmd,StudentData d)
    {
        Db.Param(cmd,"@street",d.Street);Db.Param(cmd,"@ext",d.Exterior);
        Db.Param(cmd,"@int",d.Interior.Length>0?d.Interior:null);
        Db.Param(cmd,"@col",d.Neighborhood);Db.Param(cmd,"@local",d.Locality);
        Db.Param(cmd,"@mun",d.Municipality);Db.Param(cmd,"@state",d.State);
        Db.Param(cmd,"@postal",d.Postal);
    }

    public static void Update(StudentData d,Sesion actor) => UpdateAsync(d,actor).GetAwaiter().GetResult();
    public static async Task UpdateAsync(StudentData d,Sesion actor)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        d.Validate();
        await CorreoDominio.Current.RequireAsync(d.Email,"Correo institucional").ConfigureAwait(false);
        if(d.PersonalEmail.Length>0) await CorreoDominio.Current.RequireAsync(d.PersonalEmail,"Correo personal").ConfigureAwait(false);
        using var c=Db.Connect();using var tx=c.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        // El ID de la ficha se obtiene del expediente, no del formulario.
        using(var own=Db.Sql(c,"SELECT ID_Aspirante FROM Alumno WHERE Numero_control=@control",tx))
        { Db.Param(own,"@control",d.Control); d.AspirantId=Convert.ToInt32(own.ExecuteScalar() ?? throw new ArgumentException("El alumno ya no existe.")); }
        Identidad.Check(c,tx,actor,d.First,d.Paternal,d.Maternal,d.Curp,d.Control,d.AspirantId);
        CheckNss(c,tx,d);
        using(var guard=Db.Sql(c,"SELECT estatus FROM Alumno WHERE Numero_control=@control FOR UPDATE",tx))
        {
            Db.Param(guard,"@control",d.Control);
            string? status=Convert.ToString(guard.ExecuteScalar());
            if(string.IsNullOrEmpty(status))throw new ArgumentException("El alumno ya no existe.");
            if(status.Equals("OCULTO",StringComparison.OrdinalIgnoreCase) && !actor.IsAdmin)
                throw new UnauthorizedAccessException("El expediente se ocultó; sólo Administración puede modificarlo.");
        }
        using(var cmd=Db.Sql(c,"""
            UPDATE Alumno SET curp=@curp,nombre=@nombre,apellido_paterno=@ap,apellido_materno=@am,
                fecha_nacimiento=@birth,nss=@nss,telefono=@tel,telefono_emergencia=@emerg,
                alergias=@aler,correo_institucional=@email,correo_propio=@personal
            WHERE Numero_control=@control
            """,tx))
        {
            StudentParams(cmd,d);Db.Param(cmd,"@control",d.Control);
            cmd.ExecuteNonQuery();
        }
        using(var cmd=Db.Sql(c,"""
            UPDATE Domicilio SET calle=@street,numero_exterior=@ext,numero_interior=@int,
                colonia=@col,localidad=@local,municipio=@mun,estado=@state,codigo_postal=@postal
            WHERE numero_control=@control
            """,tx))
        {
            AddressParams(cmd,d);Db.Param(cmd,"@control",d.Control);cmd.ExecuteNonQuery();
        }
        using(var asp=Db.Sql(c,"UPDATE Aspirante SET curp=@curp WHERE ID_Aspirante=@id",tx))
        {Db.Param(asp,"@curp",d.Curp);Db.Param(asp,"@id",d.AspirantId);asp.ExecuteNonQuery();}
        Db.Audit(c,tx,actor.Id,"EDICION","ALUMNOS","Actualización de datos personales/domicilio",d.Control);
        tx.Commit();
    }

    public static void Visibility(Sesion session,string control,bool restore)
    {
        if(!session.CanSeeStudents || (restore && !session.IsAdmin))
            throw new UnauthorizedAccessException("No tiene permiso para realizar esta operación.");
        using var c=Db.Connect();using var tx=c.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c,tx,session);
        using(var cmd=Db.Sql(c,"UPDATE Alumno SET estatus=@status WHERE Numero_control=@control",tx))
        {
            Db.Param(cmd,"@status",restore?"INSCRITO":"OCULTO");
            Db.Param(cmd,"@control",control);
            if(cmd.ExecuteNonQuery()==0)throw new ArgumentException("No fue posible actualizar el estado de visibilidad.");
        }
        Db.Audit(c,tx,session.Id,restore?"RESTAURAR":"OCULTAR","ALUMNOS",
            restore?"Registro restaurado por administrador":"Registro oculto para Secretaría",control);
        tx.Commit();
    }
}
