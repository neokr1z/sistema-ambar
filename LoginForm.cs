namespace AMBAR.Semana4;

internal sealed class LoginForm : Form
{
    private readonly TextBox username=Theme.Input(50);
    private readonly TextBox password=Theme.Input(200,true);
    private readonly Label status=Theme.Text("Acceso exclusivo para personal autorizado",9,false,Theme.Gray);
    private readonly Button enter=Theme.Button("Iniciar sesión");

    public LoginForm()
    {
        Theme.Setup(this,"Acceso",460,660);
        MinimumSize=Size;
        MaximizeBox=false;
        FormBorderStyle=FormBorderStyle.FixedSingle;
        var card=new RoundedPanel { Bounds=new Rectangle(18,12,424,632), Radius=32,
            BorderColor=Theme.Blue, BackColor=Color.White };
        Controls.Add(card);
        var logo=Theme.Logo(345,145); logo.Location=new Point(35,15);
        card.Controls.Add(logo);
        ReferenceUi.Label(card,"Control escolar",70,154,284,34,17,true,ContentAlignment.MiddleCenter);
        ReferenceUi.Label(card,"Usuario:",38,228,330,26,12,true);
        ReferenceUi.Input(card,username,38,262,330);
        ReferenceUi.Label(card,"Contraseña:",38,337,330,26,12,true);
        ReferenceUi.Input(card,password,38,372,330);
        var register=new LinkLabel { Text="Registrarse",Bounds=new Rectangle(295,446,104,28),
            LinkColor=Theme.Gray,Font=new Font("Segoe UI",11),TabIndex=2 };
        register.Click+=(_,_)=>MessageBox.Show("Solicita tu cuenta al administrador. El alta de personal requiere una sesión autorizada.","Registro de personal");
        card.Controls.Add(register);
        enter.AutoSize=false; enter.Bounds=new Rectangle(80,494,270,50);
        ReferenceUi.Round(enter); enter.Font=new Font("Segoe UI",16,FontStyle.Bold);
        enter.Click+=TryLogin; enter.TabIndex=3; card.Controls.Add(enter);
        status.AutoSize=false; status.Bounds=new Rectangle(35,554,355,60);
        status.Text="El sistema detecta si eres docente o\npersonal administrativo.";
        status.TextAlign=ContentAlignment.MiddleCenter; card.Controls.Add(status);
        username.TabIndex=0;password.TabIndex=1;
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
