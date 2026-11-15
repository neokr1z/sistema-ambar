namespace AMBAR.Semana4;
public partial class BitacoraForm : Form
{
    private Sesion actor=null!;
    private int page;
    public BitacoraForm()
    {
        InitializeComponent();
        from.ValueChanged += ClearQuery;
        to.ValueChanged += ClearQuery;
        filter.TextChanged += ClearQuery;
    }
    private void ClearQuery(object? sender,EventArgs e)
    {
        grid.DataSource=null;page=0;previous.Enabled=false;next.Enabled=false;
        summary.Text="Pulsa Consultar y confirma tu contraseña para aplicar los filtros.";
    }
    internal BitacoraForm(Sesion session):this()
    {
        if(!session.IsAdmin)throw new UnauthorizedAccessException("Sólo Administración puede consultar la bitácora.");
        actor=session;from.Value=DateTime.Today.AddDays(-30);to.Value=DateTime.Today;
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;}Consult(0);
    }
    private void Consult(int target)
    {
        grid.DataSource=null;previous.Enabled=false;next.Enabled=false;
        using var prompt=new ConfirmarPasswordForm();
        if(prompt.ShowDialog(this)!=DialogResult.OK){summary.Text="Consulta cancelada. No se muestran datos de la bitácora.";return;}
        try
        {
            var table=RepositorioBitacora.Query(actor,prompt.Password,from.Value,to.Value,filter.Text,target);
            grid.DataSource=table;page=target;
            grid.Columns["Fecha"].DefaultCellStyle.Format="dd/MM/yyyy HH:mm";
            grid.Columns["Detalle"].FillWeight=230;
            summary.Text=$"Página {page+1} · {table.Rows.Count} actividades. Orden: más recientes primero.";
            previous.Enabled=page>0;next.Enabled=table.Rows.Count==100;
        }
        catch(Exception ex){summary.Text="No se pudo completar la consulta.";Problems.Show(ex);}
    }
    private void SearchClick(object? sender,EventArgs e)=>Consult(0);
    private void PreviousClick(object? sender,EventArgs e)=>Consult(Math.Max(0,page-1));
    private void NextClick(object? sender,EventArgs e)=>Consult(page+1);
    private void CloseClick(object? sender,EventArgs e)=>Close();
}
