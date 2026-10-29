using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace AMBAR.Semana4;
internal static class ReporteDocentePdf
{
    public static void Create(string title,DataTable data,string file)
    {
        Document.Create(document=>document.Page(page=>
        {
            page.Size(PageSizes.A4.Landscape());page.Margin(30);page.DefaultTextStyle(x=>x.FontSize(9));
            page.Header().Column(c=>
            {
                c.Item().Text("AMBAR · Historial de calificaciones").FontSize(18).Bold().FontColor("#063763");
                c.Item().PaddingVertical(8).Text(title);
            });
            page.Content().PaddingVertical(12).Table(table=>
            {
                table.ColumnsDefinition(columns=>{columns.RelativeColumn();columns.RelativeColumn(3);columns.RelativeColumn();columns.RelativeColumn();columns.RelativeColumn(2);});
                table.Header(header=>{foreach(var label in new[]{"Control","Alumno","Unidad","Calificación","Actualización"})header.Cell().Background("#063763").Padding(6).Text(label).FontColor("#FFFFFF").Bold();});
                foreach(DataRow row in data.Rows)
                    foreach(var key in new[]{"Control","Alumno","Unidad","Calificación","Actualización"})
                        table.Cell().BorderBottom(0.5f).BorderColor("#D6E2EA").Padding(6).Text(row[key] is DateTime date?date.ToString("dd/MM/yyyy HH:mm"):Convert.ToString(row[key]));
            });
            page.Footer().AlignRight().Text(t=>{t.Span($"Generado {DateTime.Now:dd/MM/yyyy HH:mm} · Página ");t.CurrentPageNumber();});
        })).GeneratePdf(file);
    }
}
