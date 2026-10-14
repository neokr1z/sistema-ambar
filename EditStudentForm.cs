namespace AMBAR.Semana4;

internal sealed class EditStudentForm : Form
{
    private readonly Session actor;
    private readonly StudentData data;
    private readonly Dictionary<string,TextBox> fields=new();
    private readonly DateTimePicker birth=new(){Width=350,Format=DateTimePickerFormat.Short};
    private readonly Button save=Theme.Button("Guardar cambios");

    public EditStudentForm(Session session, StudentData student)
    {
        actor=session;
        data=student;
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        Theme.Setup(this,"Modificar alumno",990,790);
        var area=new Panel {Dock=DockStyle.Fill,AutoScroll=true,Padding=new Padding(15)};
        Controls.Add(area);
        var entries=Theme.Fields(900);
        area.Controls.Add(entries);
        Theme.Row(entries,"Número de control (no editable)",Theme.Text(data.Control,11,true,Theme.Blue));
        Add(entries,"Nombre(s)","First",data.First,60);
        Add(entries,"Apellido paterno","Paternal",data.Paternal,50);
        Add(entries,"Apellido materno","Maternal",data.Maternal,50);
        Add(entries,"CURP","Curp",data.Curp,18);
        birth.Value=data.Birth;
        Theme.Row(entries,"Fecha de nacimiento",birth);
        Add(entries,"NSS (opcional)","Nss",data.Nss,11);
        Add(entries,"Teléfono","Phone",data.Phone,10);
        Add(entries,"Teléfono de emergencia","Emergency",data.Emergency,10);
        Add(entries,"Alergias","Allergies",data.Allergies,200);
        Add(entries,"Correo institucional","Email",data.Email,120);
        Add(entries,"Correo personal","PersonalEmail",data.PersonalEmail,120);
        Add(entries,"Calle","Street",data.Street,100);
        Add(entries,"Número exterior","Exterior",data.Exterior,15);
        Add(entries,"Número interior","Interior",data.Interior,15);
        Add(entries,"Colonia","Neighborhood",data.Neighborhood,100);
        Add(entries,"Localidad","Locality",data.Locality,100);
        Add(entries,"Municipio","Municipality",data.Municipality,100);
        Add(entries,"Estado","State",data.State,100);
        Add(entries,"Código postal","Postal",data.Postal,5);
        var panel=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=72,FlowDirection=FlowDirection.RightToLeft,Padding=new Padding(12)};
        panel.Controls.Add(save);
        Controls.Add(panel);
        Controls.Add(Theme.Heading($"Modificar expediente · {data.Control}"));
        save.Click+=Save;
    }

    private void Add(TableLayoutPanel table,string label,string key,string value,int max)
    {
        var input=Theme.Input(max);input.Text=value;
        fields[key]=input;
        Theme.Row(table,label,input);
    }

    private string F(string key)=>fields[key].Text.Trim();

    private void Save(object? sender,EventArgs e)
    {
        try
        {
            data.First=F("First");data.Paternal=F("Paternal");data.Maternal=F("Maternal");
            data.Curp=F("Curp").ToUpperInvariant();data.Birth=birth.Value.Date;
            data.Nss=F("Nss");data.Phone=F("Phone");data.Emergency=F("Emergency");
            data.Allergies=F("Allergies");data.Email=F("Email");data.PersonalEmail=F("PersonalEmail");
            data.Street=F("Street");data.Exterior=F("Exterior");data.Interior=F("Interior");
            data.Neighborhood=F("Neighborhood");data.Locality=F("Locality");
            data.Municipality=F("Municipality");data.State=F("State");data.Postal=F("Postal");
            save.Enabled=false;
            StudentRepository.Update(data,actor);
            MessageBox.Show("Los datos se actualizaron correctamente.","AMBAR");
            Close();
        }
        catch(Exception ex){Problems.Show(ex);}
        finally{save.Enabled=true;}
    }
}
