namespace AMBAR.Semana4;

public partial class DetailForm : Form
{
    private Session actor = null!;
    private StudentData data = null!;
    public DetailForm() { InitializeComponent(); }
    internal DetailForm(Session session, StudentData d) : this()
    {
        actor=session; data=d;
        identityValue.Text=$"Nombre: {d.FullName}\nNúmero de control: {d.Control}\nCURP: {d.Curp}\nFecha de nacimiento: {d.Birth:dd/MM/yyyy}\nEstatus: {d.Status}";
        enrollmentValue.Text=$"Ficha: {d.ApplicantNumber}\nCarrera: {d.Career}\nPlan: {d.Plan}\nGrupo: {d.Group}\nPeriodo: {d.Period}\nFecha de inscripción: {d.EnrollmentDate:dd/MM/yyyy}";
        contactValue.Text=$"Correo institucional: {d.Email}\nCorreo personal: {d.PersonalEmail}\nTeléfono: {d.Phone}\nEmergencia: {d.Emergency}\nNSS: {d.Nss}\nAlergias: {d.Allergies}";
        addressValue.Text=$"{d.Street}, núm. {d.Exterior} {d.Interior}\nCol. {d.Neighborhood}, {d.Locality}\n{d.Municipality}, {d.State}, C.P. {d.Postal}";
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (actor is null) { Close(); return; }
    }

    private void ExportClick(object? sender, EventArgs e)
    {
        try
        {
            using var dlg=new SaveFileDialog {Filter="PDF (*.pdf)|*.pdf", FileName=$"AMBAR_Expediente_{data.Control}.pdf"};
            if(dlg.ShowDialog(this)==DialogResult.OK) {PdfService.Create(data,dlg.FileName);MessageBox.Show("PDF generado.");}
        }
        catch(Exception ex) {Problems.Show(ex);}
    }
}
