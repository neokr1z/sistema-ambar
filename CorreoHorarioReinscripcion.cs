using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace AMBAR.Semana4;

// Genera un PDF desde el horario real guardado en MySQL y lo envía con SMTP configurado.
// Credenciales únicamente en variables de entorno del equipo, nunca en GitHub.
internal static class CorreoHorarioReinscripcion
{
    internal static void Enviar(Sesion actor,string control,string alumno,string destinatario,int grupo)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        control=RepositorioReinscripcionDemo.Inferir(control) switch { var _ => control.Trim() };
        destinatario=Contacto.Email(destinatario,"correo del alumno");
        CorreoDominio.Current.RequireAsync(destinatario,"correo del alumno").GetAwaiter().GetResult();

        string host=Environment.GetEnvironmentVariable("AMBAR_SMTP_HOST")??"";
        string portString=Environment.GetEnvironmentVariable("AMBAR_SMTP_PORT")??"587";
        string usuario=Environment.GetEnvironmentVariable("AMBAR_SMTP_USER")??"";
        string clave=Environment.GetEnvironmentVariable("AMBAR_SMTP_PASSWORD")??"";
        if(string.IsNullOrWhiteSpace(host)||string.IsNullOrWhiteSpace(usuario)||string.IsNullOrWhiteSpace(clave)
           ||!int.TryParse(portString,out int port)||port is <1 or >65535)
            throw new InvalidOperationException("Configura AMBAR_SMTP_HOST, AMBAR_SMTP_PORT, AMBAR_SMTP_USER y AMBAR_SMTP_PASSWORD en Windows para enviar correos.");

        using var c=Db.Connect();
        Identidad.RequireActive(c,null,actor);
        // No permitir envío de horarios a números que aún no terminaron su reinscripción.
        using(var verifica=Db.Sql(c,"SELECT COUNT(*) FROM Reinscripcion_Demostracion WHERE numero_control=@control AND ID_grupo=@grupo AND UPPER(estatus)='INSCRITO'"))
        {
            Db.Param(verifica,"@control",control);Db.Param(verifica,"@grupo",grupo);
            if(Convert.ToInt32(verifica.ExecuteScalar())!=1)
                throw new ArgumentException("Primero guarda la reinscripción para poder enviar el horario.");
        }

        string nombreGrupo,periodo;
        using(var info=Db.Sql(c,"""
            SELECT g.nombre_grupo,p.nombre_periodo
            FROM Grupo g JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE g.ID_grupo=@grupo
            """))
        {
            Db.Param(info,"@grupo",grupo);
            using var reader=info.ExecuteReader();
            if(!reader.Read())throw new ArgumentException("El grupo seleccionado no existe.");
            nombreGrupo=reader.GetString(0);periodo=reader.GetString(1);
        }
        var data=new DataTable();
        using(var cmd=Db.Sql(c,"""
            SELECT h.dia,h.hora_inicio,h.hora_fin,h.asignatura,
                   COALESCE(h.aula,''),COALESCE(h.profesor,'')
            FROM Horario_Referencia h WHERE h.ID_grupo=@grupo
            ORDER BY FIELD(h.dia,'Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'),
                     h.hora_inicio,h.asignatura
            """))
        {
            Db.Param(cmd,"@grupo",grupo);
            using var reader=cmd.ExecuteReader();data.Load(reader);
        }
        if(data.Rows.Count==0)
            throw new ArgumentException("No hay horario registrado para el grupo. No se enviará un documento vacío.");

        // Archivo temporal: nunca se guarda una copia del correo ni de las credenciales en el repositorio.
        string path=Path.Combine(Path.GetTempPath(),$"AMBAR_Horario_{Guid.NewGuid():N}.pdf");
        try
        {
            QuestPDF.Fluent.Document.Create(document=>document.Page(page=>
            {
                page.Size(PageSizes.A4.Landscape());page.Margin(26);
                page.DefaultTextStyle(t=>t.FontFamily("Arial").FontSize(9));
                page.Header().Column(h=>
                {
                    h.Item().Text("AMBAR | HORARIO DEL ALUMNO").Bold().FontSize(17).FontColor("#063763");
                    h.Item().Text($"Alumno: {alumno} · No. control: {control}");
                    h.Item().Text($"Grupo: {nombreGrupo} · Periodo: {periodo}");
                });
                page.Content().PaddingTop(16).Table(table=>
                {
                    table.ColumnsDefinition(cols=>
                    {
                        cols.RelativeColumn(1.1f);cols.RelativeColumn(1.1f);
                        cols.RelativeColumn();cols.RelativeColumn();cols.RelativeColumn(3);
                        cols.RelativeColumn();cols.RelativeColumn(2);
                    });
                    table.Header(h=>
                    {
                        foreach(var heading in new[]{"Día","Inicio","Fin","Materia","Aula","Docente",""})
                        {
                            // Se remplaza el último encabezado vacío para mantener la tabla compacta.
                            h.Cell().Background("#063763").Padding(5).Text(heading).FontColor("#FFFFFF").Bold();
                        }
                    });
                    foreach(DataRow row in data.Rows)
                    {
                        string fmt(object value)=>value is TimeSpan ts?ts.ToString(@"hh\:mm",CultureInfo.InvariantCulture):Convert.ToString(value)??"";
                        foreach(string item in new[]{fmt(row[0]),fmt(row[1]),fmt(row[2]),fmt(row[3]),fmt(row[4]),fmt(row[5]),""})
                            table.Cell().BorderBottom(0.5f).BorderColor("#D6DEE8").Padding(4).Text(item);
                    }
                });
                page.Footer().AlignRight().Text("AMBAR · Horario de reinscripción").FontSize(8);
            })).GeneratePdf(path);

            using var message=new MailMessage();
            message.From=new MailAddress(usuario,"AMBAR Control Escolar");
            message.To.Add(destinatario);
            message.Subject=$"AMBAR | Horario de {nombreGrupo} · {periodo}";
            message.Body=$"Hola {alumno}:\n\nAdjuntamos tu horario de grupo {nombreGrupo} del periodo {periodo}.\n\nAMBAR | Control Escolar";
            message.Attachments.Add(new Attachment(path));
            using var smtp=new SmtpClient(host,port){EnableSsl=true,Credentials=new NetworkCredential(usuario,clave),DeliveryMethod=SmtpDeliveryMethod.Network,Timeout=30000};
            smtp.Send(message);
        }
        finally
        {
            // El Attachment mantiene el archivo abierto hasta cerrar el mensaje (using ya terminó).
            try{if(File.Exists(path))File.Delete(path);}catch(IOException){}
        }
    }
}
