
namespace AMBAR.Semana4;

public partial class MainForm : Form
{
    private Session session = null!;

    public MainForm()
    {
        InitializeComponent();
    }

    internal MainForm(Session logged) : this()
    {
        session = logged;

        enrollRow.Visible = session.CanSeeStudents;
        renewRow.Visible = session.CanSeeStudents;
        reportsRow.Visible = session.CanSeeStudents;
        documentsRow.Visible = session.CanSeeStudents;
        studentsRow.Visible = session.CanSeeStudents;
        sendRow.Visible = session.CanSeeStudents;

        usersRow.Visible = session.IsAdmin;
        auditRow.Visible = session.IsAdmin;

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
        nav.Visible = !nav.Visible;
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
        Open(new EnrollmentForm(session));
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
        Open(new StudentsForm(session));
    }

    private void SendClick(object? sender, EventArgs e)
    {
        SelectMenu(sendButton);
        Placeholder("Envío de documentos");
    }

    private void UsersClick(object? sender, EventArgs e)
    {
        SelectMenu(usersButton);
        Open(new UsersForm(session));
    }

    private void AuditClick(object? sender, EventArgs e)
    {
        SelectMenu(auditButton);
        Placeholder("Bitácora");
    }

    private void PasswordClick(object? sender, EventArgs e)
    {
        SelectMenu(passwordButton);
        Open(new ChangePasswordForm(session, false));
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
            return;

        grid.Rows.Clear();

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
                "WHERE NOT EXISTS (" +
                "SELECT 1 FROM Alumno al " +
                "WHERE al.ID_Aspirante=a.ID_Aspirante)"
            );

            // REINSCRIPCIONES
            renewals.Text = Count(
                "SELECT COUNT(*) FROM Inscripcion " +
                "WHERE UPPER(tipo_inscripcion) " +
                "IN ('REINSCRIPCION','REINSCRIPCIÓN')"
            );

            // ACTIVIDAD RECIENTE
            using var cmd = Db.Sql(c, """
                SELECT
                    b.fecha_hora,
                    u.nombre_usuario,
                    b.descripcion
                FROM Bitacora b
                JOIN Usuario u
                    ON u.Id_usuario = b.Id_usuario
                ORDER BY
                    b.fecha_hora DESC,
                    b.Id_bitacora DESC
                LIMIT 10
                """);

            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                grid.Rows.Add(
                    r.GetDateTime(0).ToString("dd/MM · HH:mm"),
                    r.GetString(1),
                    r.GetString(2)
                );
            }

            status.Text = grid.Rows.Count == 0
                ? "Sin actividad registrada. Indicadores acumulados."
                : "Indicadores acumulados · Últimas 10 actividades";
        }
        catch
        {
            status.Text =
                "No se pudieron cargar los indicadores y la actividad. " +
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
                        d.nombre,
                        p.nombre,
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

                email.Text =
                    session.Username + " · " + r.GetString(1);
            }
        }
        catch
        {
            email.Text = "No se pudo cargar el perfil.";
        }
    }

    private void title_Click(object sender, EventArgs e)
    {
    }
}

