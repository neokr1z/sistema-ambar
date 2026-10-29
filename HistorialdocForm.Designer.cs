#nullable enable
namespace AMBAR.Semana4;

partial class HistorialdocForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
        heading = new Label();
        logo = new PictureBox();
        periodLabel = new Label();
        period = new ComboBox();
        subjectLabel = new Label();
        subject = new ComboBox();
        groupLabel = new Label();
        group = new ComboBox();
        unitLabel = new Label();
        unit = new ComboBox();
        search = new Button();
        detail = new Button();
        export = new Button();
        summary = new Label();
        grid = new DataGridView();
        note = new Label();
        close = new Button();
        menuButton = new Button();
        personalLine = new Panel();
        panel1 = new Panel();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(847, 30);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Size = new Size(497, 54);
        heading.TabIndex = 0;
        heading.Text = "HISTORIAL DE CALIFICACIONES";
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
        // periodLabel
        // 
        periodLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        periodLabel.ForeColor = Color.FromArgb(6, 55, 99);
        periodLabel.Location = new Point(30, 125);
        periodLabel.Margin = new Padding(4, 0, 4, 0);
        periodLabel.Name = "periodLabel";
        periodLabel.Size = new Size(262, 30);
        periodLabel.TabIndex = 2;
        periodLabel.Text = "Periodo escolar";
        // 
        // period
        // 
        period.DropDownStyle = ComboBoxStyle.DropDownList;
        period.Location = new Point(30, 162);
        period.Margin = new Padding(4, 4, 4, 4);
        period.Name = "period";
        period.Size = new Size(286, 31);
        period.TabIndex = 3;
        // 
        // subjectLabel
        // 
        subjectLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        subjectLabel.ForeColor = Color.FromArgb(6, 55, 99);
        subjectLabel.Location = new Point(342, 125);
        subjectLabel.Margin = new Padding(4, 0, 4, 0);
        subjectLabel.Name = "subjectLabel";
        subjectLabel.Size = new Size(425, 30);
        subjectLabel.TabIndex = 4;
        subjectLabel.Text = "Materia";
        // 
        // subject
        // 
        subject.DropDownStyle = ComboBoxStyle.DropDownList;
        subject.Location = new Point(342, 162);
        subject.Margin = new Padding(4, 4, 4, 4);
        subject.Name = "subject";
        subject.Size = new Size(593, 31);
        subject.TabIndex = 5;
        // 
        // groupLabel
        // 
        groupLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        groupLabel.ForeColor = Color.FromArgb(6, 55, 99);
        groupLabel.Location = new Point(962, 125);
        groupLabel.Margin = new Padding(4, 0, 4, 0);
        groupLabel.Name = "groupLabel";
        groupLabel.Size = new Size(288, 30);
        groupLabel.TabIndex = 6;
        groupLabel.Text = "Grupo";
        // 
        // group
        // 
        group.DropDownStyle = ComboBoxStyle.DropDownList;
        group.Location = new Point(962, 162);
        group.Margin = new Padding(4, 4, 4, 4);
        group.Name = "group";
        group.Size = new Size(374, 31);
        group.TabIndex = 7;
        // 
        // unitLabel
        // 
        unitLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        unitLabel.ForeColor = Color.FromArgb(6, 55, 99);
        unitLabel.Location = new Point(30, 226);
        unitLabel.Margin = new Padding(4, 0, 4, 0);
        unitLabel.Name = "unitLabel";
        unitLabel.Size = new Size(375, 30);
        unitLabel.TabIndex = 8;
        unitLabel.Text = "Unidad";
        // 
        // unit
        // 
        unit.DropDownStyle = ComboBoxStyle.DropDownList;
        unit.Location = new Point(30, 264);
        unit.Margin = new Padding(4, 4, 4, 4);
        unit.Name = "unit";
        unit.Size = new Size(599, 31);
        unit.TabIndex = 9;
        unit.SelectedIndexChanged += UnitChanged;
        // 
        // search
        // 
        search.BackColor = Color.FromArgb(6, 55, 99);
        search.FlatAppearance.BorderSize = 0;
        search.FlatStyle = FlatStyle.Flat;
        search.ForeColor = Color.White;
        search.Location = new Point(656, 258);
        search.Margin = new Padding(4, 4, 4, 4);
        search.Name = "search";
        search.Size = new Size(212, 50);
        search.TabIndex = 10;
        search.Text = "Consultar";
        search.UseVisualStyleBackColor = false;
        search.Click += SearchClick;
        // 
        // detail
        // 
        detail.BackColor = Color.FromArgb(6, 55, 99);
        detail.FlatAppearance.BorderSize = 0;
        detail.FlatStyle = FlatStyle.Flat;
        detail.ForeColor = Color.White;
        detail.Location = new Point(894, 258);
        detail.Margin = new Padding(4, 4, 4, 4);
        detail.Name = "detail";
        detail.Size = new Size(212, 50);
        detail.TabIndex = 11;
        detail.Text = "Ver cambios";
        detail.UseVisualStyleBackColor = false;
        detail.Click += DetailClick;
        // 
        // export
        // 
        export.BackColor = Color.FromArgb(6, 55, 99);
        export.FlatAppearance.BorderSize = 0;
        export.FlatStyle = FlatStyle.Flat;
        export.ForeColor = Color.White;
        export.Location = new Point(1131, 258);
        export.Margin = new Padding(4, 4, 4, 4);
        export.Name = "export";
        export.Size = new Size(212, 50);
        export.TabIndex = 12;
        export.Text = "Exportar PDF";
        export.UseVisualStyleBackColor = false;
        export.Click += ExportClick;
        // 
        // summary
        // 
        summary.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        summary.ForeColor = Color.FromArgb(6, 55, 99);
        summary.Location = new Point(30, 338);
        summary.Margin = new Padding(4, 0, 4, 0);
        summary.Name = "summary";
        summary.Size = new Size(1312, 69);
        summary.TabIndex = 13;
        summary.Text = "Sin calificaciones.";
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        dataGridViewCellStyle16.BackColor = Color.FromArgb(240, 246, 250);
        grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle17.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle17.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        dataGridViewCellStyle17.ForeColor = Color.White;
        dataGridViewCellStyle17.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle17.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle17.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
        grid.ColumnHeadersHeight = 40;
        dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle18.BackColor = SystemColors.Window;
        dataGridViewCellStyle18.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle18.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle18.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle18.SelectionForeColor = Color.Black;
        dataGridViewCellStyle18.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle18;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(30, 425);
        grid.Margin = new Padding(4, 4, 4, 4);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1312, 350);
        grid.TabIndex = 14;
        // 
        // note
        // 
        note.Location = new Point(30, 806);
        note.Margin = new Padding(4, 0, 4, 0);
        note.Name = "note";
        note.Size = new Size(1038, 75);
        note.TabIndex = 15;
        note.Text = "El promedio corresponde a las calificaciones del filtro, no al promedio final del curso.";
        // 
        // close
        // 
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.FlatAppearance.BorderSize = 0;
        close.FlatStyle = FlatStyle.Flat;
        close.ForeColor = Color.White;
        close.Location = new Point(1125, 844);
        close.Margin = new Padding(4, 4, 4, 4);
        close.Name = "close";
        close.Size = new Size(219, 50);
        close.TabIndex = 16;
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
        menuButton.Location = new Point(12, 30);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 19;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(30, 153);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 20;
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel1.BackColor = Color.FromArgb(8, 84, 143);
        panel1.Location = new Point(30, 375);
        panel1.Name = "panel1";
        panel1.Size = new Size(1109, 2);
        panel1.TabIndex = 21;
        // 
        // HistorialdocForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1375, 925);
        Controls.Add(panel1);
        Controls.Add(personalLine);
        Controls.Add(menuButton);
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(periodLabel);
        Controls.Add(period);
        Controls.Add(subjectLabel);
        Controls.Add(subject);
        Controls.Add(groupLabel);
        Controls.Add(group);
        Controls.Add(unitLabel);
        Controls.Add(unit);
        Controls.Add(search);
        Controls.Add(detail);
        Controls.Add(export);
        Controls.Add(summary);
        Controls.Add(grid);
        Controls.Add(note);
        Controls.Add(close);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1390, 962);
        Name = "HistorialdocForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "AMBAR | Historial de calificaciones";
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private Label periodLabel = null!;
    private ComboBox period = null!;
    private Label subjectLabel = null!;
    private ComboBox subject = null!;
    private Label groupLabel = null!;
    private ComboBox group = null!;
    private Label unitLabel = null!;
    private ComboBox unit = null!;
    private Button search = null!;
    private Button detail = null!;
    private Button export = null!;
    private Label summary = null!;
    private DataGridView grid = null!;
    private Label note = null!;
    private Button close = null!;
    private Button menuButton;
    private Panel personalLine;
    private Panel panel1;
}
