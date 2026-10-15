#nullable disable
namespace AMBAR.Semana4;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
        card = new RoundedPanel();
        logo = new PictureBox();
        title = new Label();
        usernameLabel = new Label();
        usernameBorder = new RoundedPanel();
        username = new TextBox();
        passwordLabel = new Label();
        passwordBorder = new RoundedPanel();
        password = new TextBox();
        register = new LinkLabel();
        enter = new RoundedButton();
        status = new Label();
        card.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        usernameBorder.SuspendLayout();
        passwordBorder.SuspendLayout();
        SuspendLayout();
        // 
        // card
        // 
        card.BorderColor = Color.FromArgb(6, 55, 99);
        card.Controls.Add(logo);
        card.Controls.Add(title);
        card.Controls.Add(usernameLabel);
        card.Controls.Add(usernameBorder);
        card.Controls.Add(passwordLabel);
        card.Controls.Add(passwordBorder);
        card.Controls.Add(register);
        card.Controls.Add(enter);
        card.Controls.Add(status);
        card.Location = new Point(22, 15);
        card.Margin = new Padding(4);
        card.Name = "card";
        card.Padding = new Padding(2);
        card.Radius = 32;
        card.Size = new Size(530, 790);
        card.TabIndex = 0;
        // 
        // logo
        // 
        logo.Image = (Image)resources.GetObject("logo.Image");
        logo.Location = new Point(6, 20);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(431, 181);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 0;
        logo.TabStop = false;
        // 
        // title
        // 
        title.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(61, 107, 134);
        title.Location = new Point(161, 193);
        title.Margin = new Padding(4, 0, 4, 0);
        title.Name = "title";
        title.Size = new Size(227, 42);
        title.TabIndex = 1;
        title.Text = "Control escolar";
        title.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // usernameLabel
        // 
        usernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        usernameLabel.ForeColor = Color.FromArgb(61, 107, 134);
        usernameLabel.Location = new Point(48, 285);
        usernameLabel.Margin = new Padding(4, 0, 4, 0);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new Size(412, 32);
        usernameLabel.TabIndex = 2;
        usernameLabel.Text = "Usuario:";
        usernameLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // usernameBorder
        // 
        usernameBorder.BorderColor = Color.FromArgb(6, 55, 99);
        usernameBorder.Controls.Add(username);
        usernameBorder.Location = new Point(48, 328);
        usernameBorder.Margin = new Padding(4);
        usernameBorder.Name = "usernameBorder";
        usernameBorder.Padding = new Padding(2);
        usernameBorder.Radius = 16;
        usernameBorder.Size = new Size(412, 60);
        usernameBorder.TabIndex = 3;
        // 
        // username
        // 
        username.BorderStyle = BorderStyle.None;
        username.Font = new Font("Segoe UI", 11F);
        username.Location = new Point(16, 16);
        username.Margin = new Padding(4);
        username.MaxLength = 50;
        username.Name = "username";
        username.Size = new Size(380, 25);
        username.TabIndex = 6;
        // 
        // passwordLabel
        // 
        passwordLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        passwordLabel.ForeColor = Color.FromArgb(61, 107, 134);
        passwordLabel.Location = new Point(48, 421);
        passwordLabel.Margin = new Padding(4, 0, 4, 0);
        passwordLabel.Name = "passwordLabel";
        passwordLabel.Size = new Size(412, 32);
        passwordLabel.TabIndex = 4;
        passwordLabel.Text = "Contraseña:";
        passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // passwordBorder
        // 
        passwordBorder.BorderColor = Color.FromArgb(6, 55, 99);
        passwordBorder.Controls.Add(password);
        passwordBorder.Location = new Point(48, 464);
        passwordBorder.Margin = new Padding(4);
        passwordBorder.Name = "passwordBorder";
        passwordBorder.Padding = new Padding(2);
        passwordBorder.Radius = 16;
        passwordBorder.Size = new Size(412, 60);
        passwordBorder.TabIndex = 5;
        // 
        // password
        // 
        password.BorderStyle = BorderStyle.None;
        password.Font = new Font("Segoe UI", 11F);
        password.Location = new Point(16, 16);
        password.Margin = new Padding(4);
        password.MaxLength = 200;
        password.Name = "password";
        password.Size = new Size(380, 25);
        password.TabIndex = 9;
        password.UseSystemPasswordChar = true;
        // 
        // register
        // 
        register.Location = new Point(369, 558);
        register.Margin = new Padding(4, 0, 4, 0);
        register.Name = "register";
        register.Size = new Size(130, 35);
        register.TabIndex = 6;
        register.TabStop = true;
        register.Text = "Registrarse";
        register.LinkClicked += RegisterClick;
        // 
        // enter
        // 
        enter.BackColor = Color.FromArgb(6, 55, 99);
        enter.Cursor = Cursors.Hand;
        enter.FlatStyle = FlatStyle.Flat;
        enter.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        enter.ForeColor = Color.White;
        enter.Location = new Point(148, 597);
        enter.Margin = new Padding(4);
        enter.Name = "enter";
        enter.Size = new Size(221, 62);
        enter.TabIndex = 7;
        enter.Text = "Iniciar sesión";
        enter.UseVisualStyleBackColor = false;
        enter.Click += TryLogin;
        // 
        // status
        // 
        status.Font = new Font("Segoe UI", 10F);
        status.ForeColor = Color.FromArgb(61, 107, 134);
        status.Location = new Point(44, 692);
        status.Margin = new Padding(4, 0, 4, 0);
        status.Name = "status";
        status.Size = new Size(444, 75);
        status.TabIndex = 8;
        status.Text = "El sistema detecta si eres docente o\npersonal administrativo.";
        status.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // LoginForm
        // 
        AcceptButton = enter;
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(575, 825);
        Controls.Add(card);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4);
        MaximizeBox = false;
        MinimumSize = new Size(570, 801);
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AMBAR | Acceso";
        card.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        usernameBorder.ResumeLayout(false);
        usernameBorder.PerformLayout();
        passwordBorder.ResumeLayout(false);
        passwordBorder.PerformLayout();
        ResumeLayout(false);
    }

    private RoundedPanel card;
    private PictureBox logo;
    private Label title;
    private Label usernameLabel;
    private RoundedPanel usernameBorder;
    private TextBox username;
    private Label passwordLabel;
    private RoundedPanel passwordBorder;
    private TextBox password;
    private LinkLabel register;
    private RoundedButton enter;
    private Label status;
}
