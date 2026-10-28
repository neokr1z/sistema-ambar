using System.Globalization;
using System.Text.RegularExpressions;

namespace AMBAR.Semana4;

// Reglas compartidas por altas y edición. Valida sintaxis; no consulta SAT/RENAPO.
internal static class Validation
{
    public static readonly DateTime EarliestBirth = new(1926, 1, 1);
    public static string Mandatory(string text, string field)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException($"El campo {field} es obligatorio.");
        return text.Trim();
    }
    public static string Name(string text, string field, int max = 50)
    {
        string value = Regex.Replace(Mandatory(text, field), @"\s+", " ");
        if (value.Length > max || value.Count(char.IsLetter) < 2 ||
            !Regex.IsMatch(value, @"^\p{L}+(?:[ '\-’]\p{L}+)*$"))
            throw new ArgumentException($"{field}: escribe letras; sólo se permiten espacios, guiones o apóstrofos entre palabras (máximo {max} caracteres).");
        return CultureInfo.GetCultureInfo("es-MX").TextInfo.ToTitleCase(value.ToLowerInvariant());
    }
    public static bool Username(string text) => Regex.IsMatch(text, @"^[A-Za-z][A-Za-z0-9]*(?:[._-][A-Za-z0-9]+)*$") && text.Length is >= 4 and <= 50;
    public static bool Email(string text)
    {
        string s = text.Trim();
        if (s.Length > 120 || s.Contains("..")) return false;
        var parts = s.Split('@');
        return parts.Length == 2 && parts[0].Length is > 0 and <= 64 &&
            Regex.IsMatch(parts[0], @"^[A-Za-z0-9]+(?:[._%+\-][A-Za-z0-9]+)*$") &&
            Regex.IsMatch(parts[1], @"^(?:[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?\.)+[A-Za-z]{2,63}$");
    }
    public static bool InstitutionalEmail(string text)
    {
        if (!Email(text)) return false;
        string domain = text.Trim().Split('@')[1].ToLowerInvariant();
        return domain is "acapulco.tecnm.mx" or "alumnos.acapulco.tecnm.mx";
    }
    public static bool Phone(string s) => Regex.IsMatch(s.Trim(), @"^[2-9][0-9]{9}$") && s.Trim().Distinct().Count() > 1;
    public static bool Password(string s) => s.Length >= 10 && s.Any(char.IsLetter) && s.Any(char.IsDigit);
    public static bool Postal(string s) => Regex.IsMatch(s.Trim(), @"^[0-9]{5}$");
    private static bool BirthCode(string s, int? century = null)
    {
        if (!int.TryParse(s[..2], out int yy)) return false;
        foreach (int start in century.HasValue ? new[] { century.Value } : new[] { 1900, 2000 })
            if (DateTime.TryParseExact((start + yy).ToString("D4") + s[2..], "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) && date >= EarliestBirth && date < DateTime.Today) return true;
        return false;
    }
    public static bool Rfc(string text)
    {
        string s = text.Trim().ToUpperInvariant();
        return Regex.IsMatch(s, @"^[A-ZÑ&]{4}[0-9]{6}[A-Z0-9]{2}[0-9A]$") && BirthCode(s.Substring(4, 6));
    }
    public static bool Curp(string text)
    {
        string s = text.Trim().ToUpperInvariant();
        return Regex.IsMatch(s, @"^[A-Z][AEIOUX][A-Z]{2}[0-9]{6}[HM](AS|BC|BS|CC|CL|CM|CS|CH|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[A-Z0-9][0-9]$") &&
            BirthCode(s.Substring(4, 6), char.IsDigit(s[16]) ? 1900 : 2000);
    }
    public static bool CurpMatchesBirth(string curp, DateTime birth) =>
        Curp(curp) && birth >= EarliestBirth && birth < DateTime.Today &&
        curp.Substring(4, 6) == birth.ToString("yyMMdd", CultureInfo.InvariantCulture) &&
        (char.IsDigit(curp[16]) ? birth.Year < 2000 : birth.Year >= 2000);
}
