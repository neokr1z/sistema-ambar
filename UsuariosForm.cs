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

        role.SelectedIndex = 0;
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
        idNumber.Enabled = role.SelectedIndex == 1;
        if (!idNumber.Enabled)
            idNumber.Clear();
    }

    private void Save(object? sender, EventArgs e)
    {
        try
        {
            save.Enabled = false;
            RepositorioPersonal.Create(admin, new PersonalData(
                role.SelectedItem as string ?? "", first.Text, paternal.Text, maternal.Text,
                email.Text, username.Text, rfc.Text, curpInput.Text, phone.Text, idNumber.Text,
                pwd.Text, confirm.Text));

            MessageBox.Show(
                "Personal y usuario creados correctamente.",
                "AMBAR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }
        catch (Exception ex)
        {
            Problems.Show(ex);
        }
        finally
        {
            save.Enabled = true;
        }
    }

    private void logo_Click(object sender, EventArgs e)
    {

    }
}

