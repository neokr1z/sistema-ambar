namespace AMBAR.Semana4;

public partial class UsuariosForm : Form
{
    private Sesion admin = null!;

    public UsuariosForm()
    {
        InitializeComponent();
        Text = "Administrador | Alta de personal";
        cancel.Click += (_, _) => Close();
    }

    internal UsuariosForm(Sesion session) : this()
    {
        admin = session;
        if (!admin.IsAdmin)
            throw new UnauthorizedAccessException("Acceso restringido al Administrador.");

        birthInput.MaxDate = DateTime.Today.AddDays(-1);
        role.SelectedIndex = 0;
        first.TextChanged += PreviewEmail;
        paternal.TextChanged += PreviewEmail;
        maternal.TextChanged += PreviewEmail;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode ||
            System.ComponentModel.LicenseManager.UsageMode ==
            System.ComponentModel.LicenseUsageMode.Designtime)
            return;

        if (admin is null)
        {
            Close();
            return;
        }
    }

    // Mantiene visibles los botones en el Diseñador y durante la ejecución.
    private void ActionsResize(object? sender, EventArgs e)
    {
        const int margen = 46;
        const int espacio = 18;
        save.Location = new Point(Math.Max(0, actions.ClientSize.Width - save.Width - margen), 16);
        cancel.Location = new Point(Math.Max(0, save.Left - espacio - cancel.Width), 16);
    }

    // Mantiene el título dentro del encabezado.
    private void HeaderResize(object? sender, EventArgs e)
    {
        heading.Left = Math.Max(310, header.ClientSize.Width - heading.Width - 35);
    }

    private void RoleChanged(object? sender, EventArgs e)
    {
        idNumber.Enabled = true;
        idNumber.Text = "Se asigna al guardar";
    }

    private void PreviewEmail(object? sender, EventArgs e)
    {
        try { email.Text = PersonalAutomatico.EmailBase(first.Text, paternal.Text, maternal.Text); }
        catch (ArgumentException) { email.Clear(); }
    }

    private bool saving;
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if(saving) e.Cancel=true;
    }

    private async void Save(object? sender, EventArgs e)
    {
        if(saving)return;
        string buttonText=save.Text;
        void RestoreSave()
        {
            saving=false;
            if(IsDisposed)return;
            save.Text=buttonText;
            save.Enabled=true;
            body.Enabled=true;
            cancel.Enabled=true;
            UseWaitCursor=false;
        }
        saving=true;
        body.Enabled=false;
        cancel.Enabled=false;
        UseWaitCursor=true;
        save.Text="Validando…";
        try
        {
            save.Enabled = false;
            var created = await RepositorioPersonal.CreateAsync(admin, new PersonalData(
                role.SelectedItem as string ?? "", first.Text, paternal.Text, maternal.Text,
                email.Text, username.Text, rfc.Text, curpInput.Text, phone.Text, idNumber.Text,
                pwd.Text, confirm.Text, birthInput.Checked ? birthInput.Value.Date : null));

            MessageBox.Show(
                $"Personal y usuario creados correctamente.\nCorreo: {created.Email}\nNúmero de personal: {created.Number}\nEl correo generado no crea un buzón real.",
                "AMBAR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            saving=false;
            Close();
        }
        catch (Exception ex)
        {
            Problems.Show(ex);
        }
        finally
        {
            RestoreSave();
        }
    }

    private void logo_Click(object sender, EventArgs e)
    {

    }
}

