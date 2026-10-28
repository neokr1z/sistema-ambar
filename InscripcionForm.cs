namespace AMBAR.Semana4;

public partial class InscripcionForm : Form
{
    private Sesion actor = null!;
    private int aspirantId;
    private int? fichaPeriod;
    private string verifiedFicha = "";
    private List<RegistroPostal> coloniasCp = new();

    public InscripcionForm()
    {
        InitializeComponent();
        postal.TextChanged += PostalChanged;
        neighborhood.SelectedIndexChanged += ColoniaChanged;
        locality.ReadOnly = true;
        municipality.ReadOnly = true;
        state.ReadOnly = true;
        // El diseñador usa pestañas normales para que los nombres siempre se vean.
        // El estilo azul personalizado se activa al abrir una sesión real.

    }

    private void ActivarEstiloPasos()
    {
        tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabs.DrawItem -= Tabs_DrawItem;
        tabs.DrawItem += Tabs_DrawItem;
        tabs.Invalidate();
    }

    private void Tabs_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= tabs.TabCount) return;
        bool active = e.Index == tabs.SelectedIndex;
        Rectangle box = e.Bounds;
        box.Inflate(-4, -5);
        Color navy = Color.FromArgb(6, 55, 99);
        Color sky = Color.FromArgb(180, 213, 234);
        using (var fill = new SolidBrush(active ? navy : sky))
            e.Graphics.FillRectangle(fill, box);
        TextRenderer.DrawText(e.Graphics, tabs.TabPages[e.Index].Text,
            tabs.Font, box, active ? Color.White : navy,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    // Vuelve a la pantalla anterior sin realizar ninguna inscripción.
    private void MenuButton_Click(object? sender, EventArgs e) => Close();

    internal InscripcionForm(Sesion session) : this()
    {
        actor = session;
        if (!actor.CanSeeStudents) throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría y Administración.");
        Text = actor.IsAdmin ? "Administrador | Alta e inscripción" : "Secretaría | Alta e inscripción";
        ActivarEstiloPasos();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (actor is null) { Close(); return; }
        try
        {
            foreach (var item in RepositorioAlumnos.Plans()) plans.Items.Add(item);
            if (plans.Items.Count > 0) plans.SelectedIndex = 0;
            else groupNote.Text = "No existen planes activos. Configúralos en MySQL antes de inscribir.";
        }
        catch (Exception ex) { Problems.Show(ex); }
        LoadGroups();
    }
    private void FindClick(object? sender, EventArgs e) => Lookup();
    private void PlanChanged(object? sender, EventArgs e) => LoadGroups();
    private void CancelClick(object? sender, EventArgs e) => Close();

    private void LoadGroups()
    {
        groups.Items.Clear();
        if (plans.SelectedItem is not Choice c) return;
        try
        {
            foreach (var item in RepositorioAlumnos.Groups(c.Id, fichaPeriod)) groups.Items.Add(item);
            if (groups.Items.Count > 0) groups.SelectedIndex = 0;
            groupNote.Text = groups.Items.Count > 0 ? "Cupo validado nuevamente al guardar." : "Sin grupos activos con cupo para primer semestre.";
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void Lookup()
    {
        try
        {
            aspirantId = 0; verifiedFicha = ""; fichaPeriod = null; curp.Clear(); curp.ReadOnly = true;
            var f = Validation.Mandatory(ficha.Text, "número de ficha");
            var a = RepositorioAlumnos.Aspirant(f);
            aspirantId = 0; verifiedFicha = "";
            if (a is null) throw new ArgumentException("No existe un aspirante con esa ficha.");
            bool eligible = new[] { "APROBADO", "ACEPTADO", "ACREDITADO" }.Contains(a.Result.Trim().ToUpperInvariant())
                && new[] { "ACTIVO", "ACEPTADO", "APROBADO" }.Contains(a.Status.Trim().ToUpperInvariant());
            applicantStatus.Text = $"Resultado: {a.Result} | Estatus: {a.Status}";
            applicantStatus.ForeColor = eligible ? Theme.Green : Color.DarkRed;
            if (!eligible) throw new ArgumentException("El aspirante aún no está aprobado/aceptado o ya fue inscrito.");
            aspirantId = a.Id; verifiedFicha = f;
            curp.Text = a.Curp;
            curp.ReadOnly = a.Curp.Length > 0;
            fichaPeriod = a.PeriodId;
            LoadGroups();
            if (institutionalMail.TextLength == 0)
                institutionalMail.Text = $"{DateTime.Today.Year}{a.Id:D6}@alumnos.acapulco.tecnm.mx";
            MessageBox.Show("Ficha aceptada. Completa los datos y verifica el correo propuesto antes de guardar.\nAMBAR no crea buzones de correo.", "Ficha validada");
        }
        catch (Exception ex) { Problems.Show(ex); }
    }


    private void PostalChanged(object? sender, EventArgs e)
    {
        neighborhood.Items.Clear();
        coloniasCp.Clear();
        locality.Clear(); municipality.Clear(); state.Clear();
        if (!Validation.Postal(postal.Text)) return;
        try
        {
            coloniasCp = CatalogoPostal.Buscar(postal.Text.Trim());
            foreach (var item in coloniasCp)
                if (!neighborhood.Items.Contains(item.Colonia))
                    neighborhood.Items.Add(item.Colonia);
            if (neighborhood.Items.Count == 0)
                MessageBox.Show("No se encontró el CP en el catálogo postal cargado.", "Domicilio");
            else if (neighborhood.Items.Count == 1)
                neighborhood.SelectedIndex = 0;
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void ColoniaChanged(object? sender, EventArgs e)
    {
        if (neighborhood.SelectedItem is not string nombre) return;
        var info = coloniasCp.FirstOrDefault(x => x.Colonia == nombre);
        if (info is null) return;
        locality.Text = info.Localidad;
        municipality.Text = info.Municipio;
        state.Text = info.Estado;
    }

    private StudentData Collect()
    {
        var d = new StudentData
        {
            AspirantId = aspirantId,
            PlanId = (plans.SelectedItem as Choice)?.Id ?? 0,
            GroupId = (groups.SelectedItem as Choice)?.Id ?? 0,
            First = name.Text.Trim(),
            Paternal = paternal.Text.Trim(),
            Maternal = maternal.Text.Trim(),
            Curp = curp.Text.Trim().ToUpperInvariant(),
            Birth = birthday.Value.Date,
            Nss = nss.Text.Trim(),
            Phone = phone.Text.Trim(),
            Emergency = emergency.Text.Trim(),
            PersonalEmail = personalMail.Text.Trim(),
            Email = institutionalMail.Text.Trim(),
            Allergies = allergies.Text.Trim(),
            Street = street.Text.Trim(),
            Exterior = exterior.Text.Trim(),
            Interior = interior.Text.Trim(),
            Neighborhood = neighborhood.SelectedItem?.ToString() ?? "",
            Locality = locality.Text.Trim(),
            Municipality = municipality.Text.Trim(),
            State = state.Text.Trim(),
            Postal = postal.Text.Trim()
        };
        return d;
    }

    private void Save(object? sender, EventArgs args)
    {
        try
        {
            submit.Enabled = false;
            if (!string.Equals(ficha.Text.Trim(), verifiedFicha, StringComparison.Ordinal))
                throw new ArgumentException("La ficha cambió después de validarla. Vuelve a verificarla.");
            var d = Collect();
            string control = RepositorioAlumnos.Enroll(d, actor);
            MessageBox.Show($"Inscripción realizada correctamente.\nNúmero de control: {control}\n\nPuede consultar el expediente y generar el PDF en 'Consulta de alumnos'.",
                "AMBAR · Inscripción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        catch (Exception ex) { Problems.Show(ex); }
        finally { submit.Enabled = true; }
    }

    private void logo_Click(object sender, EventArgs e)
    {

    }
}


