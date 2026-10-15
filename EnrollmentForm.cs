namespace AMBAR.Semana4;

public partial class EnrollmentForm : Form
{
    private Session actor = null!;
    private int aspirantId;
    private string verifiedFicha="";
    public EnrollmentForm() { InitializeComponent(); }
    internal EnrollmentForm(Session session) : this()
    {
        actor=session;
        if (!actor.CanSeeStudents) throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría y Administración.");
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (actor is null) { Close(); return; }
        try
        {
            foreach(var item in StudentRepository.Plans())plans.Items.Add(item);
            if(plans.Items.Count>0) plans.SelectedIndex=0;
            else groupNote.Text="No existen planes activos. Configúralos en MySQL antes de inscribir.";
        }
        catch(Exception ex){Problems.Show(ex);}
        LoadGroups();
    }
    private void FindClick(object? sender, EventArgs e) => Lookup();
    private void PlanChanged(object? sender, EventArgs e) => LoadGroups();
    private void CancelClick(object? sender, EventArgs e) => Close();

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
