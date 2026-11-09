using System.Data;

namespace AMBAR.Semana4;

public partial class ConsultaA : Form
{
    private Sesion actor = null!;
    private bool ordenAscendente = true;
    private readonly Button ordenarControl = new();
    private readonly ComboBox selectorSemestre = new();

    public ConsultaA()
    {
        InitializeComponent();
        PrepararOrden();
        PrepararSemestres();
        grid.SelectionChanged+=(_,_)=>ActualizarAcciones();

    }

    internal ConsultaA(Sesion session) : this()
    {
        actor = session;
        if (!actor.CanSeeStudents)
            throw new UnauthorizedAccessException("Acceso exclusivo de Secretaría/Administración.");

        restore.Visible = session.IsAdmin;
        Text = session.IsAdmin ? "Administrador | Consultas" : "Secretaría | Consultas";
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (actor is null) { Close(); return; }
        LoadData();
    }

    private void PrepararSemestres()
    {
        searchBar.Height=122;
        var etiqueta=new Label
        {
            Text="Semestre:",Font=new Font("Segoe UI",10F,FontStyle.Bold),
            ForeColor=Theme.Blue,Location=new Point(35,79),Size=new Size(115,30),
            TextAlign=ContentAlignment.MiddleLeft
        };
        selectorSemestre.DropDownStyle=ComboBoxStyle.DropDownList;
        selectorSemestre.Font=new Font("Segoe UI",10F);
        selectorSemestre.Location=new Point(155,78);
        selectorSemestre.Width=190;
        selectorSemestre.Items.Add(new Choice(0,"Todos (1.º a 3.º)"));
        selectorSemestre.Items.Add(new Choice(1,"1.º semestre"));
        selectorSemestre.Items.Add(new Choice(2,"2.º semestre"));
        selectorSemestre.Items.Add(new Choice(3,"3.º semestre"));
        selectorSemestre.SelectedIndexChanged+=(_,_)=>
        {
            if(actor is not null)LoadData();
        };
        searchBar.Controls.Add(etiqueta);
        searchBar.Controls.Add(selectorSemestre);
        count.Location=new Point(380,77);
        count.Size=new Size(185,32);
        ordenarControl.Top=75;
        searchBar.Resize+=(_,_)=>ordenarControl.Left=Math.Max(600,searchBar.ClientSize.Width-ordenarControl.Width-25);
        ordenarControl.Left=Math.Max(600,searchBar.ClientSize.Width-ordenarControl.Width-25);
        selectorSemestre.SelectedIndex=0;
    }

    private bool EsReinscripcionSeleccionada()
        => grid.CurrentRow?.Cells["Tipo"].Value?.ToString()=="REINSCRIPCIÓN";

    private void VerReinscripcion()
    {
        if(grid.CurrentRow is null)throw new ArgumentException("Selecciona primero un alumno.");
        var c=grid.CurrentRow.Cells;
        MessageBox.Show(this,
            $"Número de control: {c["No. control"].Value}\n"+
            $"Alumno: {c["Nombre completo"].Value}\n"+
            $"Semestre: {c["Semestre"].Value}º\n"+
            $"Grupo: {c["Grupo"].Value}\n"+
            "Periodo: Agosto–diciembre 2026\n"+
            "Registro de reinscripción (datos básicos, sin expediente completo).",
            "AMBAR | Datos de reinscripción",MessageBoxButtons.OK,MessageBoxIcon.Information);
    }

    private void PrepararOrden()
    {
        ordenarControl.Name="ordenarControl";
        ordenarControl.Text="No. control ↑";
        ordenarControl.AccessibleName="Invertir orden por número de control";
        ordenarControl.Font=new Font("Segoe UI",9F,FontStyle.Bold);
        ordenarControl.Size=new Size(135,36);
        ordenarControl.Location=new Point(1035,20);
        ordenarControl.Anchor=AnchorStyles.Top|AnchorStyles.Right;
        ordenarControl.BackColor=Color.FromArgb(6,55,99);
        ordenarControl.ForeColor=Color.White;
        ordenarControl.FlatStyle=FlatStyle.Flat;
        ordenarControl.FlatAppearance.BorderSize=0;
        ordenarControl.Cursor=Cursors.Hand;
        ordenarControl.Click+=(_,_)=>
        {
            ordenAscendente=!ordenAscendente;
            ordenarControl.Text=ordenAscendente?"No. control ↑":"No. control ↓";
            LoadData();
        };
        searchBar.Controls.Add(ordenarControl);
        searchBar.Resize+=(_,_)=>ordenarControl.Left=Math.Max(600,searchBar.ClientSize.Width-ordenarControl.Width-25);
        ordenarControl.Left=Math.Max(600,searchBar.ClientSize.Width-ordenarControl.Width-25);
    }

    private void MenuButton_Click(object? sender, EventArgs e) => Close();
    private void RefreshClick(object? sender, EventArgs e) => LoadData();
    private void ViewClick(object? sender, EventArgs e) => View();
    private void EditClick(object? sender, EventArgs e) => Edit();
    private void PdfClick(object? sender, EventArgs e) => Pdf();
    private void HideClick(object? sender, EventArgs e) => ChangeVisibility(false);
    private void RestoreClick(object? sender, EventArgs e) => ChangeVisibility(true);
    private void GridDoubleClick(object? sender, DataGridViewCellEventArgs e) => View();

    private string SelectedControl()
    {
        if (grid.CurrentRow is null) throw new ArgumentException("Selecciona primero un alumno.");
        return Convert.ToString(grid.CurrentRow.Cells["No. control"].Value)
            ?? throw new ArgumentException("No fue posible obtener el número de control.");
    }

    private void LoadData()
    {
        try
        {
            int semestre=(selectorSemestre.SelectedItem as Choice)?.Id??0;
            DataTable data = RepositorioConsultaSemestres.Listar(actor,semestre,filter.Text,ordenAscendente);
            grid.DataSource = data;
            grid.Columns["Tipo"].Visible=false;
            ActualizarAcciones();
            count.Text = $"Registros: {data.Rows.Count}";
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void ActualizarAcciones()
    {
        bool completo=grid.CurrentRow is not null && !EsReinscripcionSeleccionada();
        edit.Enabled=completo;pdf.Enabled=completo;hide.Enabled=completo;
        restore.Enabled=completo && actor?.IsAdmin==true;
        horario.Enabled=grid.CurrentRow is not null;
        view.Text=completo?"Ver expediente":"Ver datos";
    }

    private StudentData SelectedStudent()
        => RepositorioAlumnos.Detail(actor, SelectedControl())
            ?? throw new ArgumentException("No se pudo acceder al expediente.");

    private void View()
    {
        try { if(EsReinscripcionSeleccionada())VerReinscripcion();
              else {using var f = new ConsultaDForm(actor, SelectedStudent()); f.ShowDialog(this); LoadData();} }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void Edit()
    {
        try { if(EsReinscripcionSeleccionada())throw new ArgumentException("Esta reinscripción solo dispone de los datos básicos.");
              using var f = new ExpedienteMForm(actor, SelectedStudent()); f.ShowDialog(this); LoadData(); }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void Pdf()
    {
        try
        {
            if(EsReinscripcionSeleccionada())throw new ArgumentException("No hay expediente completo que exportar. Usa Grupos / PDF para la lista.");
            var d = SelectedStudent();
            using var save = new SaveFileDialog
            {
                Filter = "Documento PDF (*.pdf)|*.pdf",
                FileName = $"AMBAR_Inscripcion_{d.Control}.pdf",
                Title = "Guardar expediente institucional"
            };
            if (save.ShowDialog(this) != DialogResult.OK) return;
            PdfService.Create(d, save.FileName);
            MessageBox.Show("El PDF se generó correctamente.", "AMBAR");
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void ChangeVisibility(bool restore)
    {
        try
        {
            if(EsReinscripcionSeleccionada())throw new ArgumentException("La ocultación de expedientes no aplica a registros mínimos.");
            var d = SelectedStudent();
            if (restore && d.Status.ToUpperInvariant() != "OCULTO")
                throw new ArgumentException("Sólo se restauran registros ocultos.");
            if (!restore && d.Status.ToUpperInvariant() == "OCULTO")
                throw new ArgumentException("El registro ya está oculto.");
            string message = restore ? "¿Restaurar este registro para Secretaría?" :
                "¿Ocultar este registro de Secretaría? Los datos NO se eliminan de MySQL y permanecen disponibles para Administración.";
            if (MessageBox.Show(message, "Confirmar operación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            RepositorioAlumnos.Visibility(actor, d.Control, restore);
            LoadData();
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
    private void HorarioClick(object? sender,EventArgs e)
    {
        try
        {
            if(EsReinscripcionSeleccionada())
            {
                var grupo=RepositorioConsultaSemestres.Reinscrito(actor,SelectedControl())
                    ?? throw new ArgumentException("No se encontró la reinscripción.");
                using var form=new HorarioGrupoDemoForm(actor,grupo.GrupoId,grupo.Nombre);
                form.ShowDialog(this);
            }
            else
            {
                using var form=new HorarioAlumnoForm(SelectedStudent());
                form.ShowDialog(this);
            }
        }
        catch(Exception ex){Problems.Show(ex);}
    }
}

