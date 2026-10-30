#nullable disable
namespace AMBAR.Semana4;

partial class ConsultaA
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsultaA));
        grid = new DataGridView();
        searchBar = new Panel();
        searchLabel = new Label();
        filter = new TextBox();
        refresh = new Button();
        count = new Label();
        actions = new FlowLayoutPanel();
        view = new Button();
        edit = new Button();
        pdf = new Button();
        hide = new Button();
        restore = new Button();
        header = new Panel();
        pictureBox1 = new PictureBox();
        heading = new Label();
        menuButton = new Button();
        sectionHeader = new Panel();
        sectionTitle = new Label();
        sectionLine = new Panel();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        searchBar.SuspendLayout();
        actions.SuspendLayout();
        header.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        sectionHeader.SuspendLayout();
        horario = new Button();
        SuspendLayout();
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle1.ForeColor = Color.White;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        grid.ColumnHeadersHeight = 42;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle2.SelectionForeColor = Color.Black;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle2;
        grid.Dock = DockStyle.Fill;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(0, 257);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 34;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1240, 411);
        grid.TabIndex = 3;
        grid.CellContentClick += grid_CellContentClick;
        grid.CellDoubleClick += GridDoubleClick;
        // 
        // searchBar
        // 
        searchBar.BackColor = Color.White;
        searchBar.Controls.Add(searchLabel);
        searchBar.Controls.Add(filter);
        searchBar.Controls.Add(refresh);
        searchBar.Controls.Add(count);
        searchBar.Dock = DockStyle.Top;
        searchBar.Location = new Point(0, 181);
        searchBar.Name = "searchBar";
        searchBar.Size = new Size(1240, 76);
        searchBar.TabIndex = 2;
        // 
        // searchLabel
        // 
        searchLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        searchLabel.ForeColor = Color.FromArgb(61, 107, 134);
        searchLabel.Location = new Point(35, 18);
        searchLabel.Name = "searchLabel";
        searchLabel.Size = new Size(80, 35);
        searchLabel.TabIndex = 0;
        searchLabel.Text = "Buscar:";
        searchLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // filter
        // 
        filter.Font = new Font("Segoe UI", 11F);
        filter.Location = new Point(120, 20);
        filter.MaxLength = 70;
        filter.Name = "filter";
        filter.Size = new Size(350, 32);
        filter.TabIndex = 0;
        // 
        // refresh
        // 
        refresh.BackColor = Color.FromArgb(6, 55, 99);
        refresh.Cursor = Cursors.Hand;
        refresh.FlatAppearance.BorderSize = 0;
        refresh.FlatStyle = FlatStyle.Flat;
        refresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        refresh.ForeColor = Color.White;
        refresh.Location = new Point(490, 15);
        refresh.Name = "refresh";
        refresh.Size = new Size(210, 42);
        refresh.TabIndex = 1;
        refresh.Text = "Buscar / actualizar";
        refresh.UseVisualStyleBackColor = false;
        refresh.Click += RefreshClick;
        // 
        // count
        // 
        count.Font = new Font("Segoe UI", 10F);
        count.ForeColor = Color.FromArgb(61, 107, 134);
        count.Location = new Point(720, 20);
        count.Name = "count";
        count.Size = new Size(300, 35);
        count.TabIndex = 2;
        count.Text = "Registros: 0";
        count.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // actions
        // 
        actions.AutoScroll = true;
        actions.BackColor = Color.White;
        actions.Controls.Add(view);
        actions.Controls.Add(edit);
        actions.Controls.Add(pdf);
        actions.Controls.Add(hide);
        actions.Controls.Add(restore);
        actions.Dock = DockStyle.Bottom;
        actions.Location = new Point(0, 668);
        actions.Name = "actions";
        actions.Padding = new Padding(12);
        actions.Size = new Size(1240, 72);
        actions.TabIndex = 4;
        actions.WrapContents = false;
        // 
        // view
        // 
        view.BackColor = Color.FromArgb(6, 55, 99);
        view.Cursor = Cursors.Hand;
        view.FlatAppearance.BorderSize = 0;
        view.FlatStyle = FlatStyle.Flat;
        view.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        view.ForeColor = Color.White;
        view.Location = new Point(15, 15);
        view.Name = "view";
        view.Size = new Size(175, 42);
        view.TabIndex = 0;
        view.Text = "Ver expediente";
        view.UseVisualStyleBackColor = false;
        view.Click += ViewClick;
        // 
        // edit
        // 
        edit.BackColor = Color.FromArgb(6, 55, 99);
        edit.Cursor = Cursors.Hand;
        edit.FlatAppearance.BorderSize = 0;
        edit.FlatStyle = FlatStyle.Flat;
        edit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        edit.ForeColor = Color.White;
        edit.Location = new Point(196, 15);
        edit.Name = "edit";
        edit.Size = new Size(175, 42);
        edit.TabIndex = 1;
        edit.Text = "Modificar";
        edit.UseVisualStyleBackColor = false;
        edit.Click += EditClick;
        // 
        // pdf
        // 
        pdf.BackColor = Color.FromArgb(6, 55, 99);
        pdf.Cursor = Cursors.Hand;
        pdf.FlatAppearance.BorderSize = 0;
        pdf.FlatStyle = FlatStyle.Flat;
        pdf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        pdf.ForeColor = Color.White;
        pdf.Location = new Point(377, 15);
        pdf.Name = "pdf";
        pdf.Size = new Size(175, 42);
        pdf.TabIndex = 2;
        pdf.Text = "Exportar PDF";
        pdf.UseVisualStyleBackColor = false;
        pdf.Click += PdfClick;
        // 
        // hide
        // 
        hide.BackColor = Color.FromArgb(6, 55, 99);
        hide.Cursor = Cursors.Hand;
        hide.FlatAppearance.BorderSize = 0;
        hide.FlatStyle = FlatStyle.Flat;
        hide.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        hide.ForeColor = Color.White;
        hide.Location = new Point(558, 15);
        hide.Name = "hide";
        hide.Size = new Size(175, 42);
        hide.TabIndex = 3;
        hide.Text = "Ocultar registro";
        hide.UseVisualStyleBackColor = false;
        hide.Click += HideClick;
        // 
        // restore
        // 
        restore.BackColor = Color.FromArgb(6, 55, 99);
        restore.Cursor = Cursors.Hand;
        restore.FlatAppearance.BorderSize = 0;
        restore.FlatStyle = FlatStyle.Flat;
        restore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        restore.ForeColor = Color.White;
        restore.Location = new Point(739, 15);
        restore.Name = "restore";
        restore.Size = new Size(175, 42);
        restore.TabIndex = 4;
        restore.Text = "Restaurar";
        restore.UseVisualStyleBackColor = false;
        restore.Click += RestoreClick;
        // 
        // header
        // 
        header.BackColor = Color.White;
        header.Controls.Add(pictureBox1);
        header.Controls.Add(heading);
        header.Controls.Add(menuButton);
        header.Dock = DockStyle.Top;
        header.Location = new Point(0, 0);
        header.Name = "header";
        header.Size = new Size(1240, 105);
        header.TabIndex = 0;
        // 
        // pictureBox1
        // 
        pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
        pictureBox1.Location = new Point(60, 19);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(210, 80);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 1;
        pictureBox1.TabStop = false;
        // 
        // heading
        // 
        heading.Dock = DockStyle.Right;
        heading.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(665, 0);
        heading.Name = "heading";
        heading.Padding = new Padding(0, 0, 28, 0);
        heading.Size = new Size(575, 105);
        heading.TabIndex = 2;
        heading.Text = "CONSULTA DE ALUMNOS";
        heading.TextAlign = ContentAlignment.MiddleRight;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(18, 22);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 0;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Click += MenuButton_Click;
        // 
        // sectionHeader
        // 
        sectionHeader.BackColor = Color.White;
        sectionHeader.Controls.Add(sectionTitle);
        sectionHeader.Controls.Add(sectionLine);
        sectionHeader.Dock = DockStyle.Top;
        sectionHeader.Location = new Point(0, 105);
        sectionHeader.Name = "sectionHeader";
        sectionHeader.Size = new Size(1240, 76);
        sectionHeader.TabIndex = 1;
        // 
        // sectionTitle
        // 
        sectionTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        sectionTitle.ForeColor = Color.FromArgb(6, 55, 99);
        sectionTitle.Location = new Point(35, 12);
        sectionTitle.Name = "sectionTitle";
        sectionTitle.Size = new Size(750, 43);
        sectionTitle.TabIndex = 0;
        sectionTitle.Text = "Consulta digital de expedientes";
        sectionTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // sectionLine
        // 
        sectionLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        sectionLine.BackColor = Color.FromArgb(0, 92, 184);
        sectionLine.Location = new Point(35, 60);
        sectionLine.Name = "sectionLine";
        sectionLine.Size = new Size(1170, 2);
        sectionLine.TabIndex = 1;
        // 
        // StudentsForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1240, 740);
        Controls.Add(grid);
        Controls.Add(actions);
        Controls.Add(searchBar);
        Controls.Add(sectionHeader);
        Controls.Add(header);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(900, 650);
        Name = "StudentsForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Administrador | Consultas";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        searchBar.ResumeLayout(false);
        searchBar.PerformLayout();
        actions.ResumeLayout(false);
        header.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        sectionHeader.ResumeLayout(false);
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.GridColor = Color.FromArgb(222, 233, 241);
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersHeight = 48;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(6, 55, 99);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(6, 55, 99);
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(34, 49, 71);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(215, 234, 247);
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(6, 55, 99);
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        grid.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 246, 250);
        grid.RowTemplate.Height = 42;
        refresh.BackColor = Color.FromArgb(6, 55, 99);
        refresh.ForeColor = Color.White;
        refresh.FlatStyle = FlatStyle.Flat;
        refresh.FlatAppearance.BorderSize = 0;
        refresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        refresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        refresh.UseVisualStyleBackColor = false;
        refresh.Cursor = Cursors.Hand;
        refresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        view.BackColor = Color.FromArgb(6, 55, 99);
        view.ForeColor = Color.White;
        view.FlatStyle = FlatStyle.Flat;
        view.FlatAppearance.BorderSize = 0;
        view.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        view.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        view.UseVisualStyleBackColor = false;
        view.Cursor = Cursors.Hand;
        view.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        edit.BackColor = Color.FromArgb(6, 55, 99);
        edit.ForeColor = Color.White;
        edit.FlatStyle = FlatStyle.Flat;
        edit.FlatAppearance.BorderSize = 0;
        edit.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        edit.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        edit.UseVisualStyleBackColor = false;
        edit.Cursor = Cursors.Hand;
        edit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        pdf.BackColor = Color.FromArgb(6, 55, 99);
        pdf.ForeColor = Color.White;
        pdf.FlatStyle = FlatStyle.Flat;
        pdf.FlatAppearance.BorderSize = 0;
        pdf.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        pdf.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        pdf.UseVisualStyleBackColor = false;
        pdf.Cursor = Cursors.Hand;
        pdf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        hide.BackColor = Color.FromArgb(6, 55, 99);
        hide.ForeColor = Color.White;
        hide.FlatStyle = FlatStyle.Flat;
        hide.FlatAppearance.BorderSize = 0;
        hide.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        hide.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        hide.UseVisualStyleBackColor = false;
        hide.Cursor = Cursors.Hand;
        hide.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        restore.BackColor = Color.FromArgb(6, 55, 99);
        restore.ForeColor = Color.White;
        restore.FlatStyle = FlatStyle.Flat;
        restore.FlatAppearance.BorderSize = 0;
        restore.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        restore.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        restore.UseVisualStyleBackColor = false;
        restore.Cursor = Cursors.Hand;
        restore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        horario.Name = "horario";
        horario.Text = "Ver horario";
        horario.Location = new Point(0, 0);
        horario.Size = new Size(158, 44);
        horario.Click += HorarioClick;
        horario.BackColor = Color.FromArgb(6, 55, 99);
        horario.ForeColor = Color.White;
        horario.FlatStyle = FlatStyle.Flat;
        horario.FlatAppearance.BorderSize = 0;
        horario.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        horario.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        horario.UseVisualStyleBackColor = false;
        horario.Cursor = Cursors.Hand;
        horario.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        horario.Margin = new Padding(5);
        actions.Controls.Add(horario);
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        ResumeLayout(false);
    }

    private DataGridView grid = null!;
    private Panel searchBar = null!;
    private Label searchLabel = null!;
    private TextBox filter = null!;
    private Button refresh = null!;
    private Label count = null!;
    private FlowLayoutPanel actions = null!;
    private Button view = null!;
    private Button edit = null!;
    private Button pdf = null!;
    private Button hide = null!;
    private Button restore = null!;
    private Panel header = null!;
    private Button menuButton = null!;
    private PictureBox pictureBox1 = null!;
    private Label heading = null!;
    private Panel sectionHeader = null!;
    private Label sectionTitle = null!;
    private Panel sectionLine = null!;
    private Button horario = null!;
}

