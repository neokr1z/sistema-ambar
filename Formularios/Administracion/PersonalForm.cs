namespace AMBAR.Semana4;

public partial class PersonalForm : Form
{
    private Sesion actor = null!;
    public PersonalForm() { InitializeComponent(); }
    internal PersonalForm(Sesion session) : this()
    {
        if (!session.IsAdmin) throw new UnauthorizedAccessException("Acceso exclusivo de Administración.");
        actor = session;
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (actor is null) { Close(); return; }
        roleFilter.SelectedIndex = 0;
        statusFilter.SelectedIndex = 0;
        Reload();
    }
    private void SearchClick(object? sender, EventArgs e) => Reload();
    private void CloseClick(object? sender, EventArgs e) => Close();
    private void Reload()
    {
        if (actor is null) return;
        try
        {
            grid.DataSource = RepositorioPersonal.List(actor, search.Text,
                roleFilter.SelectedIndex == 0 ? "" : roleFilter.Text,
                statusFilter.SelectedIndex == 0 ? "" : statusFilter.Text);
            grid.Columns["Id"].Visible = false;
            resultLabel.Text = $"{grid.Rows.Count} registros. La baja conserva el historial y bloquea el acceso.";
        }
        catch (Exception ex) { Problems.Show(ex); }
    }
    private void DeactivateClick(object? sender, EventArgs e)
    {
        if (grid.CurrentRow is null) return;
        try
        {
            int id = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
            string name = Convert.ToString(grid.CurrentRow.Cells["Nombre completo"].Value) ?? "";
            if (MessageBox.Show($"¿Dar de baja a {name}? Se desactivará su acceso y se conservará su historial.",
                "Baja de personal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            RepositorioPersonal.Deactivate(actor, id);
            Reload();
        }
        catch (Exception ex) { Problems.Show(ex); }
    }
}
