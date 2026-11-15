namespace AMBAR.Semana4;
public partial class ListaGrupoForm : Form
{
    private Sesion actor=null!;
    private int grupo;
    private int? asignacion;
    private ListaGrupo? lista;
    public ListaGrupoForm(){InitializeComponent();}
    internal ListaGrupoForm(Sesion actor,int grupo,int? asignacion=null):this()
    {this.actor=actor;this.grupo=grupo;this.asignacion=asignacion;}
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;} RefreshList();
    }
    private void RefreshList()
    {
        export.Enabled=false;lista=null;grid.DataSource=null;
        try
        {
            lista=RepositorioListas.Obtener(actor,grupo,asignacion);
            grid.DataSource=lista.Alumnos;
            grid.Columns["Control"].HeaderText="No. control";grid.Columns["Control"].FillWeight=25;
            grid.Columns["Alumno"].FillWeight=60;grid.Columns["Estatus"].FillWeight=15;
            summary.Text=$"{lista.Grupo} · {lista.Periodo} · {lista.Alumnos.Rows.Count} alumnos inscritos";
            export.Enabled=true;
        }
        catch(Exception ex){summary.Text="No fue posible cargar la lista. Puedes volver a intentar.";Problems.Show(ex);}
    }
    private void RefreshClick(object? sender,EventArgs e)=>RefreshList();
    private void CloseClick(object? sender,EventArgs e)=>Close();
    private void ExportClick(object? sender,EventArgs e)
    {
        try
        {
            // Revalida permisos y consulta los datos actuales antes de exportar.
            RefreshList();if(lista is null)return;
            using var dlg=new SaveFileDialog{Filter="PDF (*.pdf)|*.pdf",FileName=$"AMBAR_Lista_Grupo_{grupo}.pdf"};
            if(dlg.ShowDialog(this)!=DialogResult.OK)return;
            ListaGrupoPdf.Create(lista,dlg.FileName);MessageBox.Show(this,"Lista guardada correctamente.","AMBAR");
        }catch(Exception ex){Problems.Show(ex);}
    }
}
