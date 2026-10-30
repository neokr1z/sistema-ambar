#nullable disable
namespace AMBAR.Semana4;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        content = new Panel();
        dashboard = new Panel();
        title = new Label();
        profile = new PanelRedondeado();
        avatar = new Label();
        profileName = new Label();
        profileMail = new Label();
        profileRole = new Label();
        profileArea = new Label();
        statistics = new Panel();
        enrolledCard = new PanelRedondeado();
        enrolledCaption = new Label();
        enrolled = new Label();
        applicantsCard = new PanelRedondeado();
        applicantsCaption = new Label();
        applicants = new Label();
        renewalsCard = new PanelRedondeado();
        renewalsCaption = new Label();
        renewals = new Label();
        status = new Label();
        teacherNote = new Label();
        pending = new Panel();
        pendingTitle = new Label();
        pendingMessage = new Label();
        sidebar = new Panel();
        nav = new FlowLayoutPanel();
        homeRow = new PanelRedondeado();
        homeIcon = new Label();
        homeButton = new Button();
        enrollRow = new PanelRedondeado();
        enrollIcon = new Label();
        enrollButton = new Button();
        renewRow = new PanelRedondeado();
        renewIcon = new Label();
        renewButton = new Button();
        reportsRow = new PanelRedondeado();
        reportsIcon = new Label();
        reportsButton = new Button();
        documentsRow = new PanelRedondeado();
        documentsIcon = new Label();
        documentsButton = new Button();
        studentsRow = new PanelRedondeado();
        studentsIcon = new Label();
        studentsButton = new Button();
        sendRow = new PanelRedondeado();
        sendIcon = new Label();
        sendButton = new Button();
        usersRow = new PanelRedondeado();
        usersIcon = new Label();
        usersButton = new Button();
        personnelRow = new PanelRedondeado();
        personnelIcon = new Label();
        personnelButton = new Button();
        auditRow = new PanelRedondeado();
        auditIcon = new Label();
        auditButton = new Button();
        passwordRow = new PanelRedondeado();
        passwordIcon = new Label();
        passwordButton = new Button();
        logout = new Button();
        brand = new Panel();
        logo = new PictureBox();
        hamburger = new Button();
        content.SuspendLayout();
        dashboard.SuspendLayout();
        profile.SuspendLayout();
        statistics.SuspendLayout();
        enrolledCard.SuspendLayout();
        applicantsCard.SuspendLayout();
        renewalsCard.SuspendLayout();
        pending.SuspendLayout();
        sidebar.SuspendLayout();
        nav.SuspendLayout();
        homeRow.SuspendLayout();
        enrollRow.SuspendLayout();
        renewRow.SuspendLayout();
        reportsRow.SuspendLayout();
        documentsRow.SuspendLayout();
        studentsRow.SuspendLayout();
        sendRow.SuspendLayout();
        usersRow.SuspendLayout();
        personnelRow.SuspendLayout();
        auditRow.SuspendLayout();
        passwordRow.SuspendLayout();
        brand.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        teacherPanel = new Panel();
        teacherSubjects = new FlowLayoutPanel();
        teacherSummary = new Label();
        teacherHeading = new Label();
        teacherGrades = new DataGridView();
        teacherAlumno = new DataGridViewTextBoxColumn();
        teacherMateria = new DataGridViewTextBoxColumn();
        teacherUnidad = new DataGridViewTextBoxColumn();
        teacherNota = new DataGridViewTextBoxColumn();
        SuspendLayout();
        // 
        // content
        // 
        content.AutoScroll = true;
        content.Controls.Add(dashboard);
        content.Controls.Add(pending);
        content.Dock = DockStyle.Fill;
        content.Location = new Point(362, 0);
        content.Margin = new Padding(4);
        content.Name = "content";
        content.Size = new Size(888, 888);
        content.TabIndex = 0;
        // 
        // dashboard
        // 
        dashboard.Controls.Add(title);
        dashboard.Controls.Add(profile);
        dashboard.Controls.Add(statistics);
        dashboard.Controls.Add(teacherNote);
        dashboard.Dock = DockStyle.Top;
        dashboard.Location = new Point(0, 0);
        dashboard.Margin = new Padding(4);
        dashboard.Name = "dashboard";
        dashboard.Size = new Size(888, 862);
        dashboard.TabIndex = 0;
        // 
        // title
        // 
        title.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(6, 55, 99);
        title.Location = new Point(28, 22);
        title.Margin = new Padding(4, 0, 4, 0);
        title.Name = "title";
        title.Size = new Size(819, 65);
        title.TabIndex = 0;
        title.Text = "Administrador - Control escolar";
        title.TextAlign = ContentAlignment.MiddleRight;
        title.Click += title_Click;
        // 
        // profile
        // 
        profile.BorderColor = Color.FromArgb(6, 55, 99);
        profile.Controls.Add(avatar);
        profile.Controls.Add(profileName);
        profile.Controls.Add(profileMail);
        profile.Controls.Add(profileRole);
        profile.Controls.Add(profileArea);
        profile.Location = new Point(35, 131);
        profile.Margin = new Padding(4);
        profile.Name = "profile";
        profile.Padding = new Padding(2);
        profile.Size = new Size(812, 218);
        profile.TabIndex = 1;
        // 
        // avatar
        // 
        avatar.Font = new Font("Segoe MDL2 Assets", 70F);
        avatar.ForeColor = Color.Black;
        avatar.Location = new Point(20, 22);
        avatar.Margin = new Padding(4, 0, 4, 0);
        avatar.Name = "avatar";
        avatar.Size = new Size(180, 172);
        avatar.TabIndex = 0;
        avatar.Text = "";
        avatar.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // profileName
        // 
        profileName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        profileName.ForeColor = Color.FromArgb(61, 107, 134);
        profileName.Location = new Point(221, 8);
        profileName.Margin = new Padding(4, 0, 4, 0);
        profileName.Name = "profileName";
        profileName.Size = new Size(571, 68);
        profileName.TabIndex = 1;
        profileName.Text = "Nombre";
        profileName.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // profileMail
        // 
        profileMail.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        profileMail.ForeColor = Color.FromArgb(61, 107, 134);
        profileMail.Location = new Point(221, 82);
        profileMail.Margin = new Padding(4, 0, 4, 0);
        profileMail.Name = "profileMail";
        profileMail.Size = new Size(571, 55);
        profileMail.TabIndex = 2;
        profileMail.Text = "Clave · Correo institucional";
        profileMail.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // profileRole
        // 
        profileRole.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        profileRole.ForeColor = Color.FromArgb(61, 107, 134);
        profileRole.Location = new Point(221, 131);
        profileRole.Margin = new Padding(4, 0, 4, 0);
        profileRole.Name = "profileRole";
        profileRole.Size = new Size(231, 72);
        profileRole.TabIndex = 3;
        profileRole.Text = "Rol\nSecretaría";
        profileRole.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // profileArea
        // 
        profileArea.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        profileArea.ForeColor = Color.FromArgb(61, 107, 134);
        profileArea.Location = new Point(478, 131);
        profileArea.Margin = new Padding(4, 0, 4, 0);
        profileArea.Name = "profileArea";
        profileArea.Size = new Size(300, 72);
        profileArea.TabIndex = 4;
        profileArea.Text = "Área\nControl escolar";
        profileArea.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statistics
        // 
        statistics.Controls.Add(enrolledCard);
        statistics.Controls.Add(applicantsCard);
        statistics.Controls.Add(renewalsCard);
        statistics.Controls.Add(status);
        statistics.Location = new Point(0, 375);
        statistics.Margin = new Padding(4);
        statistics.Name = "statistics";
        statistics.Size = new Size(862, 488);
        statistics.TabIndex = 2;
        // 
        // enrolledCard
        // 
        enrolledCard.BackColor = Color.FromArgb(61, 107, 134);
        enrolledCard.BorderColor = Color.FromArgb(53, 151, 128);
        enrolledCard.Controls.Add(enrolledCaption);
        enrolledCard.Controls.Add(enrolled);
        enrolledCard.Location = new Point(56, 9);
        enrolledCard.Margin = new Padding(4);
        enrolledCard.Name = "enrolledCard";
        enrolledCard.Padding = new Padding(2);
        enrolledCard.Size = new Size(228, 130);
        enrolledCard.TabIndex = 0;
        // 
        // enrolledCaption
        // 
        enrolledCaption.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        enrolledCaption.ForeColor = Color.White;
        enrolledCaption.Location = new Point(10, 8);
        enrolledCaption.Margin = new Padding(4, 0, 4, 0);
        enrolledCaption.Name = "enrolledCaption";
        enrolledCaption.Size = new Size(208, 78);
        enrolledCaption.TabIndex = 0;
        enrolledCaption.Text = "Alumnos\ninscritos";
        enrolledCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // enrolled
        // 
        enrolled.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        enrolled.ForeColor = Color.White;
        enrolled.Location = new Point(10, 85);
        enrolled.Margin = new Padding(4, 0, 4, 0);
        enrolled.Name = "enrolled";
        enrolled.Size = new Size(208, 38);
        enrolled.TabIndex = 1;
        enrolled.Text = "—";
        enrolled.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // applicantsCard
        // 
        applicantsCard.BackColor = Color.FromArgb(61, 107, 134);
        applicantsCard.BorderColor = Color.FromArgb(53, 151, 128);
        applicantsCard.Controls.Add(applicantsCaption);
        applicantsCard.Controls.Add(applicants);
        applicantsCard.Location = new Point(314, 9);
        applicantsCard.Margin = new Padding(4);
        applicantsCard.Name = "applicantsCard";
        applicantsCard.Padding = new Padding(2);
        applicantsCard.Size = new Size(228, 130);
        applicantsCard.TabIndex = 1;
        // 
        // applicantsCaption
        // 
        applicantsCaption.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        applicantsCaption.ForeColor = Color.White;
        applicantsCaption.Location = new Point(10, 8);
        applicantsCaption.Margin = new Padding(4, 0, 4, 0);
        applicantsCaption.Name = "applicantsCaption";
        applicantsCaption.Size = new Size(208, 78);
        applicantsCaption.TabIndex = 0;
        applicantsCaption.Text = "Aspirantes\npendientes";
        applicantsCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // applicants
        // 
        applicants.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        applicants.ForeColor = Color.White;
        applicants.Location = new Point(10, 85);
        applicants.Margin = new Padding(4, 0, 4, 0);
        applicants.Name = "applicants";
        applicants.Size = new Size(208, 38);
        applicants.TabIndex = 1;
        applicants.Text = "—";
        applicants.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // renewalsCard
        // 
        renewalsCard.BackColor = Color.FromArgb(61, 107, 134);
        renewalsCard.BorderColor = Color.FromArgb(53, 151, 128);
        renewalsCard.Controls.Add(renewalsCaption);
        renewalsCard.Controls.Add(renewals);
        renewalsCard.Location = new Point(571, 9);
        renewalsCard.Margin = new Padding(4);
        renewalsCard.Name = "renewalsCard";
        renewalsCard.Padding = new Padding(2);
        renewalsCard.Size = new Size(228, 130);
        renewalsCard.TabIndex = 2;
        // 
        // renewalsCaption
        // 
        renewalsCaption.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        renewalsCaption.ForeColor = Color.White;
        renewalsCaption.Location = new Point(10, 8);
        renewalsCaption.Margin = new Padding(4, 0, 4, 0);
        renewalsCaption.Name = "renewalsCaption";
        renewalsCaption.Size = new Size(208, 78);
        renewalsCaption.TabIndex = 0;
        renewalsCaption.Text = "Reinscripciones";
        renewalsCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // renewals
        // 
        renewals.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        renewals.ForeColor = Color.White;
        renewals.Location = new Point(10, 85);
        renewals.Margin = new Padding(4, 0, 4, 0);
        renewals.Name = "renewals";
        renewals.Size = new Size(208, 38);
        renewals.TabIndex = 1;
        renewals.Text = "—";
        renewals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // status
        // 
        status.Font = new Font("Segoe UI", 10F);
        status.ForeColor = Color.FromArgb(61, 107, 134);
        status.Location = new Point(22, 154);
        status.Margin = new Padding(4, 0, 4, 0);
        status.Name = "status";
        status.Size = new Size(825, 60);
        status.TabIndex = 5;
        status.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // teacherNote
        // 
        teacherNote.Font = new Font("Segoe UI", 13F);
        teacherNote.ForeColor = Color.FromArgb(61, 107, 134);
        teacherNote.Location = new Point(35, 394);
        teacherNote.Margin = new Padding(4, 0, 4, 0);
        teacherNote.Name = "teacherNote";
        teacherNote.Size = new Size(800, 88);
        teacherNote.TabIndex = 3;
        teacherNote.Text = "Selecciona Captura o Historial de calificaciones en el menú.";
        teacherNote.TextAlign = ContentAlignment.MiddleLeft;
        teacherNote.Visible = false;
        // 
        // pending
        // 
        pending.Controls.Add(pendingTitle);
        pending.Controls.Add(pendingMessage);
        pending.Dock = DockStyle.Fill;
        pending.Location = new Point(0, 0);
        pending.Margin = new Padding(4);
        pending.Name = "pending";
        pending.Size = new Size(888, 888);
        pending.TabIndex = 1;
        pending.Visible = false;
        // 
        // pendingTitle
        // 
        pendingTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        pendingTitle.ForeColor = Color.FromArgb(61, 107, 134);
        pendingTitle.Location = new Point(22, 19);
        pendingTitle.Margin = new Padding(4, 0, 4, 0);
        pendingTitle.Name = "pendingTitle";
        pendingTitle.Size = new Size(800, 75);
        pendingTitle.TabIndex = 0;
        pendingTitle.Text = "Módulo pendiente";
        pendingTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pendingMessage
        // 
        pendingMessage.Font = new Font("Segoe UI", 13F);
        pendingMessage.ForeColor = Color.FromArgb(61, 107, 134);
        pendingMessage.Location = new Point(35, 188);
        pendingMessage.Margin = new Padding(4, 0, 4, 0);
        pendingMessage.Name = "pendingMessage";
        pendingMessage.Size = new Size(800, 188);
        pendingMessage.TabIndex = 1;
        pendingMessage.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // sidebar
        // 
        sidebar.Controls.Add(nav);
        sidebar.Controls.Add(logout);
        sidebar.Controls.Add(brand);
        sidebar.Dock = DockStyle.Left;
        sidebar.Location = new Point(0, 0);
        sidebar.Margin = new Padding(4);
        sidebar.Name = "sidebar";
        sidebar.Size = new Size(362, 888);
        sidebar.TabIndex = 1;
        sidebar.Paint += SidebarPaint;
        // 
        // nav
        // 
        nav.AutoScroll = true;
        nav.Controls.Add(homeRow);
        nav.Controls.Add(enrollRow);
        nav.Controls.Add(renewRow);
        nav.Controls.Add(reportsRow);
        nav.Controls.Add(documentsRow);
        nav.Controls.Add(studentsRow);
        nav.Controls.Add(sendRow);
        nav.Controls.Add(usersRow);
        nav.Controls.Add(personnelRow);
        nav.Controls.Add(auditRow);
        nav.Controls.Add(passwordRow);
        nav.Dock = DockStyle.Fill;
        nav.FlowDirection = FlowDirection.TopDown;
        nav.Location = new Point(0, 138);
        nav.Margin = new Padding(4);
        nav.Name = "nav";
        nav.Padding = new Padding(12, 30, 8, 35);
        nav.Size = new Size(362, 692);
        nav.TabIndex = 0;
        nav.WrapContents = false;
        // 
        // homeRow
        // 
        homeRow.BackColor = Color.FromArgb(180, 213, 234);
        homeRow.Controls.Add(homeIcon);
        homeRow.Controls.Add(homeButton);
        homeRow.Location = new Point(12, 30);
        homeRow.Margin = new Padding(0, 0, 0, 19);
        homeRow.Name = "homeRow";
        homeRow.Padding = new Padding(2);
        homeRow.Size = new Size(322, 55);
        homeRow.TabIndex = 0;
        // 
        // homeIcon
        // 
        homeIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        homeIcon.ForeColor = Color.Black;
        homeIcon.Location = new Point(6, 5);
        homeIcon.Margin = new Padding(4, 0, 4, 0);
        homeIcon.Name = "homeIcon";
        homeIcon.Size = new Size(45, 45);
        homeIcon.TabIndex = 0;
        homeIcon.Text = "";
        homeIcon.TextAlign = ContentAlignment.MiddleCenter;
        homeIcon.Click += HomeClick;
        // 
        // homeButton
        // 
        homeButton.BackColor = Color.FromArgb(180, 213, 234);
        homeButton.Cursor = Cursors.Hand;
        homeButton.FlatAppearance.BorderSize = 0;
        homeButton.FlatStyle = FlatStyle.Flat;
        homeButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        homeButton.ForeColor = Color.FromArgb(61, 107, 134);
        homeButton.Location = new Point(52, 2);
        homeButton.Margin = new Padding(4);
        homeButton.Name = "homeButton";
        homeButton.Size = new Size(265, 50);
        homeButton.TabIndex = 1;
        homeButton.Text = "Inicio";
        homeButton.TextAlign = ContentAlignment.MiddleLeft;
        homeButton.UseVisualStyleBackColor = false;
        homeButton.Click += HomeClick;
        // 
        // enrollRow
        // 
        enrollRow.BackColor = Color.FromArgb(180, 213, 234);
        enrollRow.Controls.Add(enrollIcon);
        enrollRow.Controls.Add(enrollButton);
        enrollRow.Location = new Point(12, 104);
        enrollRow.Margin = new Padding(0, 0, 0, 19);
        enrollRow.Name = "enrollRow";
        enrollRow.Padding = new Padding(2);
        enrollRow.Size = new Size(322, 55);
        enrollRow.TabIndex = 1;
        // 
        // enrollIcon
        // 
        enrollIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        enrollIcon.ForeColor = Color.Black;
        enrollIcon.Location = new Point(6, 5);
        enrollIcon.Margin = new Padding(4, 0, 4, 0);
        enrollIcon.Name = "enrollIcon";
        enrollIcon.Size = new Size(45, 45);
        enrollIcon.TabIndex = 0;
        enrollIcon.Text = "";
        enrollIcon.TextAlign = ContentAlignment.MiddleCenter;
        enrollIcon.Click += EnrollClick;
        // 
        // enrollButton
        // 
        enrollButton.BackColor = Color.FromArgb(180, 213, 234);
        enrollButton.Cursor = Cursors.Hand;
        enrollButton.FlatAppearance.BorderSize = 0;
        enrollButton.FlatStyle = FlatStyle.Flat;
        enrollButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        enrollButton.ForeColor = Color.FromArgb(61, 107, 134);
        enrollButton.Location = new Point(52, 2);
        enrollButton.Margin = new Padding(4);
        enrollButton.Name = "enrollButton";
        enrollButton.Size = new Size(265, 50);
        enrollButton.TabIndex = 1;
        enrollButton.Text = "Alta e inscripción";
        enrollButton.TextAlign = ContentAlignment.MiddleLeft;
        enrollButton.UseVisualStyleBackColor = false;
        enrollButton.Click += EnrollClick;
        // 
        // renewRow
        // 
        renewRow.BackColor = Color.FromArgb(180, 213, 234);
        renewRow.Controls.Add(renewIcon);
        renewRow.Controls.Add(renewButton);
        renewRow.Location = new Point(12, 178);
        renewRow.Margin = new Padding(0, 0, 0, 19);
        renewRow.Name = "renewRow";
        renewRow.Padding = new Padding(2);
        renewRow.Size = new Size(322, 55);
        renewRow.TabIndex = 2;
        // 
        // renewIcon
        // 
        renewIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        renewIcon.ForeColor = Color.Black;
        renewIcon.Location = new Point(6, 5);
        renewIcon.Margin = new Padding(4, 0, 4, 0);
        renewIcon.Name = "renewIcon";
        renewIcon.Size = new Size(45, 45);
        renewIcon.TabIndex = 0;
        renewIcon.Text = "";
        renewIcon.TextAlign = ContentAlignment.MiddleCenter;
        renewIcon.Click += RenewClick;
        // 
        // renewButton
        // 
        renewButton.BackColor = Color.FromArgb(180, 213, 234);
        renewButton.Cursor = Cursors.Hand;
        renewButton.FlatAppearance.BorderSize = 0;
        renewButton.FlatStyle = FlatStyle.Flat;
        renewButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        renewButton.ForeColor = Color.FromArgb(61, 107, 134);
        renewButton.Location = new Point(52, 2);
        renewButton.Margin = new Padding(4);
        renewButton.Name = "renewButton";
        renewButton.Size = new Size(265, 50);
        renewButton.TabIndex = 1;
        renewButton.Text = "Reinscripción";
        renewButton.TextAlign = ContentAlignment.MiddleLeft;
        renewButton.UseVisualStyleBackColor = false;
        renewButton.Click += RenewClick;
        // 
        // reportsRow
        // 
        reportsRow.BackColor = Color.FromArgb(180, 213, 234);
        reportsRow.Controls.Add(reportsIcon);
        reportsRow.Controls.Add(reportsButton);
        reportsRow.Location = new Point(12, 252);
        reportsRow.Margin = new Padding(0, 0, 0, 19);
        reportsRow.Name = "reportsRow";
        reportsRow.Padding = new Padding(2);
        reportsRow.Size = new Size(322, 55);
        reportsRow.TabIndex = 3;
        // 
        // reportsIcon
        // 
        reportsIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        reportsIcon.ForeColor = Color.Black;
        reportsIcon.Location = new Point(6, 5);
        reportsIcon.Margin = new Padding(4, 0, 4, 0);
        reportsIcon.Name = "reportsIcon";
        reportsIcon.Size = new Size(45, 45);
        reportsIcon.TabIndex = 0;
        reportsIcon.Text = "";
        reportsIcon.TextAlign = ContentAlignment.MiddleCenter;
        reportsIcon.Click += ReportsClick;
        // 
        // reportsButton
        // 
        reportsButton.BackColor = Color.FromArgb(180, 213, 234);
        reportsButton.Cursor = Cursors.Hand;
        reportsButton.FlatAppearance.BorderSize = 0;
        reportsButton.FlatStyle = FlatStyle.Flat;
        reportsButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        reportsButton.ForeColor = Color.FromArgb(61, 107, 134);
        reportsButton.Location = new Point(52, 2);
        reportsButton.Margin = new Padding(4);
        reportsButton.Name = "reportsButton";
        reportsButton.Size = new Size(265, 50);
        reportsButton.TabIndex = 1;
        reportsButton.Text = "Reportes";
        reportsButton.TextAlign = ContentAlignment.MiddleLeft;
        reportsButton.UseVisualStyleBackColor = false;
        reportsButton.Click += ReportsClick;
        // 
        // documentsRow
        // 
        documentsRow.BackColor = Color.FromArgb(180, 213, 234);
        documentsRow.Controls.Add(documentsIcon);
        documentsRow.Controls.Add(documentsButton);
        documentsRow.Location = new Point(12, 326);
        documentsRow.Margin = new Padding(0, 0, 0, 19);
        documentsRow.Name = "documentsRow";
        documentsRow.Padding = new Padding(2);
        documentsRow.Size = new Size(322, 55);
        documentsRow.TabIndex = 4;
        // 
        // documentsIcon
        // 
        documentsIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        documentsIcon.ForeColor = Color.Black;
        documentsIcon.Location = new Point(6, 5);
        documentsIcon.Margin = new Padding(4, 0, 4, 0);
        documentsIcon.Name = "documentsIcon";
        documentsIcon.Size = new Size(45, 45);
        documentsIcon.TabIndex = 0;
        documentsIcon.Text = "";
        documentsIcon.TextAlign = ContentAlignment.MiddleCenter;
        documentsIcon.Click += DocumentsClick;
        // 
        // documentsButton
        // 
        documentsButton.BackColor = Color.FromArgb(180, 213, 234);
        documentsButton.Cursor = Cursors.Hand;
        documentsButton.FlatAppearance.BorderSize = 0;
        documentsButton.FlatStyle = FlatStyle.Flat;
        documentsButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        documentsButton.ForeColor = Color.FromArgb(61, 107, 134);
        documentsButton.Location = new Point(52, 2);
        documentsButton.Margin = new Padding(4);
        documentsButton.Name = "documentsButton";
        documentsButton.Size = new Size(265, 50);
        documentsButton.TabIndex = 1;
        documentsButton.Text = "Documentos";
        documentsButton.TextAlign = ContentAlignment.MiddleLeft;
        documentsButton.UseVisualStyleBackColor = false;
        documentsButton.Click += DocumentsClick;
        // 
        // studentsRow
        // 
        studentsRow.BackColor = Color.FromArgb(180, 213, 234);
        studentsRow.Controls.Add(studentsIcon);
        studentsRow.Controls.Add(studentsButton);
        studentsRow.Location = new Point(12, 400);
        studentsRow.Margin = new Padding(0, 0, 0, 19);
        studentsRow.Name = "studentsRow";
        studentsRow.Padding = new Padding(2);
        studentsRow.Size = new Size(322, 55);
        studentsRow.TabIndex = 5;
        // 
        // studentsIcon
        // 
        studentsIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        studentsIcon.ForeColor = Color.Black;
        studentsIcon.Location = new Point(6, 5);
        studentsIcon.Margin = new Padding(4, 0, 4, 0);
        studentsIcon.Name = "studentsIcon";
        studentsIcon.Size = new Size(45, 45);
        studentsIcon.TabIndex = 0;
        studentsIcon.Text = "";
        studentsIcon.TextAlign = ContentAlignment.MiddleCenter;
        studentsIcon.Click += StudentsClick;
        // 
        // studentsButton
        // 
        studentsButton.BackColor = Color.FromArgb(180, 213, 234);
        studentsButton.Cursor = Cursors.Hand;
        studentsButton.FlatAppearance.BorderSize = 0;
        studentsButton.FlatStyle = FlatStyle.Flat;
        studentsButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        studentsButton.ForeColor = Color.FromArgb(61, 107, 134);
        studentsButton.Location = new Point(52, 2);
        studentsButton.Margin = new Padding(4);
        studentsButton.Name = "studentsButton";
        studentsButton.Size = new Size(265, 50);
        studentsButton.TabIndex = 1;
        studentsButton.Text = "Consulta de alumnos";
        studentsButton.TextAlign = ContentAlignment.MiddleLeft;
        studentsButton.UseVisualStyleBackColor = false;
        studentsButton.Click += StudentsClick;
        // 
        // sendRow
        // 
        sendRow.BackColor = Color.FromArgb(180, 213, 234);
        sendRow.Controls.Add(sendIcon);
        sendRow.Controls.Add(sendButton);
        sendRow.Location = new Point(12, 474);
        sendRow.Margin = new Padding(0, 0, 0, 19);
        sendRow.Name = "sendRow";
        sendRow.Padding = new Padding(2);
        sendRow.Size = new Size(322, 55);
        sendRow.TabIndex = 6;
        // 
        // sendIcon
        // 
        sendIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        sendIcon.ForeColor = Color.Black;
        sendIcon.Location = new Point(6, 5);
        sendIcon.Margin = new Padding(4, 0, 4, 0);
        sendIcon.Name = "sendIcon";
        sendIcon.Size = new Size(45, 45);
        sendIcon.TabIndex = 0;
        sendIcon.Text = "";
        sendIcon.TextAlign = ContentAlignment.MiddleCenter;
        sendIcon.Click += SendClick;
        // 
        // sendButton
        // 
        sendButton.BackColor = Color.FromArgb(180, 213, 234);
        sendButton.Cursor = Cursors.Hand;
        sendButton.FlatAppearance.BorderSize = 0;
        sendButton.FlatStyle = FlatStyle.Flat;
        sendButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        sendButton.ForeColor = Color.FromArgb(61, 107, 134);
        sendButton.Location = new Point(52, 2);
        sendButton.Margin = new Padding(4);
        sendButton.Name = "sendButton";
        sendButton.Size = new Size(265, 50);
        sendButton.TabIndex = 1;
        sendButton.Text = "Enviar documentos";
        sendButton.TextAlign = ContentAlignment.MiddleLeft;
        sendButton.UseVisualStyleBackColor = false;
        sendButton.Click += SendClick;
        // 
        // usersRow
        // 
        usersRow.BackColor = Color.FromArgb(180, 213, 234);
        usersRow.Controls.Add(usersIcon);
        usersRow.Controls.Add(usersButton);
        usersRow.Location = new Point(12, 548);
        usersRow.Margin = new Padding(0, 0, 0, 19);
        usersRow.Name = "usersRow";
        usersRow.Padding = new Padding(2);
        usersRow.Size = new Size(322, 55);
        usersRow.TabIndex = 10;
        // 
        // usersIcon
        // 
        usersIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        usersIcon.ForeColor = Color.Black;
        usersIcon.Location = new Point(6, 5);
        usersIcon.Margin = new Padding(4, 0, 4, 0);
        usersIcon.Name = "usersIcon";
        usersIcon.Size = new Size(45, 45);
        usersIcon.TabIndex = 0;
        usersIcon.Text = "";
        usersIcon.TextAlign = ContentAlignment.MiddleCenter;
        usersIcon.Click += UsersClick;
        // 
        // usersButton
        // 
        usersButton.BackColor = Color.FromArgb(180, 213, 234);
        usersButton.Cursor = Cursors.Hand;
        usersButton.FlatAppearance.BorderSize = 0;
        usersButton.FlatStyle = FlatStyle.Flat;
        usersButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        usersButton.ForeColor = Color.FromArgb(61, 107, 134);
        usersButton.Location = new Point(52, 2);
        usersButton.Margin = new Padding(4);
        usersButton.Name = "usersButton";
        usersButton.Size = new Size(265, 50);
        usersButton.TabIndex = 1;
        usersButton.Text = "Alta de personal";
        usersButton.TextAlign = ContentAlignment.MiddleLeft;
        usersButton.UseVisualStyleBackColor = false;
        usersButton.Click += UsersClick;
        // 
        // personnelRow
        // 
        personnelRow.BackColor = Color.FromArgb(180, 213, 234);
        personnelRow.Controls.Add(personnelIcon);
        personnelRow.Controls.Add(personnelButton);
        personnelRow.Location = new Point(12, 622);
        personnelRow.Margin = new Padding(0, 0, 0, 19);
        personnelRow.Name = "personnelRow";
        personnelRow.Padding = new Padding(2);
        personnelRow.Size = new Size(322, 55);
        personnelRow.TabIndex = 10;
        // 
        // personnelIcon
        // 
        personnelIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        personnelIcon.ForeColor = Color.Black;
        personnelIcon.Location = new Point(6, 5);
        personnelIcon.Margin = new Padding(4, 0, 4, 0);
        personnelIcon.Name = "personnelIcon";
        personnelIcon.Size = new Size(45, 45);
        personnelIcon.TabIndex = 0;
        personnelIcon.Text = "";
        personnelIcon.TextAlign = ContentAlignment.MiddleCenter;
        personnelIcon.Click += PersonnelClick;
        // 
        // personnelButton
        // 
        personnelButton.BackColor = Color.FromArgb(180, 213, 234);
        personnelButton.Cursor = Cursors.Hand;
        personnelButton.FlatAppearance.BorderSize = 0;
        personnelButton.FlatStyle = FlatStyle.Flat;
        personnelButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        personnelButton.ForeColor = Color.FromArgb(61, 107, 134);
        personnelButton.Location = new Point(52, 2);
        personnelButton.Margin = new Padding(4);
        personnelButton.Name = "personnelButton";
        personnelButton.Size = new Size(265, 50);
        personnelButton.TabIndex = 1;
        personnelButton.Text = "Consulta / baja de personal";
        personnelButton.TextAlign = ContentAlignment.MiddleLeft;
        personnelButton.UseVisualStyleBackColor = false;
        personnelButton.Click += PersonnelClick;
        // 
        // auditRow
        // 
        auditRow.BackColor = Color.FromArgb(180, 213, 234);
        auditRow.Controls.Add(auditIcon);
        auditRow.Controls.Add(auditButton);
        auditRow.Location = new Point(12, 696);
        auditRow.Margin = new Padding(0, 0, 0, 19);
        auditRow.Name = "auditRow";
        auditRow.Padding = new Padding(2);
        auditRow.Size = new Size(322, 55);
        auditRow.TabIndex = 11;
        // 
        // auditIcon
        // 
        auditIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        auditIcon.ForeColor = Color.Black;
        auditIcon.Location = new Point(6, 5);
        auditIcon.Margin = new Padding(4, 0, 4, 0);
        auditIcon.Name = "auditIcon";
        auditIcon.Size = new Size(45, 45);
        auditIcon.TabIndex = 0;
        auditIcon.Text = "";
        auditIcon.TextAlign = ContentAlignment.MiddleCenter;
        auditIcon.Click += AuditClick;
        // 
        // auditButton
        // 
        auditButton.BackColor = Color.FromArgb(180, 213, 234);
        auditButton.Cursor = Cursors.Hand;
        auditButton.FlatAppearance.BorderSize = 0;
        auditButton.FlatStyle = FlatStyle.Flat;
        auditButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        auditButton.ForeColor = Color.FromArgb(61, 107, 134);
        auditButton.Location = new Point(52, 2);
        auditButton.Margin = new Padding(4);
        auditButton.Name = "auditButton";
        auditButton.Size = new Size(265, 50);
        auditButton.TabIndex = 1;
        auditButton.Text = "Bitácora";
        auditButton.TextAlign = ContentAlignment.MiddleLeft;
        auditButton.UseVisualStyleBackColor = false;
        auditButton.Click += AuditClick;
        // 
        // passwordRow
        // 
        passwordRow.BackColor = Color.FromArgb(180, 213, 234);
        passwordRow.Controls.Add(passwordIcon);
        passwordRow.Controls.Add(passwordButton);
        passwordRow.Location = new Point(12, 770);
        passwordRow.Margin = new Padding(0, 0, 0, 45);
        passwordRow.Name = "passwordRow";
        passwordRow.Padding = new Padding(2);
        passwordRow.Size = new Size(322, 55);
        passwordRow.TabIndex = 12;
        // 
        // passwordIcon
        // 
        passwordIcon.Font = new Font("Segoe MDL2 Assets", 19F);
        passwordIcon.ForeColor = Color.Black;
        passwordIcon.Location = new Point(6, 5);
        passwordIcon.Margin = new Padding(4, 0, 4, 0);
        passwordIcon.Name = "passwordIcon";
        passwordIcon.Size = new Size(45, 45);
        passwordIcon.TabIndex = 0;
        passwordIcon.Text = "";
        passwordIcon.TextAlign = ContentAlignment.MiddleCenter;
        passwordIcon.Click += PasswordClick;
        // 
        // passwordButton
        // 
        passwordButton.BackColor = Color.FromArgb(180, 213, 234);
        passwordButton.Cursor = Cursors.Hand;
        passwordButton.FlatAppearance.BorderSize = 0;
        passwordButton.FlatStyle = FlatStyle.Flat;
        passwordButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        passwordButton.ForeColor = Color.FromArgb(61, 107, 134);
        passwordButton.Location = new Point(52, 2);
        passwordButton.Margin = new Padding(4);
        passwordButton.Name = "passwordButton";
        passwordButton.Size = new Size(265, 50);
        passwordButton.TabIndex = 1;
        passwordButton.Text = "Cambiar contraseña";
        passwordButton.TextAlign = ContentAlignment.MiddleLeft;
        passwordButton.UseVisualStyleBackColor = false;
        passwordButton.Click += PasswordClick;
        // 
        // logout
        // 
        logout.BackColor = Color.FromArgb(180, 213, 234);
        logout.Cursor = Cursors.Hand;
        logout.Dock = DockStyle.Bottom;
        logout.FlatStyle = FlatStyle.Flat;
        logout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        logout.ForeColor = Color.FromArgb(6, 55, 99);
        logout.Location = new Point(0, 830);
        logout.Margin = new Padding(4);
        logout.Name = "logout";
        logout.Size = new Size(362, 58);
        logout.TabIndex = 1;
        logout.Text = "⇥  Cerrar sesión";
        logout.UseVisualStyleBackColor = false;
        logout.Click += LogoutClick;
        // 
        // brand
        // 
        brand.Controls.Add(logo);
        brand.Controls.Add(hamburger);
        brand.Dock = DockStyle.Top;
        brand.Location = new Point(0, 0);
        brand.Margin = new Padding(4);
        brand.Name = "brand";
        brand.Size = new Size(362, 138);
        brand.TabIndex = 2;
        // 
        // logo
        // 
        logo.Image = (Image)resources.GetObject("logo.Image");
        logo.Location = new Point(64, 4);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(256, 118);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 0;
        logo.TabStop = false;
        // 
        // hamburger
        // 
        hamburger.BackColor = Color.White;
        hamburger.Cursor = Cursors.Hand;
        hamburger.FlatAppearance.BorderSize = 0;
        hamburger.FlatStyle = FlatStyle.Flat;
        hamburger.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        hamburger.ForeColor = Color.Black;
        hamburger.Location = new Point(4, 11);
        hamburger.Margin = new Padding(4);
        hamburger.Name = "hamburger";
        hamburger.Size = new Size(54, 70);
        hamburger.TabIndex = 1;
        hamburger.Text = "☰";
        hamburger.UseVisualStyleBackColor = false;
        hamburger.Visible = true;
        hamburger.Click += ToggleMenu;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1250, 888);
        Controls.Add(content);
        Controls.Add(sidebar);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4);
        MinimumSize = new Size(1266, 925);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AMBAR | Panel principal";
        content.ResumeLayout(false);
        dashboard.ResumeLayout(false);
        profile.ResumeLayout(false);
        statistics.ResumeLayout(false);
        enrolledCard.ResumeLayout(false);
        applicantsCard.ResumeLayout(false);
        renewalsCard.ResumeLayout(false);
        pending.ResumeLayout(false);
        sidebar.ResumeLayout(false);
        nav.ResumeLayout(false);
        homeRow.ResumeLayout(false);
        enrollRow.ResumeLayout(false);
        renewRow.ResumeLayout(false);
        reportsRow.ResumeLayout(false);
        documentsRow.ResumeLayout(false);
        studentsRow.ResumeLayout(false);
        sendRow.ResumeLayout(false);
        usersRow.ResumeLayout(false);
        personnelRow.ResumeLayout(false);
        auditRow.ResumeLayout(false);
        passwordRow.ResumeLayout(false);
        brand.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        teacherPanel.Name = "teacherPanel";
        teacherPanel.Location = new Point(25, 365);
        teacherPanel.Size = new Size(825, 455);
        teacherPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        teacherPanel.Visible = false;
        teacherSummary.Name = "teacherSummary";
        teacherSummary.Location = new Point(5, 0);
        teacherSummary.Size = new Size(800, 35);
        teacherSummary.ForeColor = Color.FromArgb(61, 107, 134);
        teacherSummary.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        teacherSubjects.Name = "teacherSubjects";
        teacherSubjects.Location = new Point(5, 40);
        teacherSubjects.Size = new Size(805, 112);
        teacherSubjects.WrapContents = false;
        teacherSubjects.AutoScroll = true;
        teacherHeading.Name = "teacherHeading";
        teacherHeading.Text = "Últimas calificaciones registradas";
        teacherHeading.Location = new Point(5, 157);
        teacherHeading.Size = new Size(800, 38);
        teacherHeading.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        teacherHeading.ForeColor = Color.FromArgb(61, 107, 134);
        teacherGrades.Name = "teacherGrades";
        teacherGrades.Location = new Point(5, 200);
        teacherGrades.Size = new Size(805, 205);
        teacherGrades.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        teacherGrades.ReadOnly = true;
        teacherGrades.AllowUserToAddRows = false;
        teacherGrades.AllowUserToDeleteRows = false;
        teacherGrades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        teacherGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        teacherAlumno.Name = "Alumno";
        teacherAlumno.HeaderText = "Alumno";
        teacherGrades.Columns.Add(teacherAlumno);
        teacherMateria.Name = "Materia";
        teacherMateria.HeaderText = "Materia";
        teacherGrades.Columns.Add(teacherMateria);
        teacherUnidad.Name = "Unidad";
        teacherUnidad.HeaderText = "Unidad";
        teacherGrades.Columns.Add(teacherUnidad);
        teacherNota.Name = "Calificación";
        teacherNota.HeaderText = "Calificación";
        teacherGrades.Columns.Add(teacherNota);
        teacherGrades.BackgroundColor = Color.White;
        teacherGrades.BorderStyle = BorderStyle.None;
        teacherGrades.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        teacherGrades.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        teacherGrades.EnableHeadersVisualStyles = false;
        teacherGrades.GridColor = Color.FromArgb(222, 233, 241);
        teacherGrades.RowHeadersVisible = false;
        teacherGrades.ColumnHeadersHeight = 48;
        teacherGrades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        teacherGrades.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(6, 55, 99);
        teacherGrades.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        teacherGrades.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(6, 55, 99);
        teacherGrades.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        teacherGrades.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        teacherGrades.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        teacherGrades.DefaultCellStyle.BackColor = Color.White;
        teacherGrades.DefaultCellStyle.ForeColor = Color.FromArgb(34, 49, 71);
        teacherGrades.DefaultCellStyle.SelectionBackColor = Color.FromArgb(215, 234, 247);
        teacherGrades.DefaultCellStyle.SelectionForeColor = Color.FromArgb(6, 55, 99);
        teacherGrades.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        teacherGrades.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        teacherGrades.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 246, 250);
        teacherGrades.RowTemplate.Height = 42;
        teacherPanel.Controls.Add(teacherSummary);
        teacherPanel.Controls.Add(teacherSubjects);
        teacherPanel.Controls.Add(teacherHeading);
        teacherPanel.Controls.Add(teacherGrades);
        dashboard.Controls.Add(teacherPanel);
        teacherGrades.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        teacherGrades.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        ResumeLayout(false);
    }

    private Panel content;
    private Panel sidebar;
    private FlowLayoutPanel nav;
    private Button logout;
    private Panel brand;
    private PictureBox logo;
    private Button hamburger;
    private PanelRedondeado homeRow;
    private Label homeIcon;
    private Button homeButton;
    private PanelRedondeado enrollRow;
    private Label enrollIcon;
    private Button enrollButton;
    private PanelRedondeado renewRow;
    private Label renewIcon;
    private Button renewButton;
    private PanelRedondeado reportsRow;
    private Label reportsIcon;
    private Button reportsButton;
    private PanelRedondeado documentsRow;
    private Label documentsIcon;
    private Button documentsButton;
    private PanelRedondeado studentsRow;
    private Label studentsIcon;
    private Button studentsButton;
    private PanelRedondeado sendRow;
    private Label sendIcon;
    private Button sendButton;
    private PanelRedondeado personnelRow;
    private Label personnelIcon;
    private Button personnelButton;
    private PanelRedondeado usersRow;
    private Label usersIcon;
    private Button usersButton;
    private PanelRedondeado auditRow;
    private Label auditIcon;
    private Button auditButton;
    private PanelRedondeado passwordRow;
    private Label passwordIcon;
    private Button passwordButton;
    private Panel dashboard;
    private Label title;
    private PanelRedondeado profile;
    private Label avatar;
    private Label profileName;
    private Label profileMail;
    private Label profileRole;
    private Label profileArea;
    private Panel statistics;
    private PanelRedondeado enrolledCard;
    private Label enrolledCaption;
    private Label enrolled;
    private PanelRedondeado applicantsCard;
    private Label applicantsCaption;
    private Label applicants;
    private PanelRedondeado renewalsCard;
    private Label renewalsCaption;
    private Label renewals;
    private Label status;
    private Label teacherNote;
    private Panel pending;
    private Label pendingTitle;
    private Label pendingMessage;
    private Panel teacherPanel = null!;
    private FlowLayoutPanel teacherSubjects = null!;
    private Label teacherSummary = null!;
    private Label teacherHeading = null!;
    private DataGridView teacherGrades = null!;
    private DataGridViewTextBoxColumn teacherAlumno = null!;
    private DataGridViewTextBoxColumn teacherMateria = null!;
    private DataGridViewTextBoxColumn teacherUnidad = null!;
    private DataGridViewTextBoxColumn teacherNota = null!;
}