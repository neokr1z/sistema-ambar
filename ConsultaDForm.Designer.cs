#nullable enable
namespace AMBAR.Semana4;

partial class ConsultaDForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        body = new Panel();
        panel3 = new Panel();
        panel2 = new Panel();
        panel1 = new Panel();
        personalLine = new Panel();
        identityTitle = new Label();
        identityValue = new Label();
        enrollmentTitle = new Label();
        enrollmentValue = new Label();
        contactTitle = new Label();
        contactValue = new Label();
        addressTitle = new Label();
        addressValue = new Label();
        actions = new Panel();
        export = new Button();
        heading = new Label();
        label1 = new Label();
        logo = new PictureBox();
        menuButton = new Button();
        body.SuspendLayout();
        actions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        SuspendLayout();
        // 
        // body
        // 
        body.AutoScroll = true;
        body.Controls.Add(panel3);
        body.Controls.Add(panel2);
        body.Controls.Add(panel1);
        body.Controls.Add(personalLine);
        body.Controls.Add(identityTitle);
        body.Controls.Add(identityValue);
        body.Controls.Add(enrollmentTitle);
        body.Controls.Add(enrollmentValue);
        body.Controls.Add(contactTitle);
        body.Controls.Add(contactValue);
        body.Controls.Add(addressTitle);
        body.Controls.Add(addressValue);
        body.Dock = DockStyle.Fill;
        body.Location = new Point(0, 75);
        body.Margin = new Padding(4);
        body.Name = "body";
        body.Size = new Size(1250, 835);
        body.TabIndex = 0;
        // 
        // panel3
        // 
        panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel3.BackColor = Color.FromArgb(8, 84, 143);
        panel3.Location = new Point(32, 897);
        panel3.Name = "panel3";
        panel3.Size = new Size(1109, 2);
        panel3.TabIndex = 16;
        // 
        // panel2
        // 
        panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel2.BackColor = Color.FromArgb(8, 84, 143);
        panel2.Location = new Point(32, 625);
        panel2.Name = "panel2";
        panel2.Size = new Size(1109, 2);
        panel2.TabIndex = 15;
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panel1.BackColor = Color.FromArgb(8, 84, 143);
        panel1.Location = new Point(32, 347);
        panel1.Name = "panel1";
        panel1.Size = new Size(1109, 2);
        panel1.TabIndex = 14;
        // 
        // personalLine
        // 
        personalLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        personalLine.BackColor = Color.FromArgb(8, 84, 143);
        personalLine.Location = new Point(32, 75);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1109, 2);
        personalLine.TabIndex = 13;
        // 
        // identityTitle
        // 
        identityTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        identityTitle.ForeColor = Color.FromArgb(61, 107, 134);
        identityTitle.Location = new Point(32, 25);
        identityTitle.Margin = new Padding(4, 0, 4, 0);
        identityTitle.Name = "identityTitle";
        identityTitle.Size = new Size(210, 44);
        identityTitle.TabIndex = 0;
        identityTitle.Text = "Identificación";
        identityTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // identityValue
        // 
        identityValue.Font = new Font("Segoe UI", 11F);
        identityValue.ForeColor = Color.FromArgb(61, 107, 134);
        identityValue.Location = new Point(32, 75);
        identityValue.Margin = new Padding(4, 0, 4, 0);
        identityValue.Name = "identityValue";
        identityValue.Size = new Size(1100, 219);
        identityValue.TabIndex = 1;
        identityValue.Text = "Datos del expediente";
        identityValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // enrollmentTitle
        // 
        enrollmentTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        enrollmentTitle.ForeColor = Color.FromArgb(61, 107, 134);
        enrollmentTitle.Location = new Point(32, 300);
        enrollmentTitle.Margin = new Padding(4, 0, 4, 0);
        enrollmentTitle.Name = "enrollmentTitle";
        enrollmentTitle.Size = new Size(144, 44);
        enrollmentTitle.TabIndex = 2;
        enrollmentTitle.Text = "Inscripción";
        enrollmentTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // enrollmentValue
        // 
        enrollmentValue.Font = new Font("Segoe UI", 11F);
        enrollmentValue.ForeColor = Color.FromArgb(61, 107, 134);
        enrollmentValue.Location = new Point(32, 350);
        enrollmentValue.Margin = new Padding(4, 0, 4, 0);
        enrollmentValue.Name = "enrollmentValue";
        enrollmentValue.Size = new Size(1100, 219);
        enrollmentValue.TabIndex = 3;
        enrollmentValue.Text = "Datos del expediente";
        enrollmentValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // contactTitle
        // 
        contactTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        contactTitle.ForeColor = Color.FromArgb(61, 107, 134);
        contactTitle.Location = new Point(32, 575);
        contactTitle.Margin = new Padding(4, 0, 4, 0);
        contactTitle.Name = "contactTitle";
        contactTitle.Size = new Size(138, 44);
        contactTitle.TabIndex = 4;
        contactTitle.Text = "Contacto";
        contactTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // contactValue
        // 
        contactValue.Font = new Font("Segoe UI", 11F);
        contactValue.ForeColor = Color.FromArgb(61, 107, 134);
        contactValue.Location = new Point(32, 625);
        contactValue.Margin = new Padding(4, 0, 4, 0);
        contactValue.Name = "contactValue";
        contactValue.Size = new Size(1100, 219);
        contactValue.TabIndex = 5;
        contactValue.Text = "Datos del expediente";
        contactValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // addressTitle
        // 
        addressTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        addressTitle.ForeColor = Color.FromArgb(61, 107, 134);
        addressTitle.Location = new Point(32, 850);
        addressTitle.Margin = new Padding(4, 0, 4, 0);
        addressTitle.Name = "addressTitle";
        addressTitle.Size = new Size(1100, 44);
        addressTitle.TabIndex = 6;
        addressTitle.Text = "Domicilio";
        addressTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // addressValue
        // 
        addressValue.Font = new Font("Segoe UI", 11F);
        addressValue.ForeColor = Color.FromArgb(61, 107, 134);
        addressValue.Location = new Point(32, 900);
        addressValue.Margin = new Padding(4, 0, 4, 0);
        addressValue.Name = "addressValue";
        addressValue.Size = new Size(1100, 219);
        addressValue.TabIndex = 7;
        addressValue.Text = "Datos del expediente";
        addressValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // actions
        // 
        actions.Controls.Add(export);
        actions.Dock = DockStyle.Bottom;
        actions.Location = new Point(0, 910);
        actions.Margin = new Padding(4);
        actions.Name = "actions";
        actions.Size = new Size(1250, 90);
        actions.TabIndex = 1;
        // 
        // export
        // 
        export.BackColor = Color.FromArgb(6, 55, 99);
        export.Cursor = Cursors.Hand;
        export.FlatStyle = FlatStyle.Flat;
        export.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        export.ForeColor = Color.White;
        export.Location = new Point(20, 18);
        export.Margin = new Padding(4);
        export.Name = "export";
        export.Size = new Size(275, 52);
        export.TabIndex = 0;
        export.Text = "Guardar como PDF";
        export.UseVisualStyleBackColor = false;
        export.Click += ExportClick;
        // 
        // heading
        // 
        heading.Dock = DockStyle.Top;
        heading.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(0, 0);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Padding = new Padding(22, 10, 0, 0);
        heading.Size = new Size(1250, 75);
        heading.TabIndex = 2;
        heading.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        label1.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(6, 55, 99);
        label1.Location = new Point(549, 14);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(688, 46);
        label1.TabIndex = 3;
        label1.Text = "EXPEDIENTE ESCOLAR - CONSULTA DIGITAL";
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(59, 0);
        logo.Margin = new Padding(4);
        logo.Name = "logo";
        logo.Size = new Size(213, 79);
        logo.SizeMode = PictureBoxSizeMode.StretchImage;
        logo.TabIndex = 17;
        logo.TabStop = false;
        // 
        // menuButton
        // 
        menuButton.BackColor = Color.White;
        menuButton.Cursor = Cursors.Hand;
        menuButton.FlatAppearance.BorderSize = 0;
        menuButton.FlatStyle = FlatStyle.Flat;
        menuButton.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        menuButton.ForeColor = Color.FromArgb(6, 55, 99);
        menuButton.Location = new Point(12, 8);
        menuButton.Name = "menuButton";
        menuButton.Size = new Size(55, 55);
        menuButton.TabIndex = 19;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        // 
        // ConsultaDForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1250, 1000);
        Controls.Add(menuButton);
        Controls.Add(logo);
        Controls.Add(label1);
        Controls.Add(body);
        Controls.Add(actions);
        Controls.Add(heading);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4);
        MinimumSize = new Size(1120, 801);
        Name = "ConsultaDForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AMBAR | Expediente digital";
        body.ResumeLayout(false);
        actions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ResumeLayout(false);
    }

    private Panel body = null!;
    private Panel actions = null!;
    private Label identityTitle = null!;
    private Label identityValue = null!;
    private Label enrollmentTitle = null!;
    private Label enrollmentValue = null!;
    private Label contactTitle = null!;
    private Label contactValue = null!;
    private Label addressTitle = null!;
    private Label addressValue = null!;
    private Button export = null!;
    private Label heading;
    private Label label1;
    private Panel panel2;
    private Panel panel1;
    private Panel personalLine;
    private Panel panel3;
    private PictureBox logo;
    private Button menuButton;
}