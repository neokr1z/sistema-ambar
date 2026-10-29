#nullable enable
namespace AMBAR.Semana4;

partial class UsuariosForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsuariosForm));
        body = new Panel();
        label2 = new Label();
        label1 = new Label();
        birthInput = new DateTimePicker();
        curpInput = new TextBox();
        roleLabel = new Label();
        role = new ComboBox();
        rfcLabel = new Label();
        rfc = new TextBox();
        firstLabel = new Label();
        first = new TextBox();
        paternalLabel = new Label();
        paternal = new TextBox();
        maternalLabel = new Label();
        maternal = new TextBox();
        phoneLabel = new Label();
        phone = new TextBox();
        emailLabel = new Label();
        email = new TextBox();
        idNumberLabel = new Label();
        idNumber = new TextBox();
        usernameLabel = new Label();
        username = new TextBox();
        pwdLabel = new Label();
        pwd = new TextBox();
        confirmLabel = new Label();
        confirm = new TextBox();
        personalTitle = new Label();
        personalLine = new Panel();
        accessTitle = new Label();
        accessLine = new Panel();
        idHint = new Label();
        actions = new Panel();
        cancel = new Button();
        save = new Button();
        header = new Panel();
        menuButton = new Button();
        heading = new Label();
        logo = new PictureBox();
        body.SuspendLayout();
        actions.SuspendLayout();
        header.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        SuspendLayout();
        // 
        // body
        // 
        body.AutoScroll = true;
        body.AutoScrollMinSize = new Size(1150, 630);
        body.BackColor = Color.White;
        body.Controls.Add(label2);
        body.Controls.Add(label1);
        body.Controls.Add(birthInput);
        body.Controls.Add(curpInput);
        body.Controls.Add(roleLabel);
        body.Controls.Add(role);
        body.Controls.Add(rfcLabel);
        body.Controls.Add(rfc);
        body.Controls.Add(firstLabel);
        body.Controls.Add(first);
        body.Controls.Add(paternalLabel);
        body.Controls.Add(paternal);
        body.Controls.Add(maternalLabel);
        body.Controls.Add(maternal);
        body.Controls.Add(phoneLabel);
        body.Controls.Add(phone);
        body.Controls.Add(emailLabel);
        body.Controls.Add(email);
        body.Controls.Add(idNumberLabel);
        body.Controls.Add(idNumber);
        body.Controls.Add(usernameLabel);
        body.Controls.Add(username);
        body.Controls.Add(pwdLabel);
        body.Controls.Add(pwd);
        body.Controls.Add(confirmLabel);
        body.Controls.Add(confirm);
        body.Controls.Add(personalTitle);
        body.Controls.Add(personalLine);
        body.Controls.Add(accessTitle);
        body.Controls.Add(accessLine);
        body.Controls.Add(idHint);
        body.Dock = DockStyle.Fill;
        body.Location = new Point(0, 98);
        body.Name = "body";
        body.Size = new Size(1190, 606);
        body.TabIndex = 0;
        // 
        // label2
        // 
        label2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(6, 55, 99);
        label2.Location = new Point(560, 324);
        label2.Name = "label2";
        label2.Size = new Size(195, 34);
        label2.TabIndex = 17;
        label2.Text = "Fecha de nacimiento";
        label2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(6, 55, 99);
        label1.Location = new Point(48, 325);
        label1.Name = "label1";
        label1.Size = new Size(195, 34);
        label1.TabIndex = 16;
        label1.Text = "CURP";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // birthInput
        // 
        birthInput.Checked = false;
        birthInput.Format = DateTimePickerFormat.Short;
        birthInput.Location = new Point(845, 328);
        birthInput.MinDate = new DateTime(1926, 1, 1, 0, 0, 0, 0);
        birthInput.Name = "birthInput";
        birthInput.ShowCheckBox = true;
        birthInput.Size = new Size(290, 30);
        birthInput.TabIndex = 10;
        // 
        // curpInput
        // 
        curpInput.CharacterCasing = CharacterCasing.Upper;
        curpInput.Location = new Point(255, 328);
        curpInput.MaxLength = 18;
        curpInput.Name = "curpInput";
        curpInput.Size = new Size(265, 30);
        curpInput.TabIndex = 9;
        // 
        // roleLabel
        // 
        roleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        roleLabel.ForeColor = Color.FromArgb(6, 55, 99);
        roleLabel.Location = new Point(49, 86);
        roleLabel.Name = "roleLabel";
        roleLabel.Size = new Size(195, 34);
        roleLabel.TabIndex = 0;
        roleLabel.Text = "Tipo de usuario";
        roleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // role
        // 
        role.DropDownStyle = ComboBoxStyle.DropDownList;
        role.Font = new Font("Segoe UI", 10.5F);
        role.Items.AddRange(new object[] { "Docente", "Secretaria" });
        role.Location = new Point(255, 88);
        role.Name = "role";
        role.Size = new Size(265, 31);
        role.TabIndex = 0;
        role.SelectedIndexChanged += RoleChanged;
        // 
        // rfcLabel
        // 
        rfcLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        rfcLabel.ForeColor = Color.FromArgb(6, 55, 99);
        rfcLabel.Location = new Point(560, 86);
        rfcLabel.Name = "rfcLabel";
        rfcLabel.Size = new Size(280, 34);
        rfcLabel.TabIndex = 1;
        rfcLabel.Text = "RFC (13 caracteres)";
        rfcLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // rfc
        // 
        rfc.BorderStyle = BorderStyle.FixedSingle;
        rfc.CharacterCasing = CharacterCasing.Upper;
        rfc.Font = new Font("Segoe UI", 10.5F);
        rfc.Location = new Point(845, 88);
        rfc.MaxLength = 13;
        rfc.Name = "rfc";
        rfc.Size = new Size(290, 31);
        rfc.TabIndex = 1;
        // 
        // firstLabel
        // 
        firstLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        firstLabel.ForeColor = Color.FromArgb(6, 55, 99);
        firstLabel.Location = new Point(49, 151);
        firstLabel.Name = "firstLabel";
        firstLabel.Size = new Size(195, 34);
        firstLabel.TabIndex = 2;
        firstLabel.Text = "Nombre(s)";
        firstLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // first
        // 
        first.BorderStyle = BorderStyle.FixedSingle;
        first.Font = new Font("Segoe UI", 10.5F);
        first.Location = new Point(255, 153);
        first.MaxLength = 60;
        first.Name = "first";
        first.Size = new Size(265, 31);
        first.TabIndex = 2;
        // 
        // paternalLabel
        // 
        paternalLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        paternalLabel.ForeColor = Color.FromArgb(6, 55, 99);
        paternalLabel.Location = new Point(560, 151);
        paternalLabel.Name = "paternalLabel";
        paternalLabel.Size = new Size(280, 34);
        paternalLabel.TabIndex = 3;
        paternalLabel.Text = "Apellido paterno";
        paternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // paternal
        // 
        paternal.BorderStyle = BorderStyle.FixedSingle;
        paternal.Font = new Font("Segoe UI", 10.5F);
        paternal.Location = new Point(845, 153);
        paternal.MaxLength = 50;
        paternal.Name = "paternal";
        paternal.Size = new Size(290, 31);
        paternal.TabIndex = 3;
        // 
        // maternalLabel
        // 
        maternalLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        maternalLabel.ForeColor = Color.FromArgb(6, 55, 99);
        maternalLabel.Location = new Point(49, 216);
        maternalLabel.Name = "maternalLabel";
        maternalLabel.Size = new Size(195, 34);
        maternalLabel.TabIndex = 4;
        maternalLabel.Text = "Apellido materno";
        maternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // maternal
        // 
        maternal.BorderStyle = BorderStyle.FixedSingle;
        maternal.Font = new Font("Segoe UI", 10.5F);
        maternal.Location = new Point(255, 218);
        maternal.MaxLength = 50;
        maternal.Name = "maternal";
        maternal.Size = new Size(265, 31);
        maternal.TabIndex = 4;
        // 
        // phoneLabel
        // 
        phoneLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        phoneLabel.ForeColor = Color.FromArgb(6, 55, 99);
        phoneLabel.Location = new Point(560, 216);
        phoneLabel.Name = "phoneLabel";
        phoneLabel.Size = new Size(280, 34);
        phoneLabel.TabIndex = 5;
        phoneLabel.Text = "Teléfono (opcional)";
        phoneLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // phone
        // 
        phone.BorderStyle = BorderStyle.FixedSingle;
        phone.Font = new Font("Segoe UI", 10.5F);
        phone.Location = new Point(845, 218);
        phone.MaxLength = 25;
        phone.Name = "phone";
        phone.Size = new Size(290, 31);
        phone.TabIndex = 5;
        // 
        // emailLabel
        // 
        emailLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        emailLabel.ForeColor = Color.FromArgb(6, 55, 99);
        emailLabel.Location = new Point(49, 281);
        emailLabel.Name = "emailLabel";
        emailLabel.Size = new Size(195, 34);
        emailLabel.TabIndex = 6;
        emailLabel.Text = "Correo institucional";
        emailLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // email
        // 
        email.BorderStyle = BorderStyle.FixedSingle;
        email.Font = new Font("Segoe UI", 10.5F);
        email.Location = new Point(255, 283);
        email.MaxLength = 120;
        email.Name = "email";
        email.ReadOnly = true;
        email.Size = new Size(265, 31);
        email.TabIndex = 6;
        // 
        // idNumberLabel
        // 
        idNumberLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        idNumberLabel.ForeColor = Color.FromArgb(6, 55, 99);
        idNumberLabel.Location = new Point(560, 281);
        idNumberLabel.Name = "idNumberLabel";
        idNumberLabel.Size = new Size(280, 34);
        idNumberLabel.TabIndex = 7;
        idNumberLabel.Text = "Número administrativo (Secretaría)";
        idNumberLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // idNumber
        // 
        idNumber.BorderStyle = BorderStyle.FixedSingle;
        idNumber.Enabled = false;
        idNumber.Font = new Font("Segoe UI", 10.5F);
        idNumber.Location = new Point(845, 283);
        idNumber.MaxLength = 20;
        idNumber.Name = "idNumber";
        idNumber.ReadOnly = true;
        idNumber.Size = new Size(290, 31);
        idNumber.TabIndex = 7;
        // 
        // usernameLabel
        // 
        usernameLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        usernameLabel.ForeColor = Color.FromArgb(6, 55, 99);
        usernameLabel.Location = new Point(49, 433);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new Size(195, 34);
        usernameLabel.TabIndex = 8;
        usernameLabel.Text = "Usuario para iniciar sesión";
        usernameLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // username
        // 
        username.BorderStyle = BorderStyle.FixedSingle;
        username.Font = new Font("Segoe UI", 10.5F);
        username.Location = new Point(255, 435);
        username.MaxLength = 50;
        username.Name = "username";
        username.Size = new Size(265, 31);
        username.TabIndex = 8;
        // 
        // pwdLabel
        // 
        pwdLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        pwdLabel.ForeColor = Color.FromArgb(6, 55, 99);
        pwdLabel.Location = new Point(560, 433);
        pwdLabel.Name = "pwdLabel";
        pwdLabel.Size = new Size(280, 34);
        pwdLabel.TabIndex = 9;
        pwdLabel.Text = "Contraseña";
        pwdLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pwd
        // 
        pwd.BorderStyle = BorderStyle.FixedSingle;
        pwd.Font = new Font("Segoe UI", 10.5F);
        pwd.Location = new Point(845, 435);
        pwd.MaxLength = 150;
        pwd.Name = "pwd";
        pwd.Size = new Size(290, 31);
        pwd.TabIndex = 9;
        pwd.UseSystemPasswordChar = true;
        // 
        // confirmLabel
        // 
        confirmLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        confirmLabel.ForeColor = Color.FromArgb(6, 55, 99);
        confirmLabel.Location = new Point(49, 498);
        confirmLabel.Name = "confirmLabel";
        confirmLabel.Size = new Size(195, 34);
        confirmLabel.TabIndex = 10;
        confirmLabel.Text = "Confirmar contraseña";
        confirmLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // confirm
        // 
        confirm.BorderStyle = BorderStyle.FixedSingle;
        confirm.Font = new Font("Segoe UI", 10.5F);
        confirm.Location = new Point(255, 500);
        confirm.MaxLength = 150;
        confirm.Name = "confirm";
        confirm.Size = new Size(265, 31);
        confirm.TabIndex = 10;
        confirm.UseSystemPasswordChar = true;
        // 
        // personalTitle
        // 
        personalTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        personalTitle.ForeColor = Color.FromArgb(6, 55, 99);
        personalTitle.Location = new Point(48, 20);
        personalTitle.Name = "personalTitle";
        personalTitle.Size = new Size(500, 39);
        personalTitle.TabIndex = 11;
        personalTitle.Text = "Datos personales";
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(48, 60);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 12;
        // 
        // accessTitle
        // 
        accessTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        accessTitle.ForeColor = Color.FromArgb(6, 55, 99);
        accessTitle.Location = new Point(48, 366);
        accessTitle.Name = "accessTitle";
        accessTitle.Size = new Size(500, 39);
        accessTitle.TabIndex = 13;
        accessTitle.Text = "Datos de acceso";
        // 
        // accessLine
        // 
        accessLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        accessLine.BackColor = Color.FromArgb(8, 84, 143);
        accessLine.Location = new Point(48, 406);
        accessLine.Name = "accessLine";
        accessLine.Size = new Size(1109, 2);
        accessLine.TabIndex = 14;
        // 
        // idHint
        // 
        idHint.Font = new Font("Segoe UI", 9F);
        idHint.ForeColor = Color.FromArgb(6, 55, 99);
        idHint.Location = new Point(49, 570);
        idHint.Name = "idHint";
        idHint.Size = new Size(900, 35);
        idHint.TabIndex = 15;
        idHint.Text = "RFC obligatorio para docentes y secretarias (13 caracteres).";
        // 
        // actions
        // 
        actions.BackColor = Color.White;
        actions.Controls.Add(cancel);
        actions.Controls.Add(save);
        actions.Dock = DockStyle.Bottom;
        actions.Location = new Point(0, 704);
        actions.Name = "actions";
        actions.Size = new Size(1190, 76);
        actions.TabIndex = 1;
        actions.Resize += ActionsResize;
        // 
        // cancel
        // 
        cancel.BackColor = Color.FromArgb(6, 55, 99);
        cancel.Cursor = Cursors.Hand;
        cancel.DialogResult = DialogResult.Cancel;
        cancel.FlatAppearance.BorderSize = 0;
        cancel.FlatStyle = FlatStyle.Flat;
        cancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        cancel.ForeColor = Color.White;
        cancel.Location = new Point(800, 16);
        cancel.Name = "cancel";
        cancel.Size = new Size(150, 44);
        cancel.TabIndex = 13;
        cancel.Text = "Cancelar";
        cancel.UseVisualStyleBackColor = false;
        // 
        // save
        // 
        save.BackColor = Color.FromArgb(6, 55, 99);
        save.Cursor = Cursors.Hand;
        save.FlatAppearance.BorderSize = 0;
        save.FlatStyle = FlatStyle.Flat;
        save.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        save.ForeColor = Color.White;
        save.Location = new Point(970, 16);
        save.Name = "save";
        save.Size = new Size(174, 44);
        save.TabIndex = 12;
        save.Text = "Crear usuario";
        save.UseVisualStyleBackColor = false;
        save.Click += Save;
        // 
        // header
        // 
        header.BackColor = Color.White;
        header.Controls.Add(menuButton);
        header.Controls.Add(heading);
        header.Controls.Add(logo);
        header.Dock = DockStyle.Top;
        header.Location = new Point(0, 0);
        header.Name = "header";
        header.Size = new Size(1190, 98);
        header.TabIndex = 2;
        header.Resize += HeaderResize;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 17);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 4;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(640, 20);
        heading.Name = "heading";
        heading.Size = new Size(515, 54);
        heading.TabIndex = 2;
        heading.Text = "ALTA DE DOCENTE Y SECRETARIA";
        heading.TextAlign = ContentAlignment.MiddleRight;
        // 
        // logo
        // 
        logo.Image = (Image)resources.GetObject("logo.Image");
        logo.Location = new Point(58, 0);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(224, 101);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 3;
        logo.TabStop = false;
        logo.Click += logo_Click;
        // 
        // UsuariosForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        CancelButton = cancel;
        ClientSize = new Size(1190, 780);
        Controls.Add(body);
        Controls.Add(actions);
        Controls.Add(header);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1120, 730);
        Name = "UsuariosForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Administrador | Alta de personal";
        body.ResumeLayout(false);
        body.PerformLayout();
        actions.ResumeLayout(false);
        header.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ResumeLayout(false);
    }

    private Panel body = null!;
    private Panel actions = null!;
    private Panel header = null!;
    private Button btnMenu = null!;
    private Label heading = null!;
    private Label personalTitle = null!;
    private Panel personalLine = null!;
    private DateTimePicker birthInput = null!;
    private TextBox curpInput = null!;
    private Label accessTitle = null!;
    private Panel accessLine = null!;
    private Label idHint = null!;
    private Button save = null!;
    private Button cancel = null!;
    private Label roleLabel = null!;
    private Label firstLabel = null!;
    private Label paternalLabel = null!;
    private Label maternalLabel = null!;
    private Label emailLabel = null!;
    private Label phoneLabel = null!;
    private Label rfcLabel = null!;
    private Label idNumberLabel = null!;
    private Label usernameLabel = null!;
    private Label pwdLabel = null!;
    private Label confirmLabel = null!;
    private ComboBox role = null!;
    private TextBox first = null!;
    private TextBox paternal = null!;
    private TextBox maternal = null!;
    private TextBox email = null!;
    private TextBox phone = null!;
    private TextBox rfc = null!;
    private TextBox idNumber = null!;
    private TextBox username = null!;
    private TextBox pwd = null!;
    private TextBox confirm = null!;
    private PictureBox logo;
    private Label label2;
    private Label label1;
    private Button menuButton;
}
