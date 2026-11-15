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
    public static bool Email(string text) => Contacto.TryEmail(text, out _);
    public static bool InstitutionalEmail(string text) => Contacto.TryEmail(text, out var email) &&
        email.Split('@')[1] == "acapulco.tecnm.mx";
    public static bool Phone(string text) => Contacto.TryPhone(text, out _);
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
    // Verificación local de congruencia, no certificación ante RENAPO.
    public static void CurpIdentity(string curp, string first, string paternal, string maternal, DateTime birth)
    {
        curp = curp.Trim().ToUpperInvariant();
        if (!Curp(curp)) throw new ArgumentException("CURP inválida: revisa formato, entidad y fecha (desde 1926).");
        if (!CurpMatchesBirth(curp, birth.Date)) throw new ArgumentException("La fecha de nacimiento no coincide con la CURP (incluido el siglo).");
        string given = CurpWord(first, true), ap = CurpWord(paternal, false), am = CurpWord(maternal, false);
        char Initial(string value) => value.Length == 0 || value[0] == 'Ñ' ? 'X' : value[0];
        char Internal(string value, bool vowel)
        {
            foreach (char ch in value.Skip(1))
                if (char.IsLetter(ch) && ("AEIOU".Contains(ch) == vowel)) return ch == 'Ñ' ? 'X' : ch;
            return 'X';
        }
        string prefix = $"{Initial(ap)}{Internal(ap, true)}{Initial(am)}{Initial(given)}";
        const string inconvenient = "BACA BAKA BUEI BUEY CACA CACO CAGA CAGO CAKA CAKO COGE COGI COJA COJE COJI COJO COLA CULO FALO FETO GETA GUEI GUEY JETA JOTO KACA KACO KAGA KAGO KAKA KAKO KOGE KOGI KOJA KOJE KOJI KOJO KOLA KULO LILO LOCA LOCO LOKA LOKO MAME MAMO MEAR MEAS MEON MIAR MION MOCO MOKO MULA MULO NACA NACO PEDA PEDO PENE PIPI PITO POPO PUTA PUTO QULO RATA ROBA ROBE ROBO RUIN SENO TETA VACA VAGA VAGO VAKA VUEI VUEY WUEI WUEY";
        if (inconvenient.Split(' ').Contains(prefix)) prefix = prefix[0] + "X" + prefix[2..];
        if (curp[..4] != prefix)
            throw new ArgumentException("Las primeras cuatro letras de la CURP no coinciden con el nombre y los apellidos. Revisa los datos conforme al acta de nacimiento.");
        if (curp.Substring(13, 3) != $"{Internal(ap, false)}{Internal(am, false)}{Internal(given, false)}")
            throw new ArgumentException("Las consonantes internas de la CURP no coinciden con el nombre y los apellidos.");
        const string alphabet = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        int sum = 0;
        for (int n = 0; n < 17; n++) sum += alphabet.IndexOf(curp[n]) * (18 - n);
        if ((10 - sum % 10) % 10 != curp[17] - '0')
            throw new ArgumentException("El dígito verificador de la CURP es incorrecto. Copia la clave de tu constancia.");
    }
    private static string CurpWord(string value, bool given)
    {
        // Preserva Ñ: en posiciones seleccionadas se sustituye por X, no por N.
        value = value.ToUpperInvariant().Replace("Ñ", "\u0001");
        value = string.Concat(value.Normalize(System.Text.NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace("\u0001", "Ñ");
        var particles = new HashSet<string>("DA DAS DE DEL DER DI DIE DD EL LA LAS LOS LE LES MAC MC VAN VON Y".Split(' '));
        var words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(w => !particles.Contains(w)).ToArray();
        if (words.Length == 0) return "";
        return (given && words.Length > 1 && (words[0] == "MARIA" || words[0] == "JOSE") ? words[1] : words[0]).Replace("-", "X").Replace("\'", "X").Replace("’", "X");
    }
}
