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
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        heading = new Label();
        logo = new PictureBox();
        subtitle = new Label();
        grid = new DataGridView();
        close = new Button();
        menuButton = new Button();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(947, 38);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Size = new Size(397, 59);
        heading.TabIndex = 0;
        heading.Text = "HISTORIAL DE CAMBIOS";
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(50, 13);
        logo.Margin = new Padding(4, 4, 4, 4);
        logo.Name = "logo";
        logo.Size = new Size(275, 100);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 1;
        logo.TabStop = false;
        // 
        // subtitle
        // 
        subtitle.Location = new Point(30, 138);
        subtitle.Margin = new Padding(4, 0, 4, 0);
        subtitle.Name = "subtitle";
        subtitle.Size = new Size(1312, 50);
        subtitle.TabIndex = 2;
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(240, 246, 250);
        grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        dataGridViewCellStyle5.ForeColor = Color.White;
        dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        grid.ColumnHeadersHeight = 40;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = SystemColors.Window;
        dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle6.SelectionForeColor = Color.Black;
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle6;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(30, 212);
        grid.Margin = new Padding(4, 4, 4, 4);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1312, 525);
        grid.TabIndex = 3;
        // 
        // close
        // 
        close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.FlatAppearance.BorderSize = 0;
        close.FlatStyle = FlatStyle.Flat;
        close.ForeColor = Color.White;
        close.Location = new Point(1125, 775);
        close.Margin = new Padding(4, 4, 4, 4);
        close.Name = "close";
        close.Size = new Size(219, 50);
        close.TabIndex = 4;
        close.Text = "Cerrar";
        close.UseVisualStyleBackColor = false;
        close.Click += CloseClick;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 32);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 26;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // DetalleCapturaForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1375, 862);
        Controls.Add(menuButton);
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(subtitle);
        Controls.Add(grid);
        Controls.Add(close);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1390, 899);
        Name = "DetalleCapturaForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "AMBAR | Historial de cambios";
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private Label subtitle = null!;
    private DataGridView grid = null!;
    private Button close = null!;
    private Button menuButton;
}
