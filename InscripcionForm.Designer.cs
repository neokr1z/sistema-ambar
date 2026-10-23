#nullable enable

namespace AMBAR.Semana4;

partial class InscripcionForm
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InscripcionForm));
        personalSectionTitle = new Label();
        personalSectionLine = new Panel();
        addressSectionTitle = new Label();
        addressSectionLine = new Panel();
        enrollmentSectionTitle = new Label();
        enrollmentSectionLine = new Panel();
        tabs = new TabControl();
        personalTab = new TabPage();
        fichaLabel = new Label();
        ficha = new TextBox();
        findLabel = new Label();
        find = new Button();
        applicantStatusLabel = new Label();
        applicantStatus = new Label();
        nameLabel = new Label();
        name = new TextBox();
        paternalLabel = new Label();
        paternal = new TextBox();
        maternalLabel = new Label();
        maternal = new TextBox();
        curpLabel = new Label();
        curp = new TextBox();
        birthdayLabel = new Label();
        birthday = new DateTimePicker();
        phoneLabel = new Label();
        phone = new TextBox();
        emergencyLabel = new Label();
        emergency = new TextBox();
        personalMailLabel = new Label();
        personalMail = new TextBox();
        institutionalMailLabel = new Label();
        institutionalMail = new TextBox();
        nssLabel = new Label();
        nss = new TextBox();
        allergiesLabel = new Label();
        allergies = new TextBox();
        addressTab = new TabPage();
        streetLabel = new Label();
        street = new TextBox();
        exteriorLabel = new Label();
        exterior = new TextBox();
        interiorLabel = new Label();
        interior = new TextBox();
        neighborhoodLabel = new Label();
        neighborhood = new ComboBox();
        localityLabel = new Label();
        locality = new TextBox();
        municipalityLabel = new Label();
        municipality = new TextBox();
        stateLabel = new Label();
        state = new TextBox();
        postalLabel = new Label();
        postal = new TextBox();
        enrollmentTab = new TabPage();
        plansLabel = new Label();
        plans = new ComboBox();
        groupsLabel = new Label();
        groups = new ComboBox();
        groupNoteLabel = new Label();
        groupNote = new Label();
        controlNoteLabel = new Label();
        controlNote = new Label();
        periodNoteLabel = new Label();
        periodNote = new Label();
        statusNoteLabel = new Label();
        statusNote = new Label();
        actions = new Panel();
        submit = new Button();
        cancel = new Button();
        heading = new Label();
        header = new Panel();
        menuButton = new Button();
        logo = new PictureBox();
        tabs.SuspendLayout();
        personalTab.SuspendLayout();
        addressTab.SuspendLayout();
        enrollmentTab.SuspendLayout();
        actions.SuspendLayout();
        header.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
        SuspendLayout();
        // 
        // personalSectionTitle
        // 
        personalSectionTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        personalSectionTitle.ForeColor = Color.FromArgb(6, 55, 99);
        personalSectionTitle.Location = new Point(45, 165);
        personalSectionTitle.Name = "personalSectionTitle";
        personalSectionTitle.Size = new Size(1060, 43);
        personalSectionTitle.TabIndex = 1;
        personalSectionTitle.Text = "Datos personales";
        // 
        // personalSectionLine
        // 
        personalSectionLine.BackColor = Color.FromArgb(0, 92, 184);
        personalSectionLine.Location = new Point(45, 210);
        personalSectionLine.Name = "personalSectionLine";
        personalSectionLine.Size = new Size(1090, 2);
        personalSectionLine.TabIndex = 0;
        // 
        // addressSectionTitle
        // 
        addressSectionTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        addressSectionTitle.ForeColor = Color.FromArgb(6, 55, 99);
        addressSectionTitle.Location = new Point(45, 25);
        addressSectionTitle.Name = "addressSectionTitle";
        addressSectionTitle.Size = new Size(1060, 43);
        addressSectionTitle.TabIndex = 1;
        addressSectionTitle.Text = "Domicilio del alumno";
        // 
        // addressSectionLine
        // 
        addressSectionLine.BackColor = Color.FromArgb(0, 92, 184);
        addressSectionLine.Location = new Point(45, 70);
        addressSectionLine.Name = "addressSectionLine";
        addressSectionLine.Size = new Size(1090, 2);
        addressSectionLine.TabIndex = 0;
        // 
        // enrollmentSectionTitle
        // 
        enrollmentSectionTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        enrollmentSectionTitle.ForeColor = Color.FromArgb(6, 55, 99);
        enrollmentSectionTitle.Location = new Point(45, 25);
        enrollmentSectionTitle.Name = "enrollmentSectionTitle";
        enrollmentSectionTitle.Size = new Size(1060, 43);
        enrollmentSectionTitle.TabIndex = 1;
        enrollmentSectionTitle.Text = "Datos de inscripción";
        // 
        // enrollmentSectionLine
        // 
        enrollmentSectionLine.BackColor = Color.FromArgb(0, 92, 184);
        enrollmentSectionLine.Location = new Point(45, 70);
        enrollmentSectionLine.Name = "enrollmentSectionLine";
        enrollmentSectionLine.Size = new Size(1090, 2);
        enrollmentSectionLine.TabIndex = 0;
        // 
        // tabs
        // 
        tabs.Controls.Add(personalTab);
        tabs.Controls.Add(addressTab);
        tabs.Controls.Add(enrollmentTab);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        tabs.ItemSize = new Size(300, 48);
        tabs.Location = new Point(0, 105);
        tabs.Margin = new Padding(4);
        tabs.Name = "tabs";
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1275, 700);
        tabs.SizeMode = TabSizeMode.Fixed;
        tabs.TabIndex = 0;
        // 
        // personalTab
        // 
        personalTab.AutoScroll = true;
        personalTab.BackColor = Color.White;
        personalTab.Controls.Add(personalSectionLine);
        personalTab.Controls.Add(personalSectionTitle);
        personalTab.Controls.Add(fichaLabel);
        personalTab.Controls.Add(ficha);
        personalTab.Controls.Add(findLabel);
        personalTab.Controls.Add(find);
        personalTab.Controls.Add(applicantStatusLabel);
        personalTab.Controls.Add(applicantStatus);
        personalTab.Controls.Add(nameLabel);
        personalTab.Controls.Add(name);
        personalTab.Controls.Add(paternalLabel);
        personalTab.Controls.Add(paternal);
        personalTab.Controls.Add(maternalLabel);
        personalTab.Controls.Add(maternal);
        personalTab.Controls.Add(curpLabel);
        personalTab.Controls.Add(curp);
        personalTab.Controls.Add(birthdayLabel);
        personalTab.Controls.Add(birthday);
        personalTab.Controls.Add(phoneLabel);
        personalTab.Controls.Add(phone);
        personalTab.Controls.Add(emergencyLabel);
        personalTab.Controls.Add(emergency);
        personalTab.Controls.Add(personalMailLabel);
        personalTab.Controls.Add(personalMail);
        personalTab.Controls.Add(institutionalMailLabel);
        personalTab.Controls.Add(institutionalMail);
        personalTab.Controls.Add(nssLabel);
        personalTab.Controls.Add(nss);
        personalTab.Controls.Add(allergiesLabel);
        personalTab.Controls.Add(allergies);
        personalTab.Location = new Point(4, 52);
        personalTab.Margin = new Padding(4);
        personalTab.Name = "personalTab";
        personalTab.Size = new Size(1267, 644);
        personalTab.TabIndex = 0;
        personalTab.Text = "01  DATOS PERSONALES";
        // 
        // fichaLabel
        // 
        fichaLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        fichaLabel.ForeColor = Color.FromArgb(61, 107, 134);
        fichaLabel.Location = new Point(45, 25);
        fichaLabel.Margin = new Padding(4, 0, 4, 0);
        fichaLabel.Name = "fichaLabel";
        fichaLabel.Size = new Size(213, 40);
        fichaLabel.TabIndex = 0;
        fichaLabel.Text = "Número de ficha";
        fichaLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ficha
        // 
        ficha.Font = new Font("Segoe UI", 11F);
        ficha.Location = new Point(270, 29);
        ficha.Margin = new Padding(4);
        ficha.MaxLength = 20;
        ficha.Name = "ficha";
        ficha.Size = new Size(345, 32);
        ficha.TabIndex = 8;
        // 
        // findLabel
        // 
        findLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        findLabel.ForeColor = Color.FromArgb(61, 107, 134);
        findLabel.Location = new Point(35, 80);
        findLabel.Margin = new Padding(4, 0, 4, 0);
        findLabel.Name = "findLabel";
        findLabel.Size = new Size(365, 48);
        findLabel.TabIndex = 9;
        findLabel.Text = "Buscar en aspirantes";
        findLabel.TextAlign = ContentAlignment.MiddleLeft;
        findLabel.Visible = false;
        // 
        // find
        // 
        find.BackColor = Color.FromArgb(6, 55, 99);
        find.Cursor = Cursors.Hand;
        find.FlatStyle = FlatStyle.Flat;
        find.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        find.ForeColor = Color.White;
        find.Location = new Point(643, 28);
        find.Margin = new Padding(4);
        find.Name = "find";
        find.Size = new Size(225, 38);
        find.TabIndex = 10;
        find.Text = "Verificar ficha";
        find.UseVisualStyleBackColor = false;
        find.Click += FindClick;
        // 
        // applicantStatusLabel
        // 
        applicantStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        applicantStatusLabel.ForeColor = Color.FromArgb(61, 107, 134);
        applicantStatusLabel.Location = new Point(45, 92);
        applicantStatusLabel.Margin = new Padding(4, 0, 4, 0);
        applicantStatusLabel.Name = "applicantStatusLabel";
        applicantStatusLabel.Size = new Size(210, 40);
        applicantStatusLabel.TabIndex = 11;
        applicantStatusLabel.Text = "Estatus de admisión";
        applicantStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // applicantStatus
        // 
        applicantStatus.Font = new Font("Segoe UI", 10F);
        applicantStatus.ForeColor = Color.FromArgb(61, 107, 134);
        applicantStatus.Location = new Point(270, 94);
        applicantStatus.Margin = new Padding(4, 0, 4, 0);
        applicantStatus.Name = "applicantStatus";
        applicantStatus.Size = new Size(850, 44);
        applicantStatus.TabIndex = 12;
        applicantStatus.Text = "Busca una ficha para verificar la admisión.";
        applicantStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // nameLabel
        // 
        nameLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        nameLabel.ForeColor = Color.FromArgb(61, 107, 134);
        nameLabel.Location = new Point(45, 245);
        nameLabel.Margin = new Padding(4, 0, 4, 0);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(213, 40);
        nameLabel.TabIndex = 13;
        nameLabel.Text = "Nombre(s)";
        nameLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // name
        // 
        name.Font = new Font("Segoe UI", 11F);
        name.Location = new Point(270, 249);
        name.Margin = new Padding(4);
        name.MaxLength = 60;
        name.Name = "name";
        name.Size = new Size(290, 32);
        name.TabIndex = 14;
        // 
        // paternalLabel
        // 
        paternalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        paternalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        paternalLabel.Location = new Point(650, 245);
        paternalLabel.Margin = new Padding(4, 0, 4, 0);
        paternalLabel.Name = "paternalLabel";
        paternalLabel.Size = new Size(216, 40);
        paternalLabel.TabIndex = 15;
        paternalLabel.Text = "Apellido paterno";
        paternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // paternal
        // 
        paternal.Font = new Font("Segoe UI", 11F);
        paternal.Location = new Point(878, 249);
        paternal.Margin = new Padding(4);
        paternal.MaxLength = 50;
        paternal.Name = "paternal";
        paternal.Size = new Size(250, 32);
        paternal.TabIndex = 16;
        // 
        // maternalLabel
        // 
        maternalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        maternalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        maternalLabel.Location = new Point(45, 310);
        maternalLabel.Margin = new Padding(4, 0, 4, 0);
        maternalLabel.Name = "maternalLabel";
        maternalLabel.Size = new Size(213, 40);
        maternalLabel.TabIndex = 17;
        maternalLabel.Text = "Apellido materno";
        maternalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // maternal
        // 
        maternal.Font = new Font("Segoe UI", 11F);
        maternal.Location = new Point(270, 314);
        maternal.Margin = new Padding(4);
        maternal.MaxLength = 50;
        maternal.Name = "maternal";
        maternal.Size = new Size(290, 32);
        maternal.TabIndex = 18;
        // 
        // curpLabel
        // 
        curpLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        curpLabel.ForeColor = Color.FromArgb(61, 107, 134);
        curpLabel.Location = new Point(650, 310);
        curpLabel.Margin = new Padding(4, 0, 4, 0);
        curpLabel.Name = "curpLabel";
        curpLabel.Size = new Size(216, 40);
        curpLabel.TabIndex = 19;
        curpLabel.Text = "CURP";
        curpLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // curp
        // 
        curp.Font = new Font("Segoe UI", 11F);
        curp.Location = new Point(878, 314);
        curp.Margin = new Padding(4);
        curp.MaxLength = 18;
        curp.Name = "curp";
        curp.ReadOnly = true;
        curp.Size = new Size(250, 32);
        curp.TabIndex = 20;
        // 
        // birthdayLabel
        // 
        birthdayLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        birthdayLabel.ForeColor = Color.FromArgb(61, 107, 134);
        birthdayLabel.Location = new Point(45, 375);
        birthdayLabel.Margin = new Padding(4, 0, 4, 0);
        birthdayLabel.Name = "birthdayLabel";
        birthdayLabel.Size = new Size(213, 40);
        birthdayLabel.TabIndex = 21;
        birthdayLabel.Text = "Fecha de nacimiento";
        birthdayLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // birthday
        // 
        birthday.Format = DateTimePickerFormat.Short;
        birthday.Location = new Point(270, 379);
        birthday.Margin = new Padding(4);
        birthday.MinDate = new DateTime(1926, 1, 1);
        birthday.Name = "birthday";
        birthday.Size = new Size(290, 32);
        birthday.TabIndex = 22;
        birthday.Value = new DateTime(2005, 1, 1, 0, 0, 0, 0);
        // 
        // phoneLabel
        // 
        phoneLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        phoneLabel.ForeColor = Color.FromArgb(61, 107, 134);
        phoneLabel.Location = new Point(45, 440);
        phoneLabel.Margin = new Padding(4, 0, 4, 0);
        phoneLabel.Name = "phoneLabel";
        phoneLabel.Size = new Size(213, 40);
        phoneLabel.TabIndex = 23;
        phoneLabel.Text = "Teléfono (opcional)";
        phoneLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // phone
        // 
        phone.Font = new Font("Segoe UI", 11F);
        phone.Location = new Point(270, 444);
        phone.Margin = new Padding(4);
        phone.MaxLength = 10;
        phone.Name = "phone";
        phone.Size = new Size(290, 32);
        phone.TabIndex = 24;
        // 
        // emergencyLabel
        // 
        emergencyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        emergencyLabel.ForeColor = Color.FromArgb(61, 107, 134);
        emergencyLabel.Location = new Point(650, 440);
        emergencyLabel.Margin = new Padding(4, 0, 4, 0);
        emergencyLabel.Name = "emergencyLabel";
        emergencyLabel.Size = new Size(216, 40);
        emergencyLabel.TabIndex = 25;
        emergencyLabel.Text = "Teléfono de emergencia";
        emergencyLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // emergency
        // 
        emergency.Font = new Font("Segoe UI", 11F);
        emergency.Location = new Point(878, 444);
        emergency.Margin = new Padding(4);
        emergency.MaxLength = 10;
        emergency.Name = "emergency";
        emergency.Size = new Size(250, 32);
        emergency.TabIndex = 26;
        // 
        // personalMailLabel
        // 
        personalMailLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        personalMailLabel.ForeColor = Color.FromArgb(61, 107, 134);
        personalMailLabel.Location = new Point(45, 505);
        personalMailLabel.Margin = new Padding(4, 0, 4, 0);
        personalMailLabel.Name = "personalMailLabel";
        personalMailLabel.Size = new Size(213, 40);
        personalMailLabel.TabIndex = 27;
        personalMailLabel.Text = "Correo personal (opcional)";
        personalMailLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // personalMail
        // 
        personalMail.Font = new Font("Segoe UI", 11F);
        personalMail.Location = new Point(270, 509);
        personalMail.Margin = new Padding(4);
        personalMail.MaxLength = 120;
        personalMail.Name = "personalMail";
        personalMail.Size = new Size(290, 32);
        personalMail.TabIndex = 28;
        // 
        // institutionalMailLabel
        // 
        institutionalMailLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        institutionalMailLabel.ForeColor = Color.FromArgb(61, 107, 134);
        institutionalMailLabel.Location = new Point(650, 505);
        institutionalMailLabel.Margin = new Padding(4, 0, 4, 0);
        institutionalMailLabel.Name = "institutionalMailLabel";
        institutionalMailLabel.Size = new Size(216, 40);
        institutionalMailLabel.TabIndex = 29;
        institutionalMailLabel.Text = "Correo institucional";
        institutionalMailLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // institutionalMail
        // 
        institutionalMail.Font = new Font("Segoe UI", 11F);
        institutionalMail.Location = new Point(878, 509);
        institutionalMail.Margin = new Padding(4);
        institutionalMail.MaxLength = 120;
        institutionalMail.Name = "institutionalMail";
        institutionalMail.Size = new Size(250, 32);
        institutionalMail.TabIndex = 30;
        // 
        // nssLabel
        // 
        nssLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        nssLabel.ForeColor = Color.FromArgb(61, 107, 134);
        nssLabel.Location = new Point(650, 375);
        nssLabel.Margin = new Padding(4, 0, 4, 0);
        nssLabel.Name = "nssLabel";
        nssLabel.Size = new Size(216, 40);
        nssLabel.TabIndex = 31;
        nssLabel.Text = "NSS (si ya fue verificado)";
        nssLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // nss
        // 
        nss.Font = new Font("Segoe UI", 11F);
        nss.Location = new Point(878, 379);
        nss.Margin = new Padding(4);
        nss.MaxLength = 11;
        nss.Name = "nss";
        nss.Size = new Size(250, 32);
        nss.TabIndex = 32;
        // 
        // allergiesLabel
        // 
        allergiesLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        allergiesLabel.ForeColor = Color.FromArgb(61, 107, 134);
        allergiesLabel.Location = new Point(45, 570);
        allergiesLabel.Margin = new Padding(4, 0, 4, 0);
        allergiesLabel.Name = "allergiesLabel";
        allergiesLabel.Size = new Size(213, 40);
        allergiesLabel.TabIndex = 33;
        allergiesLabel.Text = "Alergias (opcional)";
        allergiesLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // allergies
        // 
        allergies.Font = new Font("Segoe UI", 11F);
        allergies.Location = new Point(270, 574);
        allergies.Margin = new Padding(4);
        allergies.MaxLength = 200;
        allergies.Name = "allergies";
        allergies.Size = new Size(858, 32);
        allergies.TabIndex = 34;
        // 
        // addressTab
        // 
        addressTab.AutoScroll = true;
        addressTab.BackColor = Color.White;
        addressTab.Controls.Add(addressSectionLine);
        addressTab.Controls.Add(addressSectionTitle);
        addressTab.Controls.Add(streetLabel);
        addressTab.Controls.Add(street);
        addressTab.Controls.Add(exteriorLabel);
        addressTab.Controls.Add(exterior);
        addressTab.Controls.Add(interiorLabel);
        addressTab.Controls.Add(interior);
        addressTab.Controls.Add(neighborhoodLabel);
        addressTab.Controls.Add(neighborhood);
        addressTab.Controls.Add(localityLabel);
        addressTab.Controls.Add(locality);
        addressTab.Controls.Add(municipalityLabel);
        addressTab.Controls.Add(municipality);
        addressTab.Controls.Add(stateLabel);
        addressTab.Controls.Add(state);
        addressTab.Controls.Add(postalLabel);
        addressTab.Controls.Add(postal);
        addressTab.Location = new Point(4, 52);
        addressTab.Margin = new Padding(4);
        addressTab.Name = "addressTab";
        addressTab.Size = new Size(1267, 754);
        addressTab.TabIndex = 1;
        addressTab.Text = "02  DOMICILIO";
        // 
        // streetLabel
        // 
        streetLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        streetLabel.ForeColor = Color.FromArgb(61, 107, 134);
        streetLabel.Location = new Point(650, 110);
        streetLabel.Margin = new Padding(4, 0, 4, 0);
        streetLabel.Name = "streetLabel";
        streetLabel.Size = new Size(213, 40);
        streetLabel.TabIndex = 0;
        streetLabel.Text = "Calle";
        streetLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // street
        // 
        street.Font = new Font("Segoe UI", 11F);
        street.Location = new Point(878, 114);
        street.Margin = new Padding(4);
        street.MaxLength = 100;
        street.Name = "street";
        street.Size = new Size(290, 32);
        street.TabIndex = 36;
        // 
        // exteriorLabel
        // 
        exteriorLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        exteriorLabel.ForeColor = Color.FromArgb(61, 107, 134);
        exteriorLabel.Location = new Point(650, 175);
        exteriorLabel.Margin = new Padding(4, 0, 4, 0);
        exteriorLabel.Name = "exteriorLabel";
        exteriorLabel.Size = new Size(216, 40);
        exteriorLabel.TabIndex = 37;
        exteriorLabel.Text = "Número exterior";
        exteriorLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // exterior
        // 
        exterior.Font = new Font("Segoe UI", 11F);
        exterior.Location = new Point(878, 179);
        exterior.Margin = new Padding(4);
        exterior.MaxLength = 15;
        exterior.Name = "exterior";
        exterior.Size = new Size(250, 32);
        exterior.TabIndex = 38;
        // 
        // interiorLabel
        // 
        interiorLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        interiorLabel.ForeColor = Color.FromArgb(61, 107, 134);
        interiorLabel.Location = new Point(650, 240);
        interiorLabel.Margin = new Padding(4, 0, 4, 0);
        interiorLabel.Name = "interiorLabel";
        interiorLabel.Size = new Size(213, 40);
        interiorLabel.TabIndex = 39;
        interiorLabel.Text = "Número interior (opcional)";
        interiorLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // interior
        // 
        interior.Font = new Font("Segoe UI", 11F);
        interior.Location = new Point(878, 244);
        interior.Margin = new Padding(4);
        interior.MaxLength = 15;
        interior.Name = "interior";
        interior.Size = new Size(290, 32);
        interior.TabIndex = 40;
        // 
        // neighborhoodLabel
        // 
        neighborhoodLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        neighborhoodLabel.ForeColor = Color.FromArgb(61, 107, 134);
        neighborhoodLabel.Location = new Point(45, 175);
        neighborhoodLabel.Margin = new Padding(4, 0, 4, 0);
        neighborhoodLabel.Name = "neighborhoodLabel";
        neighborhoodLabel.Size = new Size(216, 40);
        neighborhoodLabel.TabIndex = 41;
        neighborhoodLabel.Text = "Colonia";
        neighborhoodLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // neighborhood
        // 
        neighborhood.Font = new Font("Segoe UI", 11F);
        neighborhood.Location = new Point(270, 179);
        neighborhood.Margin = new Padding(4);
        neighborhood.DropDownStyle = ComboBoxStyle.DropDownList;
        neighborhood.Name = "neighborhood";
        neighborhood.Size = new Size(250, 32);
        neighborhood.TabIndex = 42;
        // 
        // localityLabel
        // 
        localityLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        localityLabel.ForeColor = Color.FromArgb(61, 107, 134);
        localityLabel.Location = new Point(45, 240);
        localityLabel.Margin = new Padding(4, 0, 4, 0);
        localityLabel.Name = "localityLabel";
        localityLabel.Size = new Size(213, 40);
        localityLabel.TabIndex = 43;
        localityLabel.Text = "Localidad";
        localityLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // locality
        // 
        locality.Font = new Font("Segoe UI", 11F);
        locality.Location = new Point(270, 244);
        locality.Margin = new Padding(4);
        locality.MaxLength = 100;
        locality.Name = "locality";
        locality.Size = new Size(290, 32);
        locality.TabIndex = 44;
        // 
        // municipalityLabel
        // 
        municipalityLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        municipalityLabel.ForeColor = Color.FromArgb(61, 107, 134);
        municipalityLabel.Location = new Point(45, 305);
        municipalityLabel.Margin = new Padding(4, 0, 4, 0);
        municipalityLabel.Name = "municipalityLabel";
        municipalityLabel.Size = new Size(216, 40);
        municipalityLabel.TabIndex = 45;
        municipalityLabel.Text = "Municipio";
        municipalityLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // municipality
        // 
        municipality.Font = new Font("Segoe UI", 11F);
        municipality.Location = new Point(270, 309);
        municipality.Margin = new Padding(4);
        municipality.MaxLength = 100;
        municipality.Name = "municipality";
        municipality.Size = new Size(250, 32);
        municipality.TabIndex = 46;
        // 
        // stateLabel
        // 
        stateLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        stateLabel.ForeColor = Color.FromArgb(61, 107, 134);
        stateLabel.Location = new Point(45, 370);
        stateLabel.Margin = new Padding(4, 0, 4, 0);
        stateLabel.Name = "stateLabel";
        stateLabel.Size = new Size(213, 40);
        stateLabel.TabIndex = 47;
        stateLabel.Text = "Estado";
        stateLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // state
        // 
        state.Font = new Font("Segoe UI", 11F);
        state.Location = new Point(270, 374);
        state.Margin = new Padding(4);
        state.MaxLength = 100;
        state.Name = "state";
        state.Size = new Size(290, 32);
        state.TabIndex = 48;
        // 
        // postalLabel
        // 
        postalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        postalLabel.ForeColor = Color.FromArgb(61, 107, 134);
        postalLabel.Location = new Point(45, 110);
        postalLabel.Margin = new Padding(4, 0, 4, 0);
        postalLabel.Name = "postalLabel";
        postalLabel.Size = new Size(216, 40);
        postalLabel.TabIndex = 49;
        postalLabel.Text = "Código postal";
        postalLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // postal
        // 
        postal.Font = new Font("Segoe UI", 11F);
        postal.Location = new Point(270, 114);
        postal.Margin = new Padding(4);
        postal.MaxLength = 5;
        postal.Name = "postal";
        postal.Size = new Size(250, 32);
        postal.TabIndex = 50;
        // 
        // enrollmentTab
        // 
        enrollmentTab.AutoScroll = true;
        enrollmentTab.BackColor = Color.White;
        enrollmentTab.Controls.Add(enrollmentSectionLine);
        enrollmentTab.Controls.Add(enrollmentSectionTitle);
        enrollmentTab.Controls.Add(plansLabel);
        enrollmentTab.Controls.Add(plans);
        enrollmentTab.Controls.Add(groupsLabel);
        enrollmentTab.Controls.Add(groups);
        enrollmentTab.Controls.Add(groupNoteLabel);
        enrollmentTab.Controls.Add(groupNote);
        enrollmentTab.Controls.Add(controlNoteLabel);
        enrollmentTab.Controls.Add(controlNote);
        enrollmentTab.Controls.Add(periodNoteLabel);
        enrollmentTab.Controls.Add(periodNote);
        enrollmentTab.Controls.Add(statusNoteLabel);
        enrollmentTab.Controls.Add(statusNote);
        enrollmentTab.Location = new Point(4, 52);
        enrollmentTab.Margin = new Padding(4);
        enrollmentTab.Name = "enrollmentTab";
        enrollmentTab.Size = new Size(1267, 754);
        enrollmentTab.TabIndex = 2;
        enrollmentTab.Text = "03  INSCRIPCIÓN";
        // 
        // plansLabel
        // 
        plansLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        plansLabel.ForeColor = Color.FromArgb(61, 107, 134);
        plansLabel.Location = new Point(45, 110);
        plansLabel.Margin = new Padding(4, 0, 4, 0);
        plansLabel.Name = "plansLabel";
        plansLabel.Size = new Size(213, 40);
        plansLabel.TabIndex = 0;
        plansLabel.Text = "Carrera / Plan de estudios";
        plansLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // plans
        // 
        plans.DropDownStyle = ComboBoxStyle.DropDownList;
        plans.Location = new Point(270, 114);
        plans.Margin = new Padding(4);
        plans.Name = "plans";
        plans.Size = new Size(310, 33);
        plans.TabIndex = 1;
        plans.SelectedIndexChanged += PlanChanged;
        // 
        // groupsLabel
        // 
        groupsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        groupsLabel.ForeColor = Color.FromArgb(61, 107, 134);
        groupsLabel.Location = new Point(650, 110);
        groupsLabel.Margin = new Padding(4, 0, 4, 0);
        groupsLabel.Name = "groupsLabel";
        groupsLabel.Size = new Size(216, 40);
        groupsLabel.TabIndex = 2;
        groupsLabel.Text = "Grupo disponible (semestre 1)";
        groupsLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // groups
        // 
        groups.DropDownStyle = ComboBoxStyle.DropDownList;
        groups.Location = new Point(878, 114);
        groups.Margin = new Padding(4);
        groups.Name = "groups";
        groups.Size = new Size(345, 33);
        groups.DropDownWidth = 480;
        groups.TabIndex = 3;
        // 
        // groupNoteLabel
        // 
        groupNoteLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        groupNoteLabel.ForeColor = Color.FromArgb(61, 107, 134);
        groupNoteLabel.Location = new Point(45, 195);
        groupNoteLabel.Margin = new Padding(4, 0, 4, 0);
        groupNoteLabel.Name = "groupNoteLabel";
        groupNoteLabel.Size = new Size(205, 40);
        groupNoteLabel.TabIndex = 4;
        groupNoteLabel.Text = "Disponibilidad";
        groupNoteLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // groupNote
        // 
        groupNote.ForeColor = Color.FromArgb(61, 107, 134);
        groupNote.Location = new Point(270, 197);
        groupNote.Margin = new Padding(4, 0, 4, 0);
        groupNote.Name = "groupNote";
        groupNote.Size = new Size(855, 45);
        groupNote.TabIndex = 5;
        groupNote.Text = "El sistema ofrecerá grupos vigentes de primer semestre con cupo.";
        groupNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // controlNoteLabel
        // 
        controlNoteLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        controlNoteLabel.ForeColor = Color.FromArgb(61, 107, 134);
        controlNoteLabel.Location = new Point(45, 265);
        controlNoteLabel.Margin = new Padding(4, 0, 4, 0);
        controlNoteLabel.Name = "controlNoteLabel";
        controlNoteLabel.Size = new Size(205, 40);
        controlNoteLabel.TabIndex = 6;
        controlNoteLabel.Text = "Número de control";
        controlNoteLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // controlNote
        // 
        controlNote.ForeColor = Color.FromArgb(61, 107, 134);
        controlNote.Location = new Point(270, 267);
        controlNote.Margin = new Padding(4, 0, 4, 0);
        controlNote.Name = "controlNote";
        controlNote.Size = new Size(855, 45);
        controlNote.TabIndex = 7;
        controlNote.Text = "Asignado al guardar; formato temporal AAAA + ID de aspirante";
        controlNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // periodNoteLabel
        // 
        periodNoteLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        periodNoteLabel.ForeColor = Color.FromArgb(61, 107, 134);
        periodNoteLabel.Location = new Point(45, 335);
        periodNoteLabel.Margin = new Padding(4, 0, 4, 0);
        periodNoteLabel.Name = "periodNoteLabel";
        periodNoteLabel.Size = new Size(205, 40);
        periodNoteLabel.TabIndex = 8;
        periodNoteLabel.Text = "Periodo escolar";
        periodNoteLabel.Visible = false;
        periodNoteLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // periodNote
        // 
        periodNote.ForeColor = Color.FromArgb(61, 107, 134);
        periodNote.Location = new Point(270, 337);
        periodNote.Margin = new Padding(4, 0, 4, 0);
        periodNote.Name = "periodNote";
        periodNote.Size = new Size(855, 45);
        periodNote.TabIndex = 9;
        periodNote.Text = "Agosto-Diciembre 2026";
        periodNote.Visible = false;
        periodNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusNoteLabel
        // 
        statusNoteLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        statusNoteLabel.ForeColor = Color.FromArgb(61, 107, 134);
        statusNoteLabel.Location = new Point(45, 405);
        statusNoteLabel.Margin = new Padding(4, 0, 4, 0);
        statusNoteLabel.Name = "statusNoteLabel";
        statusNoteLabel.Size = new Size(205, 40);
        statusNoteLabel.TabIndex = 10;
        statusNoteLabel.Text = "Estatus";
        statusNoteLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusNote
        // 
        statusNote.ForeColor = Color.FromArgb(61, 107, 134);
        statusNote.Location = new Point(270, 407);
        statusNote.Margin = new Padding(4, 0, 4, 0);
        statusNote.Name = "statusNote";
        statusNote.Size = new Size(855, 45);
        statusNote.TabIndex = 11;
        statusNote.Text = "INSCRITO";
        statusNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // actions
        // 
        actions.Controls.Add(submit);
        actions.Controls.Add(cancel);
        actions.Dock = DockStyle.Bottom;
        actions.Location = new Point(0, 805);
        actions.Margin = new Padding(4);
        actions.Name = "actions";
        actions.Size = new Size(1275, 85);
        actions.TabIndex = 1;
        // 
        // submit
        // 
        submit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        submit.BackColor = Color.FromArgb(6, 55, 99);
        submit.Cursor = Cursors.Hand;
        submit.FlatStyle = FlatStyle.Flat;
        submit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        submit.ForeColor = Color.White;
        submit.Location = new Point(962, 15);
        submit.Margin = new Padding(4);
        submit.Name = "submit";
        submit.Size = new Size(275, 52);
        submit.TabIndex = 0;
        submit.Text = "Guardar inscripción";
        submit.UseVisualStyleBackColor = false;
        submit.Click += Save;
        // 
        // cancel
        // 
        cancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cancel.BackColor = Color.FromArgb(6, 55, 99);
        cancel.Cursor = Cursors.Hand;
        cancel.FlatStyle = FlatStyle.Flat;
        cancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        cancel.ForeColor = Color.White;
        cancel.Location = new Point(775, 15);
        cancel.Margin = new Padding(4);
        cancel.Name = "cancel";
        cancel.Size = new Size(162, 52);
        cancel.TabIndex = 1;
        cancel.Text = "Cancelar";
        cancel.UseVisualStyleBackColor = false;
        cancel.Click += CancelClick;
        // 
        // heading
        // 
        heading.Dock = DockStyle.Right;
        heading.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        heading.ForeColor = Color.FromArgb(6, 55, 99);
        heading.Location = new Point(700, 0);
        heading.Margin = new Padding(4, 0, 4, 0);
        heading.Name = "heading";
        heading.Padding = new Padding(0, 0, 28, 0);
        heading.Size = new Size(575, 105);
        heading.TabIndex = 2;
        heading.Text = "ALTA E INSCRIPCIÓN";
        heading.TextAlign = ContentAlignment.MiddleRight;
        // 
        // header
        // 
        header.BackColor = Color.White;
        header.Controls.Add(menuButton);
        header.Controls.Add(heading);
        header.Controls.Add(logo);
        header.Dock = DockStyle.Top;
        header.Location = new Point(0, 0);
        header.Name = "header";
        header.Size = new Size(1275, 105);
        header.TabIndex = 3;
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
        menuButton.TabIndex = 3;
        menuButton.Text = "☰";
        menuButton.UseVisualStyleBackColor = false;
        menuButton.Visible = false;
        menuButton.Click += MenuButton_Click;
        // 
        // logo
        // 
        logo.Image = (Image)resources.GetObject("logo.Image");
        logo.Location = new Point(39, 5);
        logo.Name = "logo";
        logo.Size = new Size(253, 93);
        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.TabIndex = 4;
        logo.TabStop = false;
        logo.Click += logo_Click;
        // 
        // InscripcionForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(1275, 890);
        Controls.Add(tabs);
        Controls.Add(actions);
        Controls.Add(header);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(4);
        MinimumSize = new Size(1120, 801);
        Name = "InscripcionForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Administrador | Alta e inscripción";
        tabs.ResumeLayout(false);
        personalTab.ResumeLayout(false);
        personalTab.PerformLayout();
        addressTab.ResumeLayout(false);
        addressTab.PerformLayout();
        enrollmentTab.ResumeLayout(false);
        actions.ResumeLayout(false);
        header.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)logo).EndInit();
        ResumeLayout(false);
    }

    private Label personalSectionTitle = null!;
    private Panel personalSectionLine = null!;
    private Label addressSectionTitle = null!;
    private Panel addressSectionLine = null!;
    private Label enrollmentSectionTitle = null!;
    private Panel enrollmentSectionLine = null!;
    private Panel header = null!;
    private Button menuButton = null!;
    private PictureBox logo = null!;
    private TabControl tabs = null!;
    private TabPage personalTab = null!;
    private TabPage addressTab = null!;
    private TabPage enrollmentTab = null!;
    private Panel actions = null!;
    private Label heading = null!;
    private Label fichaLabel = null!;
    private TextBox ficha = null!;
    private Label findLabel = null!;
    private Button find = null!;
    private Label applicantStatusLabel = null!;
    private Label applicantStatus = null!;
    private Label nameLabel = null!;
    private TextBox name = null!;
    private Label paternalLabel = null!;
    private TextBox paternal = null!;
    private Label maternalLabel = null!;
    private TextBox maternal = null!;
    private Label curpLabel = null!;
    private TextBox curp = null!;
    private Label birthdayLabel = null!;
    private DateTimePicker birthday = null!;
    private Label phoneLabel = null!;
    private TextBox phone = null!;
    private Label emergencyLabel = null!;
    private TextBox emergency = null!;
    private Label personalMailLabel = null!;
    private TextBox personalMail = null!;
    private Label institutionalMailLabel = null!;
    private TextBox institutionalMail = null!;
    private Label nssLabel = null!;
    private TextBox nss = null!;
    private Label allergiesLabel = null!;
    private TextBox allergies = null!;
    private Label streetLabel = null!;
    private TextBox street = null!;
    private Label exteriorLabel = null!;
    private TextBox exterior = null!;
    private Label interiorLabel = null!;
    private TextBox interior = null!;
    private Label neighborhoodLabel = null!;
    private ComboBox neighborhood = null!;
    private Label localityLabel = null!;
    private TextBox locality = null!;
    private Label municipalityLabel = null!;
    private TextBox municipality = null!;
    private Label stateLabel = null!;
    private TextBox state = null!;
    private Label postalLabel = null!;
    private TextBox postal = null!;
    private Label plansLabel = null!;
    private ComboBox plans = null!;
    private Label groupsLabel = null!;
    private ComboBox groups = null!;
    private Label groupNoteLabel = null!;
    private Label groupNote = null!;
    private Label controlNoteLabel = null!;
    private Label controlNote = null!;
    private Label periodNoteLabel = null!;
    private Label periodNote = null!;
    private Label statusNoteLabel = null!;
    private Label statusNote = null!;
    private Button submit = null!;
    private Button cancel = null!;
}