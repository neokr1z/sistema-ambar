namespace AMBAR.Semana4;
public partial class MateriasDocenteForm : Form
{
    private Sesion actor=null!;
    public MateriasDocenteForm(){InitializeComponent();}
    internal MateriasDocenteForm(Sesion session):this(){actor=session;if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");}
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;}
        try{assignment.Items.AddRange(RepositorioDocente.Assignments(actor).Cast<object>().ToArray());
            if(assignment.Items.Count>0)assignment.SelectedIndex=0;else note.Text="No tienes asignaciones. Administración debe registrar tus materias y grupos.";}
        catch(Exception ex){Problems.Show(ex);}capture.Enabled=assignment.SelectedItem is Asignacion;
    }
    private void SelectionChanged(object? sender,EventArgs e)
    {
        grid.DataSource=null;schedule.DataSource=null;capture.Enabled=false;
        if(assignment.SelectedItem is not Asignacion a)return;
        try
        {
            grid.DataSource=RepositorioDocente.Roster(actor,a.Id);grid.Columns["Nota"].Visible=false;
            schedule.DataSource=RepositorioDocente.Schedule(actor,a.Id);
            note.Text=$"{grid.Rows.Count} alumnos inscritos. "+(schedule.Rows.Count==0?"Sin horario operativo registrado.":"");capture.Enabled=true;
        }catch(Exception ex){Problems.Show(ex);}
    }
    private void CaptureClick(object? sender,EventArgs e){if(assignment.SelectedItem is Asignacion a){using var form=new CapturaCLForm(actor,a.Id);form.ShowDialog(this);}}
    private void ListClick(object? sender,EventArgs e)
    {
        if(assignment.SelectedItem is not Asignacion a){MessageBox.Show("Selecciona una materia y grupo.");return;}
        using var form=new ListaGrupoForm(actor,a.GrupoId,a.Id);form.ShowDialog(this);
    }
    private void CloseClick(object? sender,EventArgs e)=>Close();
}
