using System.Data;
namespace AMBAR.Semana4;
public partial class DetalleCapturaForm : Form
{
    public DetalleCapturaForm(){InitializeComponent();}
    internal DetalleCapturaForm(string title,DataTable table):this()
    {
        subtitle.Text=title+(table.Rows.Count==0?" · Sin cambios registrados":"");grid.DataSource=table;
        if(grid.Columns.Contains("Fecha"))grid.Columns["Fecha"].DefaultCellStyle.Format="dd/MM/yyyy HH:mm";
        if(grid.Columns.Contains("Alumno"))grid.Columns["Alumno"].FillWeight=180;
    }
    private void CloseClick(object? sender,EventArgs e)=>Close();
}
