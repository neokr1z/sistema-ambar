using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace AMBAR.Semana4;

internal sealed record DnsAnswer(int Type,string Data);
internal sealed record DnsReply(int Status,List<DnsAnswer> Answers);

// Consulta únicamente el dominio a DNS público; no envía la dirección completa ni mensajes.
// Un dominio enrutable NO certifica que exista el buzón o que pertenezca a la persona.
internal sealed class CorreoDominio
{
    internal static CorreoDominio Current { get; set; } = new(new HttpClient { Timeout=Timeout.InfiniteTimeSpan });
    private readonly HttpClient http;
    internal CorreoDominio(HttpClient client) { http=client; }
    public async Task RequireAsync(string email,string field,CancellationToken cancellation=default)
    {
        string normalized=Contacto.Email(email,field);
        string domain=normalized.Split('@')[1];
        using var limit=CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        limit.CancelAfter(TimeSpan.FromSeconds(20));
        try
        {
            if(!await CanReceiveAsync(domain,limit.Token).ConfigureAwait(false))
                throw new ArgumentException($"{field}: el dominio @{domain} no existe o no tiene una ruta de correo válida. Revisa la dirección.");
        }
        catch(Exception ex) when(ex is HttpRequestException or JsonException or OperationCanceledException or InvalidDataException)
        {
            if(cancellation.IsCancellationRequested)throw;
            throw new InvalidOperationException($"No se pudo comprobar el dominio de {field.ToLowerInvariant()}. Revisa tu conexión y vuelve a guardar. No se guardaron los cambios.",ex);
        }
    }
    private async Task<bool> CanReceiveAsync(string domain,CancellationToken token)
    {
        var reply=await QueryAsync(domain,15,token).ConfigureAwait(false);
        if(reply.Status==3)return false;
        var mx=reply.Answers.Where(a=>a.Type==15).ToList();
        if(mx.Count==0)return await HasAddressAsync(domain,token).ConfigureAwait(false); // MX implícito, RFC 5321.
        foreach(var record in mx)
        {
            var parts=record.Data.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length!=2 || !ushort.TryParse(parts[0],out _))throw new InvalidDataException("Respuesta MX inválida.");
            if(parts[1]==".")return false; // Null MX, RFC 7505: no recibe correo.
        }
        foreach(var record in mx.Take(10))
        {
            string host=record.Data.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1].TrimEnd('.');
            if(await HasAddressAsync(host,token).ConfigureAwait(false))return true;
        }
        return false;
    }
    private async Task<bool> HasAddressAsync(string name,CancellationToken token)
    {
        var ipv4=await QueryAsync(name,1,token).ConfigureAwait(false);
        if(ipv4.Status==3)return false;
        if(ipv4.Answers.Any(a=>a.Type==1 && IPAddress.TryParse(a.Data,out _)))return true;
        var ipv6=await QueryAsync(name,28,token).ConfigureAwait(false);
        return ipv6.Status==0 && ipv6.Answers.Any(a=>a.Type==28 && IPAddress.TryParse(a.Data,out _));
    }
    private async Task<DnsReply> QueryAsync(string name,int type,CancellationToken token)
    {
        Exception? last=null;
        foreach(string endpoint in new[]{"https://cloudflare-dns.com/dns-query","https://dns.google/resolve"})
        {
            using var attempt=CancellationTokenSource.CreateLinkedTokenSource(token);
            attempt.CancelAfter(TimeSpan.FromSeconds(4));
            try
            {
                using var request=new HttpRequestMessage(HttpMethod.Get,$"{endpoint}?name={Uri.EscapeDataString(name)}&type={type}");
                request.Headers.Accept.ParseAdd("application/dns-json");
                using var response=await http.SendAsync(request,attempt.Token).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                using var json=JsonDocument.Parse(await response.Content.ReadAsStringAsync(attempt.Token).ConfigureAwait(false));
                var root=json.RootElement;
                if(!root.TryGetProperty("Status",out var statusField) || !statusField.TryGetInt32(out int status))throw new InvalidDataException("Falta estado DNS.");
                if(status!=0 && status!=3)throw new InvalidDataException("DNS no disponible.");
                if(root.TryGetProperty("TC",out var truncated) && truncated.ValueKind==JsonValueKind.True)throw new InvalidDataException("Respuesta DNS truncada.");
                var records=new List<DnsAnswer>();
                if(root.TryGetProperty("Answer",out var answers))
                {
                    if(answers.ValueKind!=JsonValueKind.Array)throw new InvalidDataException("Respuesta DNS inválida.");
                    foreach(var entry in answers.EnumerateArray())
                    {
                        if(!entry.TryGetProperty("type",out var t) || !t.TryGetInt32(out int rt) ||
                           !entry.TryGetProperty("data",out var d) || d.ValueKind!=JsonValueKind.String)throw new InvalidDataException("Registro DNS inválido.");
                        records.Add(new(rt,d.GetString()!));
                    }
                }
                return new(status,records);
            }
            catch(Exception ex) when(ex is HttpRequestException or JsonException or OperationCanceledException or InvalidDataException)
            {
                token.ThrowIfCancellationRequested();last=ex;
            }
        }
        throw new HttpRequestException("No respondió ningún resolvedor DNS.",last);
    }
}
