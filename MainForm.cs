namespace AMBAR.Semana4;

internal sealed class MainForm : Form
{
    private readonly Session session;
    private readonly Panel content=new() {Dock=DockStyle.Fill,BackColor=Color.White,AutoScroll=true};
    private readonly FlowLayoutPanel nav=new() {Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,
        WrapContents=false,AutoScroll=true,Padding=new Padding(10,24,6,8)};
    private readonly List<Button> menuButtons=new();
    private readonly Panel sidebar=new() {Dock=DockStyle.Left,Width=290,BackColor=Color.White};

    public MainForm(Session logged)
    {
        session=logged;
        Theme.Setup(this,"Panel principal",1000,710);MinimumSize=Size;
        Controls.Add(content);Controls.Add(sidebar);
        sidebar.Paint+=(_,e)=> {using var pen=new Pen(Theme.Blue,3);e.Graphics.DrawLine(pen,sidebar.Width-2,0,sidebar.Width-2,sidebar.Height);};
        var brand=new Panel {Dock=DockStyle.Top,Height=110};
        var logo=Theme.Logo(205,94);logo.Location=new Point(63,6);brand.Controls.Add(logo);
        var hamburger=new Button {Text="☰",Bounds=new Rectangle(14,26,44,44),FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",20)};
        hamburger.FlatAppearance.BorderSize=0;hamburger.Click+=(_,_)=>nav.Visible=!nav.Visible;brand.Controls.Add(hamburger);
        var logout=Theme.Button("⇥  Cerrar sesión",false);logout.Dock=DockStyle.Bottom;logout.Height=46;logout.BackColor=Theme.Sky;
        logout.Click+=(_,_)=>Close();sidebar.Controls.Add(nav);sidebar.Controls.Add(logout);sidebar.Controls.Add(brand);
        Menu("\uE80F","Inicio",Home);
        if(session.CanSeeStudents)
        {
            Menu("\uE77B","Alta e inscripción",()=>Open(new EnrollmentForm(session)));
            Menu("\uE895","Reinscripción",()=>Placeholder("Reinscripción"));
            Menu("\uE9D9","Reportes",()=>Placeholder("Reportes"));
            Menu("\uE8A5","Documentos",()=>Placeholder("Documentos"));
            Menu("\uE721","Consulta de alumnos",()=>Open(new StudentsForm(session)));
            Menu("\uE715","Enviar documentos",()=>Placeholder("Envío de documentos"));
        }
        if(session.IsTeacher)
        {
            Menu("\uE8F1","Mis materias y grupos",()=>Placeholder("Materias y grupos"));
            Menu("\uE70F","Captura de calificaciones",()=>Open(new GradesForm(session,false)));
            Menu("\uE81C","Historial de calificaciones",()=>Open(new GradesForm(session,true)));
        }
        if(session.IsAdmin)
        {
            Menu("\uE716","Alta de personal",()=>Open(new UsersForm(session)));
            Menu("\uE9D9","Bitácora",()=>Placeholder("Bitácora"));
        }
        Menu("\uE72E","Cambiar contraseña",()=>Open(new ChangePasswordForm(session,false)));
        Home();
    }
    private void Menu(string icon,string label,Action action)
    {
        var row=new RoundedPanel {Width=258,Height=44,BackColor=Theme.Sky,Margin=new Padding(0,0,0,15)};
        ReferenceUi.Round(row);
        var glyph=new Label {Text=icon,Font=new Font("Segoe MDL2 Assets",19),ForeColor=Color.Black,
            Bounds=new Rectangle(5,4,36,36),TextAlign=ContentAlignment.MiddleCenter};
        var button=new Button {Text=label,FlatStyle=FlatStyle.Flat,BackColor=Theme.Sky,ForeColor=Theme.Gray,
            Bounds=new Rectangle(42,2,212,40),TextAlign=ContentAlignment.MiddleLeft,Cursor=Cursors.Hand,
            Font=new Font("Segoe UI",11,FontStyle.Bold)};
        button.FlatAppearance.BorderSize=0;menuButtons.Add(button);
        void Select()
        {
            foreach(var b in menuButtons){b.BackColor=Theme.Sky;b.ForeColor=Theme.Gray;b.Parent!.BackColor=Theme.Sky;}
            row.BackColor=Theme.Gray;button.BackColor=Theme.Gray;button.ForeColor=Color.White;action();
        }
        button.Click+=(_,_)=>Select();glyph.Click+=(_,_)=>Select();row.Controls.Add(button);row.Controls.Add(glyph);nav.Controls.Add(row);
        if(label=="Inicio"){row.BackColor=Theme.Gray;button.BackColor=Theme.Gray;button.ForeColor=Color.White;}
    }
    private void Open(Form form) {using(form)form.ShowDialog(this);}
    private void ClearContent()
    {
        while(content.Controls.Count>0)content.Controls[0].Dispose();
    }
    private void Placeholder(string title)
    {
        ClearContent();content.Controls.Add(Theme.Message("Este módulo estará disponible en una próxima entrega."));
        content.Controls.Add(Theme.Heading(title));
    }
    private void Home()
    {
        ClearContent();
        var body=new Panel {Dock=DockStyle.Top,Height=690,Width=690};content.Controls.Add(body);
        var title=ReferenceUi.Label(body,$"{(session.IsSecretary?"Secretaría":session.Role)} - Control escolar",22,18,655,52,18,true,ContentAlignment.MiddleRight);
        title.ForeColor=Theme.Blue;title.Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right;
        var profile=new RoundedPanel {Bounds=new Rectangle(28,105,650,174),BorderColor=Theme.Blue};body.Controls.Add(profile);
        var avatar=ReferenceUi.Label(profile,"\uE77B",16,18,144,138,70,false,ContentAlignment.MiddleCenter);
        avatar.Font=new Font("Segoe MDL2 Assets",70);avatar.ForeColor=Color.Black;
        var name=ReferenceUi.Label(profile,session.Username,177,18,457,32,16,true);
        var mail=ReferenceUi.Label(profile,"Correo institucional no registrado",177,55,457,44,11,true);
        ReferenceUi.Label(profile,$"Rol\n{session.Role}",177,105,185,58,12,true);
        ReferenceUi.Label(profile,"Área\n"+(session.IsTeacher?"Docencia":"Control escolar"),382,105,240,58,12,true);
        if(session.IsTeacher)
        {
            ReferenceUi.Label(body,"Selecciona Captura o Historial de calificaciones en el menú.",28,315,640,70,13);
            LoadProfile(name,mail);return;
        }
        var enrolled=ReferenceUi.Stat(body,"Alumnos\ninscritos",45,307,182);
        var applicants=ReferenceUi.Stat(body,"Aspirantes\npendientes",251,307,182);
        var renewals=ReferenceUi.Stat(body,"Reinscripciones",457,307,182);
        ReferenceUi.Label(body,"Actividad reciente",18,423,640,35,15,true);
        var grid=ReferenceUi.Grid("Fecha","Usuario","Acción");grid.Bounds=new Rectangle(18,467,660,160);body.Controls.Add(grid);
        var status=ReferenceUi.Label(body,"",18,632,660,48,10);
        LoadProfile(name,mail);
        try
        {
            using var c=Db.Connect();
            string Count(string sql){using var cmd=Db.Sql(c,sql);return Convert.ToString(cmd.ExecuteScalar())??"0";}
            enrolled.Text=Count("SELECT COUNT(DISTINCT Numero_control) FROM Inscripcion WHERE UPPER(estatus)='INSCRITO'");
            applicants.Text=Count("SELECT COUNT(*) FROM Aspirante a WHERE NOT EXISTS (SELECT 1 FROM Alumno al WHERE al.ID_Aspirante=a.ID_Aspirante)");
            renewals.Text=Count("SELECT COUNT(*) FROM Inscripcion WHERE UPPER(tipo_inscripcion) IN ('REINSCRIPCION','REINSCRIPCIÓN')");
            using var cmd=Db.Sql(c,"SELECT b.fecha_hora,u.nombre_usuario,b.descripcion FROM Bitacora b JOIN Usuario u ON u.Id_usuario=b.Id_usuario ORDER BY b.fecha_hora DESC,b.Id_bitacora DESC LIMIT 10");
            using var r=cmd.ExecuteReader();while(r.Read())grid.Rows.Add(r.GetDateTime(0).ToString("dd/MM · HH:mm"),r.GetString(1),r.GetString(2));
            status.Text=grid.Rows.Count==0?"Sin actividad registrada. Indicadores acumulados.":"Indicadores acumulados · Últimas 10 actividades";
        }
        catch {status.Text="No se pudieron cargar los indicadores y la actividad. Revisa la conexión y vuelve a Inicio.";status.ForeColor=Color.DarkRed;}
    }
    private void LoadProfile(Label name,Label email)
    {
        try
        {
            using var c=Db.Connect();
            using var cmd=Db.Sql(c,"""
                SELECT COALESCE(d.nombre,p.nombre,u.nombre_usuario),COALESCE(d.correo_institucional,p.correo_institucional,'')
                FROM Usuario u LEFT JOIN Docente d ON d.ID_docente=u.ID_docente
                LEFT JOIN Personal_Administrativo p ON p.num_administrativo=u.num_administrativo
                WHERE u.Id_usuario=@id
                """);
            Db.Param(cmd,"@id",session.Id);using var r=cmd.ExecuteReader();
            if(r.Read()){name.Text=r.GetString(0);email.Text=session.Username+" · "+r.GetString(1);}
        }
        catch {email.Text="No se pudo cargar el perfil.";}
    }
}
