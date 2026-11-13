namespace AMBAR.Semana4;

public partial class LoginForm : Form
{

    public LoginForm() { InitializeComponent(); }

    private void RegisterClick(object? sender, LinkLabelLinkClickedEventArgs e)
        => MessageBox.Show("Solicita tu cuenta al administrador. El alta de personal requiere una sesión autorizada.", "Registro de personal");

    private void TryLogin(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
        {
            status.Text = "Escribe el usuario y la contraseña.";
            status.ForeColor = Color.DarkRed;
            return;
        }
        try
        {
            enter.Enabled = false;
            var session = Auth.Login(username.Text.Trim(), password.Text);
            if (session is null)
            {
                status.Text = "Usuario o contraseña incorrectos, o cuenta inactiva.";
                status.ForeColor = Color.DarkRed;
                password.Clear();
                password.Focus();
                return;
            }
            if (session.RequiresPasswordChange)
            {
                MessageBox.Show("Debes cambiar la contraseña temporal del administrador antes de continuar.", "Seguridad AMBAR");
                using var change = new CambiarPasswordForm(session, true);
                if (change.ShowDialog(this) != DialogResult.OK)
                {
                    status.Text = "El cambio de contraseña es obligatorio para el primer acceso.";
                    return;
                }
            }
            status.ForeColor = Theme.Green;
            status.Text = "Acceso autorizado.";
            Hide();
            try { using var main = new MainForm(session); main.ShowDialog(this); }
            finally { Show(); Activate(); password.Clear(); username.Focus(); }
        }
        catch (Exception ex) { Problems.Show(ex); status.Text = "No fue posible iniciar sesión."; }
        finally { enter.Enabled = true; }
    }

    private void logo_Click(object sender, EventArgs e)
    {

    }
}