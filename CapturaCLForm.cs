using System.Data;
using System.Globalization;
namespace AMBAR.Semana4;
public partial class CapturaCLForm : Form
{
    private Sesion actor=null!;
    private FiltrosDocente? filters;
    private int initial,loadedAssignment,loadedUnit,loadedWindow;
    private bool loading,dirty;
    private readonly Dictionary<string,decimal?> originals=new();
    public CapturaCLForm(){InitializeComponent();FormClosing+=CheckClosing;}
    internal CapturaCLForm(Sesion session,int assignment=0):this()
    {actor=session;initial=assignment;if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");}
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;}
        try{filters=new FiltrosDocente(actor,period,subject,group,AssignmentChanged,initial);AssignmentChanged();LoadRoster();}
        catch(Exception ex){Problems.Show(ex);}
    }
    private void AssignmentChanged()
    {
        if(filters is null)return;
        loading=true;unit.Items.Clear();window.Items.Clear();save.Enabled=false;grid.ReadOnly=true;
        try
        {
            if(filters.Selected is Asignacion a)
            {
                unit.Items.AddRange(RepositorioDocente.Units(actor,a.Id).Cast<object>().ToArray());
                window.Items.AddRange(RepositorioDocente.Windows(actor,a.Id).Cast<object>().ToArray());
                if(unit.Items.Count>0)unit.SelectedIndex=0;if(window.Items.Count>0)window.SelectedIndex=0;
            }
            note.Text="Selecciona unidad y captura; pulsa Cargar alumnos. Los cambios anteriores aún no se guardan.";
        }catch(Exception ex){Problems.Show(ex);}finally{loading=false;}
    }
    private void SelectionChanged(object? sender,EventArgs e){if(!loading){save.Enabled=false;grid.ReadOnly=true;note.Text="Pulsa Cargar alumnos para aplicar la selección.";}}
    private bool Discard()=>!dirty || MessageBox.Show("Hay cambios sin guardar. ¿Descartarlos?","Calificaciones",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes;
    private void LoadClick(object? sender,EventArgs e)=>LoadRoster();
    private void LoadRoster()
    {
        if(grid.IsCurrentCellInEditMode && !grid.EndEdit())return;
        if(!Discard())return;
        loading=true;save.Enabled=false;grid.DataSource=null;originals.Clear();dirty=false;
        try
        {
            if(filters?.Selected is not Asignacion a || unit.SelectedItem is not Choice u)
            {note.Text="No hay asignación o unidades disponibles. Solicita su configuración a Administración.";return;}
            var table=RepositorioDocente.Roster(actor,a.Id,u.Id);
            foreach(DataRow row in table.Rows)originals[(string)row["Control"]]=row.IsNull("Nota")?null:Convert.ToDecimal(row["Nota"]);
            grid.DataSource=table;
            foreach(DataGridViewColumn column in grid.Columns)column.ReadOnly=column.Name!="Nota";
            grid.Columns["Nota"].HeaderText="Calificación (0–100)";
            grid.Columns["Nota"].DefaultCellStyle.Format="0.##";
            grid.Columns["Alumno"].FillWeight=200;
            loadedAssignment=a.Id;loadedUnit=u.Id;loadedWindow=(window.SelectedItem as Choice)?.Id??0;
            save.Enabled=loadedWindow>0 && table.Rows.Count>0;
            grid.ReadOnly=!save.Enabled;
            note.Text=loadedWindow>0?"Deja vacías las notas aún no capturadas. Guardar registra los cambios en el historial.":"Sólo lectura: no hay un periodo de captura abierto para este grupo.";
            UpdateSummary();
        }catch(Exception ex){Problems.Show(ex);}finally{loading=false;}
    }
    private void DirtyChanged(object? sender,EventArgs e){if(grid.IsCurrentCellDirty)grid.CommitEdit(DataGridViewDataErrorContexts.Commit);}
    private void ValueChanged(object? sender,DataGridViewCellEventArgs e){if(!loading && e.RowIndex>=0){dirty=true;UpdateSummary();}}
    private void UpdateSummary()
    {
        var values=grid.Rows.Cast<DataGridViewRow>().Select(r=>r.Cells["Nota"].Value).Where(v=>v is not null && v!=DBNull.Value).Select(Convert.ToDecimal).ToList();
        summary.Text=$"Alumnos: {grid.Rows.Count} · Capturadas: {values.Count} · Promedio de esta unidad: "+(values.Count==0?"—":values.Average().ToString("0.##"));
    }
    private void SaveClick(object? sender,EventArgs e)
    {
        try
        {
            if(filters?.Selected?.Id!=loadedAssignment || (unit.SelectedItem as Choice)?.Id!=loadedUnit || (window.SelectedItem as Choice)?.Id!=loadedWindow)
                throw new ArgumentException("La selección cambió. Carga nuevamente los alumnos antes de guardar.");
            if(!grid.EndEdit())throw new ArgumentException("Corrige la calificación antes de guardar.");
            var changes=new List<NotaCambio>();
            foreach(DataGridViewRow row in grid.Rows)
            {
                string control=Convert.ToString(row.Cells["Control"].Value)!;
                object? raw=row.Cells["Nota"].Value;
                if(raw is null || raw==DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(raw)))
                {if(originals[control].HasValue)throw new ArgumentException("No se puede borrar una nota existente. Captura el valor corregido.");continue;}
                if(!decimal.TryParse(Convert.ToString(raw),NumberStyles.AllowDecimalPoint,CultureInfo.CurrentCulture,out decimal value))throw new ArgumentException("Escribe una calificación numérica válida.");
                if(originals[control]!=value)changes.Add(new(control,value,originals[control]));
            }
            save.Enabled=false;RepositorioDocente.Save(actor,loadedAssignment,loadedUnit,loadedWindow,changes);
            dirty=false;MessageBox.Show("Calificaciones guardadas correctamente.","AMBAR");LoadRoster();
        }catch(Exception ex){Problems.Show(ex);save.Enabled=loadedWindow>0;}
    }
    private void CheckClosing(object? sender,FormClosingEventArgs e)
    {
        if(grid.IsCurrentCellInEditMode && !grid.EndEdit()){e.Cancel=true;return;}
        if(!Discard())e.Cancel=true;
    }
    private void CloseClick(object? sender,EventArgs e)=>Close();
    private void GridError(object? sender,DataGridViewDataErrorEventArgs e){e.ThrowException=false;e.Cancel=true;MessageBox.Show("Escribe un número entre 0 y 100, con hasta dos decimales.","Calificación inválida");}
}
