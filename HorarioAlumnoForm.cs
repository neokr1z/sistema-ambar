using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Size = System.Drawing.Size;
using Color = System.Drawing.Color;

namespace AMBAR.Semana4;

internal sealed record ClaseHorario(string Dia, string Inicio, string Fin, string Materia, string Aula, string Profesor);

internal sealed class HorarioAlumnoForm : Form
{
    private readonly StudentData alumno;
    private readonly List<ClaseHorario> clases = new();
    private readonly DataGridView tabla = new();

    internal HorarioAlumnoForm(StudentData alumno)
    {
        this.alumno = alumno;
        Text = "AMBAR | Horario del alumno";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1060, 650);
        MinimumSize = new Size(850, 550);
        BackColor = Color.White;

        var titulo = new Label {
            Dock=DockStyle.Top, Height=90, Padding=new Padding(24,12,16,0),
            Font=new Font("Segoe UI",15,FontStyle.Bold),
            ForeColor=Theme.Blue,
            Text=$"HORARIO DEL ALUMNO\n{alumno.FullName} · {alumno.Control} · {alumno.Group} · {alumno.Period}"
        };
        var acciones = new Panel { Dock=DockStyle.Bottom, Height=70 };
        var descargar = new Button {
            Text="Descargar horario PDF", Width=245, Height=42,
            Location=new Point(20,12), BackColor=Theme.Blue,
            ForeColor=Color.White, FlatStyle=FlatStyle.Flat,
            Font=new Font("Segoe UI",10,FontStyle.Bold)
        };
        descargar.FlatAppearance.BorderSize=0;
        descargar.Click+=Descargar;
        acciones.Controls.Add(descargar);

        tabla.Dock=DockStyle.Fill;
        tabla.BackgroundColor=Color.White;
        tabla.ReadOnly=true;
        tabla.AllowUserToAddRows=false;
        tabla.AllowUserToDeleteRows=false;
        tabla.RowHeadersVisible=false;
        tabla.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        tabla.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
        tabla.Columns.Add("dia","Día");
        tabla.Columns.Add("inicio","Inicio");
        tabla.Columns.Add("fin","Fin");
        tabla.Columns.Add("materia","Materia");
        tabla.Columns.Add("aula","Aula");
        tabla.Columns.Add("profesor","Docente");
        Controls.Add(tabla);
        Controls.Add(acciones);
        Controls.Add(titulo);
        Load+=Cargar;
    }

    private void Cargar(object? sender,EventArgs e)
    {
        try
        {
            using var c=Db.Connect();
            using var cmd=Db.Sql(c, """
                SELECT h.dia, TIME_FORMAT(h.hora_inicio,'%H:%i'),
                       TIME_FORMAT(h.hora_fin,'%H:%i'),h.asignatura,
                       COALESCE(h.aula,''),COALESCE(h.profesor,'')
                FROM Horario_Referencia h
                JOIN Grupo g ON g.ID_grupo=h.ID_grupo
                JOIN Inscripcion i ON i.ID_grupo=g.ID_grupo
                JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
                WHERE i.Numero_control=@control AND UPPER(i.estatus)='INSCRITO'
                  AND pe.nombre_periodo=@periodo
                  AND g.nombre_grupo=@grupo
                ORDER BY FIELD(h.dia,'Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'),
                         h.hora_inicio,h.asignatura
                """);
            Db.Param(cmd,"@control",alumno.Control);
            Db.Param(cmd,"@periodo",alumno.Period);
            Db.Param(cmd,"@grupo",alumno.Group);
            using var reader=cmd.ExecuteReader();
            while(reader.Read())
            {
                var item=new ClaseHorario(reader.GetString(0),reader.GetString(1),
                    reader.GetString(2),reader.GetString(3),reader.GetString(4),reader.GetString(5));
                clases.Add(item);
                tabla.Rows.Add(item.Dia,item.Inicio,item.Fin,item.Materia,item.Aula,item.Profesor);
            }
            if(clases.Count==0)
                MessageBox.Show("No hay horario registrado para el grupo y periodo de esta inscripción.","AMBAR");
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private void Descargar(object? sender,EventArgs e)
    {
        if(clases.Count==0){MessageBox.Show("No hay horario que exportar.");return;}
        using var dialog=new SaveFileDialog {
            Filter="Documento PDF (*.pdf)|*.pdf",
            FileName=$"AMBAR_Horario_{alumno.Control}.pdf"
        };
        if(dialog.ShowDialog(this)!=DialogResult.OK)return;
        try
        {
            var logo=Path.Combine(AppContext.BaseDirectory,"Assets","membrete.png");
            Document.Create(doc=>doc.Page(page=>{
                page.Size(PageSizes.A4.Landscape());
                page.Margin(26);
                page.DefaultTextStyle(x=>x.FontFamily("Arial").FontSize(9));
                page.Header().Column(c=>{
                    if(File.Exists(logo))c.Item().Height(45).Image(logo).FitArea();
                    c.Item().AlignCenter().Text("HORARIO DEL ALUMNO").Bold().FontSize(16);
                    c.Item().Text($"Alumno: {alumno.FullName}    Control: {alumno.Control}");
                    c.Item().Text($"Grupo: {alumno.Group}    Periodo: {alumno.Period}");
                });
                page.Content().PaddingTop(14).Table(t=>{
                    t.ColumnsDefinition(cols=>{
                        cols.ConstantColumn(68); cols.ConstantColumn(68);
                        cols.ConstantColumn(68);cols.RelativeColumn(3);
                        cols.ConstantColumn(70);cols.RelativeColumn(2);
                    });
                    t.Header(h=>{
                        foreach(var caption in new[]{"Día","Inicio","Fin","Materia","Aula","Docente"})
                            h.Cell().Background("#063763").Padding(5).Text(caption).FontColor(Colors.White).Bold();
                    });
                    foreach(var x in clases)
                        foreach(var value in new[]{x.Dia,x.Inicio,x.Fin,x.Materia,x.Aula,x.Profesor})
                            t.Cell().BorderBottom(0.5f).BorderColor("#D0D8E0").Padding(5).Text(value);
                });
                page.Footer().AlignRight().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
            })).GeneratePdf(dialog.FileName);
            MessageBox.Show("Horario PDF guardado correctamente.","AMBAR");
        }
        catch(Exception ex){Problems.Show(ex);}
    }
}
