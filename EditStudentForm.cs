using System.ComponentModel;

namespace AMBAR.Semana4;

public partial class EditStudentForm : Form
{
    private Session actor = null!;
    private StudentData data = null!;
    private readonly Dictionary<string, TextBox> fields = new();

    public EditStudentForm()
    {
        InitializeComponent();
        // La imagen del encabezado se obtiene al ejecutar el formulario.
        // No se asigna en Designer.cs para conservar su compatibilidad.
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            CargarLogo();
    }

    private void CargarLogo()
    {
        try
        {
            var resources = new ComponentResourceManager(typeof(MainForm));
            logo.Image = resources.GetObject("logo.Image") as Image;
        }
        catch (System.Resources.MissingManifestResourceException)
        {
            // Si el recurso no está disponible, se conserva el formulario funcional.
        }
    }

    internal EditStudentForm(Session session, StudentData student) : this()
    {
        actor = session;
        data = student;
        if (!actor.CanSeeStudents)
            throw new UnauthorizedAccessException("Acceso no autorizado.");

        Text = actor.IsAdmin ? "Administrador | Modificar Alumno"
                             : "Secretaría | Modificar Alumno";
        controlValue.Text = data.Control;
        heading.Text = $"MODIFICAR EXPEDIENTE · {data.Control}";
        birth.Value = data.Birth;

        fields["First"] = inputFirst; inputFirst.Text = data.First;
        fields["Paternal"] = inputPaternal; inputPaternal.Text = data.Paternal;
        fields["Maternal"] = inputMaternal; inputMaternal.Text = data.Maternal;
        fields["Curp"] = inputCurp; inputCurp.Text = data.Curp;
        fields["Nss"] = inputNss; inputNss.Text = data.Nss;
        fields["Phone"] = inputPhone; inputPhone.Text = data.Phone;
        fields["Emergency"] = inputEmergency; inputEmergency.Text = data.Emergency;
        fields["Allergies"] = inputAllergies; inputAllergies.Text = data.Allergies;
        fields["Email"] = inputEmail; inputEmail.Text = data.Email;
        fields["PersonalEmail"] = inputPersonalEmail; inputPersonalEmail.Text = data.PersonalEmail;
        fields["Street"] = inputStreet; inputStreet.Text = data.Street;
        fields["Exterior"] = inputExterior; inputExterior.Text = data.Exterior;
        fields["Interior"] = inputInterior; inputInterior.Text = data.Interior;
        fields["Neighborhood"] = inputNeighborhood; inputNeighborhood.Text = data.Neighborhood;
        fields["Locality"] = inputLocality; inputLocality.Text = data.Locality;
        fields["Municipality"] = inputMunicipality; inputMunicipality.Text = data.Municipality;
        fields["State"] = inputState; inputState.Text = data.State;
        fields["Postal"] = inputPostal; inputPostal.Text = data.Postal;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        if (actor is null) { Close(); return; }
    }

    private void CancelClick(object? sender, EventArgs e) => Close();

    private string F(string key) => fields[key].Text.Trim();

    private void Save(object? sender, EventArgs e)
    {
        try
        {
            data.First = F("First"); data.Paternal = F("Paternal"); data.Maternal = F("Maternal");
            data.Curp = F("Curp").ToUpperInvariant(); data.Birth = birth.Value.Date;
            data.Nss = F("Nss"); data.Phone = F("Phone"); data.Emergency = F("Emergency");
            data.Allergies = F("Allergies"); data.Email = F("Email"); data.PersonalEmail = F("PersonalEmail");
            data.Street = F("Street"); data.Exterior = F("Exterior"); data.Interior = F("Interior");
            data.Neighborhood = F("Neighborhood"); data.Locality = F("Locality");
            data.Municipality = F("Municipality"); data.State = F("State"); data.Postal = F("Postal");
            save.Enabled = false;
            StudentRepository.Update(data, actor);
            MessageBox.Show("Los datos se actualizaron correctamente.", "AMBAR");
            Close();
        }
        catch (Exception ex) { Problems.Show(ex); }
        finally { save.Enabled = true; }
    }
}




