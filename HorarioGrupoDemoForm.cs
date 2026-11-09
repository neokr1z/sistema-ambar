using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AMBAR.Semana4;

// Horario horizontal: una fila por bloque de hora, días como columnas.
internal sealed class HorarioGrupoDemoForm : Form
{
    private static readonly string[] Dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };
    private readonly List<(string Materia,string Dia,string Inicio,string Fin,string Aula,string Docente)> clases = new();
    private readonly string? controlAlumno;
    private readonly string? nombreAlumno;
    private readonly string etiquetaGrupo;

    internal HorarioGrupoDemoForm(Sesion actor, int grupo, string etiqueta, string? control = null, string? alumno = null)
    {
        if (!actor.CanSeeStudents)
            throw new UnauthorizedAccessException("Sin permiso.");

        controlAlumno = control;
        nombreAlumno = alumno;
        etiquetaGrupo = etiqueta;
        Text = "AMBAR | Horario del grupo";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1120, 650);
        MinimumSize = new Size(850, 490);
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);

        var titulo = new Label
        {
            Text = "HORARIO | " + etiqueta,
            Dock = DockStyle.Top,
            Height = 76,
            Padding = new Padding(20, 17, 12, 0),
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = Theme.Blue
        };

        var tabla = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AllowUserToOrderColumns = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight = 44,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        };
        tabla.ColumnHeadersDefaultCellStyle.BackColor = Theme.Blue;
        tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        tabla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        tabla.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        tabla.DefaultCellStyle.Padding = new Padding(7, 10, 7, 10);
        tabla.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        tabla.DefaultCellStyle.SelectionBackColor = Theme.Sky;
        tabla.DefaultCellStyle.SelectionForeColor = Theme.Blue;
        tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 247, 251);

        tabla.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Hora", HeaderText = "Hora", FillWeight = 16,
            MinimumWidth = 105, SortMode = DataGridViewColumnSortMode.NotSortable
        });
        foreach (string dia in Dias)
            tabla.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = dia, HeaderText = dia, FillWeight = 24,
                MinimumWidth = 135, SortMode = DataGridViewColumnSortMode.NotSortable
            });
        tabla.Columns["Hora"]!.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

        var cerrar = new Button
        {
            Text = "Cerrar",
            Dock = DockStyle.Right,
            Width = 160,
            Height = 48,
            BackColor = Theme.Blue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        cerrar.FlatAppearance.BorderSize = 0;
        cerrar.Click += (_, _) => Close();

        var acciones = new Panel { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White };
        var descargar = new Button {
            Text = "Descargar PDF", Dock = DockStyle.Left, Width = 195,
            BackColor = Theme.Blue, ForeColor = Color.White,
            Font = new Font("Segoe UI",10F,FontStyle.Bold),
            FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
        };
        descargar.FlatAppearance.BorderSize = 0;
        descargar.Click += (_,_) => DescargarPdf();
        acciones.Controls.Add(descargar);
        acciones.Controls.Add(cerrar);

        Controls.Add(tabla);
        Controls.Add(acciones);
        Controls.Add(titulo);

        Shown += (_, _) => Cargar(actor, grupo, tabla);
    }

    private void Cargar(Sesion actor, int grupo, DataGridView tabla)
    {
        try
        {
            using var c = Db.Connect();
            Identidad.RequireActive(c, null, actor);
            using var cmd = Db.Sql(c, """
                SELECT h.asignatura, h.dia,
                    TIME_FORMAT(h.hora_inicio,'%H:%i') AS Inicio,
                    TIME_FORMAT(h.hora_fin,'%H:%i') AS Fin,
                    COALESCE(h.aula,''), COALESCE(h.profesor,'')
                FROM Horario_Referencia h
                WHERE h.ID_grupo=@grupo
                ORDER BY h.hora_inicio, h.hora_fin, h.dia, h.asignatura
                """);
            Db.Param(cmd, "@grupo", grupo);

            clases.Clear();
            using (var rd = cmd.ExecuteReader())
                while (rd.Read())
                    clases.Add((
                        rd.GetString(0), rd.GetString(1), rd.GetString(2),
                        rd.GetString(3), rd.GetString(4), rd.GetString(5)));

            tabla.Rows.Clear();
            foreach (var bloque in clases
                .Select(x => (x.Inicio, x.Fin))
                .Distinct()
                .OrderBy(x => x.Inicio)
                .ThenBy(x => x.Fin))
            {
                int fila = tabla.Rows.Add();
                tabla.Rows[fila].Cells["Hora"].Value = bloque.Inicio + "\n" + bloque.Fin;

                foreach (string dia in Dias)
                {
                    var materias = clases.Where(x =>
                        x.Inicio == bloque.Inicio && x.Fin == bloque.Fin &&
                        NormalizarDia(x.Dia) == dia);

                    var textos = materias.Select(x =>
                        x.Materia +
                        (string.IsNullOrWhiteSpace(x.Aula) ? "" : "\nAula: " + x.Aula) +
                        (string.IsNullOrWhiteSpace(x.Docente) ? "" : "\n" + x.Docente));

                    tabla.Rows[fila].Cells[dia].Value = string.Join("\n\n", textos);
                }
            }

            tabla.ClearSelection();
            if (clases.Count == 0)
                MessageBox.Show(this,
                    "Este grupo aún no tiene un horario cargado.",
                    "AMBAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { Problems.Show(ex); }
    }

    private static string NormalizarDia(string dia)
        => dia.Trim().Equals("Miercoles", StringComparison.OrdinalIgnoreCase)
            ? "Miércoles"
            : Dias.FirstOrDefault(d => d.Equals(dia.Trim(), StringComparison.OrdinalIgnoreCase)) ?? "";
}
