#nullable enable
namespace AMBAR.Semana4;

partial class MateriasDocenteForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle29 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle30 = new DataGridViewCellStyle();
        heading = new Label();
        logo = new PictureBox();
        assignment = new ComboBox();
        capture = new Button();
        grid = new DataGridView();
        schedule = new DataGridView();
        note = new Label();
        close = new Button();
        personalLine = new Panel();
        panel1 = new Panel();
        label1 = new Label();
        label2 = new Label();
        menuButton = new Button();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)schedule).BeginInit();
        SuspendLayout();
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(971, 37);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Size = new Size(373, 56);
        heading.TabIndex = 0;
        heading.Text = "MATERIAS Y GRUPOS";
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(30, 12);
        logo.Margin = new Padding(4, 4, 4, 4);
        logo.Name = "logo";
        logo.Size = new Size(275, 100);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 1;
        logo.TabStop = false;
        // 
        // assignment
        // 
        assignment.DropDownStyle = ComboBoxStyle.DropDownList;
        assignment.Location = new Point(30, 138);
        assignment.Margin = new Padding(4, 4, 4, 4);
        assignment.Name = "assignment";
        assignment.Size = new Size(1036, 31);
        assignment.TabIndex = 2;
        assignment.SelectedIndexChanged += SelectionChanged;
        // 
        // capture
        // 
        capture.BackColor = Color.FromArgb(6, 55, 99);
        capture.FlatAppearance.BorderSize = 0;
        capture.FlatStyle = FlatStyle.Flat;
        capture.ForeColor = Color.White;
        capture.Location = new Point(1094, 131);
        capture.Margin = new Padding(4, 4, 4, 4);
        capture.Name = "capture";
        capture.Size = new Size(250, 52);
        capture.TabIndex = 3;
        capture.Text = "Capturar calificaciones";
        capture.UseVisualStyleBackColor = false;
        capture.Click += CaptureClick;
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        dataGridViewCellStyle25.BackColor = Color.FromArgb(240, 246, 250);
        grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle25;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle26.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle26.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle26.ForeColor = Color.White;
        dataGridViewCellStyle26.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle26.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle26.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
        grid.ColumnHeadersHeight = 40;
        dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle27.BackColor = SystemColors.Window;
        dataGridViewCellStyle27.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle27.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle27.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle27.SelectionForeColor = Color.Black;
        dataGridViewCellStyle27.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle27;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(30, 250);
        grid.Margin = new Padding(4, 4, 4, 4);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1312, 325);
        grid.TabIndex = 5;
        // 
        // schedule
        // 
        schedule.AllowUserToAddRows = false;
        schedule.AllowUserToDeleteRows = false;
        dataGridViewCellStyle28.BackColor = Color.FromArgb(240, 246, 250);
        schedule.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle28;
        schedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        schedule.BackgroundColor = Color.White;
        schedule.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle29.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle29.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle29.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle29.ForeColor = Color.White;
        dataGridViewCellStyle29.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle29.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle29.WrapMode = DataGridViewTriState.True;
        schedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
        schedule.ColumnHeadersHeight = 40;
        dataGridViewCellStyle30.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle30.BackColor = SystemColors.Window;
        dataGridViewCellStyle30.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle30.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle30.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle30.SelectionForeColor = Color.Black;
        dataGridViewCellStyle30.WrapMode = DataGridViewTriState.False;
        schedule.DefaultCellStyle = dataGridViewCellStyle30;
        schedule.EnableHeadersVisualStyles = false;
        schedule.Location = new Point(30, 644);
        schedule.Margin = new Padding(4, 4, 4, 4);
        schedule.MultiSelect = false;
        schedule.Name = "schedule";
        schedule.ReadOnly = true;
        schedule.RowHeadersVisible = false;
        schedule.RowHeadersWidth = 51;
        schedule.RowTemplate.Height = 38;
        schedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        schedule.Size = new Size(1312, 175);
        schedule.TabIndex = 7;
        // 
        // note
        // 
        note.Location = new Point(30, 848);
        note.Margin = new Padding(4, 0, 4, 0);
        note.Name = "note";
        note.Size = new Size(1038, 56);
        note.TabIndex = 8;
        note.Text = "Selecciona una materia y grupo.";
        // 
        // close
        // 
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.FlatAppearance.BorderSize = 0;
        close.FlatStyle = FlatStyle.Flat;
        close.ForeColor = Color.White;
        close.Location = new Point(1125, 848);
        close.Margin = new Padding(4, 4, 4, 4);
        close.Name = "close";
        close.Size = new Size(219, 50);
        close.TabIndex = 9;
        close.Text = "Cerrar";
        close.UseVisualStyleBackColor = false;
        close.Click += CloseClick;
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(30, 235);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 13;
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel1.BackColor = Color.FromArgb(8, 84, 143);
        panel1.Location = new Point(30, 631);
        panel1.Name = "panel1";
        panel1.Size = new Size(1109, 2);
        panel1.TabIndex = 14;
        // 
        // label1
        // 
        label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(6, 55, 99);
        label1.Location = new Point(30, 208);
        label1.Name = "label1";
        label1.Size = new Size(155, 24);
        label1.TabIndex = 16;
        label1.Text = "Alumnos inscritos:";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        label2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(6, 55, 99);
        label2.Location = new Point(30, 604);
        label2.Name = "label2";
        label2.Size = new Size(203, 24);
        label2.TabIndex = 17;
        label2.Text = "Horario de la asignación:";
        label2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 37);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 18;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // MateriasDocenteForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1375, 925);
        Controls.Add(menuButton);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(panel1);
        Controls.Add(personalLine);
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(assignment);
        Controls.Add(capture);
        Controls.Add(grid);
        Controls.Add(schedule);
        Controls.Add(note);
        Controls.Add(close);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1390, 962);
        Name = "MateriasDocenteForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "AMBAR | Mis materias y grupos";
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)schedule).EndInit();
        ResumeLayout(false);
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private ComboBox assignment = null!;
    private Button capture = null!;
    private DataGridView grid = null!;
    private DataGridView schedule = null!;
    private Label note = null!;
    private Button close = null!;
    private Panel personalLine;
    private Panel panel1;
    private Label label1;
    private Label label2;
    private Button menuButton;
}
