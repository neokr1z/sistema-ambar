using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AMBAR.Semana4;

// Lista de calificaciones DEFINITIVAS. Solo se genera tras cerrar semana 17.
internal static class CalificacionesFinalesPdf
{
    public static void Create(string titulo,DataTable alumnos,string destino)
    {
        var columnas=new[]{"Control","Alumno","Semana5","Semana9","Semana13","Semana17","Final"};
        var titulos=new[]{"No. control","Alumno","Sem. 5","Sem. 9","Sem. 13","Sem. 17","Final"};
        Document.Create(document=>document.Page(page=>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(26);
            page.DefaultTextStyle(x=>x.FontSize(9));
            page.Header().Column(c=>
            {
                c.Item().Text("AMBAR | CALIFICACIONES FINALES").FontSize(17).Bold().FontColor("#063763");
                c.Item().PaddingTop(6).Text(titulo);
                c.Item().Text("Corte 17 guardado · Materia cerrada · Resultados definitivos");
            });
            page.Content().PaddingTop(14).Table(table=>
            {
                table.ColumnsDefinition(columns=>
                {
                    columns.RelativeColumn(1.5f);
                    columns.RelativeColumn(3.3f);
                    for(int i=0;i<5;i++)columns.RelativeColumn(1);
                });
                table.Header(header=>
                {
                    foreach(var label in titulos)
                        header.Cell().Background("#063763").Padding(6).Text(label).FontColor("#FFFFFF").Bold();
                });
                int n=0;
                foreach(DataRow fila in alumnos.Rows)
                {
                    n++;
                    for(int i=0;i<columnas.Length;i++)
                    {
                        string value=fila.IsNull(columnas[i])?"PENDIENTE":
                            fila[columnas[i]] is decimal numero?numero.ToString("0.##"):
                            Convert.ToString(fila[columnas[i]])??"";
                        table.Cell().Background(n%2==0?"#EEF5FA":"#FFFFFF")
                            .BorderBottom(0.5f).BorderColor("#D6E2EA")
                            .Padding(6).Text(value);
                    }
                }
            });
            page.Footer().AlignRight().Text(t=>
            {
                t.Span($"AMBAR · Impreso {DateTime.Now:dd/MM/yyyy HH:mm} · Página ");
                t.CurrentPageNumber();
            });
        })).GeneratePdf(destino);
    }
}
