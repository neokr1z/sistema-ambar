using System.Data;

namespace AMBAR.Semana4;

internal sealed class StudentsForm : Form
{
    private readonly Session actor;
    private readonly DataGridView grid=new() {Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,
        AllowUserToDeleteRows=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,MultiSelect=false,
        AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,RowHeadersVisible=false,
        BackgroundColor=Color.White,BorderStyle=BorderStyle.None};
    private readonly TextBox filter=Theme.Input(70);
    private readonly Label count=Theme.Text("",10,false,Theme.Gray);

    public StudentsForm(Session session)
    {
        actor=session;
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría/Administración.");
        Theme.Setup(this,"Consulta de alumnos",1240,740);
        Controls.Add(grid);
        var line=new FlowLayoutPanel {Dock=DockStyle.Top,Height=82,Padding=new Padding(12),WrapContents=false};
        line.Controls.Add(Theme.Text("Buscar:",10,true));
        line.Controls.Add(filter);
        var refresh=Theme.Button("Buscar / actualizar",false);
        refresh.Click+=(_,_)=>LoadData();
        line.Controls.Add(refresh);
        line.Controls.Add(count);
        Controls.Add(line);
        var actions=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=72,Padding=new Padding(12)};
        var view=Theme.Button("Ver expediente");
        var edit=Theme.Button("Modificar",false);
        var pdf=Theme.Button("Exportar PDF",false);
        var hide=Theme.Button("Ocultar registro",false);
        actions.Controls.Add(view);actions.Controls.Add(edit);actions.Controls.Add(pdf);actions.Controls.Add(hide);
        if(session.IsAdmin)
        {
            var restore=Theme.Button("Restaurar",false);
            restore.Click+=(_,_)=>ChangeVisibility(true);
            actions.Controls.Add(restore);
        }
        Controls.Add(actions);
        Controls.Add(Theme.Heading("Consulta digital de expedientes"));
        view.Click+=(_,_)=>View();
        edit.Click+=(_,_)=>Edit();
        pdf.Click+=(_,_)=>Pdf();
        hide.Click+=(_,_)=>ChangeVisibility(false);
        grid.CellDoubleClick+=(_,_)=>View();
        LoadData();
    }

    private string SelectedControl()
    {
        if(grid.CurrentRow is null)throw new ArgumentException("Selecciona primero un alumno.");
        return Convert.ToString(grid.CurrentRow.Cells["No. control"].Value)
            ?? throw new ArgumentException("No fue posible obtener el número de control.");
    }

    private void LoadData()
    {
        try
        {
            DataTable data=StudentRepository.ListStudents(actor,filter.Text);
            grid.DataSource=data;
            grid.ColumnHeadersDefaultCellStyle.BackColor=Theme.Blue;
            grid.ColumnHeadersDefaultCellStyle.ForeColor=Color.White;
            grid.EnableHeadersVisualStyles=false;
            grid.ColumnHeadersHeight=42;
            grid.RowTemplate.Height=34;
            grid.DefaultCellStyle.SelectionBackColor=Theme.Sky;
            grid.DefaultCellStyle.SelectionForeColor=Theme.Ink;
            count.Text=$"Registros: {data.Rows.Count}";
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private StudentData SelectedStudent()
        => StudentRepository.Detail(actor,SelectedControl())
            ?? throw new ArgumentException("No se pudo acceder al expediente.");

    private void View()
    {
        try {using var f=new DetailForm(actor,SelectedStudent());f.ShowDialog(this);LoadData();}
        catch(Exception ex){Problems.Show(ex);}
    }
    private void Edit()
    {
        try {using var f=new EditStudentForm(actor,SelectedStudent());f.ShowDialog(this);LoadData();}
        catch(Exception ex){Problems.Show(ex);}
    }
    private void Pdf()
    {
        try
        {
            var d=SelectedStudent();
            using var save=new SaveFileDialog {Filter="Documento PDF (*.pdf)|*.pdf",FileName=$"AMBAR_Inscripcion_{d.Control}.pdf",
                Title="Guardar expediente institucional"};
            if(save.ShowDialog(this)!=DialogResult.OK)return;
            PdfService.Create(d,save.FileName);
            MessageBox.Show("El PDF se generó correctamente.","AMBAR");
        }
        catch(Exception ex){Problems.Show(ex);}
    }
    private void ChangeVisibility(bool restore)
    {
        try
        {
            var d=SelectedStudent();
            if(restore && d.Status.ToUpperInvariant()!="OCULTO")
                throw new ArgumentException("Sólo se restauran registros ocultos.");
            if(!restore && d.Status.ToUpperInvariant()=="OCULTO")
                throw new ArgumentException("El registro ya está oculto.");
            string message=restore?"¿Restaurar este registro para Secretaría?":
                "¿Ocultar este registro de Secretaría? Los datos NO se eliminan de MySQL y permanecen disponibles para Administración.";
            if(MessageBox.Show(message,"Confirmar operación",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            StudentRepository.Visibility(actor,d.Control,restore);
            LoadData();
        }
        catch(Exception ex){Problems.Show(ex);}
    }
}

internal sealed class DetailForm : Form
{
    public DetailForm(Session actor,StudentData d)
    {
        Theme.Setup(this,"Expediente digital",1000,800);
        var area=new FlowLayoutPanel {Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,WrapContents=false,
            AutoScroll=true,Padding=new Padding(26)};
        Controls.Add(area);
        void Section(string name,string text)
        {
            var header=Theme.Text(name,14,true,Theme.Blue);
            header.Margin=new Padding(0,18,0,5);
            area.Controls.Add(header);
            var box=new Label {Text=text,BackColor=Color.White,ForeColor=Theme.Ink,
                Font=new Font("Segoe UI",11),Padding=new Padding(18),Width=880,AutoSize=true,
                Margin=new Padding(0,0,0,8)};
            area.Controls.Add(box);
        }
        Section("Identificación",$"Nombre: {d.FullName}\nNúmero de control: {d.Control}\nCURP: {d.Curp}\nFecha de nacimiento: {d.Birth:dd/MM/yyyy}\nEstatus: {d.Status}");
        Section("Inscripción",$"Ficha: {d.ApplicantNumber}\nCarrera: {d.Career}\nPlan: {d.Plan}\nGrupo: {d.Group}\nPeriodo: {d.Period}\nFecha de inscripción: {d.EnrollmentDate:dd/MM/yyyy}");
        Section("Contacto",$"Correo institucional: {d.Email}\nCorreo personal: {d.PersonalEmail}\nTeléfono: {d.Phone}\nEmergencia: {d.Emergency}\nNSS: {d.Nss}\nAlergias: {d.Allergies}");
        Section("Domicilio",$"{d.Street}, núm. {d.Exterior} {d.Interior}\nCol. {d.Neighborhood}, {d.Locality}\n{d.Municipality}, {d.State}, C.P. {d.Postal}");
        var actions=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=70,Padding=new Padding(10)};
        var export=Theme.Button("Guardar como PDF");
        export.Click+=(_,_)=>
        {
            try
            {
                using var dlg=new SaveFileDialog{Filter="PDF (*.pdf)|*.pdf",FileName=$"AMBAR_Expediente_{d.Control}.pdf"};
                if(dlg.ShowDialog(this)==DialogResult.OK){PdfService.Create(d,dlg.FileName);MessageBox.Show("PDF generado.");}
            }
            catch(Exception ex){Problems.Show(ex);}
        };
        actions.Controls.Add(export);
        Controls.Add(actions);
        Controls.Add(Theme.Heading("Expediente escolar · consulta digital"));
    }
}
