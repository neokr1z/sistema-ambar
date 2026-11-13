using System.Data;
using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace AMBAR.Semana4;

public partial class HorarioAlumnoForm : Form
{
    private StudentData alumno = null!;
    private readonly List<BloqueHorario> clases = new();

    private static readonly string[] DiasOrden =
    {
        "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"
    };

    public HorarioAlumnoForm(){InitializeComponent();}
    internal HorarioAlumnoForm(StudentData alumno):this()
    {
        this.alumno=alumno;
        summary.Text=$"{alumno.FullName} · {alumno.Control}\n{alumno.Group} · {alumno.Period}";
    }
    private void CloseClick(object? sender,EventArgs e)=>Close();
    private void CargarHorario(object? sender, EventArgs e)
    {
        if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(alumno is null){Close();return;}
        try
        {
            clases.Clear();
            tabla.Rows.Clear();

            using var c = Db.Connect();
            using var cmd = Db.Sql(c, """
                SELECT
                    h.dia,
                    TIME_FORMAT(h.hora_inicio,'%H:%i'),
                    TIME_FORMAT(h.hora_fin,'%H:%i'),
                    h.asignatura,
                    COALESCE(h.aula,''),
                    COALESCE(h.profesor,'')
                FROM Horario_Referencia h
                JOIN Grupo g ON g.ID_grupo = h.ID_grupo
                JOIN Inscripcion i ON i.ID_grupo = g.ID_grupo
                JOIN Periodo_Escolar pe ON pe.ID_Periodo = g.ID_Periodo
                WHERE i.Numero_control = @control
                  AND UPPER(i.estatus) = 'INSCRITO'
                  AND pe.nombre_periodo = @periodo
                  AND g.nombre_grupo = @grupo
                ORDER BY
                    FIELD(h.dia,'Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'),
                    h.hora_inicio,
                    h.asignatura
                """);

            Db.Param(cmd, "@control", alumno.Control);
            Db.Param(cmd, "@periodo", alumno.Period);
            Db.Param(cmd, "@grupo", alumno.Group);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                clases.Add(new BloqueHorario(
                    rd.GetString(0),
                    rd.GetString(1),
                    rd.GetString(2),
                    rd.GetString(3),
                    rd.GetString(4),
                    rd.GetString(5)
                ));
            }

            if (clases.Count == 0)
            {
                MessageBox.Show(
                    "No hay horario registrado para el grupo y periodo de esta inscripción.",
                    "AMBAR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            LlenarTablaHorario();
        }
        catch (Exception ex)
        {
            Problems.Show(ex);
        }
    }

    private void LlenarTablaHorario()
    {
        var horas = clases
            .Select(x => $"{x.HoraInicio}-{x.HoraFin}")
            .Distinct()
            .OrderBy(x => TimeOnly.ParseExact(x.Split('-')[0], "HH:mm", CultureInfo.InvariantCulture))
            .ToList();

        foreach (var hora in horas)
        {
            var partes = hora.Split('-');
            var inicio = partes[0];
            var fin = partes[1];

            int rowIndex = tabla.Rows.Add();
            var row = tabla.Rows[rowIndex];
            row.Height = 72;
            row.Cells["Hora"].Value = $"{inicio}\n{fin}";

            foreach (var dia in DiasOrden)
            {
                var bloque = clases.FirstOrDefault(x =>
                    x.Dia.Equals(dia, StringComparison.OrdinalIgnoreCase) &&
                    x.HoraInicio == inicio &&
                    x.HoraFin == fin);

                if (bloque is null)
                {
                    row.Cells[dia].Value = "";
                    continue;
                }

                string texto = bloque.Materia;

                if (!string.IsNullOrWhiteSpace(bloque.Aula))
                    texto += "\nAula: " + bloque.Aula;

                if (!string.IsNullOrWhiteSpace(bloque.Profesor))
                    texto += "\n" + bloque.Profesor;

                row.Cells[dia].Value = texto;
            }
        }
    }

    private void DescargarPdf(object? sender, EventArgs e)
    {
        if (clases.Count == 0)
        {
            MessageBox.Show(
                "No hay horario que exportar.",
                "AMBAR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Documento PDF (*.pdf)|*.pdf",
            FileName = $"AMBAR_Horario_{alumno.Control}.pdf"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            var logo = Path.Combine(AppContext.BaseDirectory, "Assets", "membrete.png");

            var horas = clases
                .Select(x => $"{x.HoraInicio}-{x.HoraFin}")
                .Distinct()
                .OrderBy(x => TimeOnly.ParseExact(x.Split('-')[0], "HH:mm", CultureInfo.InvariantCulture))
                .ToList();

            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(22);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(9));

                    page.Header().Column(col =>
                    {
                        if (File.Exists(logo))
                            col.Item().Height(48).Image(logo).FitArea();

                        col.Item().AlignCenter().Text("HORARIO DEL ALUMNO")
                            .Bold().FontSize(16).FontColor("#063763");

                        col.Item().PaddingTop(4).Text($"Alumno: {alumno.FullName}");
                        col.Item().Text($"Número de control: {alumno.Control}");
                        col.Item().Text($"Grupo: {alumno.Group}    Periodo: {alumno.Period}");
                    });

                    page.Content().PaddingTop(14).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(70); // Hora
                            foreach (var _ in DiasOrden)
                                cols.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background("#063763").Padding(6)
                                .Text("Hora").FontColor(Colors.White).Bold().AlignCenter();

                            foreach (var dia in DiasOrden)
                            {
                                header.Cell().Background("#063763").Padding(6)
                                    .Text(dia).FontColor(Colors.White).Bold().AlignCenter();
                            }
                        });

                        foreach (var hora in horas)
                        {
                            var partes = hora.Split('-');
                            var inicio = partes[0];
                            var fin = partes[1];

                            table.Cell().Border(0.5f).BorderColor("#D6DEE8")
                                .Padding(6).AlignMiddle().AlignCenter()
                                .Text($"{inicio}\n{fin}");

                            foreach (var dia in DiasOrden)
                            {
                                var bloque = clases.FirstOrDefault(x =>
                                    x.Dia.Equals(dia, StringComparison.OrdinalIgnoreCase) &&
                                    x.HoraInicio == inicio &&
                                    x.HoraFin == fin);

                                string valor = "";

                                if (bloque is not null)
                                {
                                    valor = bloque.Materia;

                                    if (!string.IsNullOrWhiteSpace(bloque.Aula))
                                        valor += "\nAula: " + bloque.Aula;

                                    if (!string.IsNullOrWhiteSpace(bloque.Profesor))
                                        valor += "\n" + bloque.Profesor;
                                }

                                table.Cell().Border(0.5f).BorderColor("#D6DEE8")
                                    .MinHeight(54)
                                    .Padding(5)
                                    .AlignMiddle()
                                    .AlignCenter()
                                    .Text(valor);
                            }
                        }
                    });

                    page.Footer().AlignRight().Text(
                        $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}"
                    ).FontSize(8);
                });
            }).GeneratePdf(dialog.FileName);

            MessageBox.Show(
                "Horario PDF guardado correctamente.",
                "AMBAR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            Problems.Show(ex);
        }
    }
}