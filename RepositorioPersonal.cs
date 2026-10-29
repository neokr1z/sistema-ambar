using System.Data;

namespace AMBAR.Semana4;

internal static class RepositorioPersonal
{
    public static PersonalCreated Create(Sesion actor, PersonalData data)
    {
        string rol = data.Role;
        if (rol != "Docente" && rol != "Secretaria") throw new ArgumentException("Selecciona un tipo de usuario válido.");
        string nombre = Validation.Name(data.First, "Nombre(s)",60);
        string ap = Validation.Name(data.Paternal, "Apellido paterno");
        string am = Validation.Name(data.Maternal, "Apellido materno");
        string mail = PersonalAutomatico.EmailBase(nombre, ap, am);
        string user = Validation.Mandatory(data.Username, "Usuario");
        string taxId = Validation.Mandatory(data.Rfc, "RFC").ToUpperInvariant();
        string ident = "";

        if (!Validation.InstitutionalEmail(mail))
            throw new ArgumentException("Usa un correo válido @acapulco.tecnm.mx o @alumnos.acapulco.tecnm.mx.");

        if (data.Phone.Trim().Length > 0 && !Validation.Phone(data.Phone))
            throw new ArgumentException("El teléfono debe tener 10 dígitos.");

        if (!Validation.Username(user))
            throw new ArgumentException("El usuario debe tener de 4 a 50 caracteres, comenzar con letra y contener letras, números o separadores . _ - entre segmentos.");

        if (!Validation.Rfc(taxId))
            throw new ArgumentException(
                "El RFC debe tener 13 caracteres: cuatro letras, una fecha real de nacimiento y homoclave válida en formato.");

        string curpValue = Validation.Mandatory(data.Curp, "CURP").ToUpperInvariant();
        if (!Validation.Curp(curpValue)) throw new ArgumentException("CURP inválida: revisa formato, entidad y fecha de nacimiento (desde 1926).");
        if (taxId.Substring(4, 6) != curpValue.Substring(4, 6)) throw new ArgumentException("Las fechas de nacimiento del RFC y la CURP no coinciden.");
        if (ident.Length > 20) throw new ArgumentException("El número administrativo admite hasta 20 caracteres.");

        if (data.Password != data.Confirmation)
            throw new ArgumentException("Las contraseñas no coinciden.");

        if (!Validation.Password(data.Password))
            throw new ArgumentException(
                "La contraseña requiere al menos 10 caracteres, letras y números.");


        using var c = Db.Connect();
        using var tx = c.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c, tx, actor, true);
        Identidad.Check(c, tx, actor, nombre, ap, am, curpValue, rfc: taxId);
        mail = PersonalAutomatico.AvailableEmail(c, tx, mail);
        if (rol == "Secretaria") ident = PersonalAutomatico.NextNumber(c, tx);
        int docenteId = 0;

        if (rol == "Docente")
        {
            using var cmd = Db.Sql(c, """
                INSERT INTO Docente(nombre,apellido_paterno,apellido_materno,correo_institucional,RFC,telefono,curp,estatus)
                VALUES (@nombre,@ap,@am,@mail,@rfc,@tel,@curp,'ACTIVO')
                """, tx);
            Db.Param(cmd, "@curp", curpValue);
            Db.Param(cmd, "@nombre", nombre);
            Db.Param(cmd, "@ap", ap);
            Db.Param(cmd, "@am", am);
            Db.Param(cmd, "@mail", mail);
            Db.Param(cmd, "@rfc", taxId);
            Db.Param(cmd, "@tel", string.IsNullOrWhiteSpace(data.Phone) ? null : data.Phone.Trim());
            cmd.ExecuteNonQuery();
            docenteId = Convert.ToInt32(cmd.LastInsertedId);
        }
        else
        {
            using var cmd = Db.Sql(c, """
                INSERT INTO Personal_Administrativo(num_administrativo,nombre,apellido_paterno,
                    apellido_materno,correo_institucional,RFC,telefono,curp,puesto,estatus)
                VALUES (@id,@nombre,@ap,@am,@mail,@rfc,@tel,@curp,'Secretaría','ACTIVO')
                """, tx);
            Db.Param(cmd, "@id", ident);
            Db.Param(cmd, "@curp", curpValue);
            Db.Param(cmd, "@nombre", nombre);
            Db.Param(cmd, "@rfc", taxId);
            Db.Param(cmd, "@ap", ap);
            Db.Param(cmd, "@am", am);
            Db.Param(cmd, "@mail", mail);
            Db.Param(cmd, "@tel", string.IsNullOrWhiteSpace(data.Phone) ? null : data.Phone.Trim());
            cmd.ExecuteNonQuery();
        }

        int roleId;
        using (var find = Db.Sql(c, "SELECT id_rol FROM Rol WHERE nombre=@nombre", tx))
        {
            Db.Param(find, "@nombre", rol);
            roleId = Convert.ToInt32(find.ExecuteScalar()
                ?? throw new InvalidOperationException("Falta precargar el rol en MySQL."));
        }

        using (var cmd = Db.Sql(c, """
            INSERT INTO Usuario(nombre_usuario,password_hash,Id_rol,estatus,ID_docente,num_administrativo,debe_cambiar_password)
            VALUES (@usuario,@hash,@rol,'ACTIVO',@doc,@admin,0)
            """, tx))
        {
            Db.Param(cmd, "@usuario", user);
            Db.Param(cmd, "@hash", PasswordHasher.Hash(data.Password));
            Db.Param(cmd, "@rol", roleId);
            Db.Param(cmd, "@doc", rol == "Docente" ? docenteId : null);
            Db.Param(cmd, "@admin", rol == "Secretaria" ? ident : null);
            cmd.ExecuteNonQuery();
        }

        Db.Audit(c, tx, actor.Id, "ALTA_USUARIO", "USUARIOS", $"Alta de {rol}: {user}", user);
        tx.Commit();
        return new PersonalCreated(mail, rol == "Secretaria" ? ident : $"DOC{docenteId:D6}");
    }

    public static DataTable List(Sesion actor, string search, string role, string status)
    {
        using var c = Db.Connect();
        Identidad.RequireActive(c, null, actor, true);
        using var cmd = Db.Sql(c, """
            SELECT u.Id_usuario AS Id,r.nombre AS Rol,u.nombre_usuario AS Usuario,
                CONCAT_WS(' ',COALESCE(d.nombre,p.nombre),COALESCE(d.apellido_paterno,p.apellido_paterno),
                    COALESCE(d.apellido_materno,p.apellido_materno)) AS 'Nombre completo',
                COALESCE(d.curp,p.curp) AS CURP,COALESCE(d.RFC,p.RFC) AS RFC,
                COALESCE(d.correo_institucional,p.correo_institucional) AS Correo,
                COALESCE(d.telefono,p.telefono) AS 'Teléfono',u.estatus AS Estatus
            FROM Usuario u JOIN Rol r ON r.id_rol=u.Id_rol
            LEFT JOIN Docente d ON d.ID_docente=u.ID_docente
            LEFT JOIN Personal_Administrativo p ON p.num_administrativo=u.num_administrativo
            WHERE r.nombre IN ('Docente','Secretaria') AND (@role='' OR r.nombre=@role)
                AND (@status='' OR UPPER(u.estatus)=@status)
                AND (@search='' OR u.nombre_usuario LIKE @like OR COALESCE(d.curp,p.curp) LIKE @like
                    OR COALESCE(d.RFC,p.RFC) LIKE @like OR
                    CONCAT_WS(' ',COALESCE(d.nombre,p.nombre),COALESCE(d.apellido_paterno,p.apellido_paterno),
                        COALESCE(d.apellido_materno,p.apellido_materno)) LIKE @like)
            ORDER BY r.nombre,`Nombre completo`
            """);
        Db.Param(cmd,"@role",role); Db.Param(cmd,"@status",status);
        Db.Param(cmd,"@search",search.Trim()); Db.Param(cmd,"@like","%"+search.Trim()+"%");
        using var reader = cmd.ExecuteReader();
        var table = new DataTable(); table.Load(reader); return table;
    }
    public static void Deactivate(Sesion actor, int userId)
    {
        using var c = Db.Connect(); using var tx = c.BeginTransaction();
        Identidad.RequireActive(c,tx,actor,true);
        if (userId == actor.Id) throw new ArgumentException("No puedes dar de baja tu propia cuenta.");
        int? teacher; string? secretary; string role;
        using (var find = Db.Sql(c, """
            SELECT u.ID_docente,u.num_administrativo,r.nombre FROM Usuario u JOIN Rol r ON r.id_rol=u.Id_rol
            WHERE u.Id_usuario=@id AND UPPER(u.estatus)='ACTIVO' FOR UPDATE
            """,tx))
        {
            Db.Param(find,"@id",userId); using var r=find.ExecuteReader();
            if (!r.Read()) throw new ArgumentException("La cuenta no existe o ya está inactiva.");
            teacher=r.IsDBNull(0)?null:r.GetInt32(0); secretary=r.IsDBNull(1)?null:r.GetString(1); role=r.GetString(2);
        }
        if (role != "Docente" && role != "Secretaria") throw new UnauthorizedAccessException("Sólo se pueden dar de baja docentes y secretarias.");
        using (var user=Db.Sql(c,"UPDATE Usuario SET estatus='INACTIVO' WHERE Id_usuario=@id",tx))
        { Db.Param(user,"@id",userId); user.ExecuteNonQuery(); }
        string sql=role=="Docente" ? "UPDATE Docente SET estatus='INACTIVO' WHERE ID_docente=@id"
            : "UPDATE Personal_Administrativo SET estatus='INACTIVO' WHERE num_administrativo=@id";
        using (var person=Db.Sql(c,sql,tx))
        {
            Db.Param(person,"@id",role=="Docente"?(object?)teacher:secretary);
            if (person.ExecuteNonQuery()!=1) throw new ArgumentException("La cuenta no tiene un registro de personal activo válido.");
        }
        Db.Audit(c,tx,actor.Id,"BAJA_PERSONAL","USUARIOS",$"Baja lógica de {role}",userId.ToString());
        tx.Commit();
    }
}

internal sealed record PersonalData(string Role, string First, string Paternal, string Maternal,
    string Email, string Username, string Rfc, string Curp, string Phone, string Number,
    string Password, string Confirmation);

internal sealed record PersonalCreated(string Email, string Number);
