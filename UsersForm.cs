namespace AMBAR.Semana4;

internal sealed class UsersForm : Form
{
    private readonly Session admin;
    private readonly ComboBox role=Theme.Combo();
    private readonly TextBox first=Theme.Input(60), paternal=Theme.Input(50), maternal=Theme.Input(50);
    private readonly TextBox email=Theme.Input(120), phone=Theme.Input(20), idNumber=Theme.Input(20);
    private readonly TextBox username=Theme.Input(50), pwd=Theme.Input(150,true), confirm=Theme.Input(150,true);
    private readonly Label idHint=Theme.Text("RFC del docente (13 caracteres)",9,false,Theme.Gray);
    private readonly Button save=Theme.Button("Crear usuario");

    public UsersForm(Session session)
    {
        admin=session;
        if(!admin.IsAdmin) throw new UnauthorizedAccessException("Acceso restringido al Administrador.");
        Theme.Setup(this,"Alta de personal",970,820);
        var body=new Panel {Dock=DockStyle.Fill,AutoScroll=true,Padding=new Padding(24)};
        Controls.Add(body);
        var fields=Theme.Fields(890);
        body.Controls.Add(fields);
        role.Items.AddRange(new object[] { "Docente", "Secretaria" });
        role.SelectedIndex=0;
        Theme.Row(fields,"Tipo de usuario",role);
        Theme.Row(fields,"Nombre(s)",first);
        Theme.Row(fields,"Apellido paterno",paternal);
        Theme.Row(fields,"Apellido materno",maternal);
        Theme.Row(fields,"Correo institucional",email);
        Theme.Row(fields,"Teléfono (10 dígitos, opcional)",phone);
        Theme.Row(fields,"RFC / Número administrativo",idNumber);
        Theme.Row(fields,"Usuario para iniciar sesión",username);
        Theme.Row(fields,"Contraseña",pwd);
        Theme.Row(fields,"Confirmar contraseña",confirm);
        Theme.Row(fields,"Formato de la clave",idHint);
        var actions=new FlowLayoutPanel {Dock=DockStyle.Bottom,Height=72,FlowDirection=FlowDirection.RightToLeft,Padding=new Padding(18)};
        actions.Controls.Add(save);
        Controls.Add(actions);
        Controls.Add(Theme.Heading("Alta de docente o secretaria"));
        role.SelectedIndexChanged += (_,_)=> idHint.Text=role.SelectedIndex==0
            ? "Para docentes: RFC (13 caracteres)." : "Para secretaría: número de empleado administrativo.";
        save.Click+=Save;
    }

    private void Save(object? sender,EventArgs e)
    {
        try
        {
            string rol=(string)role.SelectedItem!;
            string nombre=Validation.Mandatory(first.Text,"Nombre(s)");
            string ap=Validation.Mandatory(paternal.Text,"Apellido paterno");
            string am=Validation.Mandatory(maternal.Text,"Apellido materno");
            string mail=Validation.Mandatory(email.Text,"Correo institucional");
            string user=Validation.Mandatory(username.Text,"Usuario");
            string ident=Validation.Mandatory(idNumber.Text,"RFC / Número administrativo").ToUpperInvariant();
            if(!Validation.Email(mail)) throw new ArgumentException("Escribe un correo institucional válido.");
            if(phone.Text.Trim().Length>0 && !Validation.Phone(phone.Text))
                throw new ArgumentException("El teléfono debe tener 10 dígitos.");
            if(user.Length<4) throw new ArgumentException("El usuario debe tener al menos 4 caracteres.");
            if(rol=="Docente" && !Validation.Rfc(ident))
                throw new ArgumentException("El RFC del docente debe tener 13 caracteres y el formato esperado.");
            if(pwd.Text!=confirm.Text) throw new ArgumentException("Las contraseñas no coinciden.");
            if(!Validation.Password(pwd.Text))
                throw new ArgumentException("La contraseña requiere al menos 10 caracteres, letras y números.");

            save.Enabled=false;
            using var c=Db.Connect();
            using var tx=c.BeginTransaction();
            int docenteId=0;
            if(rol=="Docente")
            {
                using var cmd=Db.Sql(c,"""
                    INSERT INTO Docente(nombre,apellido_paterno,apellido_materno,correo_institucional,RFC,telefono,estatus)
                    VALUES (@nombre,@ap,@am,@mail,@rfc,@tel,'ACTIVO')
                    """,tx);
                Db.Param(cmd,"@nombre",nombre); Db.Param(cmd,"@ap",ap); Db.Param(cmd,"@am",am);
                Db.Param(cmd,"@mail",mail); Db.Param(cmd,"@rfc",ident);
                Db.Param(cmd,"@tel",string.IsNullOrWhiteSpace(phone.Text)?null:phone.Text.Trim());
                cmd.ExecuteNonQuery();
                docenteId=Convert.ToInt32(cmd.LastInsertedId);
            }
            else
            {
                using var cmd=Db.Sql(c,"""
                    INSERT INTO Personal_Administrativo(num_administrativo,nombre,apellido_paterno,
                        apellido_materno,correo_institucional,telefono,puesto,estatus)
                    VALUES (@id,@nombre,@ap,@am,@mail,@tel,'Secretaría','ACTIVO')
                    """,tx);
                Db.Param(cmd,"@id",ident);Db.Param(cmd,"@nombre",nombre);
                Db.Param(cmd,"@ap",ap);Db.Param(cmd,"@am",am);Db.Param(cmd,"@mail",mail);
                Db.Param(cmd,"@tel",string.IsNullOrWhiteSpace(phone.Text)?null:phone.Text.Trim());
                cmd.ExecuteNonQuery();
            }
            int roleId;
            using(var find=Db.Sql(c,"SELECT id_rol FROM Rol WHERE nombre=@nombre",tx))
            {
                Db.Param(find,"@nombre",rol);
                roleId=Convert.ToInt32(find.ExecuteScalar()
                    ??throw new InvalidOperationException("Falta precargar el rol en MySQL."));
            }
            using(var cmd=Db.Sql(c,"""
                INSERT INTO Usuario(nombre_usuario,password_hash,Id_rol,estatus,ID_docente,num_administrativo,debe_cambiar_password)
                VALUES (@usuario,@hash,@rol,'ACTIVO',@doc,@admin,0)
                """,tx))
            {
                Db.Param(cmd,"@usuario",user);Db.Param(cmd,"@hash",PasswordHasher.Hash(pwd.Text));
                Db.Param(cmd,"@rol",roleId);
                Db.Param(cmd,"@doc",rol=="Docente"?docenteId:null);
                Db.Param(cmd,"@admin",rol=="Secretaria"?ident:null);
                cmd.ExecuteNonQuery();
            }
            Db.Audit(c,tx,admin.Id,"ALTA_USUARIO","USUARIOS",$"Alta de {rol}: {user}",user);
            tx.Commit();
            MessageBox.Show("Personal y usuario creados correctamente.","AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Close();
        }
        catch(Exception ex){Problems.Show(ex);}
        finally{save.Enabled=true;}
    }
}
