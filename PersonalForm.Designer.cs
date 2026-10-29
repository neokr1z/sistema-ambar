#nullable enable
namespace AMBAR.Semana4;

partial class PersonalForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        title = new Label();
        search = new TextBox();
        roleFilter = new ComboBox();
        statusFilter = new ComboBox();
        searchButton = new Button();
        deactivateButton = new Button();
        closeButton = new Button();
        grid = new DataGridView();
        resultLabel = new Label();
        menuButton = new Button();
        logo = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        SuspendLayout();
        // 
        // title
        // 
        title.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(6, 55, 99);
        title.Location = new Point(501, 20);
        title.Name = "title";
        title.Size = new Size(567, 50);
        title.TabIndex = 0;
        title.Text = "CONSULTA Y BAJA DE PERSONAL";
        // 
        // search
        // 
        search.Location = new Point(24, 90);
        search.Name = "search";
        search.PlaceholderText = "Nombre, usuario, CURP o RFC";
        search.Size = new Size(370, 32);
        search.TabIndex = 0;
        // 
        // roleFilter
        // 
        roleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        roleFilter.Items.AddRange(new object[] { "Todos los roles", "Docente", "Secretaria" });
        roleFilter.Location = new Point(410, 90);
        roleFilter.Name = "roleFilter";
        roleFilter.Size = new Size(180, 33);
        roleFilter.TabIndex = 1;
        // 
        // statusFilter
        // 
        statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        statusFilter.Items.AddRange(new object[] { "Todos los estados", "ACTIVO", "INACTIVO" });
        statusFilter.Location = new Point(606, 90);
        statusFilter.Name = "statusFilter";
        statusFilter.Size = new Size(180, 33);
        statusFilter.TabIndex = 2;
        // 
        // searchButton
        // 
        searchButton.Location = new Point(802, 86);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(150, 40);
        searchButton.TabIndex = 3;
        searchButton.Text = "Consultar";
        searchButton.Click += SearchClick;
        // 
        // deactivateButton
        // 
        deactivateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        deactivateButton.Location = new Point(706, 614);
        deactivateButton.Name = "deactivateButton";
        deactivateButton.Size = new Size(170, 44);
        deactivateButton.TabIndex = 5;
        deactivateButton.Text = "Dar de baja";
        deactivateButton.Click += DeactivateClick;
        // 
        // closeButton
        // 
        closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        closeButton.Location = new Point(892, 614);
        closeButton.Name = "closeButton";
        closeButton.Size = new Size(164, 44);
        closeButton.TabIndex = 6;
        closeButton.Text = "Cerrar";
        closeButton.Click += CloseClick;
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        grid.BackgroundColor = Color.White;
        grid.ColumnHeadersHeight = 29;
        grid.Location = new Point(24, 145);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersWidth = 51;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1032, 410);
        grid.TabIndex = 4;
        // 
        // resultLabel
        // 
        resultLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        resultLabel.Location = new Point(24, 568);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(1032, 32);
        resultLabel.TabIndex = 5;
        resultLabel.Text = "Consulta docentes y secretarias, activos e inactivos.";
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 12);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 27;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(60, -3);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(224, 86);
        logo.SizeMode = PictureBoxSizeMode.StretchImage;
        logo.TabIndex = 28;
        logo.TabStop = false;
        // 
        // PersonalForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(244, 248, 252);
        ClientSize = new Size(1080, 682);
        Controls.Add(logo);
        Controls.Add(menuButton);
        Controls.Add(title);
        Controls.Add(search);
        Controls.Add(roleFilter);
        Controls.Add(statusFilter);
        Controls.Add(searchButton);
        Controls.Add(grid);
        Controls.Add(resultLabel);
        Controls.Add(deactivateButton);
        Controls.Add(closeButton);
        Font = new Font("Segoe UI", 11F);
        MinimumSize = new Size(1000, 580);
        Name = "PersonalForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Administrador | Consulta y baja de personal";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    private Label title = null!;
    private TextBox search = null!;
    private ComboBox roleFilter = null!;
    private ComboBox statusFilter = null!;
    private Button searchButton = null!;
    private Button deactivateButton = null!;
    private Button closeButton = null!;
    private DataGridView grid = null!;
    private Label resultLabel = null!;
    private Button menuButton;
    private PictureBox logo;
}
