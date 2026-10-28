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
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        title.Text = "Consulta y baja de personal";
        title.Name = "title";
        title.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(6, 55, 99);
        title.Location = new Point(24, 20);
        title.Size = new Size(850, 50);
        search.Name = "search";
        search.PlaceholderText = "Nombre, usuario, CURP o RFC";
        search.Location = new Point(24, 90);
        search.Size = new Size(370, 31);
        search.TabIndex = 0;
        roleFilter.Name = "roleFilter";
        roleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        roleFilter.Items.AddRange(new object[] { "Todos los roles", "Docente", "Secretaria" });
        roleFilter.Location = new Point(410, 90);
        roleFilter.Size = new Size(180, 31);
        roleFilter.TabIndex = 1;
        statusFilter.Name = "statusFilter";
        statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        statusFilter.Items.AddRange(new object[] { "Todos los estados", "ACTIVO", "INACTIVO" });
        statusFilter.Location = new Point(606, 90);
        statusFilter.Size = new Size(180, 31);
        statusFilter.TabIndex = 2;
        searchButton.Name = "searchButton";
        searchButton.Text = "Consultar";
        searchButton.Location = new Point(802, 86);
        searchButton.Size = new Size(150, 40);
        searchButton.TabIndex = 3;
        searchButton.Click += SearchClick;
        grid.Name = "grid";
        grid.Location = new Point(24, 145);
        grid.Size = new Size(1032, 410);
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        grid.BackgroundColor = Color.White;
        grid.TabIndex = 4;
        resultLabel.Name = "resultLabel";
        resultLabel.Text = "Consulta docentes y secretarias, activos e inactivos.";
        resultLabel.Location = new Point(24, 568);
        resultLabel.Size = new Size(1032, 32);
        resultLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        deactivateButton.Name = "deactivateButton";
        deactivateButton.Text = "Dar de baja";
        deactivateButton.Location = new Point(706, 614);
        deactivateButton.Size = new Size(170, 44);
        deactivateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        deactivateButton.TabIndex = 5;
        deactivateButton.Click += DeactivateClick;
        closeButton.Name = "closeButton";
        closeButton.Text = "Cerrar";
        closeButton.Location = new Point(892, 614);
        closeButton.Size = new Size(164, 44);
        closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        closeButton.TabIndex = 6;
        closeButton.Click += CloseClick;
        Controls.Add(title);
        Controls.Add(search);
        Controls.Add(roleFilter);
        Controls.Add(statusFilter);
        Controls.Add(searchButton);
        Controls.Add(grid);
        Controls.Add(resultLabel);
        Controls.Add(deactivateButton);
        Controls.Add(closeButton);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 11F);
        BackColor = Color.FromArgb(244, 248, 252);
        ClientSize = new Size(1080, 682);
        MinimumSize = new Size(1000, 580);
        Name = "PersonalForm";
        Text = "Administrador | Consulta y baja de personal";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
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
}
