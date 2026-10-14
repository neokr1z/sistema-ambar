namespace AMBAR.Semana4;

internal sealed class MainForm : Form
{
    private readonly Session session;
    private readonly Panel content=new() { Dock=DockStyle.Fill,BackColor=Theme.Light };
    private readonly FlowLayoutPanel nav=new() { Dock=DockStyle.Fill, FlowDirection=FlowDirection.TopDown,
        WrapContents=false,AutoScroll=true,Padding=new Padding(14,22,5,10) };

    public MainForm(Session logged)
    {
        session=logged;
        Theme.Setup(this,"Panel principal",1180,780);
        var top=new Panel { Dock=DockStyle.Top,Height=86,BackColor=Color.White };
        var logo=Theme.Logo(180,80); logo.Dock=DockStyle.Left;
        top.Controls.Add(logo);
        var title=Theme.Text("SISTEMA AMBAR  /  CONTROL ESCOLAR",17,true,Theme.Blue);
        title.Location=new Point(198,28);
        top.Controls.Add(title);
        var role=Theme.Text($"{session.Username}  ·  {session.Role}",10,true,Theme.Gray);
        role.Dock=DockStyle.Right; role.TextAlign=ContentAlignment.MiddleRight;
        top.Controls.Add(role);
        Controls.Add(content);
        var sidebar=new Panel {Dock=DockStyle.Left,Width=258,BackColor=Theme.Blue};
        nav.BackColor=Theme.Blue;
        sidebar.Controls.Add(nav);
        Controls.Add(sidebar);
        Controls.Add(top);

        Menu("\uE80F", "Inicio", Home);
        if(session.CanSeeStudents)
        {
            Menu("\uE710", "Alta e inscripción", ()=>Open(new EnrollmentForm(session)));
            Menu("\uE8A5", "Consulta de alumnos", ()=>Open(new StudentsForm(session)));
            Menu("\uE787", "Reinscripción", ()=>Placeholder("Reinscripción"));
            Menu("\uE8B7", "Reportes y listas", ()=>Placeholder("Reportes y listas"));
            Menu("\uE8A5", "Documentos", ()=>Placeholder("Documentos"));
            Menu("\uE715", "Enviar documentos", ()=>Placeholder("Envío por correo"));
        }
        if(session.IsTeacher)
        {
            Menu("\uE8F1", "Mis materias y grupos", ()=>Placeholder("Materias y grupos"));
            Menu("\uE8D4", "Captura de calificaciones", ()=>Placeholder("Calificaciones"));
            Menu("\uE81D", "Historial de calificaciones", ()=>Placeholder("Historial de calificaciones"));
        }
        if(session.IsAdmin)
        {
            Menu("\uE716", "Alta de personal", ()=>Open(new UsersForm(session)));
            Menu("\uE9CE", "Bitácora", ()=>Placeholder("Bitácora"));
        }
        Menu("\uE72E", "Cambiar contraseña", ()=>Open(new ChangePasswordForm(session,false)));
        Menu("\uE8AC", "Cerrar sesión", Close);
        Home();
    }

    private void Menu(string icon,string label,Action callback)
    {
        var item=new Panel { Width=224,Height=52,Margin=new Padding(0,0,0,7) };
        var glyph=new Label {Text=icon,Font=new Font("Segoe MDL2 Assets",17), ForeColor=Color.White,
            Width=45,Height=42,TextAlign=ContentAlignment.MiddleCenter,Dock=DockStyle.Left};
        var button=new Button {Text=label,ForeColor=Color.White, BackColor=Theme.Blue,FlatStyle=FlatStyle.Flat,
            Width=177,Height=43,TextAlign=ContentAlignment.MiddleLeft,
            Font=new Font("Segoe UI",10,FontStyle.Bold),Dock=DockStyle.Fill,Cursor=Cursors.Hand};
        glyph.Click += (_,_)=>callback(); button.Click+=(_,_)=>callback();
        item.Controls.Add(button); item.Controls.Add(glyph);
        nav.Controls.Add(item);
    }

    private void Open(Form form)
    {
        using (form) form.ShowDialog(this);
    }

    private void Placeholder(string title)
    {
        content.Controls.Clear();
        content.Controls.Add(Theme.Message($"{title}\n\nMódulo visible de acuerdo con los requisitos de la semana 4.\nSu funcionalidad se implementará en una entrega posterior."));
        content.Controls.Add(Theme.Heading(title));
    }

    private void Home()
    {
        content.Controls.Clear();
        var head=Theme.Heading($"Bienvenido · {session.Role}");
        var body=new FlowLayoutPanel {Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,
            WrapContents=false,AutoScroll=true,Padding=new Padding(30)};
        body.Controls.Add(Theme.Text("Tecnológico Nacional de México · Instituto Tecnológico de Acapulco",11,true));
        body.Controls.Add(Theme.Text($"Usuario: {session.Username}     |     Rol: {session.Role}",11));
        body.Controls.Add(Theme.Text("Elige un módulo en el menú de la izquierda.",11));
        body.Controls.Add(Theme.Text("Funcional en la semana 4: inicio de sesión, alta de personal, inscripción, consulta, edición, ocultamiento y PDF.",10,false,Theme.Green));
        body.Controls.Add(Theme.Text("Los demás módulos se muestran con sus iconos, pero están pendientes de desarrollo.",10,false,Theme.Gray));
        content.Controls.Add(body);
        content.Controls.Add(head);
    }
}
