namespace AMBAR.Semana4;

public partial class GradesForm : Form
{
    private Session session = null!;
    public GradesForm() { InitializeComponent(); }
    internal GradesForm(Session actor) : this()
    {
        session=actor;
        if(!session.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
        subject.SelectedIndex=0;group.SelectedIndex=0;period.SelectedIndex=0;
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (session is null) { Close(); return; }
    }

    private void UnitClick(object? sender, EventArgs e)
    {
        foreach(var button in new[] {unit1,unit2,unit3,unit4}) {button.BackColor=Theme.Sky;button.ForeColor=Theme.Gray;}
        if(sender is Button selected){selected.BackColor=Theme.Gray;selected.ForeColor=Color.White;}
    }
    private void CloseClick(object? sender, EventArgs e) => Close();
}
