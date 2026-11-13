namespace AMBAR.Semana4;

// Catálogo local de dominios de correo. No consulta DNS ni utiliza Internet.
// El catálogo verifica pertenencia a dominios conocidos, no la existencia del buzón.
internal sealed class CorreoDominio
{
    internal static CorreoDominio Current { get; set; } = new();
    public Task RequireAsync(string email, string field, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        string normalized = Contacto.Email(email, field);
        string domain = normalized.Split('@')[1];
        using var connection = Db.Connect();
        using var command = Db.Sql(connection, """
            SELECT COUNT(*) FROM Dominio_Correo
            WHERE dominio=@dominio AND activo=1
            """);
        Db.Param(command, "@dominio", domain);
        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
            throw new ArgumentException($"{field}: el dominio @{domain} no está registrado en el catálogo local de AMBAR. Solicita al administrador incorporarlo si es legítimo.");
        return Task.CompletedTask;
    }
}
