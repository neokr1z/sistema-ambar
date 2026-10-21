#nullable enable
namespace AMBAR.Semana4;

partial class ExpedienteMForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        layout = new TableLayoutPanel();
        header = new Panel();
        logo = new PictureBox();
        heading = new Label();
        controlStrip = new Panel();
        controlValueLabel = new Label();
        controlValue = new Label();
        body = new Panel();
        personalTitle = new Label();
        personalLine = new Panel();
        addressTitle = new Label();
        addressLine = new Panel();
        inputFirstLabel = new Label();
        inputFirst = new TextBox();
        inputPaternalLabel = new Label();
        inputPaternal = new TextBox();
        inputMaternalLabel = new Label();
        inputMaternal = new TextBox();
        inputCurpLabel = new Label();
        inputCurp = new TextBox();
        birthLabel = new Label();
        birth = new DateTimePicker();
        inputNssLabel = new Label();
        inputNss = new TextBox();
        inputPhoneLabel = new Label();
        inputPhone = new TextBox();
        inputEmergencyLabel = new Label();
        inputEmergency = new TextBox();
        inputPersonalEmailLabel = new Label();
        inputPersonalEmail = new TextBox();
        inputEmailLabel = new Label();
        inputEmail = new TextBox();
        inputAllergiesLabel = new Label();
        inputAllergies = new TextBox();
        inputStreetLabel = new Label();
        inputStreet = new TextBox();
        inputExteriorLabel = new Label();
        inputExterior = new TextBox();
        inputInteriorLabel = new Label();
        inputInterior = new TextBox();
        inputNeighborhoodLabel = new Label();
        inputNeighborhood = new TextBox();
        inputLocalityLabel = new Label();
        inputLocality = new TextBox();
        inputMunicipalityLabel = new Label();
        inputMunicipality = new TextBox();
        inputStateLabel = new Label();
        inputState = new TextBox();
        inputPostalLabel = new Label();
        inputPostal = new TextBox();
        actions = new FlowLayoutPanel();
        save = new Button();
        cancel = new Button();
        layout.SuspendLayout();
        header.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        controlStrip.SuspendLayout();
        body.SuspendLayout();
        actions.SuspendLayout();
        SuspendLayout();
        // 
        // layout
        // 
        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(header, 0, 0);
        layout.Controls.Add(controlStrip, 0, 1);
        layout.Controls.Add(body, 0, 2);
        layout.Controls.Add(actions, 0, 3);
        layout.Dock = DockStyle.Fill;
        layout.Location = new Point(0, 0);
        layout.Margin = new Padding(0);
        layout.Name = "layout";
        layout.RowCount = 4;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
        layout.Size = new Size(1240, 870);
        layout.TabIndex = 0;
        // 
        // header
        // 
        header.BackColor = Color.White;
        header.Controls.Add(logo);
        header.Controls.Add(heading);
        header.Dock = DockStyle.Fill;
        header.Location = new Point(0, 0);
        header.Margin = new Padding(0);
        header.Name = "header";
        header.Size = new Size(1240, 105);
        header.TabIndex = 2;
        // 
        // logo
        // 
        logo.Image = Properties.Resources.tecnm_logo;
        logo.Location = new Point(35, 12);
        logo.Name = "logo";
        logo.Size = new Size(212, 80);
        logo.SizeMode = PictureBoxSizeMode.StretchImage;
        logo.TabIndex = 0;
        logo.TabStop = false;
        // 
        // heading
        // 
        heading.Dock = DockStyle.Right;
        heading.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(440, 0);
        heading.Name = "heading";
        heading.Padding = new Padding(0, 0, 28, 0);
        heading.Size = new Size(800, 105);
        heading.TabIndex = 1;
        heading.Text = "MODIFICAR EXPEDIENTE · Número de control";
        heading.TextAlign = ContentAlignment.MiddleRight;
        // 
        // controlStrip
        // 
        controlStrip.BackColor = Color.White;
        controlStrip.Controls.Add(controlValueLabel);
        controlStrip.Controls.Add(controlValue);
        controlStrip.Dock = DockStyle.Fill;
        controlStrip.Location = new Point(0, 105);
        controlStrip.Margin = new Padding(0);
        controlStrip.Name = "controlStrip";
        controlStrip.Size = new Size(1240, 58);
        controlStrip.TabIndex = 1;
        // 
        // controlValueLabel
        // 
        controlValueLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        controlValueLabel.ForeColor = Color.FromArgb(61, 107, 134);
        controlValueLabel.Location = new Point(48, 10);
        controlValueLabel.Name = "controlValueLabel";
        controlValueLabel.Size = new Size(276, 36);
        controlValueLabel.TabIndex = 0;
        controlValueLabel.Text = "Número de control (no editable)";
        controlValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // controlValue
        // 
        controlValue.Font = new Font("Segoe UI", 11F);
        controlValue.ForeColor = Color.FromArgb(6, 55, 99);
        controlValue.Location = new Point(330, 10);
        controlValue.Name = "controlValue";
        controlValue.Size = new Size(325, 36);
        controlValue.TabIndex = 1;
        controlValue.Text = "Número de control";
        controlValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // body
        // 
        body.AutoScroll = true;
        body.BackColor = Color.White;
        body.Controls.Add(personalTitle);
        body.Controls.Add(personalLine);
        body.Controls.Add(addressTitle);
        body.Controls.Add(addressLine);
        body.Controls.Add(inputFirstLabel);
        body.Controls.Add(inputFirst);
        body.Controls.Add(inputPaternalLabel);
        body.Controls.Add(inputPaternal);
        body.Controls.Add(inputMaternalLabel);
        body.Controls.Add(inputMaternal);
        body.Controls.Add(inputCurpLabel);
        body.Controls.Add(inputCurp);
        body.Controls.Add(birthLabel);
        body.Controls.Add(birth);
        body.Controls.Add(inputNssLabel);
        body.Controls.Add(inputNss);
        body.Controls.Add(inputPhoneLabel);
        body.Controls.Add(inputPhone);
        body.Controls.Add(inputEmergencyLabel);
        body.Controls.Add(inputEmergency);
        body.Controls.Add(inputPersonalEmailLabel);
        body.Controls.Add(inputPersonalEmail);
        body.Controls.Add(inputEmailLabel);
        body.Controls.Add(inputEmail);
        body.Controls.Add(inputAllergiesLabel);
        body.Controls.Add(inputAllergies);
        body.Controls.Add(inputStreetLabel);
        body.Controls.Add(inputStreet);
        body.Controls.Add(inputExteriorLabel);
        body.Controls.Add(inputExterior);
        body.Controls.Add(inputInteriorLabel);
        body.Controls.Add(inputInterior);
        body.Controls.Add(inputNeighborhoodLabel);
        body.Controls.Add(inputNeighborhood);
        body.Controls.Add(inputLocalityLabel);
        body.Controls.Add(inputLocality);
        body.Controls.Add(inputMunicipalityLabel);
        body.Controls.Add(inputMunicipality);
        body.Controls.Add(inputStateLabel);
        body.Controls.Add(inputState);
        body.Controls.Add(inputPostalLabel);
        body.Controls.Add(inputPostal);
        body.Dock = DockStyle.Fill;
        body.Location = new Point(0, 163);
        body.Margin = new Padding(0);
        body.Name = "body";
        body.Size = new Size(1240, 625);
        body.TabIndex = 0;
        // 
        // personalTitle
        // 
        personalTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        personalTitle.ForeColor = Color.FromArgb(6, 55, 99);
        personalTitle.Location = new Point(38, 28);
        personalTitle.Name = "personalTitle";
        personalTitle.Size = new Size(1110, 40);
        personalTitle.TabIndex = 0;
        personalTitle.Text = "1. Datos personales";
        // 
        // personalLine
        // 
        personalLine.BackColor = Color.FromArgb(0, 92, 184);
        personalLine.Location = new Point(38, 72);
        personalLine.Name = "personalLine";
        personalLine.Size = new Size(1110, 2);
        personalLine.TabIndex = 1;
        // 
        // addressTitle
        // 
        addressTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        addressTitle.ForeColor = Color.FromArgb(6, 55, 99);
        addressTitle.Location = new Point(38, 433);
        addressTitle.Name = "addressTitle";
        addressTitle.Size = new Size(1110, 40);
        addressTitle.TabIndex = 2;
        addressTitle.Text = "2. Domicilio";
        // 
        // addressLine
        // 
        addressLine.BackColor = Color.FromArgb(0, 92, 184);
        addressLine.Location = new Point(38, 477);
        addressLine.Name = "addressLine";
        addressLine.Size = new Size(1110, 2);
        addressLine.TabIndex = 3;
        // 
        // inputFirstLabel
        // 
        inputFirstLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputFirstLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputFirstLabel.Location = new Point(40, 101);
        inputFirstLabel.Name = "inputFirstLabel";
        inputFirstLabel.Size = new Size(187, 37);
        inputFirstLabel.TabIndex = 4;
        inputFirstLabel.Text = "Nombre(s)";
        inputFirstLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputFirst
        // 
        inputFirst.Font = new Font("Segoe UI", 11F);
        inputFirst.Location = new Point(237, 105);
        inputFirst.MaxLength = 60;
        inputFirst.Name = "inputFirst";
        inputFirst.Size = new Size(310, 32);
        inputFirst.TabIndex = 5;
        // 
        // inputPaternalLabel
        // 
        inputPaternalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputPaternalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputPaternalLabel.Location = new Point(615, 101);
        inputPaternalLabel.Name = "inputPaternalLabel";
        inputPaternalLabel.Size = new Size(225, 37);
        inputPaternalLabel.TabIndex = 6;
        inputPaternalLabel.Text = "Apellido paterno";
        inputPaternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputPaternal
        // 
        inputPaternal.Font = new Font("Segoe UI", 11F);
        inputPaternal.Location = new Point(850, 105);
        inputPaternal.MaxLength = 50;
        inputPaternal.Name = "inputPaternal";
        inputPaternal.Size = new Size(315, 32);
        inputPaternal.TabIndex = 7;
        // 
        // inputMaternalLabel
        // 
        inputMaternalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputMaternalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputMaternalLabel.Location = new Point(40, 152);
        inputMaternalLabel.Name = "inputMaternalLabel";
        inputMaternalLabel.Size = new Size(187, 37);
        inputMaternalLabel.TabIndex = 8;
        inputMaternalLabel.Text = "Apellido materno";
        inputMaternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputMaternal
        // 
        inputMaternal.Font = new Font("Segoe UI", 11F);
        inputMaternal.Location = new Point(237, 156);
        inputMaternal.MaxLength = 50;
        inputMaternal.Name = "inputMaternal";
        inputMaternal.Size = new Size(310, 32);
        inputMaternal.TabIndex = 9;
        // 
        // inputCurpLabel
        // 
        inputCurpLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputCurpLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputCurpLabel.Location = new Point(615, 152);
        inputCurpLabel.Name = "inputCurpLabel";
        inputCurpLabel.Size = new Size(225, 37);
        inputCurpLabel.TabIndex = 10;
        inputCurpLabel.Text = "CURP";
        inputCurpLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputCurp
        // 
        inputCurp.Font = new Font("Segoe UI", 11F);
        inputCurp.Location = new Point(850, 156);
        inputCurp.MaxLength = 18;
        inputCurp.Name = "inputCurp";
        inputCurp.Size = new Size(315, 32);
        inputCurp.TabIndex = 11;
        // 
        // birthLabel
        // 
        birthLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        birthLabel.ForeColor = Color.FromArgb(61, 107, 134);
        birthLabel.Location = new Point(40, 203);
        birthLabel.Name = "birthLabel";
        birthLabel.Size = new Size(187, 37);
        birthLabel.TabIndex = 12;
        birthLabel.Text = "Fecha de nacimiento";
        birthLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // birth
        // 
        birth.Format = DateTimePickerFormat.Short;
        birth.Location = new Point(237, 207);
        birth.Name = "birth";
        birth.Size = new Size(310, 30);
        birth.TabIndex = 13;
        // 
        // inputNssLabel
        // 
        inputNssLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputNssLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputNssLabel.Location = new Point(615, 203);
        inputNssLabel.Name = "inputNssLabel";
        inputNssLabel.Size = new Size(225, 37);
        inputNssLabel.TabIndex = 14;
        inputNssLabel.Text = "NSS (opcional)";
        inputNssLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputNss
        // 
        inputNss.Font = new Font("Segoe UI", 11F);
        inputNss.Location = new Point(850, 207);
        inputNss.MaxLength = 11;
        inputNss.Name = "inputNss";
        inputNss.Size = new Size(315, 32);
        inputNss.TabIndex = 15;
        // 
        // inputPhoneLabel
        // 
        inputPhoneLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputPhoneLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputPhoneLabel.Location = new Point(40, 254);
        inputPhoneLabel.Name = "inputPhoneLabel";
        inputPhoneLabel.Size = new Size(187, 37);
        inputPhoneLabel.TabIndex = 16;
        inputPhoneLabel.Text = "Teléfono";
        inputPhoneLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputPhone
        // 
        inputPhone.Font = new Font("Segoe UI", 11F);
        inputPhone.Location = new Point(237, 258);
        inputPhone.MaxLength = 10;
        inputPhone.Name = "inputPhone";
        inputPhone.Size = new Size(310, 32);
        inputPhone.TabIndex = 17;
        // 
        // inputEmergencyLabel
        // 
        inputEmergencyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputEmergencyLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputEmergencyLabel.Location = new Point(615, 254);
        inputEmergencyLabel.Name = "inputEmergencyLabel";
        inputEmergencyLabel.Size = new Size(225, 37);
        inputEmergencyLabel.TabIndex = 18;
        inputEmergencyLabel.Text = "Teléfono de emergencia";
        inputEmergencyLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputEmergency
        // 
        inputEmergency.Font = new Font("Segoe UI", 11F);
        inputEmergency.Location = new Point(850, 258);
        inputEmergency.MaxLength = 10;
        inputEmergency.Name = "inputEmergency";
        inputEmergency.Size = new Size(315, 32);
        inputEmergency.TabIndex = 19;
        // 
        // inputPersonalEmailLabel
        // 
        inputPersonalEmailLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputPersonalEmailLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputPersonalEmailLabel.Location = new Point(40, 305);
        inputPersonalEmailLabel.Name = "inputPersonalEmailLabel";
        inputPersonalEmailLabel.Size = new Size(187, 37);
        inputPersonalEmailLabel.TabIndex = 20;
        inputPersonalEmailLabel.Text = "Correo personal";
        inputPersonalEmailLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputPersonalEmail
        // 
        inputPersonalEmail.Font = new Font("Segoe UI", 11F);
        inputPersonalEmail.Location = new Point(237, 309);
        inputPersonalEmail.MaxLength = 120;
        inputPersonalEmail.Name = "inputPersonalEmail";
        inputPersonalEmail.Size = new Size(310, 32);
        inputPersonalEmail.TabIndex = 21;
        // 
        // inputEmailLabel
        // 
        inputEmailLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputEmailLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputEmailLabel.Location = new Point(615, 305);
        inputEmailLabel.Name = "inputEmailLabel";
        inputEmailLabel.Size = new Size(225, 37);
        inputEmailLabel.TabIndex = 22;
        inputEmailLabel.Text = "Correo institucional";
        inputEmailLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputEmail
        // 
        inputEmail.Font = new Font("Segoe UI", 11F);
        inputEmail.Location = new Point(850, 309);
        inputEmail.MaxLength = 120;
        inputEmail.Name = "inputEmail";
        inputEmail.Size = new Size(315, 32);
        inputEmail.TabIndex = 23;
        // 
        // inputAllergiesLabel
        // 
        inputAllergiesLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputAllergiesLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputAllergiesLabel.Location = new Point(40, 356);
        inputAllergiesLabel.Name = "inputAllergiesLabel";
        inputAllergiesLabel.Size = new Size(187, 37);
        inputAllergiesLabel.TabIndex = 24;
        inputAllergiesLabel.Text = "Alergias";
        inputAllergiesLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputAllergies
        // 
        inputAllergies.Font = new Font("Segoe UI", 11F);
        inputAllergies.Location = new Point(237, 360);
        inputAllergies.MaxLength = 200;
        inputAllergies.Name = "inputAllergies";
        inputAllergies.Size = new Size(310, 32);
        inputAllergies.TabIndex = 25;
        // 
        // inputStreetLabel
        // 
        inputStreetLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputStreetLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputStreetLabel.Location = new Point(40, 505);
        inputStreetLabel.Name = "inputStreetLabel";
        inputStreetLabel.Size = new Size(187, 37);
        inputStreetLabel.TabIndex = 26;
        inputStreetLabel.Text = "Calle";
        inputStreetLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputStreet
        // 
        inputStreet.Font = new Font("Segoe UI", 11F);
        inputStreet.Location = new Point(237, 509);
        inputStreet.MaxLength = 100;
        inputStreet.Name = "inputStreet";
        inputStreet.Size = new Size(310, 32);
        inputStreet.TabIndex = 27;
        // 
        // inputExteriorLabel
        // 
        inputExteriorLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputExteriorLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputExteriorLabel.Location = new Point(615, 505);
        inputExteriorLabel.Name = "inputExteriorLabel";
        inputExteriorLabel.Size = new Size(225, 37);
        inputExteriorLabel.TabIndex = 28;
        inputExteriorLabel.Text = "Número exterior";
        inputExteriorLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputExterior
        // 
        inputExterior.Font = new Font("Segoe UI", 11F);
        inputExterior.Location = new Point(850, 509);
        inputExterior.MaxLength = 15;
        inputExterior.Name = "inputExterior";
        inputExterior.Size = new Size(315, 32);
        inputExterior.TabIndex = 29;
        // 
        // inputInteriorLabel
        // 
        inputInteriorLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputInteriorLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputInteriorLabel.Location = new Point(40, 556);
        inputInteriorLabel.Name = "inputInteriorLabel";
        inputInteriorLabel.Size = new Size(187, 37);
        inputInteriorLabel.TabIndex = 30;
        inputInteriorLabel.Text = "Número interior";
        inputInteriorLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputInterior
        // 
        inputInterior.Font = new Font("Segoe UI", 11F);
        inputInterior.Location = new Point(237, 560);
        inputInterior.MaxLength = 15;
        inputInterior.Name = "inputInterior";
        inputInterior.Size = new Size(310, 32);
        inputInterior.TabIndex = 31;
        // 
        // inputNeighborhoodLabel
        // 
        inputNeighborhoodLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputNeighborhoodLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputNeighborhoodLabel.Location = new Point(615, 556);
        inputNeighborhoodLabel.Name = "inputNeighborhoodLabel";
        inputNeighborhoodLabel.Size = new Size(225, 37);
        inputNeighborhoodLabel.TabIndex = 32;
        inputNeighborhoodLabel.Text = "Colonia";
        inputNeighborhoodLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputNeighborhood
        // 
        inputNeighborhood.Font = new Font("Segoe UI", 11F);
        inputNeighborhood.Location = new Point(850, 560);
        inputNeighborhood.MaxLength = 100;
        inputNeighborhood.Name = "inputNeighborhood";
        inputNeighborhood.Size = new Size(315, 32);
        inputNeighborhood.TabIndex = 33;
        // 
        // inputLocalityLabel
        // 
        inputLocalityLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputLocalityLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputLocalityLabel.Location = new Point(40, 607);
        inputLocalityLabel.Name = "inputLocalityLabel";
        inputLocalityLabel.Size = new Size(187, 37);
        inputLocalityLabel.TabIndex = 34;
        inputLocalityLabel.Text = "Localidad";
        inputLocalityLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputLocality
        // 
        inputLocality.Font = new Font("Segoe UI", 11F);
        inputLocality.Location = new Point(237, 611);
        inputLocality.MaxLength = 100;
        inputLocality.Name = "inputLocality";
        inputLocality.Size = new Size(310, 32);
        inputLocality.TabIndex = 35;
        // 
        // inputMunicipalityLabel
        // 
        inputMunicipalityLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputMunicipalityLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputMunicipalityLabel.Location = new Point(615, 607);
        inputMunicipalityLabel.Name = "inputMunicipalityLabel";
        inputMunicipalityLabel.Size = new Size(225, 37);
        inputMunicipalityLabel.TabIndex = 36;
        inputMunicipalityLabel.Text = "Municipio";
        inputMunicipalityLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputMunicipality
        // 
        inputMunicipality.Font = new Font("Segoe UI", 11F);
        inputMunicipality.Location = new Point(850, 611);
        inputMunicipality.MaxLength = 100;
        inputMunicipality.Name = "inputMunicipality";
        inputMunicipality.Size = new Size(315, 32);
        inputMunicipality.TabIndex = 37;
        // 
        // inputStateLabel
        // 
        inputStateLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputStateLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputStateLabel.Location = new Point(40, 658);
        inputStateLabel.Name = "inputStateLabel";
        inputStateLabel.Size = new Size(187, 37);
        inputStateLabel.TabIndex = 38;
        inputStateLabel.Text = "Estado";
        inputStateLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputState
        // 
        inputState.Font = new Font("Segoe UI", 11F);
        inputState.Location = new Point(237, 662);
        inputState.MaxLength = 100;
        inputState.Name = "inputState";
        inputState.Size = new Size(310, 32);
        inputState.TabIndex = 39;
        // 
        // inputPostalLabel
        // 
        inputPostalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        inputPostalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        inputPostalLabel.Location = new Point(615, 658);
        inputPostalLabel.Name = "inputPostalLabel";
        inputPostalLabel.Size = new Size(225, 37);
        inputPostalLabel.TabIndex = 40;
        inputPostalLabel.Text = "Código postal";
        inputPostalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // inputPostal
        // 
        inputPostal.Font = new Font("Segoe UI", 11F);
        inputPostal.Location = new Point(850, 662);
        inputPostal.MaxLength = 5;
        inputPostal.Name = "inputPostal";
        inputPostal.Size = new Size(315, 32);
        inputPostal.TabIndex = 41;
        // 
        // actions
        // 
        actions.BackColor = Color.White;
        actions.Controls.Add(save);
        actions.Controls.Add(cancel);
        actions.Dock = DockStyle.Fill;
        actions.FlowDirection = FlowDirection.RightToLeft;
        actions.Location = new Point(0, 788);
        actions.Margin = new Padding(0);
        actions.Name = "actions";
        actions.Padding = new Padding(12, 16, 18, 8);
        actions.Size = new Size(1240, 82);
        actions.TabIndex = 3;
        actions.WrapContents = false;
        // 
        // save
        // 
        save.BackColor = Color.FromArgb(6, 55, 99);
        save.Cursor = Cursors.Hand;
        save.FlatAppearance.BorderSize = 0;
        save.FlatStyle = FlatStyle.Flat;
        save.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        save.ForeColor = Color.White;
        save.Location = new Point(1000, 16);
        save.Margin = new Padding(5, 0, 5, 0);
        save.Name = "save";
        save.Size = new Size(205, 44);
        save.TabIndex = 0;
        save.Text = "Guardar cambios";
        save.UseVisualStyleBackColor = false;
        save.Click += Save;
        // 
        // cancel
        // 
        cancel.BackColor = Color.FromArgb(6, 55, 99);
        cancel.Cursor = Cursors.Hand;
        cancel.FlatAppearance.BorderSize = 0;
        cancel.FlatStyle = FlatStyle.Flat;
        cancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        cancel.ForeColor = Color.White;
        cancel.Location = new Point(825, 16);
        cancel.Margin = new Padding(5, 0, 5, 0);
        cancel.Name = "cancel";
        cancel.Size = new Size(165, 44);
        cancel.TabIndex = 1;
        cancel.Text = "Cancelar";
        cancel.UseVisualStyleBackColor = false;
        cancel.Click += CancelClick;
        // 
        // EditStudentForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1240, 870);
        Controls.Add(layout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1020, 730);
        Name = "EditStudentForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Administrador | Modificar Alumno";
        layout.ResumeLayout(false);
        header.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        controlStrip.ResumeLayout(false);
        body.ResumeLayout(false);
        body.PerformLayout();
        actions.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TableLayoutPanel layout = null!;
    private Panel body = null!;
    private FlowLayoutPanel actions = null!;
    private Panel header = null!;
    private Panel controlStrip = null!;
    private PictureBox logo = null!;
    private Label heading = null!;
    private Label personalTitle = null!;
    private Panel personalLine = null!;
    private Label addressTitle = null!;
    private Panel addressLine = null!;
    private Label controlValueLabel = null!;
    private Label controlValue = null!;
    private Button save = null!;
    private Button cancel = null!;
    private Label birthLabel = null!;
    private DateTimePicker birth = null!;
    private Label inputFirstLabel = null!;
    private TextBox inputFirst = null!;
    private Label inputPaternalLabel = null!;
    private TextBox inputPaternal = null!;
    private Label inputMaternalLabel = null!;
    private TextBox inputMaternal = null!;
    private Label inputCurpLabel = null!;
    private TextBox inputCurp = null!;
    private Label inputNssLabel = null!;
    private TextBox inputNss = null!;
    private Label inputPhoneLabel = null!;
    private TextBox inputPhone = null!;
    private Label inputEmergencyLabel = null!;
    private TextBox inputEmergency = null!;
    private Label inputAllergiesLabel = null!;
    private TextBox inputAllergies = null!;
    private Label inputEmailLabel = null!;
    private TextBox inputEmail = null!;
    private Label inputPersonalEmailLabel = null!;
    private TextBox inputPersonalEmail = null!;
    private Label inputStreetLabel = null!;
    private TextBox inputStreet = null!;
    private Label inputExteriorLabel = null!;
    private TextBox inputExterior = null!;
    private Label inputInteriorLabel = null!;
    private TextBox inputInterior = null!;
    private Label inputNeighborhoodLabel = null!;
    private TextBox inputNeighborhood = null!;
    private Label inputLocalityLabel = null!;
    private TextBox inputLocality = null!;
    private Label inputMunicipalityLabel = null!;
    private TextBox inputMunicipality = null!;
    private Label inputStateLabel = null!;
    private TextBox inputState = null!;
    private Label inputPostalLabel = null!;
    private TextBox inputPostal = null!;
}



