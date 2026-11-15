namespace AMBAR.Semana4;

// Lista local limitada de dominios conocidos, sin DNS ni conexión a Internet.
// No verifica la existencia del buzón. Para permitir otro dominio, modificar esta lista
// y desplegar una versión nueva del sistema; no se requiere tabla en MySQL.
internal sealed class CorreoDominio
{
    private static readonly HashSet<string> Permitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "gmail.com", "googlemail.com", "outlook.com", "hotmail.com", "hotmail.com.mx",
        "live.com", "live.com.mx", "msn.com", "yahoo.com", "yahoo.com.mx",
        "ymail.com", "icloud.com", "me.com", "mac.com", "aol.com",
        "proton.me", "protonmail.com", "zoho.com", "mail.com", "gmx.com",
        "gmx.es", "acapulco.tecnm.mx", "tecnm.mx"
    };

    internal static CorreoDominio Current { get; set; } = new();

    public Task RequireAsync(string email, string field, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        string normalized = Contacto.Email(email, field);
        string domain = normalized.Split('@')[1];
        if (!Permitidos.Contains(domain))
            throw new ArgumentException($"{field}: el dominio @{domain} no figura en la lista local de AMBAR.");
        return Task.CompletedTask;
    }
}
