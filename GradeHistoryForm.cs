namespace AMBAR.Semana4;

public partial class GradeHistoryForm : Form
{
    private Session session = null!;
    public GradeHistoryForm() { InitializeComponent(); }
    internal GradeHistoryForm(Session actor) : this()
    {
        session=actor;
        if(!session.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
        subject.SelectedIndex=0;group.SelectedIndex=0;period.SelectedIndex=0;
        unit.SelectedIndex=0;
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (session is null) { Close(); return; }
    }

    private void CloseClick(object? sender, EventArgs e) => Close();
}
