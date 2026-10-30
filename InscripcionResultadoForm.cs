namespace AMBAR.Semana4;
public partial class InscripcionResultadoForm : Form
{
    private Sesion actor=null!;private int grupo;
    public InscripcionResultadoForm(){InitializeComponent();}
    internal InscripcionResultadoForm(Sesion actor,int grupo,string control):this()
    {this.actor=actor;this.grupo=grupo;summary.Text=$"La inscripción se guardó correctamente.\nNúmero de control: {control}\nPuedes consultar el grupo y descargar su lista en PDF.";}
    private void ListClick(object? sender,EventArgs e)
    {if(actor is null)return;using var form=new ListaGrupoForm(actor,grupo);form.ShowDialog(this);}
    private void CloseClick(object? sender,EventArgs e)=>Close();
}
