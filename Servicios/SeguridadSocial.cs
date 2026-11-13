namespace AMBAR.Semana4;

internal static class SeguridadSocial
{
    // Generador didáctico. El prefijo 99 es una convención local, no un rango reservado por IMSS.
    public static string GenerarPrueba(DateTime nacimiento, ISet<string> ocupados)
    {
        if(nacimiento.Date < Validation.EarliestBirth || nacimiento.Date >= DateTime.Today)
            throw new ArgumentException("Selecciona una fecha de nacimiento válida antes de generar el NSS.");
        string prefix="99"+DateTime.Today.ToString("yy",System.Globalization.CultureInfo.InvariantCulture)
            +nacimiento.ToString("yy",System.Globalization.CultureInfo.InvariantCulture);
        int start=System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000);
        for(int offset=0;offset<10000;offset++)
        {
            string digits=prefix+((start+offset)%10000).ToString("D4",System.Globalization.CultureInfo.InvariantCulture);
            int sum=0;
            for(int i=0;i<10;i++)
            {
                int product=(digits[i]-'0')*(i%2==0?1:2);
                sum+=product/10+product%10;
            }
            string candidate=digits+((10-sum%10)%10).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if(!ocupados.Contains(candidate))return candidate;
        }
        throw new InvalidOperationException("Se agotaron los números de prueba disponibles para ese año de nacimiento.");
    }

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
