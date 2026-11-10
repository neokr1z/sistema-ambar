namespace AMBAR.Semana4;

// Edición de registros mínimos: no se requieren CURP, domicilio ni expediente.
internal sealed class ModificarReinscripcionForm : Form
{
    private readonly Sesion actor;
    private readonly ReinscritoEditable original;
    private readonly TextBox numero=new();
    private readonly TextBox nombre=new();
    private readonly TextBox paterno=new();
    private readonly TextBox materno=new();
    private readonly ComboBox semestre=new();
    private readonly ComboBox grupo=new();
    private readonly Label aviso=new();
    private readonly Button guardar=new();

    internal ModificarReinscripcionForm(Sesion sesion,string control)
    {
        if(!sesion.CanSeeStudents)
            throw new UnauthorizedAccessException("Acceso exclusivo para Secretaría y Administración.");
        actor=sesion;
        original=RepositorioMantenimientoReinscripcion.Obtener(actor,control);
        if(!original.Estatus.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("No es posible modificar una reinscripción dada de baja.");

        Text=(actor.IsAdmin?"Administrador":"Secretaría")+" | Modificar reinscripción";
        ClientSize=new Size(780,560);
        MinimumSize=new Size(720,530);
        StartPosition=FormStartPosition.CenterParent;
        BackColor=Color.White;
        Font=new Font("Segoe UI",10F);
        NavegacionEnter.Instalar(this);

        var titulo=new Label {
            Dock=DockStyle.Top,Height=72,Text="MODIFICAR ALUMNO REINSCRITO",
            Font=new Font("Segoe UI",17F,FontStyle.Bold),ForeColor=Theme.Blue,
            Padding=new Padding(24,20,0,0)
        };
        var pie=new FlowLayoutPanel {
            Dock=DockStyle.Bottom,Height=76,FlowDirection=FlowDirection.RightToLeft,
            WrapContents=false,Padding=new Padding(16,10,22,12)
        };
        Button Boton(string texto,int ancho)
        {
            var b=new Button {
                Text=texto,Width=ancho,Height=44,
                BackColor=Theme.Blue,ForeColor=Color.White,
                FlatStyle=FlatStyle.Flat,Cursor=Cursors.Hand,
                Font=new Font("Segoe UI",10F,FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize=0;
            pie.Controls.Add(b);
            return b;
        }
        var cerrar=Boton("Cancelar",135);
        cerrar.Click+=(_,_)=>Close();
        guardar=Boton("Guardar cambios",205);
        guardar.Click+=(_,_)=>Guardar();

        var cuerpo=new Panel {Dock=DockStyle.Fill,AutoScroll=true,Padding=new Padding(24,12,24,12)};
        var tabla=new TableLayoutPanel {
            Dock=DockStyle.Top,ColumnCount=2,RowCount=7,
            Height=366,Padding=new Padding(4)
        };
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,220));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        for(int i=0;i<7;i++)tabla.RowStyles.Add(new RowStyle(SizeType.Absolute,51));
        void Fila(int i,string etiqueta,Control campo)
        {
            tabla.Controls.Add(new Label {
                Text=etiqueta,Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleLeft,
                ForeColor=Theme.Blue,Font=new Font("Segoe UI",10F,FontStyle.Bold)
            },0,i);
            campo.Dock=DockStyle.Fill;campo.Margin=new Padding(3,7,12,7);
            tabla.Controls.Add(campo,1,i);
        }
        numero.MaxLength=10;numero.Text=original.Control;
        nombre.MaxLength=60;nombre.Text=original.Nombre;
        paterno.MaxLength=50;paterno.Text=original.Paterno;
        materno.MaxLength=50;materno.Text=original.Materno;
        semestre.DropDownStyle=ComboBoxStyle.DropDownList;
        grupo.DropDownStyle=ComboBoxStyle.DropDownList;
        semestre.Items.Add(new Choice(2,
            original.Control.StartsWith("2025")?"2.º semestre · Repetidor":"2.º semestre"));
        if(original.Control.StartsWith("2025"))
            semestre.Items.Add(new Choice(3,"3.º semestre · Regular"));
        semestre.SelectedIndexChanged+=(_,_)=>ActualizarGrupos();
        numero.TextChanged+=(_,_)=>ValidarGeneracionVisual();

        Fila(0,"Número de control",numero);
        Fila(1,"Nombre(s)",nombre);
        Fila(2,"Apellido paterno",paterno);
        Fila(3,"Apellido materno",materno);
        Fila(4,"Semestre de destino",semestre);
        Fila(5,"Grupo de destino",grupo);
        aviso.Text="El correo institucional se actualizará automáticamente si cambias el nombre.\n"+
            "No se solicitarán CURP ni datos de expediente.";
        aviso.ForeColor=Theme.Blue;aviso.AutoSize=true;aviso.Padding=new Padding(8,8,8,0);
        tabla.Controls.Add(aviso,0,6);tabla.SetColumnSpan(aviso,2);
        cuerpo.Controls.Add(tabla);
        Controls.Add(cuerpo);Controls.Add(pie);Controls.Add(titulo);

        for(int i=0;i<semestre.Items.Count;i++)
            if((semestre.Items[i] as Choice)?.Id==original.Semestre)
            {semestre.SelectedIndex=i;break;}
        if(semestre.SelectedIndex<0 && semestre.Items.Count>0)semestre.SelectedIndex=0;
        grupo.Enabled=original.Estatus.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase);
    }

    private void ValidarGeneracionVisual()
    {
        try
        {
            var (anio,_) = RepositorioReinscripcionDemo.Inferir(numero.Text.Trim());
            int actual=(semestre.SelectedItem as Choice)?.Id??original.Semestre;
            semestre.Items.Clear();
            semestre.Items.Add(new Choice(2,
                anio==2025?"2.º semestre · Repetidor":"2.º semestre"));
            if(anio==2025)semestre.Items.Add(new Choice(3,"3.º semestre · Regular"));
            int buscado=anio==2026?2:actual;
            semestre.SelectedIndex=semestre.Items.OfType<Choice>().ToList()
                .FindIndex(x=>x.Id==buscado);
            if(semestre.SelectedIndex<0)semestre.SelectedIndex=0;
            aviso.ForeColor=Theme.Blue;
            aviso.Text="El correo se genera automáticamente. Los datos se validarán al guardar.";
        }
        catch
        {
            aviso.ForeColor=Color.DarkRed;
            aviso.Text="Número de control: escribe exactamente 10 dígitos de generación 2025 o 2026.";
            guardar.Enabled=false;
            return;
        }
        guardar.Enabled=true;
    }

    private void ActualizarGrupos()
    {
        if(semestre.SelectedItem is not Choice elegido)return;
        int anterior=(grupo.SelectedItem as Choice)?.Id??original.Grupo;
        try
        {
            grupo.Items.Clear();
            foreach(var g in RepositorioMantenimientoReinscripcion.Grupos(
                actor,elegido.Id,original.Grupo))grupo.Items.Add(g);
            int indice=grupo.Items.OfType<Choice>().ToList().FindIndex(x=>x.Id==anterior);
            if(indice>=0)grupo.SelectedIndex=indice;
            else if(grupo.Items.Count>0)grupo.SelectedIndex=0;
            guardar.Enabled=grupo.Items.Count>0;
        }
        catch(Exception ex) {guardar.Enabled=false;Problems.Show(ex);}
    }

    private void Guardar()
    {
        if(semestre.SelectedItem is not Choice s || grupo.SelectedItem is not Choice g)
        {
            MessageBox.Show(this,"Selecciona un semestre y grupo válidos.","AMBAR",
                MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }
        if(MessageBox.Show(this,
            $"¿Guardar los cambios de {original.Control}?\n"+
            $"Número nuevo: {numero.Text.Trim()}\nSemestre: {s.Id}º\nGrupo: {g.Name}",
            "Confirmar modificación",MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)!=DialogResult.Yes)return;
        try
        {
            guardar.Enabled=false;
            RepositorioMantenimientoReinscripcion.Modificar(actor,original.Control,
                numero.Text,nombre.Text,paterno.Text,materno.Text,s.Id,g.Id);
            MessageBox.Show(this,"Los datos del alumno se actualizaron en MySQL.",
                "AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Information);
            DialogResult=DialogResult.OK;
            Close();
        }
        catch(Exception ex){Problems.Show(ex);guardar.Enabled=true;}
    }
}
