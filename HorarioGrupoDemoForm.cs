using System.Data;

namespace AMBAR.Semana4;

internal sealed class HorarioGrupoDemoForm : Form
{
    internal HorarioGrupoDemoForm(Sesion actor,int grupo,string etiqueta)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Sin permiso.");
        Text="AMBAR | Horario del grupo";
        StartPosition=FormStartPosition.CenterParent;
        ClientSize=new Size(910,510);MinimumSize=new Size(730,410);
        Font=new Font("Segoe UI",10F);
        var titulo=new Label{Text="HORARIO | "+etiqueta,Dock=DockStyle.Top,Height=65,
            Font=new Font("Segoe UI",15F,FontStyle.Bold),ForeColor=Theme.Blue,
            Padding=new Padding(16,12,0,0)};
        var tabla=new DataGridView{
            Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AllowUserToDeleteRows=false,
            AllowUserToResizeRows=false,AllowUserToOrderColumns=false,
            AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor=Color.White};
        var cerrar=new Button{Text="Cerrar",Dock=DockStyle.Bottom,Height=48,
            BackColor=Theme.Blue,ForeColor=Color.White,FlatStyle=FlatStyle.Flat};
        cerrar.Click+=(_,_)=>Close();
        Controls.Add(tabla);Controls.Add(titulo);Controls.Add(cerrar);
        Shown+=(_,_)=>
        {
            try
            {
                using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
                using var cmd=Db.Sql(c,"""
                    SELECT h.asignatura AS Materia,h.dia AS Día,
                      TIME_FORMAT(h.hora_inicio,'%H:%i') AS Inicio,
                      TIME_FORMAT(h.hora_fin,'%H:%i') AS Fin,
                      h.aula AS Aula,COALESCE(h.profesor,'') AS Docente
                    FROM Horario_Referencia h
                    WHERE h.ID_grupo=@grupo
                    ORDER BY FIELD(h.dia,'Lunes','Martes','Miércoles','Miercoles','Jueves','Viernes'),
                             h.hora_inicio,h.asignatura
                    """);
                Db.Param(cmd,"@grupo",grupo);
                using var r=cmd.ExecuteReader();
                var dt=new DataTable();dt.Load(r);
                tabla.DataSource=dt;
                if(dt.Rows.Count==0)
                    MessageBox.Show(this,"Este grupo aún no tiene un horario cargado.","AMBAR",
                        MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch(Exception ex){Problems.Show(ex);}
        };
    }
}
