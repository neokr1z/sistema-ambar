using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
namespace AMBAR.Semana4;
internal static class ListaGrupoPdf
{
    public static void Create(ListaGrupo lista,string path)
    {
        Document.Create(doc=>doc.Page(page=>
        {
            page.Size(PageSizes.A4);page.Margin(30);page.DefaultTextStyle(x=>x.FontFamily(OperatingSystem.IsWindows()?"Arial":"DejaVu Sans").FontSize(9));
            page.Header().Column(c=>
            {
                var logo=Path.Combine(AppContext.BaseDirectory,"Assets","membrete.png");
                if(File.Exists(logo))c.Item().Height(48).Image(logo).FitArea();
                c.Item().Text("LISTA DE GRUPO").FontSize(20).Bold().FontColor("#063763");
                c.Item().PaddingTop(6).Text($"Grupo: {lista.Grupo} · Periodo: {lista.Periodo}");
                c.Item().Text($"Alumnos inscritos: {lista.Alumnos.Rows.Count}");
            });
            page.Content().PaddingVertical(14).Column(c=>
            {
                if(lista.Alumnos.Rows.Count==0)c.Item().Text("Sin alumnos inscritos en este grupo.");
                c.Item().Table(t=>
                {
                    t.ColumnsDefinition(cols=>{cols.ConstantColumn(30);cols.RelativeColumn(2);cols.RelativeColumn(5);});
                    t.Header(h=>{foreach(string label in new[]{"#","No. control","Alumno"})h.Cell().Background("#063763").Padding(8).Text(label).FontColor("#FFFFFF").Bold();});
                    int n=0;
                    foreach(DataRow r in lista.Alumnos.Rows)
                    {
                        n++;
                        foreach(string value in new[]{n.ToString(),Convert.ToString(r["Control"])!,Convert.ToString(r["Alumno"])!})
                            t.Cell().Background(n%2==0?"#F0F6FA":"#FFFFFF").BorderBottom(0.5f).BorderColor("#D6E2EA").Padding(5).Text(value);
                    }
                });
            });
            page.Footer().AlignRight().Text(t=>{t.Span($"{DateTime.Now:dd/MM/yyyy HH:mm} · Página ");t.CurrentPageNumber();t.Span(" de ");t.TotalPages();});
        })).GeneratePdf(path);
    }
}
