#nullable enable
namespace AMBAR.Semana4;

partial class DetalleCapturaForm
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
        logo = new PictureBox();
        subtitle = new Label();
        grid = new DataGridView();
        close = new Button();
        SuspendLayout();
        heading.Text = "Historial de cambios";
        heading.Location = new Point(270, 24);
        heading.Size = new Size(806, 62);
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(24, 10);
        logo.Size = new Size(220, 80);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        subtitle.Name = "subtitle";
        subtitle.Location = new Point(24, 110);
        subtitle.Size = new Size(1050, 40);
        grid.Name = "grid";
        grid.Location = new Point(24, 170);
        grid.Size = new Size(1050, 420);
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.RowHeadersVisible = false;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(6, 55, 99);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.ColumnHeadersHeight = 40;
        grid.RowTemplate.Height = 38;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 246, 250);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 213, 234);
        grid.DefaultCellStyle.SelectionForeColor = Color.Black;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        close.Name = "close";
        close.Location = new Point(900, 620);
        close.Size = new Size(175, 40);
        close.Text = "Cerrar";
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.ForeColor = Color.White;
        close.FlatStyle = FlatStyle.Flat;
        close.FlatAppearance.BorderSize = 0;
        close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        close.Click += CloseClick;
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(subtitle);
        Controls.Add(grid);
        Controls.Add(close);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.White;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1100, 690);
        MinimumSize = new Size(1116, 729);
        StartPosition = FormStartPosition.CenterParent;
        Name = "DetalleCapturaForm";
        Text = "AMBAR | Historial de cambios";
        ResumeLayout(false);
        PerformLayout();
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private Label subtitle = null!;
    private DataGridView grid = null!;
    private Button close = null!;
}
