#nullable enable
namespace AMBAR.Semana4;

partial class CapturaCLForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
        heading = new Label();
        logo = new PictureBox();
        period = new ComboBox();
        subject = new ComboBox();
        group = new ComboBox();
        unit = new ComboBox();
        window = new ComboBox();
        load = new Button();
        grid = new DataGridView();
        cancel = new Button();
        save = new Button();
        previousLabel = new Label();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        label4 = new Label();
        note = new Label();
        summary = new Label();
        personalLine = new Panel();
        panel1 = new Panel();
        menuButton = new Button();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(338, 30);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Size = new Size(1008, 78);
        heading.TabIndex = 0;
        heading.Text = "Captura de calificaciones";
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
        // period
        // 
        period.DropDownStyle = ComboBoxStyle.DropDownList;
        period.Location = new Point(30, 172);
        period.Margin = new Padding(4, 4, 4, 4);
        period.Name = "period";
        period.Size = new Size(286, 31);
        period.TabIndex = 3;
        // 
        // subject
        // 
        subject.DropDownStyle = ComboBoxStyle.DropDownList;
        subject.Location = new Point(342, 172);
        subject.Margin = new Padding(4, 4, 4, 4);
        subject.Name = "subject";
        subject.Size = new Size(593, 31);
        subject.TabIndex = 5;
        // 
        // group
        // 
        group.DropDownStyle = ComboBoxStyle.DropDownList;
        group.Location = new Point(962, 172);
        group.Margin = new Padding(4, 4, 4, 4);
        group.Name = "group";
        group.Size = new Size(374, 31);
        group.TabIndex = 7;
        // 
        // unit
        // 
        unit.DropDownStyle = ComboBoxStyle.DropDownList;
        unit.Location = new Point(30, 260);
        unit.Margin = new Padding(4, 4, 4, 4);
        unit.Name = "unit";
        unit.Size = new Size(499, 31);
        unit.TabIndex = 9;
        unit.SelectedIndexChanged += SelectionChanged;
        // 
        // window
        // 
        window.DropDownStyle = ComboBoxStyle.DropDownList;
        window.Location = new Point(556, 260);
        window.Margin = new Padding(4, 4, 4, 4);
        window.Name = "window";
        window.Size = new Size(499, 31);
        window.TabIndex = 11;
        window.SelectedIndexChanged += SelectionChanged;
        // 
        // load
        // 
        load.BackColor = Color.FromArgb(6, 55, 99);
        load.FlatAppearance.BorderSize = 0;
        load.FlatStyle = FlatStyle.Flat;
        load.ForeColor = Color.White;
        load.Location = new Point(1081, 260);
        load.Margin = new Padding(4, 4, 4, 4);
        load.Name = "load";
        load.Size = new Size(262, 52);
        load.TabIndex = 12;
        load.Text = "Cargar alumnos";
        load.UseVisualStyleBackColor = false;
        load.Click += LoadClick;
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        dataGridViewCellStyle10.BackColor = Color.FromArgb(240, 246, 250);
        grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle11.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle11.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle11.ForeColor = Color.White;
        dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
        grid.ColumnHeadersHeight = 40;
        dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle12.BackColor = SystemColors.Window;
        dataGridViewCellStyle12.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle12.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle12.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle12.SelectionForeColor = Color.Black;
        dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle12;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(30, 344);
        grid.Margin = new Padding(4, 4, 4, 4);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1312, 412);
        grid.TabIndex = 13;
        grid.CellValueChanged += ValueChanged;
        grid.CurrentCellDirtyStateChanged += DirtyChanged;
        grid.DataError += GridError;
        // 
        // cancel
        // 
        cancel.BackColor = Color.FromArgb(6, 55, 99);
        cancel.FlatAppearance.BorderSize = 0;
        cancel.FlatStyle = FlatStyle.Flat;
        cancel.ForeColor = Color.White;
        cancel.Location = new Point(900, 844);
        cancel.Margin = new Padding(4, 4, 4, 4);
        cancel.Name = "cancel";
        cancel.Size = new Size(206, 50);
        cancel.TabIndex = 16;
        cancel.Text = "Cerrar";
        cancel.UseVisualStyleBackColor = false;
        cancel.Click += CloseClick;
        // 
        // save
        // 
        save.BackColor = Color.FromArgb(6, 55, 99);
        save.FlatAppearance.BorderSize = 0;
        save.FlatStyle = FlatStyle.Flat;
        save.ForeColor = Color.White;
        save.Location = new Point(1131, 844);
        save.Margin = new Padding(4, 4, 4, 4);
        save.Name = "save";
        save.Size = new Size(212, 50);
        save.TabIndex = 17;
        save.Text = "Guardar";
        save.UseVisualStyleBackColor = false;
        save.Click += SaveClick;
        // 
        // previousLabel
        // 
        previousLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        previousLabel.ForeColor = Color.FromArgb(61, 107, 134);
        previousLabel.Location = new Point(30, 136);
        previousLabel.Margin = new Padding(4, 0, 4, 0);
        previousLabel.Name = "previousLabel";
        previousLabel.Size = new Size(139, 22);
        previousLabel.TabIndex = 18;
        previousLabel.Text = "Periodo escolar";
        previousLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(61, 107, 134);
        label1.Location = new Point(342, 133);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(80, 22);
        label1.TabIndex = 19;
        label1.Text = "Materia";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(61, 107, 134);
        label2.Location = new Point(962, 133);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(62, 22);
        label2.TabIndex = 20;
        label2.Text = "Grupo";
        label2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label3
        // 
        label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label3.ForeColor = Color.FromArgb(61, 107, 134);
        label3.Location = new Point(30, 226);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(190, 22);
        label3.TabIndex = 21;
        label3.Text = "Unidad de evaluación";
        label3.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label4
        // 
        label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label4.ForeColor = Color.FromArgb(61, 107, 134);
        label4.Location = new Point(556, 226);
        label4.Margin = new Padding(4, 0, 4, 0);
        label4.Name = "label4";
        label4.Size = new Size(237, 22);
        label4.TabIndex = 22;
        label4.Text = "Periodo de captura abierto";
        label4.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // note
        // 
        note.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        note.ForeColor = Color.FromArgb(61, 107, 134);
        note.Location = new Point(30, 825);
        note.Margin = new Padding(4, 0, 4, 0);
        note.Name = "note";
        note.Size = new Size(825, 72);
        note.TabIndex = 15;
        note.Text = "Selecciona una asignación con unidades y captura abierta.";
        // 
        // summary
        // 
        summary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        summary.ForeColor = Color.FromArgb(61, 107, 134);
        summary.Location = new Point(30, 775);
        summary.Margin = new Padding(4, 0, 4, 0);
        summary.Name = "summary";
        summary.Size = new Size(1312, 38);
        summary.TabIndex = 14;
        summary.Text = "Capturadas: 0";
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(30, 163);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 23;
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel1.BackColor = Color.FromArgb(8, 84, 143);
        panel1.Location = new Point(30, 251);
        panel1.Name = "panel1";
        panel1.Size = new Size(1109, 2);
        panel1.TabIndex = 24;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 30);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 25;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // CapturaCLForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1375, 925);
        Controls.Add(menuButton);
        Controls.Add(panel1);
        Controls.Add(personalLine);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(previousLabel);
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(period);
        Controls.Add(subject);
        Controls.Add(group);
        Controls.Add(unit);
        Controls.Add(window);
        Controls.Add(load);
        Controls.Add(grid);
        Controls.Add(summary);
        Controls.Add(note);
        Controls.Add(cancel);
        Controls.Add(save);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1390, 962);
        Name = "CapturaCLForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "AMBAR | Captura de calificaciones";
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private ComboBox period = null!;
    private ComboBox subject = null!;
    private ComboBox group = null!;
    private ComboBox unit = null!;
    private ComboBox window = null!;
    private Button load = null!;
    private DataGridView grid = null!;
    private Button cancel = null!;
    private Button save = null!;
    private Label previousLabel;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label note;
    private Label summary;
    private Panel personalLine;
    private Panel panel1;
    private Button menuButton;
}
