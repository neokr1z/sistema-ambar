#nullable enable
namespace AMBAR.Semana4;
partial class InscripcionResultadoForm
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if(disposing)components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        heading = new Label();
        summary = new Label();
        generate = new Button();
        close = new Button();
        SuspendLayout();
        heading.Name = "heading";
        heading.Text = "INSCRIPCIÓN COMPLETADA";
        heading.Location = new Point(26, 20);
        heading.Size = new Size(600, 60);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        heading.TextAlign = ContentAlignment.MiddleLeft;
        summary.Name = "summary";
        summary.Text = "El alumno se registró correctamente.";
        summary.Location = new Point(26, 90);
        summary.Size = new Size(600, 130);
        summary.ForeColor = Color.FromArgb(6, 55, 99);
        summary.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
        summary.TextAlign = ContentAlignment.MiddleLeft;
        generate.Name = "generate";
        generate.Text = "Generar lista de grupo";
        generate.Location = new Point(26, 240);
        generate.Size = new Size(290, 44);
        generate.Click += ListClick;
        generate.BackColor = Color.FromArgb(6, 55, 99);
        generate.ForeColor = Color.White;
        generate.FlatStyle = FlatStyle.Flat;
        generate.FlatAppearance.BorderSize = 0;
        generate.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        generate.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        generate.UseVisualStyleBackColor = false;
        generate.Cursor = Cursors.Hand;
        generate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        close.Name = "close";
        close.Text = "Finalizar";
        close.Location = new Point(410, 240);
        close.Size = new Size(210, 44);
        close.Click += CloseClick;
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.ForeColor = Color.White;
        close.FlatStyle = FlatStyle.Flat;
        close.FlatAppearance.BorderSize = 0;
        close.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        close.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        close.UseVisualStyleBackColor = false;
        close.Cursor = Cursors.Hand;
        close.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        Controls.Add(heading);
        Controls.Add(summary);
        Controls.Add(generate);
        Controls.Add(close);

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(244, 248, 251);
        Font = new Font("Segoe UI", 10F);
        ClientSize = new Size(650, 320);
        MinimumSize = new Size(650, 320);
        StartPosition = FormStartPosition.CenterParent;
        Name = "InscripcionResultadoForm";
        Text = "AMBAR | Inscripción completada";
        generate.BackColor = Color.FromArgb(6, 55, 99);
        generate.ForeColor = Color.White;
        generate.FlatStyle = FlatStyle.Flat;
        generate.FlatAppearance.BorderSize = 0;
        generate.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        generate.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        generate.UseVisualStyleBackColor = false;
        generate.Cursor = Cursors.Hand;
        generate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.ForeColor = Color.White;
        close.FlatStyle = FlatStyle.Flat;
        close.FlatAppearance.BorderSize = 0;
        close.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        close.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        close.UseVisualStyleBackColor = false;
        close.Cursor = Cursors.Hand;
        close.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        ResumeLayout(false);
    }
    private Label heading = null!;
    private Label summary = null!;
    private Button generate = null!;
    private Button close = null!;
}
