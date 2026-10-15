using System.Data;

namespace AMBAR.Semana4;

public partial class StudentsForm : Form
{
    private Session actor = null!;

    public StudentsForm()
    {
        InitializeComponent();
    }

    internal StudentsForm(Session session) : this()
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
            DataTable data = StudentRepository.ListStudents(actor, filter.Text);
            grid.DataSource = data;
            count.Text = $"Registros: {data.Rows.Count}";
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private StudentData SelectedStudent()
        => StudentRepository.Detail(actor, SelectedControl())
            ?? throw new ArgumentException("No se pudo acceder al expediente.");

    private void View()
    {
        try { using var f = new DetailForm(actor, SelectedStudent()); f.ShowDialog(this); LoadData(); }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void Edit()
    {
        try { using var f = new EditStudentForm(actor, SelectedStudent()); f.ShowDialog(this); LoadData(); }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void Pdf()
    {
        try
        {
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
            var d = SelectedStudent();
            if (restore && d.Status.ToUpperInvariant() != "OCULTO")
                throw new ArgumentException("Sólo se restauran registros ocultos.");
            if (!restore && d.Status.ToUpperInvariant() == "OCULTO")
                throw new ArgumentException("El registro ya está oculto.");
            string message = restore ? "¿Restaurar este registro para Secretaría?" :
                "¿Ocultar este registro de Secretaría? Los datos NO se eliminan de MySQL y permanecen disponibles para Administración.";
            if (MessageBox.Show(message, "Confirmar operación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            StudentRepository.Visibility(actor, d.Control, restore);
            LoadData();
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}


