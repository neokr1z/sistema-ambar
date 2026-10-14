using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AMBAR.Semana4;

internal static class PdfService
{
    private const string Navy="#1B396A";
    public static void Create(StudentData d,string file)
    {
        var logo=Path.Combine(AppContext.BaseDirectory,"Assets","membrete.png");
        Document.Create(document=>
        {
            document.Page(page=>
            {
                page.Size(PageSizes.A4);
                page.Margin(36);
                page.DefaultTextStyle(t=>t.FontFamily("Arial").FontSize(10).FontColor("#243247"));
                page.Header().Column(header=>
                {
                    if(File.Exists(logo))
                        header.Item().Height(58).Image(logo).FitArea();
                    header.Item().PaddingTop(6).AlignCenter().Text("TECNOLÓGICO NACIONAL DE MÉXICO")
                        .FontSize(11).SemiBold().FontColor(Navy);
                    header.Item().AlignCenter().Text("INSTITUTO TECNOLÓGICO DE ACAPULCO")
                        .FontSize(10).FontColor(Navy);
                    header.Item().PaddingTop(8).LineHorizontal(2).LineColor(Navy);
                });
                page.Content().PaddingVertical(20).Column(content=>
                {
                    content.Spacing(13);
                    content.Item().AlignCenter().Text("CONSTANCIA DE REGISTRO E INSCRIPCIÓN")
                        .FontSize(14).Bold().FontColor(Navy);
                    content.Item().AlignCenter().Text("Sistema AMBAR · Expediente escolar")
                        .FontSize(10).FontColor("#807E82");
                    Section(content,"DATOS DEL ALUMNO",new (string,string)[]{
                        ("Número de control",d.Control),("Nombre completo",d.FullName),
                        ("CURP",d.Curp),("Fecha de nacimiento",d.Birth.ToString("dd/MM/yyyy")),
                        ("Estatus",d.Status)
                    });
                    Section(content,"DATOS DE INSCRIPCIÓN",new (string,string)[]{
                        ("Ficha de aspirante",d.ApplicantNumber),("Carrera",d.Career),
                        ("Plan de estudios",d.Plan),("Grupo asignado",d.Group),
                        ("Periodo escolar",d.Period),("Fecha de inscripción",d.EnrollmentDate?.ToString("dd/MM/yyyy")??"—")
                    });
                    Section(content,"DATOS DE CONTACTO",new (string,string)[]{
                        ("Correo institucional",d.Email),("Correo personal",d.PersonalEmail),
                        ("Teléfono",d.Phone),("Teléfono de emergencia",d.Emergency)
                    });
                    content.Item().PaddingTop(16).Text("Domicilio: "+
                        $"{d.Street} {d.Exterior} {d.Interior}, Col. {d.Neighborhood}, " +
                        $"{d.Locality}, {d.Municipality}, {d.State}, C.P. {d.Postal}").FontSize(9);
                    content.Item().PaddingTop(15).Text("Documento generado electrónicamente a partir de los datos registrados en AMBAR. " +
                        "Este comprobante no sustituye los documentos oficiales expedidos por Control Escolar.")
                        .Italic().FontSize(8).FontColor("#807E82");
                });
                page.Footer().Column(footer=>
                {
                    footer.Item().LineHorizontal(1).LineColor(Navy);
                    footer.Item().PaddingTop(6).Row(row=>
                    {
                        row.RelativeItem().Text($"Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
                        row.RelativeItem().AlignRight().Text(t=>
                        {
                            t.Span("Página ");t.CurrentPageNumber();t.Span(" de ");t.TotalPages();
                        });
                    });
                });
            });
        }).GeneratePdf(file);
    }

    private static void Section(ColumnDescriptor content,string title, (string Label,string Value)[] items)
    {
        content.Item().Background("#E6ECF5").Padding(7).Text(title).Bold().FontSize(10).FontColor(Navy);
        foreach(var item in items)
        {
            content.Item().Row(row=>
            {
                row.ConstantItem(150).Text(item.Label).SemiBold().FontSize(9);
                row.RelativeItem().Text(string.IsNullOrWhiteSpace(item.Value)?"—":item.Value).FontSize(9);
            });
        }
    }
}
