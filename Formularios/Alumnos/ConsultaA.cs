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
            $"Estatus: {c["Estatus"].Value}\n"+
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
    private void HideClick(object? sender, EventArgs e)
    {
        if(EsReinscripcionSeleccionada())DarDeBajaReinscripcion();
        else ChangeVisibility(false);
    }
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
        bool seleccionado=grid.CurrentRow is not null;
        bool esReinscripcion=seleccionado && EsReinscripcionSeleccionada();
        bool activo=seleccionado &&
            string.Equals(Convert.ToString(grid.CurrentRow!.Cells["Estatus"].Value),
                "INSCRITO",StringComparison.OrdinalIgnoreCase);
        bool completo=seleccionado && !esReinscripcion;
        edit.Enabled=completo || (esReinscripcion && activo);
        pdf.Enabled=completo;
        hide.Enabled=completo || (esReinscripcion && activo);
        hide.Text=esReinscripcion?"Dar de baja":"Ocultar registro";
        restore.Enabled=completo && actor?.IsAdmin==true;
        horario.Enabled=seleccionado && (!esReinscripcion || activo);
        view.Enabled=seleccionado;
        view.Text=esReinscripcion?"Ver datos":"Ver expediente";
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
        try
        {
            if(EsReinscripcionSeleccionada())
            {
                using var f=new ModificarReinscripcionForm(actor,SelectedControl());
                if(f.ShowDialog(this)==DialogResult.OK)LoadData();
            }
            else
            {
                using var f=new ExpedienteMForm(actor,SelectedStudent());
                f.ShowDialog(this);
                LoadData();
            }
        }
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

    private void DarDeBajaReinscripcion()
    {
        try
        {
            string controlAlumno=SelectedControl();
            var alumno=RepositorioMantenimientoReinscripcion.Obtener(actor,controlAlumno);
            if(!alumno.Estatus.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("El alumno ya está dado de baja.");

            using var ventana=new Form
            {
                Text="AMBAR | Dar de baja alumno reinscrito",
                StartPosition=FormStartPosition.CenterParent,
                ClientSize=new Size(520,315),MinimumSize=new Size(490,315),
                FormBorderStyle=FormBorderStyle.FixedDialog,
                MaximizeBox=false,MinimizeBox=false,BackColor=Color.White,
                Font=new Font("Segoe UI",10F)
            };
            var titulo=new Label
            {
                Text=$"DAR DE BAJA\n{alumno.Nombre} {alumno.Paterno} {alumno.Materno}\nControl: {controlAlumno}",
                Dock=DockStyle.Top,Height=100,ForeColor=Theme.Blue,
                Font=new Font("Segoe UI",11F,FontStyle.Bold),
                Padding=new Padding(16,12,12,0)
            };
            var motivo=new TextBox
            {
                Multiline=true,MaxLength=250,Dock=DockStyle.Fill,
                ScrollBars=ScrollBars.Vertical,Font=new Font("Segoe UI",11F),
                Margin=new Padding(15,5,15,5)
            };
            var panelMotivo=new Panel{Dock=DockStyle.Fill,Padding=new Padding(15,5,15,8)};
            panelMotivo.Controls.Add(motivo);
            var etiqueta=new Label
            {
                Text="Motivo de la baja (obligatorio):",
                Dock=DockStyle.Top,Height=34,Padding=new Padding(16,5,0,0)
            };
            var acciones=new FlowLayoutPanel
            {
                Dock=DockStyle.Bottom,Height=64,
                FlowDirection=FlowDirection.RightToLeft,
                Padding=new Padding(12,9,12,9),WrapContents=false
            };
            var cancelar=new Button
            {
                Text="Cancelar",Width=125,Height=40,
                DialogResult=DialogResult.Cancel
            };
            var confirmar=new Button
            {
                Text="Confirmar baja",Width=165,Height=40,
                BackColor=Theme.Blue,ForeColor=Color.White,
                FlatStyle=FlatStyle.Flat
            };
            confirmar.FlatAppearance.BorderSize=0;
            cancelar.Click+=(_,_)=>ventana.Close();
            confirmar.Click+=(_,_)=>
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(motivo.Text))
                    {
                        MessageBox.Show(ventana,"Escribe el motivo de la baja.","AMBAR",
                            MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    if(MessageBox.Show(ventana,
                        "¿Confirmar la baja del alumno? Su registro se conservará en MySQL y se liberará su lugar.",
                        "Confirmación de baja",MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning)!=DialogResult.Yes)return;

                    RepositorioMantenimientoReinscripcion.Baja(actor,controlAlumno,motivo.Text);
                    ventana.DialogResult=DialogResult.OK;
                    ventana.Close();
                }
                catch(Exception ex){Problems.Show(ex);}
            };
            acciones.Controls.Add(cancelar);
            acciones.Controls.Add(confirmar);
            ventana.Controls.Add(panelMotivo);
            ventana.Controls.Add(etiqueta);
            ventana.Controls.Add(titulo);
            ventana.Controls.Add(acciones);
            ventana.CancelButton=cancelar;
            if(ventana.ShowDialog(this)==DialogResult.OK)
            {
                MessageBox.Show(this,"La baja quedó registrada en MySQL y en la Bitácora.",
                    "AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Information);
                LoadData();
            }
        }
        catch(Exception ex){Problems.Show(ex);}
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
                string nombreAlumno=Convert.ToString(grid.CurrentRow?.Cells["Nombre completo"].Value)??"";
                using var form=new HorarioGrupoForm(actor,grupo.GrupoId,grupo.Nombre,
                    SelectedControl(),nombreAlumno);
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

