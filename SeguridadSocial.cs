namespace AMBAR.Semana4;

internal static class SeguridadSocial
{
    // Validación local del formato y dígito de control; no acredita afiliación al IMSS.
    public static string Validate(string value)
    {
        string nss = (value ?? "").Trim();
        if (nss.Length == 0) return "";
        if (nss.Length != 11 || nss.Any(c => c < '0' || c > '9') || nss.Distinct().Count() == 1)
            throw new ArgumentException("El NSS debe contener exactamente 11 dígitos. Si no lo tienes, deja el campo vacío y tramítalo en el IMSS.");
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            int product = (nss[i] - '0') * (i % 2 == 0 ? 1 : 2);
            sum += product / 10 + product % 10;
        }
        if ((10 - sum % 10) % 10 != nss[10] - '0')
            throw new ArgumentException("El dígito verificador del NSS no coincide. Revisa tu constancia del IMSS.");
        return nss;
    }
    public static string Display(string value) => string.IsNullOrWhiteSpace(value) ? "Pendiente de asignación por el IMSS" : value;
}
