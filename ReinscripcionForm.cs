using System.Data;

namespace AMBAR.Semana4;

// Reinscripción independiente: captura sin expedientes ni calificaciones precargadas.
internal sealed class ReinscripcionForm : Form
{
    private readonly Sesion actor;
    private readonly TextBox control = new();
    private readonly TextBox nombre = new();
    private readonly TextBox paterno = new();
    private readonly TextBox materno = new();
    private readonly Label resultado = new();
    private readonly Label informacion = new();
    private readonly ComboBox grupos = new();
    private readonly Button guardar;
    private readonly Button horario;
    private string controlValidado = "";
    private int semestreValidado;

    internal ReinscripcionForm(Sesion session)
    {
        if(!session.CanSeeStudents)throw new UnauthorizedAccessException("Acceso exclusivo para Secretaría y Administración.");
        actor=session;
        Text=(actor.IsAdmin?"Administrador":"Secretaría")+" | Reinscripción";
        StartPosition=FormStartPosition.CenterParent;
        ClientSize=new Size(1080,660);
        MinimumSize=new Size(920,570);
        BackColor=Color.White;
        Font=new Font("Segoe UI",10F);
        NavegacionEnter.Instalar(this);

        var encabezado=new Label
        {
            Text="REINSCRIPCIÓN | AGOSTO–DICIEMBRE 2026",
            Dock=DockStyle.Top,Height=88,Font=new Font("Segoe UI",19F,FontStyle.Bold),
            ForeColor=Theme.Blue,Padding=new Padding(26,22,0,0)
        };
        var contenido=new Panel{Dock=DockStyle.Fill,Padding=new Padding(28,8,28,16),AutoScroll=true};
        var buscador=new FlowLayoutPanel
        {
            Dock=DockStyle.Top,Height=72,WrapContents=false,
            FlowDirection=FlowDirection.LeftToRight
        };
        buscador.Controls.Add(new Label
        {Text="Número de control",Width=170,Height=42,TextAlign=ContentAlignment.MiddleLeft,
         Font=new Font("Segoe UI",10F,FontStyle.Bold),ForeColor=Theme.Blue});
        control.Width=260;control.MaxLength=20;control.Font=new Font("Segoe UI",12F);
        buscador.Controls.Add(control);
        var validar=Boton("Validar número",180);
        validar.Click+=(_,_)=>ValidarNumero();
        buscador.Controls.Add(validar);

        informacion.Dock=DockStyle.Top;informacion.Height=84;
        informacion.Font=new Font("Segoe UI",11F,FontStyle.Bold);
        informacion.ForeColor=Theme.Blue;
        informacion.Text="Escribe un número de control para determinar el semestre correspondiente.";
        resultado.Dock=DockStyle.Top;resultado.Height=47;
        resultado.ForeColor=Color.FromArgb(61,107,134);
        resultado.Text="Los datos personales se capturan después de validar el número.";

        var tituloDatos=new Label
        {
            Text="DATOS DEL ALUMNO",Dock=DockStyle.Top,Height=47,
            Font=new Font("Segoe UI",14F,FontStyle.Bold),ForeColor=Theme.Blue
        };
        var campos=new TableLayoutPanel{Dock=DockStyle.Top,Height=170,ColumnCount=2,RowCount=3};
        campos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,25));
        campos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,75));
        AgregarCampo(campos,0,"Nombre(s)",nombre);
        AgregarCampo(campos,1,"Apellido paterno",paterno);
        AgregarCampo(campos,2,"Apellido materno",materno);
        foreach(var tb in new[]{nombre,paterno,materno}){tb.Enabled=false;tb.MaxLength=60;tb.Font=new Font("Segoe UI",11F);}

        var seleccionar=new FlowLayoutPanel
        {
            Dock=DockStyle.Top,Height=75,WrapContents=false,
            FlowDirection=FlowDirection.LeftToRight
        };
        seleccionar.Controls.Add(new Label{Text="Grupo de destino",Width=170,Height=40,
            TextAlign=ContentAlignment.MiddleLeft,ForeColor=Theme.Blue,
            Font=new Font("Segoe UI",10F,FontStyle.Bold)});
        grupos.Width=445;grupos.DropDownStyle=ComboBoxStyle.DropDownList;grupos.Enabled=false;
        seleccionar.Controls.Add(grupos);
        horario=Boton("Ver horario",150);
        horario.Enabled=false;
        horario.Click+=(_,_)=>VerHorario();
        seleccionar.Controls.Add(horario);
        grupos.SelectedIndexChanged+=(_,_)=>horario.Enabled=grupos.SelectedItem is Choice;

        // Agregar de abajo hacia arriba por el orden de DockStyle.Top.
        contenido.Controls.Add(seleccionar);
        contenido.Controls.Add(campos);
        contenido.Controls.Add(tituloDatos);
        contenido.Controls.Add(resultado);
        contenido.Controls.Add(informacion);
        contenido.Controls.Add(buscador);

        var pie=new FlowLayoutPanel
        {
            Dock=DockStyle.Bottom,Height=84,FlowDirection=FlowDirection.RightToLeft,
            WrapContents=false,Padding=new Padding(20,16,20,12)
        };
        var cerrar=Boton("Cerrar",110);cerrar.Click+=(_,_)=>Close();
        guardar=Boton("Guardar reinscripción",218);
        guardar.Enabled=false;guardar.Click+=(_,_)=>Guardar();
        var verGrupos=Boton("Grupos / PDF",160);
        verGrupos.Click+=(_,_)=>{using var f=new ConsultaGruposForm(actor,true);f.ShowDialog(this);RefrescarGrupos();};
        pie.Controls.Add(cerrar);pie.Controls.Add(guardar);pie.Controls.Add(verGrupos);
        Controls.Add(contenido);Controls.Add(encabezado);Controls.Add(pie);
        AcceptButton=validar;
    }

    private static Button Boton(string label,int width)
    {
        var b=new Button{Text=label,Width=width,Height=43,BackColor=Theme.Blue,
            ForeColor=Color.White,FlatStyle=FlatStyle.Flat,
            Font=new Font("Segoe UI",10F,FontStyle.Bold),Cursor=Cursors.Hand};
        b.FlatAppearance.BorderSize=0;
        b.FlatAppearance.MouseOverBackColor=Color.FromArgb(31,79,120);
        return b;
    }
    private static void AgregarCampo(TableLayoutPanel panel,int fila,string texto,TextBox campo)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.Percent,33.33F));
        panel.Controls.Add(new Label{Text=texto,Dock=DockStyle.Fill,
            TextAlign=ContentAlignment.MiddleLeft,ForeColor=Theme.Blue,
            Font=new Font("Segoe UI",10F,FontStyle.Bold)},0,fila);
        campo.Dock=DockStyle.Fill;campo.Margin=new Padding(4,7,28,10);
        panel.Controls.Add(campo,1,fila);
    }

    private void LimpiarCaptura()
    {
        controlValidado="";
        semestreValidado=0;
        informacion.Text="";
        resultado.Text="";
        nombre.Clear();paterno.Clear();materno.Clear();
        grupos.Items.Clear();grupos.Enabled=false;guardar.Enabled=false;horario.Enabled=false;
        foreach(var tb in new[]{nombre,paterno,materno})tb.Enabled=false;
    }

    private void ValidarNumero()
    {
        LimpiarCaptura();
        try
        {
            string numero=control.Text.Trim();
            var (anio,semestre)=RepositorioReinscripcionDemo.Inferir(numero);
            var anterior=RepositorioReinscripcionDemo.Existente(actor,numero);
            if(anterior!=null)
            {
                informacion.ForeColor=Color.DarkRed;
                informacion.Text=$"El número {numero} ya se registró en la demostración.\nGrupo: {anterior.GrupoNombre} · {anterior.Semestre}.º semestre.";
                resultado.Text="No se permitirá volver a registrarlo.";
                return;
            }
            // No existe en la BD es válido: los campos comienzan vacíos.
            controlValidado=numero;semestreValidado=semestre;
            informacion.ForeColor=Color.DarkGreen;
            informacion.Text=$"Número válido · Generación {anio}\nSemestre de destino: {semestre}.º | Agosto–diciembre 2026";
            resultado.Text="Completa los datos del alumno y selecciona el grupo.";
            foreach(var tb in new[]{nombre,paterno,materno})tb.Enabled=true;
            RefrescarGrupos();
            nombre.Focus();
        }
        catch(Exception ex){informacion.ForeColor=Color.DarkRed;informacion.Text=ex.Message;}
    }

    private void RefrescarGrupos()
    {
        if(semestreValidado==0)return;
        int seleccionado=(grupos.SelectedItem as Choice)?.Id??0;
        try
        {
            grupos.Items.Clear();
            foreach(var item in RepositorioReinscripcionDemo.Grupos(actor,semestreValidado))grupos.Items.Add(item);
            var anterior=grupos.Items.OfType<Choice>().FirstOrDefault(x=>x.Id==seleccionado);
            if(anterior!=null)grupos.SelectedItem=anterior;
            else if(grupos.Items.Count>0)grupos.SelectedIndex=0;
            grupos.Enabled=grupos.Items.Count>0;
            guardar.Enabled=grupos.Enabled;
            horario.Enabled=grupos.SelectedItem is Choice;
            if(grupos.Items.Count==0)resultado.Text="No hay grupos disponibles con cupo para este semestre.";
        }
        catch(Exception ex){guardar.Enabled=false;Problems.Show(ex);}
    }

    private void VerHorario()
    {
        if(grupos.SelectedItem is not Choice grupo)return;
        using var f=new HorarioGrupoDemoForm(actor,grupo.Id,grupo.Name);
        f.ShowDialog(this);
    }

    private void Guardar()
    {
        if(string.IsNullOrWhiteSpace(controlValidado)||control.Text.Trim()!=controlValidado)
        {
            MessageBox.Show(this,"Primero valida el número de control actual.","AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }
        if(grupos.SelectedItem is not Choice grupo)return;
        if(MessageBox.Show(this,$"¿Registrar reinscripción de {nombre.Text.Trim()} en {grupo.Name}?",
            "Confirmar",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
        try
        {
            guardar.Enabled=false;
            RepositorioReinscripcionDemo.Guardar(actor,controlValidado,nombre.Text,paterno.Text,materno.Text,grupo.Id);
            MessageBox.Show(this,$"Reinscripción de demostración guardada.\nNúmero de control: {controlValidado}\nGrupo: {grupo.Name}",
                "AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Information);
            LimpiarCaptura();control.Clear();control.Focus();
        }
        catch(Exception ex){Problems.Show(ex);guardar.Enabled=grupos.SelectedItem is Choice;}
    }
}
