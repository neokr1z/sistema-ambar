namespace AMBAR.Semana4;

internal sealed class LoginForm : Form
{
    private readonly TextBox username=Theme.Input(50);
    private readonly TextBox password=Theme.Input(200,true);
    private readonly Label status=Theme.Text("Acceso exclusivo para personal autorizado",9,false,Theme.Gray);
    private readonly Button enter=Theme.Button("Iniciar sesión");

    public LoginForm()
    {
        Theme.Setup(this,"Acceso",880,630);
        MaximizeBox=false;
        var centered=new TableLayoutPanel { Dock=DockStyle.Fill, ColumnCount=3, RowCount=1 };
        centered.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,25));
        centered.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        centered.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,25));
        Controls.Add(centered);
        var panel=new FlowLayoutPanel { FlowDirection=FlowDirection.TopDown,WrapContents=false,
            AutoScroll=true,BackColor=Color.White, Dock=DockStyle.Fill,Padding=new Padding(30),
            Margin=new Padding(0,35,0,35) };
        centered.Controls.Add(panel,1,0);
        var logo=Theme.Logo(320,160); logo.Margin=new Padding(25,0,10,0);
        panel.Controls.Add(logo);
        panel.Controls.Add(Theme.Text("AMBAR · Control Escolar",18,true,Theme.Blue));
        panel.Controls.Add(Theme.Text("Instituto Tecnológico de Acapulco",10,false,Theme.Gray));
        panel.Controls.Add(Theme.Text("Usuario",10,true));
        panel.Controls.Add(username);
        panel.Controls.Add(Theme.Text("Contraseña",10,true));
        panel.Controls.Add(password);
        enter.Width=350;
        enter.Click += TryLogin;
        panel.Controls.Add(enter);
        status.MaximumSize=new Size(360,90);
        panel.Controls.Add(status);
        AcceptButton=enter;
    }

    private void TryLogin(object? sender, EventArgs args)
    {
        if(string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
        {
            status.Text="Escribe el usuario y la contraseña.";
            status.ForeColor=Color.DarkRed;
            return;
        }
        try
        {
            enter.Enabled=false;
            var session=Auth.Login(username.Text.Trim(),password.Text);
            if(session is null)
            {
                status.Text="Usuario o contraseña incorrectos, o cuenta inactiva.";
                status.ForeColor=Color.DarkRed;
                password.Clear();
                password.Focus();
                return;
            }
            if (session.RequiresPasswordChange)
            {
                MessageBox.Show("Debes cambiar la contraseña temporal del administrador antes de continuar.","Seguridad AMBAR");
                using var change = new ChangePasswordForm(session, true);
                if (change.ShowDialog(this)!=DialogResult.OK)
                {
                    status.Text="El cambio de contraseña es obligatorio para el primer acceso.";
                    return;
                }
            }
            status.ForeColor=Theme.Green;
            status.Text="Acceso autorizado.";
            Hide();
            try { using var main=new MainForm(session); main.ShowDialog(this); }
            finally { Show(); Activate(); password.Clear(); username.Focus(); }
        }
        catch(Exception ex) { Problems.Show(ex); status.Text="No fue posible iniciar sesión."; }
        finally { enter.Enabled=true; }
    }
}
