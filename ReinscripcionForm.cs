using System.Data;

namespace AMBAR.Semana4;

// Ventana de reinscripción: número de control → historial → grupo → confirmación.
internal sealed class ReinscripcionForm : Form
{
    private readonly Sesion actor;
    private readonly TextBox control = new();
    private readonly Label identificacion = new();
    private readonly Label estado = new();
    private readonly ComboBox grupos = new();
    private readonly DataGridView calificaciones = new();
    private readonly Button guardar = new();
    private ResumenReinscripcion? consulta;

    internal ReinscripcionForm(Sesion session)
    {
        if(!session.CanSeeStudents)throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría y Administración.");
        actor=session;
        Text=(actor.IsAdmin?"Administrador":"Secretaría")+" | Reinscripción";
        StartPosition=FormStartPosition.CenterParent;
        MinimumSize=new Size(860,620);ClientSize=new Size(1090,710);
        BackColor=Color.White;
        Font=new Font("Segoe UI",10F);
        NavegacionEnter.Instalar(this);

        var title=new Label
        {
            Text="REINSCRIPCIÓN | AGOSTO–DICIEMBRE 2026",
            Dock=DockStyle.Top,Height=80,Font=new Font("Segoe UI",19F,FontStyle.Bold),
            ForeColor=Color.FromArgb(6,55,99),Padding=new Padding(25,20,0,0)
        };
        var searchRow=new FlowLayoutPanel
        {
            Dock=DockStyle.Top,Height=75,Padding=new Padding(24,14,0,0),
            FlowDirection=FlowDirection.LeftToRight,WrapContents=false
        };
        searchRow.Controls.Add(new Label{Text="Número de control",Width=170,Height=38,TextAlign=ContentAlignment.MiddleLeft,ForeColor=Color.FromArgb(6,55,99)});
        control.Width=250;control.Height=32;control.MaxLength=20;control.TabIndex=0;
        searchRow.Controls.Add(control);
        var buscar=Boton("Buscar alumno",175);
        buscar.Click+=(_,_)=>Buscar();
        buscar.TabIndex=1;
        searchRow.Controls.Add(buscar);

        var info=new Panel{Dock=DockStyle.Top,Height=110,Padding=new Padding(25,5,25,5)};
        identificacion.Dock=DockStyle.Top;identificacion.Height=60;
        identificacion.Font=new Font("Segoe UI",11F,FontStyle.Bold);
        identificacion.ForeColor=Color.FromArgb(6,55,99);
        identificacion.Text="Introduce un número de control para consultar el expediente.";
        estado.Dock=DockStyle.Fill;estado.ForeColor=Color.DarkRed;
        info.Controls.Add(estado);info.Controls.Add(identificacion);

        var subtitulo=new Label{Text="CALIFICACIONES FINALES DEL SEMESTRE ANTERIOR",Dock=DockStyle.Top,Height=45,
            Padding=new Padding(25,8,0,0),ForeColor=Color.FromArgb(6,55,99),
            Font=new Font("Segoe UI",12F,FontStyle.Bold)};
        calificaciones.Dock=DockStyle.Fill;calificaciones.ReadOnly=true;
        calificaciones.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        calificaciones.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
        calificaciones.MultiSelect=false;calificaciones.AllowUserToAddRows=false;
        calificaciones.RowHeadersVisible=false;calificaciones.BackgroundColor=Color.White;
        calificaciones.BorderStyle=BorderStyle.None;

        var actions=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=94,Padding=new Padding(20,18,20,10),
            FlowDirection=FlowDirection.RightToLeft,WrapContents=false};
        var cerrar=Boton("Cerrar",140);cerrar.Click+=(_,_)=>Close();
        guardar=Boton("Confirmar reinscripción",240);guardar.Enabled=false;guardar.Click+=(_,_)=>Guardar();
        grupos.DropDownStyle=ComboBoxStyle.DropDownList;grupos.Width=360;grupos.Height=38;grupos.TabIndex=2;
        actions.Controls.Add(cerrar);
        actions.Controls.Add(guardar);
        actions.Controls.Add(grupos);
        actions.Controls.Add(new Label{Text="Grupo de destino",Width=145,Height=38,TextAlign=ContentAlignment.MiddleLeft,ForeColor=Color.FromArgb(6,55,99)});

        Controls.Add(calificaciones);
        Controls.Add(subtitulo);Controls.Add(info);Controls.Add(searchRow);Controls.Add(title);Controls.Add(actions);
        AcceptButton=buscar;
    }

    private static Button Boton(string label,int width)
    {
        var b=new Button{Text=label,Width=width,Height=43,
            BackColor=Color.FromArgb(6,55,99),ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",10F,FontStyle.Bold),Cursor=Cursors.Hand};
        b.FlatAppearance.BorderSize=0;
        b.FlatAppearance.MouseOverBackColor=Color.FromArgb(31,79,120);
        return b;
    }

    private void Buscar()
    {
        consulta=null;guardar.Enabled=false;grupos.Items.Clear();calificaciones.DataSource=null;
        identificacion.Text="";estado.Text="";
        try
        {
            var datos=RepositorioReinscripcion.Consultar(actor,control.Text);
            consulta=datos;
            identificacion.Text=$"{datos.Alumno} | {datos.Control}\n{datos.Carrera} | {datos.SemestreAnterior}.º → {datos.SemestreDestino}.º semestre";
            calificaciones.DataSource=datos.Calificaciones;
            if(calificaciones.Columns.Contains("Calificación"))
                calificaciones.Columns["Calificación"].DefaultCellStyle.Format="N2";
            estado.ForeColor=datos.PuedeReinscribir?Color.DarkGreen:Color.DarkRed;
            estado.Text=$"{datos.Situacion}. Aprobadas: {datos.Aprobadas}/{datos.TotalMaterias}. Periodo anterior: {datos.PeriodoAnterior}.";
            if(datos.PuedeReinscribir)
            {
                foreach(var grupo in RepositorioReinscripcion.Grupos(actor,datos.SemestreDestino))
                    grupos.Items.Add(grupo);
                if(grupos.Items.Count>0)grupos.SelectedIndex=0;
                guardar.Enabled=grupos.Items.Count>0;
                if(grupos.Items.Count==0)
                {
                    estado.ForeColor=Color.DarkRed;
                    estado.Text="El alumno cumple los requisitos, pero no hay grupos con cupo para el semestre siguiente.";
                }
            }
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private void Guardar()
    {
        if(consulta is null || !consulta.PuedeReinscribir || grupos.SelectedItem is not Choice grupo)return;
        if(MessageBox.Show(this,$"¿Reinscribir a {consulta.Alumno} al grupo {grupo.Text} del periodo agosto–diciembre 2026?",
            "Confirmar reinscripción",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
        try
        {
            guardar.Enabled=false;
            RepositorioReinscripcion.Guardar(actor,consulta.Control,grupo.Id);
            MessageBox.Show(this,$"Reinscripción registrada correctamente.\nAlumno: {consulta.Alumno}\nGrupo: {grupo.Text}\nPeriodo: Agosto–diciembre 2026",
                "AMBAR | Reinscripción completada",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Buscar();
        }
        catch(Exception ex){Problems.Show(ex);Buscar();}
    }
}
