using System.Data;

namespace AMBAR.Semana4;

// Consulta independiente: muestra incluso grupos llenos, a diferencia del selector de inscripción.
internal sealed class ConsultaGruposForm : Form
{
    private readonly Sesion actor;
    private readonly DataGridView tabla = new();
    private readonly Label resumen = new();
    private readonly Button lista = new();
    internal ConsultaGruposForm(Sesion sesion)
    {
        if (!sesion.CanSeeStudents) throw new UnauthorizedAccessException("Sin permiso para consultar los grupos.");
        actor=sesion;
        Text="AMBAR | Grupos de primer semestre";
        StartPosition=FormStartPosition.CenterParent;
        ClientSize=new Size(850,510);
        MinimumSize=new Size(700,440);
        BackColor=Color.White;
        Font=new Font("Segoe UI",10F);
        var titulo=new Label {Text="GRUPOS | PRIMER SEMESTRE",Dock=DockStyle.Top,Height=65,Font=new Font("Segoe UI",17F,FontStyle.Bold),ForeColor=Color.FromArgb(6,55,99),Padding=new Padding(20,14,0,0)};
        resumen.Text="Seleccione un grupo para consultar alumnos y generar el PDF.";
        resumen.Dock=DockStyle.Top; resumen.Height=44;resumen.Padding=new Padding(20,10,0,0);
        tabla.Dock=DockStyle.Fill;tabla.ReadOnly=true;tabla.MultiSelect=false;tabla.SelectionMode=DataGridViewSelectionMode.FullRowSelect;tabla.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;tabla.AllowUserToAddRows=false;tabla.RowHeadersVisible=false;
        var acciones=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=68,FlowDirection=FlowDirection.RightToLeft,Padding=new Padding(12)};
        Button Create(string name,EventHandler click)
        {
            var b=new Button{Text=name,Width=180,Height=43,BackColor=Color.FromArgb(6,55,99),ForeColor=Color.White,FlatStyle=FlatStyle.Flat,Cursor=Cursors.Hand};
            b.FlatAppearance.BorderSize=0;b.Click+=click;acciones.Controls.Add(b);return b;
        }
        Create("Cerrar",(_,_)=>Close());
        lista=Create("Ver lista / PDF",(_,_)=>AbrirLista());
        Create("Actualizar",(_,_)=>Cargar());
        Controls.Add(tabla);Controls.Add(resumen);Controls.Add(titulo);Controls.Add(acciones);
        Shown+=(_,_)=>Cargar();
    }
    private void Cargar()
    {
        try
        {
            using var conexion=Db.Connect();
            Identidad.RequireActive(conexion,null,actor);
            using var cmd=Db.Sql(conexion,"""
                SELECT g.ID_grupo AS Id, g.nombre_grupo AS Grupo,
                  pe.nombre_periodo AS Periodo, g.Capacidad AS Capacidad,
                  COUNT(DISTINCT CASE WHEN UPPER(i.estatus)='INSCRITO' THEN i.Numero_control END) AS Inscritos,
                  GREATEST(0,g.Capacidad-COUNT(DISTINCT CASE WHEN UPPER(i.estatus)='INSCRITO' THEN i.Numero_control END)) AS Disponibles
                FROM Grupo g
                JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
                JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
                LEFT JOIN Inscripcion i ON i.ID_grupo=g.ID_grupo
                WHERE s.numero_semestre=1 AND g.habilitado_fichas=1
                  AND pe.nombre_periodo='Agosto-Diciembre 2026'
                GROUP BY g.ID_grupo,g.nombre_grupo,pe.nombre_periodo,g.Capacidad
                ORDER BY g.nombre_grupo
                """);
            using var rd=cmd.ExecuteReader();
            var datos=new DataTable();datos.Load(rd);
            tabla.DataSource=datos;
            tabla.Columns["Id"].Visible=false;
            resumen.Text=$"{datos.Rows.Count} grupos. Selecciona uno para ver alumnos o exportar PDF.";
            lista.Enabled=datos.Rows.Count>0;
        }
        catch(Exception ex){lista.Enabled=false;Problems.Show(ex);}
    }
    private void AbrirLista()
    {
        if(tabla.CurrentRow?.Cells["Id"].Value is null)return;
        int id=Convert.ToInt32(tabla.CurrentRow.Cells["Id"].Value);
        using var detalle=new ListaGrupoForm(actor,id);
        detalle.ShowDialog(this);
        Cargar();
    }
}
