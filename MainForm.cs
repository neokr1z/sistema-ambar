
namespace AMBAR.Semana4;

public partial class MainForm : Form
{
    private Sesion session = null!;
    private static readonly bool ModulosDocenteActivos = true;
    private Panel? teacherPanel;
    private FlowLayoutPanel? teacherSubjects;
    private DataGridView? teacherGrades;
    private Label? teacherSummary;

    public MainForm()
    {
        InitializeComponent();
    }

    internal MainForm(Sesion logged) : this()
    {
        session = logged;

        enrollRow.Visible = session.CanSeeStudents;
        renewRow.Visible = session.CanSeeStudents;
        reportsRow.Visible = session.CanSeeStudents;
        documentsRow.Visible = session.CanSeeStudents;
        studentsRow.Visible = session.CanSeeStudents;
        sendRow.Visible = session.CanSeeStudents;

        usersRow.Visible = session.IsAdmin;
        personnelRow.Visible = session.IsAdmin;
        auditRow.Visible = session.IsAdmin;

        if (session.IsTeacher)
            SetupTeacherInterface();

        SelectMenu(homeButton);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode ||
            System.ComponentModel.LicenseManager.UsageMode ==
            System.ComponentModel.LicenseUsageMode.Designtime)
            return;

        if (session is null)
        {
            Close();
            return;
        }

        Home();
    }

    private void ToggleMenu(object? sender, EventArgs e)
    {
        bool expanded = sidebar.Width > 80;
        nav.Visible = !expanded;
        logout.Visible = !expanded;
        logo.Visible = !expanded;
        sidebar.Width = expanded ? 64 : 362;
        hamburger.AccessibleName = expanded ? "Abrir menú lateral" : "Cerrar menú lateral";
    }

    private void LogoutClick(object? sender, EventArgs e)
    {
        Close();
    }

    private void SidebarPaint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Theme.Blue, 3);

        e.Graphics.DrawLine(
            pen,
            sidebar.Width - 2,
            0,
            sidebar.Width - 2,
            sidebar.Height
        );
    }

    private void SelectMenu(Button selected)
    {
        foreach (var row in nav.Controls.OfType<Panel>())
        {
            foreach (var button in row.Controls.OfType<Button>())
            {
                button.BackColor = Theme.Sky;
                button.ForeColor = Theme.Gray;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
            }

            row.BackColor = Theme.Sky;
        }

        selected.BackColor = Theme.Gray;
        selected.ForeColor = Color.White;

        selected.Parent!.BackColor = Theme.Gray;
        selected.Parent.Invalidate();
    }

    private void Open(Form form)
    {
        using (form)
        {
            form.ShowDialog(this);
        }
    }

    private void Placeholder(string text)
    {
        dashboard.Visible = false;

        pendingTitle.Text = text;

        pending.Visible = true;
        pending.BringToFront();
    }

    private void HomeClick(object? sender, EventArgs e)
    {
        SelectMenu(homeButton);
        Home();
    }

    private void EnrollClick(object? sender, EventArgs e)
    {
        SelectMenu(enrollButton);
        Open(new InscripcionForm(session));
    }

    private void RenewClick(object? sender, EventArgs e)
    {
        SelectMenu(renewButton);
        Placeholder("Reinscripción");
    }

    private void ReportsClick(object? sender, EventArgs e)
    {
        SelectMenu(reportsButton);
        Placeholder("Reportes");
    }

    private void DocumentsClick(object? sender, EventArgs e)
    {
        SelectMenu(documentsButton);
        Placeholder("Documentos");
    }

    private void StudentsClick(object? sender, EventArgs e)
    {
        SelectMenu(studentsButton);
        Open(new ConsultaA(session));
    }

    private void SendClick(object? sender, EventArgs e)
    {
        SelectMenu(sendButton);
        Placeholder("Envío de documentos");
    }

    private void UsersClick(object? sender, EventArgs e)
    {
        SelectMenu(usersButton);
        Open(new UsuariosForm(session));
    }

    private void PersonnelClick(object? sender, EventArgs e)
    {
        SelectMenu(personnelButton);
        Open(new PersonalForm(session));
    }

    private void AuditClick(object? sender, EventArgs e)
    {
        SelectMenu(auditButton);
        Open(new BitacoraForm(session));
    }

    private void PasswordClick(object? sender, EventArgs e)
    {
        SelectMenu(passwordButton);
        Open(new CambiarPasswordForm(session, false));
    }

    private void Home()
    {
        pending.Visible = false;
        dashboard.Visible = true;
        dashboard.BringToFront();

        // TÍTULO PRINCIPAL EN MAYÚSCULAS
        title.Text = session.IsAdmin
            ? "ADMINISTRADOR - CONTROL ESCOLAR"
            : session.IsSecretary
                ? "SECRETARÍA - CONTROL ESCOLAR"
                : $"{session.Role.ToUpperInvariant()} - CONTROL ESCOLAR";

        // INFORMACIÓN DEL PERFIL
        profileName.Text = session.Username;

        profileRole.Text = $"Rol\n{session.Role}";

        profileArea.Text = "Área\n" +
            (session.IsTeacher ? "Docencia" : "Control escolar");

        LoadProfile(profileName, profileMail);

        // ESTADÍSTICAS
        statistics.Visible = !session.IsTeacher;

        if (session.IsTeacher)
        {
            LoadTeacherDashboard();
            return;
        }


        enrolled.Text = "—";
        applicants.Text = "—";
        renewals.Text = "—";

        status.Text = "";
        status.ForeColor = Theme.Gray;

        try
        {
            using var c = Db.Connect();

            string Count(string sql)
            {
                using var cmd = Db.Sql(c, sql);

                return Convert.ToString(cmd.ExecuteScalar()) ?? "0";
            }

            // ALUMNOS INSCRITOS
            enrolled.Text = Count(
                "SELECT COUNT(DISTINCT Numero_control) " +
                "FROM Inscripcion " +
                "WHERE UPPER(estatus)='INSCRITO'"
            );

            // ASPIRANTES PENDIENTES
            applicants.Text = Count(
                "SELECT COUNT(*) FROM Aspirante a " +
                "JOIN Periodo_Escolar pe ON pe.ID_Periodo=a.periodo_fichas " +
                "WHERE pe.nombre_periodo=\u0027Agosto-Diciembre 2026\u0027 " +
                "AND NOT EXISTS (" +
                "SELECT 1 FROM Alumno al " +
                "WHERE al.ID_Aspirante=a.ID_Aspirante)"
            );

            // REINSCRIPCIONES
            renewals.Text = Count(
                "SELECT COUNT(*) FROM Inscripcion " +
                "WHERE UPPER(tipo_inscripcion) " +
                "IN ('REINSCRIPCION','REINSCRIPCIÓN')"
            );

        }
        catch
        {
            status.Text =
                "No se pudieron cargar los indicadores. " +
                "Revisa la conexión y vuelve a Inicio.";

            status.ForeColor = Color.DarkRed;
        }
    }

    private void LoadProfile(Label name, Label email)
    {
        try
        {
            using var c = Db.Connect();

            using var cmd = Db.Sql(c, """
                SELECT
                    COALESCE(
                        NULLIF(TRIM(CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)),''),
                        NULLIF(TRIM(CONCAT_WS(' ',p.nombre,p.apellido_paterno,p.apellido_materno)),''),
                        u.nombre_usuario
                    ),
                    COALESCE(
                        d.correo_institucional,
                        p.correo_institucional,
                        ''
                    )
                FROM Usuario u
                LEFT JOIN Docente d
                    ON d.ID_docente = u.ID_docente
                LEFT JOIN Personal_Administrativo p
                    ON p.num_administrativo = u.num_administrativo
                WHERE u.Id_usuario = @id
                """);

            Db.Param(cmd, "@id", session.Id);

            using var r = cmd.ExecuteReader();

            if (r.Read())
            {
                name.Text = r.GetString(0);
                name.Font = new Font("Segoe UI", name.Text.Length > 100 ? 10F : name.Text.Length > 55 ? 13F : 16F, FontStyle.Bold);

                email.Text =
                    session.Username + " · " + r.GetString(1);
            }
        }
        catch
        {
            email.Text = "No se pudo cargar el perfil.";
        }
    }

    private void SetupTeacherInterface()
    {
        title.Text = "DOCENTE";
        teacherNote.Visible = false;
        statistics.Visible = false;

        AddTeacherOption("\uE82D", "Materias y grupos",
            (_, _) => Open(new MateriasDocenteForm(session)));

        AddTeacherOption("\uE8A5", "Captura de calificaciones",
            (_, _) => Open(new CapturaCLForm(session)));

        AddTeacherOption("\uE81C", "Historial",
            (_, _) => Open(new HistorialdocForm(session)));

        teacherPanel = new Panel
        {
            Location = new Point(25, 365),
            Size = new Size(825, 455)
        };

        teacherSummary = new Label
        {
            Location = new Point(5, 0),
            Size = new Size(800, 35),
            ForeColor = Theme.Gray,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };

        teacherSubjects = new FlowLayoutPanel
        {
            Location = new Point(5, 40),
            Size = new Size(805, 112),
            WrapContents = false,
            AutoScroll = true
        };

        var heading = new Label
        {
            Text = "Últimas calificaciones registradas",
            Location = new Point(5, 157),
            Size = new Size(800, 38),
            ForeColor = Theme.Gray,
            Font = new Font("Segoe UI", 15, FontStyle.Bold)
        };

        teacherGrades = ComponentesVisuales.Grid(
            "Alumno",
            "Materia",
            "Unidad",
            "Calificación"
        );

        teacherGrades.Location = new Point(5, 200);
        teacherGrades.Size = new Size(805, 205);

        teacherPanel.Controls.AddRange(new Control[]
        {
        teacherSummary,
        teacherSubjects,
        heading,
        teacherGrades
        });

        dashboard.Controls.Add(teacherPanel);
    }



    private void AddTeacherOption(
        string glyph,
        string caption,
        EventHandler action)
    {
        var row = new PanelRedondeado
        {
            Size = new Size(322, 55),
            Margin = new Padding(12, 0, 0, 19),
            BackColor = Theme.Sky,
            Padding = new Padding(2)
        };

        var icon = new Label
        {
            Text = glyph,
            Location = new Point(6, 5),
            Size = new Size(45, 45),
            Font = new Font("Segoe MDL2 Assets", 20F),
            ForeColor = Color.Black,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand
        };

        var button = new Button
        {
            Text = caption,
            Location = new Point(52, 2),
            Size = new Size(265, 50),
            BackColor = Theme.Sky,
            ForeColor = Theme.Gray,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Cursor = Cursors.Hand
        };

        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = Theme.Gray;

        if (ModulosDocenteActivos)
        {
            button.Click += action;
            icon.Click += action;
        }
        else
        {
            button.Cursor = Cursors.Default;
            icon.Cursor = Cursors.Default;
            button.FlatAppearance.MouseOverBackColor = Theme.Sky;
        }
        row.Controls.Add(icon);
        row.Controls.Add(button);

        int idx = nav.Controls.GetChildIndex(passwordRow);
        nav.Controls.Add(row);
        nav.Controls.SetChildIndex(row, idx);
    }



    private void LoadTeacherDashboard()
    {
        if (teacherSubjects is null ||
            teacherGrades is null ||
            teacherSummary is null)
            return;

        teacherSubjects.Controls.Clear();
        teacherGrades.Rows.Clear();
        teacherSummary.Text = "Materias: 0  |  Grupos: 0";

        profileArea.Text = "Materias\n0";

        try
        {
            using var db = Db.Connect();
            Identidad.RequireActive(db, null, session);

            using var cmd = Db.Sql(db, """
            SELECT
                m.ID_Materia,
                m.nombre_materia,
                g.ID_grupo,
                g.nombre_grupo,
                (
                    SELECT COUNT(DISTINCT i.Numero_control)
                    FROM Inscripcion i
                    WHERE i.ID_grupo = g.ID_grupo
                      AND UPPER(i.estatus) = 'INSCRITO'
                )
            FROM Usuario u
            JOIN Asignacion_Docente ad
                ON ad.ID_docente = u.ID_docente
            JOIN Materia m
                ON m.ID_Materia = ad.ID_Materia
            JOIN Grupo g
                ON g.ID_grupo = ad.ID_grupo
            WHERE u.Id_usuario = @id
            ORDER BY m.nombre_materia, g.nombre_grupo
            """);

            Db.Param(cmd, "@id", session.Id);

            var materias = new HashSet<int>();
            var grupos = new HashSet<int>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    materias.Add(reader.GetInt32(0));
                    grupos.Add(reader.GetInt32(2));

                    var card = new PanelRedondeado
                    {
                        Size = new Size(250, 82),
                        Margin = new Padding(6),
                        BackColor = Theme.Gray,
                        BorderColor = Theme.Green
                    };

                    card.Controls.Add(new Label
                    {
                        Dock = DockStyle.Fill,
                        Padding = new Padding(8),
                        Font = new Font(
                            "Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoEllipsis = true,
                        Text = reader.GetString(1) + "\n" +
                               reader.GetString(3) + " - " +
                               Convert.ToInt32(reader.GetValue(4)) +
                               " alumnos"
                    });

                    teacherSubjects.Controls.Add(card);
                }
            }

            teacherSummary.Text =
                $"Materias: {materias.Count}  |  Grupos: {grupos.Count}";

            profileArea.Text =
                $"Materias: {materias.Count}\nGrupos: {grupos.Count}";

            using var latest = Db.Sql(db, """
            SELECT
                CONCAT(
                    a.apellido_paterno, ' ',
                    a.apellido_materno, ' ',
                    a.nombre
                ),
                m.nombre_materia,
                ue.numero_unidad,
                c.calificacion
            FROM Calificacion c
            JOIN Asignacion_Docente ad
                ON ad.Id_asignacion = c.Id_asignacion
            JOIN Usuario u
                ON u.ID_docente = ad.ID_docente
            JOIN Alumno a
                ON a.Numero_control = c.numero_control
            JOIN Materia m
                ON m.ID_Materia = ad.ID_Materia
            JOIN Unidad_Evaluacion ue
                ON ue.ID_Unidad = c.ID_Unidad
            WHERE u.Id_usuario = @id
            ORDER BY c.fecha_registro DESC,
                     c.Id_calificacion DESC
            LIMIT 10
            """);

            Db.Param(latest, "@id", session.Id);

            using var grades = latest.ExecuteReader();

            while (grades.Read())
            {
                teacherGrades.Rows.Add(
                    grades.GetString(0),
                    grades.GetString(1),
                    grades.GetInt32(2),
                    grades.GetDecimal(3).ToString("0.##")
                );
            }

            if (materias.Count == 0)
            {
                teacherSummary.Text +=
                    "  ·  Sin asignaciones registradas";
            }
        }
        catch (Exception ex)
        {
            teacherSummary.Text =
                "No se pudieron cargar las asignaciones: " +
                ex.Message;

            teacherSummary.ForeColor = Color.DarkRed;
        }
    }

    private void title_Click(object sender, EventArgs e)
    {
    }
}
