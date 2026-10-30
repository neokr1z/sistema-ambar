#nullable enable
namespace AMBAR.Semana4;

partial class BitacoraForm
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
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        heading = new Label();
        logo = new PictureBox();
        from = new DateTimePicker();
        to = new DateTimePicker();
        filter = new TextBox();
        search = new Button();
        grid = new DataGridView();
        summary = new Label();
        previous = new Button();
        next = new Button();
        close = new Button();
        menuButton = new Button();
        roleLabel = new Label();
        label1 = new Label();
        personalLine = new Panel();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        // 
        // heading
        // 
        heading.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(988, 30);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Size = new Size(422, 78);
        heading.TabIndex = 0;
        heading.Text = "BITÁCORA DE ACTIVIDAD";
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(30, 12);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(275, 100);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 1;
        logo.TabStop = false;
        // 
        // from
        // 
        from.Format = DateTimePickerFormat.Short;
        from.Location = new Point(30, 164);
        from.Margin = new Padding(4);
        from.Name = "from";
        from.Size = new Size(224, 30);
        from.TabIndex = 3;
        // 
        // to
        // 
        to.Format = DateTimePickerFormat.Short;
        to.Location = new Point(275, 164);
        to.Margin = new Padding(4);
        to.Name = "to";
        to.Size = new Size(224, 30);
        to.TabIndex = 5;
        // 
        // filter
        // 
        filter.Location = new Point(525, 164);
        filter.Margin = new Padding(4);
        filter.Name = "filter";
        filter.PlaceholderText = "Buscar usuario, actividad o descripción";
        filter.Size = new Size(574, 30);
        filter.TabIndex = 6;
        // 
        // search
        // 
        search.BackColor = Color.FromArgb(6, 55, 99);
        search.FlatAppearance.BorderSize = 0;
        search.FlatStyle = FlatStyle.Flat;
        search.ForeColor = Color.White;
        search.Location = new Point(1125, 156);
        search.Margin = new Padding(4);
        search.Name = "search";
        search.Size = new Size(312, 50);
        search.TabIndex = 7;
        search.Text = "Consultar";
        search.UseVisualStyleBackColor = false;
        search.Click += SearchClick;
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 246, 250);
        grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(6, 55, 99);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        grid.ColumnHeadersHeight = 40;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Window;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(180, 213, 234);
        dataGridViewCellStyle3.SelectionForeColor = Color.Black;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        grid.DefaultCellStyle = dataGridViewCellStyle3;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(30, 238);
        grid.Margin = new Padding(4);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1410, 562);
        grid.TabIndex = 8;
        // 
        // summary
        // 
        summary.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        summary.Location = new Point(30, 825);
        summary.Margin = new Padding(4, 0, 4, 0);
        summary.Name = "summary";
        summary.Size = new Size(875, 75);
        summary.TabIndex = 9;
        summary.Text = "Confirma tu contraseña para consultar.";
        // 
        // previous
        // 
        previous.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        previous.BackColor = Color.FromArgb(6, 55, 99);
        previous.FlatAppearance.BorderSize = 0;
        previous.FlatStyle = FlatStyle.Flat;
        previous.ForeColor = Color.White;
        previous.Location = new Point(912, 845);
        previous.Margin = new Padding(4);
        previous.Name = "previous";
        previous.Size = new Size(162, 50);
        previous.TabIndex = 10;
        previous.Text = "Anterior";
        previous.UseVisualStyleBackColor = false;
        previous.Click += PreviousClick;
        // 
        // next
        // 
        next.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        next.BackColor = Color.FromArgb(6, 55, 99);
        next.FlatAppearance.BorderSize = 0;
        next.FlatStyle = FlatStyle.Flat;
        next.ForeColor = Color.White;
        next.Location = new Point(1094, 845);
        next.Margin = new Padding(4);
        next.Name = "next";
        next.Size = new Size(162, 50);
        next.TabIndex = 11;
        next.Text = "Siguiente";
        next.UseVisualStyleBackColor = false;
        next.Click += NextClick;
        // 
        // close
        // 
        close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.FlatAppearance.BorderSize = 0;
        close.FlatStyle = FlatStyle.Flat;
        close.ForeColor = Color.White;
        close.Location = new Point(1275, 845);
        close.Margin = new Padding(4);
        close.Name = "close";
        close.Size = new Size(165, 50);
        close.TabIndex = 12;
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
        menuButton.TabIndex = 13;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // roleLabel
        // 
        roleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        roleLabel.ForeColor = Color.FromArgb(6, 55, 99);
        roleLabel.Location = new Point(30, 125);
        roleLabel.Name = "roleLabel";
        roleLabel.Size = new Size(66, 24);
        roleLabel.TabIndex = 14;
        roleLabel.Text = "Desde:";
        roleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(6, 55, 99);
        label1.Location = new Point(275, 125);
        label1.Name = "label1";
        label1.Size = new Size(66, 24);
        label1.TabIndex = 15;
        label1.Text = "Hasta:";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(30, 156);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 16;
        // 
        // BitacoraForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1475, 950);
        Controls.Add(personalLine);
        Controls.Add(label1);
        Controls.Add(roleLabel);
        Controls.Add(menuButton);
        Controls.Add(heading);
        Controls.Add(logo);
        Controls.Add(from);
        Controls.Add(to);
        Controls.Add(filter);
        Controls.Add(search);
        Controls.Add(grid);
        Controls.Add(summary);
        Controls.Add(previous);
        Controls.Add(next);
        Controls.Add(close);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4);
        MinimumSize = new Size(1490, 987);
        Name = "BitacoraForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "AMBAR | Bitácora de actividad";
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
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
        search.BackColor = Color.FromArgb(6, 55, 99);
        search.ForeColor = Color.White;
        search.FlatStyle = FlatStyle.Flat;
        search.FlatAppearance.BorderSize = 0;
        search.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        search.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        search.UseVisualStyleBackColor = false;
        search.Cursor = Cursors.Hand;
        search.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        previous.BackColor = Color.FromArgb(6, 55, 99);
        previous.ForeColor = Color.White;
        previous.FlatStyle = FlatStyle.Flat;
        previous.FlatAppearance.BorderSize = 0;
        previous.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        previous.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        previous.UseVisualStyleBackColor = false;
        previous.Cursor = Cursors.Hand;
        previous.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        next.BackColor = Color.FromArgb(6, 55, 99);
        next.ForeColor = Color.White;
        next.FlatStyle = FlatStyle.Flat;
        next.FlatAppearance.BorderSize = 0;
        next.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        next.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        next.UseVisualStyleBackColor = false;
        next.Cursor = Cursors.Hand;
        next.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        close.BackColor = Color.FromArgb(6, 55, 99);
        close.ForeColor = Color.White;
        close.FlatStyle = FlatStyle.Flat;
        close.FlatAppearance.BorderSize = 0;
        close.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 79, 120);
        close.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 36, 66);
        close.UseVisualStyleBackColor = false;
        close.Cursor = Cursors.Hand;
        close.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        ResumeLayout(false);
        PerformLayout();
    }
    private Label heading = null!;
    private PictureBox logo = null!;
    private DateTimePicker from = null!;
    private DateTimePicker to = null!;
    private TextBox filter = null!;
    private Button search = null!;
    private DataGridView grid = null!;
    private Label summary = null!;
    private Button previous = null!;
    private Button next = null!;
    private Button close = null!;
    private Button menuButton;
    private Label roleLabel;
    private Label label1;
    private Panel personalLine;
}
