#nullable enable
namespace AMBAR.Semana4;

partial class ConfirmarPasswordForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        heading = new Label();
        description = new Label();
        password = new TextBox();
        accept = new Button();
        cancel = new Button();
        SuspendLayout();
        heading.Text = "Confirma tu contraseña de administrador";
        heading.Location = new Point(24, 24);
        heading.Size = new Size(462, 62);
        heading.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        description.Name = "description";
        description.Location = new Point(24, 90);
        description.Size = new Size(460, 26);
        description.Text = "La bitácora requiere verificar tu identidad en cada consulta.";
        password.Name = "password";
        password.Location = new Point(24, 125);
        password.Size = new Size(460, 30);
        password.UseSystemPasswordChar = true;
        accept.Name = "accept";
        accept.Location = new Point(304, 185);
        accept.Size = new Size(180, 42);
        accept.Text = "Consultar";
        accept.BackColor = Color.FromArgb(6, 55, 99);
        accept.ForeColor = Color.White;
        accept.FlatStyle = FlatStyle.Flat;
        accept.FlatAppearance.BorderSize = 0;
        accept.DialogResult = DialogResult.OK;
        cancel.Name = "cancel";
        cancel.Location = new Point(114, 185);
        cancel.Size = new Size(175, 42);
        cancel.Text = "Cancelar";
        cancel.BackColor = Color.FromArgb(6, 55, 99);
        cancel.ForeColor = Color.White;
        cancel.FlatStyle = FlatStyle.Flat;
        cancel.FlatAppearance.BorderSize = 0;
        cancel.DialogResult = DialogResult.Cancel;
        AcceptButton = accept; CancelButton = cancel;
        Controls.Add(heading);
        Controls.Add(description);
        Controls.Add(password);
        Controls.Add(accept);
        Controls.Add(cancel);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.White;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(510, 255);
        MinimumSize = new Size(526, 294);
        StartPosition = FormStartPosition.CenterParent;
        Name = "ConfirmarPasswordForm";
        Text = "AMBAR | Confirma tu contraseña de administrador";
        ResumeLayout(false);
        PerformLayout();
    }
    private Label heading = null!;
    private Label description = null!;
    private TextBox password = null!;
    private Button accept = null!;
    private Button cancel = null!;
}
