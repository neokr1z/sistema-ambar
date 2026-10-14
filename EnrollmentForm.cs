namespace AMBAR.Semana4;

internal sealed class EnrollmentForm : Form
{
    private readonly Session actor;
    private readonly TextBox ficha=Theme.Input(20), name=Theme.Input(60), paternal=Theme.Input(50), maternal=Theme.Input(50);
    private readonly TextBox curp=Theme.Input(18), nss=Theme.Input(11), phone=Theme.Input(10), emergency=Theme.Input(10);
    private readonly TextBox personalMail=Theme.Input(120), institutionalMail=Theme.Input(120), allergies=Theme.Input(200);
    private readonly TextBox street=Theme.Input(100), exterior=Theme.Input(15), interior=Theme.Input(15), neighborhood=Theme.Input(100);
    private readonly TextBox locality=Theme.Input(100), municipality=Theme.Input(100), state=Theme.Input(100), postal=Theme.Input(5);
    private readonly DateTimePicker birthday=new() { Width=350,Format=DateTimePickerFormat.Short,Value=new DateTime(2005,1,1) };
    private readonly ComboBox plans=Theme.Combo(),groups=Theme.Combo();
    private readonly Label applicantStatus=Theme.Text("Busca una ficha para verificar la admisión.",10,false,Theme.Gray);
    private readonly Label groupNote=Theme.Text("El sistema ofrecerá grupos vigentes de primer semestre con cupo.",9,false,Theme.Gray);
    private readonly Button submit=Theme.Button("Guardar inscripción");
    private int aspirantId;
    private string verifiedFicha="";

    public EnrollmentForm(Session session)
    {
        actor=session;
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría y Administración.");
        Theme.Setup(this,"Inscripción de alumnos",1020,800);
        var tabs=new TabControl{Dock=DockStyle.Fill,Font=new Font("Segoe UI",10,FontStyle.Bold)};
        var first=Tab("01 · Datos personales y contacto");
        var second=Tab("02 · Domicilio");
        var third=Tab("03 · Inscripción");
        tabs.TabPages.AddRange(new[]{first,second,third});
        Controls.Add(tabs);
        var bar=new FlowLayoutPanel{Dock=DockStyle.Bottom,FlowDirection=FlowDirection.RightToLeft,Height=68,Padding=new Padding(12)};
        submit.Click+=Save;
        var cancel=Theme.Button("Cancelar",false);cancel.Click+=(_,_)=>Close();
        bar.Controls.Add(submit);bar.Controls.Add(cancel);
        Controls.Add(bar);
        Controls.Add(Theme.Heading("Alta e inscripción · nuevo ingreso"));

        var personal=Theme.Fields(915);first.Controls.Add(personal);
        Theme.Row(personal,"Número de ficha",ficha);
        var find=Theme.Button("Verificar ficha",false);
        Theme.Row(personal,"Buscar en aspirantes",find);
        find.Click+=(_,_)=>Lookup();
        Theme.Row(personal,"Estatus de admisión",applicantStatus);
        Theme.Row(personal,"Nombre(s)",name);
        Theme.Row(personal,"Apellido paterno",paternal);
        Theme.Row(personal,"Apellido materno",maternal);
        Theme.Row(personal,"CURP",curp);
        curp.ReadOnly=true;
        Theme.Row(personal,"Fecha de nacimiento",birthday);
        Theme.Row(personal,"Teléfono (opcional)",phone);
        Theme.Row(personal,"Teléfono de emergencia",emergency);
        Theme.Row(personal,"Correo personal (opcional)",personalMail);
        Theme.Row(personal,"Correo institucional",institutionalMail);
        Theme.Row(personal,"NSS (si ya fue verificado)",nss);
        Theme.Row(personal,"Alergias (opcional)",allergies);

        var address=Theme.Fields(915);second.Controls.Add(address);
        Theme.Row(address,"Calle",street);Theme.Row(address,"Número exterior",exterior);
        Theme.Row(address,"Número interior (opcional)",interior);
        Theme.Row(address,"Colonia",neighborhood);Theme.Row(address,"Localidad",locality);
        Theme.Row(address,"Municipio",municipality);Theme.Row(address,"Estado",state);
        Theme.Row(address,"Código postal",postal);

        var inscription=Theme.Fields(915);third.Controls.Add(inscription);
        Theme.Row(inscription,"Carrera / Plan de estudios",plans);
        Theme.Row(inscription,"Grupo disponible (semestre 1)",groups);
        Theme.Row(inscription,"Disponibilidad",groupNote);
        Theme.Row(inscription,"Número de control",Theme.Text("Asignado al guardar; formato temporal AAAA + ID de aspirante",10,false,Theme.Gray));
        Theme.Row(inscription,"Periodo escolar",Theme.Text("Determinado por el grupo vigente seleccionado",10,false,Theme.Gray));
        Theme.Row(inscription,"Estatus",Theme.Text("INSCRITO",10,true,Theme.Green));
        try
        {
            foreach(var item in StudentRepository.Plans())plans.Items.Add(item);
            if(plans.Items.Count>0) plans.SelectedIndex=0;
            else groupNote.Text="No existen planes activos. Configúralos en MySQL antes de inscribir.";
        }
        catch(Exception ex){Problems.Show(ex);}
        plans.SelectedIndexChanged+=(_,_)=>LoadGroups();
        LoadGroups();
    }

    private static TabPage Tab(string title)
        => new(title){BackColor=Theme.Light,AutoScroll=true,Padding=new Padding(8)};

    private void LoadGroups()
    {
        groups.Items.Clear();
        if(plans.SelectedItem is not Choice c)return;
        try
        {
            foreach(var item in StudentRepository.Groups(c.Id))groups.Items.Add(item);
            if(groups.Items.Count>0)groups.SelectedIndex=0;
            groupNote.Text=groups.Items.Count>0?"Cupo validado nuevamente al guardar.":"Sin grupos activos con cupo para primer semestre.";
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private void Lookup()
    {
        try
        {
            var f=Validation.Mandatory(ficha.Text,"número de ficha");
            var a=StudentRepository.Aspirant(f);
            aspirantId=0; verifiedFicha="";
            if(a is null)throw new ArgumentException("No existe un aspirante con esa ficha.");
            bool eligible=new[]{"APROBADO","ACEPTADO","ACREDITADO"}.Contains(a.Result.Trim().ToUpperInvariant())
                && new[]{"ACTIVO","ACEPTADO","APROBADO"}.Contains(a.Status.Trim().ToUpperInvariant());
            applicantStatus.Text=$"Resultado: {a.Result} | Estatus: {a.Status}";
            applicantStatus.ForeColor=eligible?Theme.Green:Color.DarkRed;
            if(!eligible)throw new ArgumentException("El aspirante aún no está aprobado/aceptado o ya fue inscrito.");
            aspirantId=a.Id; verifiedFicha=f;
            curp.Text=a.Curp;
            if(institutionalMail.TextLength==0)
                institutionalMail.Text=$"{DateTime.Today.Year}{a.Id:D6}@alumnos.acapulco.tecnm.mx";
            MessageBox.Show("Ficha aceptada. Completa los datos y verifica el correo propuesto antes de guardar.\nAMBAR no crea buzones de correo.","Ficha validada");
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private StudentData Collect()
    {
        var d=new StudentData {
            AspirantId=aspirantId,
            PlanId=(plans.SelectedItem as Choice)?.Id??0,
            GroupId=(groups.SelectedItem as Choice)?.Id??0,
            First=name.Text.Trim(),Paternal=paternal.Text.Trim(),Maternal=maternal.Text.Trim(),
            Curp=curp.Text.Trim().ToUpperInvariant(),Birth=birthday.Value.Date,
            Nss=nss.Text.Trim(),Phone=phone.Text.Trim(),Emergency=emergency.Text.Trim(),
            PersonalEmail=personalMail.Text.Trim(),Email=institutionalMail.Text.Trim(),Allergies=allergies.Text.Trim(),
            Street=street.Text.Trim(),Exterior=exterior.Text.Trim(),Interior=interior.Text.Trim(),
            Neighborhood=neighborhood.Text.Trim(),Locality=locality.Text.Trim(),Municipality=municipality.Text.Trim(),
            State=state.Text.Trim(),Postal=postal.Text.Trim()
        };
        return d;
    }

    private void Save(object? sender,EventArgs args)
    {
        try
        {
            submit.Enabled=false;
            if(!string.Equals(ficha.Text.Trim(),verifiedFicha,StringComparison.Ordinal))
                throw new ArgumentException("La ficha cambió después de validarla. Vuelve a verificarla.");
            var d=Collect();
            string control=StudentRepository.Enroll(d,actor);
            MessageBox.Show($"Inscripción realizada correctamente.\nNúmero de control: {control}\n\nPuede consultar el expediente y generar el PDF en 'Consulta de alumnos'.",
                "AMBAR · Inscripción exitosa",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Close();
        }
        catch(Exception ex){Problems.Show(ex);}
        finally{submit.Enabled=true;}
    }
}
