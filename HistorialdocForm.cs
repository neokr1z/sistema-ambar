using System.Data;
namespace AMBAR.Semana4;
public partial class HistorialdocForm : Form
{
    private Sesion actor=null!;
    private FiltrosDocente? filters;
    private DataTable? result;
    private bool loading;
    private int queriedAssignment,queriedUnit;
    private string queriedTitle="";
    public HistorialdocForm(){InitializeComponent();}
    internal HistorialdocForm(Sesion session):this(){actor=session;if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");}
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;}
        try{filters=new FiltrosDocente(actor,period,subject,group,AssignmentChanged);AssignmentChanged();SearchClick(null,EventArgs.Empty);}
        catch(Exception ex){Problems.Show(ex);}
    }
    private void Clear(){grid.DataSource=null;result=null;detail.Enabled=false;export.Enabled=false;summary.Text="Selecciona filtros y pulsa Consultar.";}
    private void AssignmentChanged()
    {
        if(filters is null)return;loading=true;Clear();unit.Items.Clear();unit.Items.Add(new Choice(0,"Todas las unidades"));
        try{if(filters.Selected is Asignacion a)unit.Items.AddRange(RepositorioDocente.Units(actor,a.Id,false).Cast<object>().ToArray());unit.SelectedIndex=0;}
        catch(Exception ex){Problems.Show(ex);}finally{loading=false;}
    }
    private void UnitChanged(object? sender,EventArgs e){if(!loading)Clear();}
    private void SearchClick(object? sender,EventArgs e)
    {
        Clear();if(filters?.Selected is not Asignacion a){summary.Text="No tienes asignaciones registradas.";return;}
        try
        {
            int u=(unit.SelectedItem as Choice)?.Id??0;result=RepositorioDocente.History(actor,a.Id,u);
            queriedAssignment=a.Id;queriedUnit=u;queriedTitle=a.ToString();grid.DataSource=result;
            grid.Columns["Actualización"].DefaultCellStyle.Format="dd/MM/yyyy HH:mm";grid.Columns["Calificación"].DefaultCellStyle.Format="0.##";grid.Columns["Alumno"].FillWeight=200;
            var values=result.Rows.Cast<DataRow>().Select(r=>Convert.ToDecimal(r["Calificación"])).ToArray();
            summary.Text=$"Calificaciones: {values.Length} · Calificaciones ≥70: {values.Count(x=>x>=70)} · Promedio del filtro: "+(values.Length==0?"—":values.Average().ToString("0.##"));
            detail.Enabled=true;export.Enabled=values.Length>0;
        }catch(Exception ex){Problems.Show(ex);}
    }
    private void DetailClick(object? sender,EventArgs e)
    {
        try{using var form=new DetalleCapturaForm(queriedTitle,RepositorioDocente.Changes(actor,queriedAssignment,queriedUnit));form.ShowDialog(this);}
        catch(Exception ex){Problems.Show(ex);}
    }
    private void ExportClick(object? sender,EventArgs e)
    {
        if(result is null)return;
        try
        {
            // Verifica nuevamente propiedad y estado de la cuenta antes de exportar.
            var table=RepositorioDocente.History(actor,queriedAssignment,queriedUnit);
            using var dialog=new SaveFileDialog{Filter="PDF (*.pdf)|*.pdf",FileName="Calificaciones.pdf"};
            if(dialog.ShowDialog(this)!=DialogResult.OK)return;
            ReporteDocentePdf.Create(queriedTitle,table,dialog.FileName);MessageBox.Show("PDF guardado.","AMBAR");
        }catch(Exception ex){Problems.Show(ex);}
    }
    private void CloseClick(object? sender,EventArgs e)=>Close();
}
