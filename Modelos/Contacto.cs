using System.Globalization;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace AMBAR.Semana4;

// Compartido por los repositorios: no depende del formulario ni de eventos KeyPress.
internal static class Contacto
{
    private static readonly PhoneNumberUtil Numbers = PhoneNumberUtil.GetInstance();
    public static bool TryPhone(string? value, out string national)
    {
        national="";
        string raw=(value??"").Trim();
        if(raw.Length is <10 or >25 || !Regex.IsMatch(raw,@"\A\+?[0-9 ()-]+\z"))return false;
        // No elimina letras, extensiones o prefijos extranjeros para convertirlos en un número aceptable.
        if(raw.Count(c=>c=='(')!=raw.Count(c=>c==')') || raw.Count(c=>c=='(')>1 ||
            (raw.Contains('(') && raw.IndexOf('(')>raw.IndexOf(')'))) return false;
        string digits=Regex.Replace(raw,@"[ ()-]","");
        if(digits.StartsWith("+52",StringComparison.Ordinal))digits=digits[3..];
        if(!Regex.IsMatch(digits,@"\A[2-9][0-9]{9}\z") || digits.Distinct().Count()==1)return false;
        try
        {
            var phone=Numbers.Parse("+52"+digits,"MX");
            if(!Numbers.IsValidNumberForRegion(phone,"MX"))return false;
            // Contactos personales y de emergencia: fijo o móvil, no 800/900/servicios cortos.
            if(Numbers.GetNumberType(phone) is not (PhoneNumberType.FIXED_LINE or PhoneNumberType.MOBILE or PhoneNumberType.FIXED_LINE_OR_MOBILE))return false;
            national=digits;return true;
        }
        catch(NumberParseException){return false;}
    }
    public static string Phone(string? text,string field,bool optional=false)
    {
        if(optional && string.IsNullOrWhiteSpace(text))return "";
        if(TryPhone(text,out var phone))return phone;
        throw new ArgumentException($"{field}: escribe un teléfono fijo o celular válido de México, con 10 dígitos. También se admite +52. No uses 01, 044, 045, extensiones ni números de otro país.");
    }
    public static bool TryEmail(string? text,out string normalized)
    {
        normalized="";
        string value=(text??"").Trim();
        if(value.Length==0 || value.Any(char.IsWhiteSpace) || value.Any(char.IsControl))return false;
        var parts=value.Split('@');
        if(parts.Length!=2 || parts[0].Length is <1 or >64)return false;
        // Dirección sola (dot-atom ASCII), sin nombres de presentación, comillas ni listas.
        if(!Regex.IsMatch(parts[0],@"\A[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*\z"))return false;
        string domain;
        try{domain=new IdnMapping{UseStd3AsciiRules=true}.GetAscii(parts[1]).ToLowerInvariant();}
        catch(ArgumentException){return false;}
        if(domain.Length>253 || !Regex.IsMatch(domain,@"\A(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+(?:[a-z]{2,63}|xn--[a-z0-9-]{2,59})\z"))return false;
        normalized=parts[0]+"@"+domain;
        if(normalized.Length>120){normalized="";return false;} // Capacidad de los campos MySQL actuales.
        return true;
    }
    public static string Email(string? text,string field,bool optional=false,bool institutional=false)
    {
        if(optional && string.IsNullOrWhiteSpace(text))return "";
        if(!TryEmail(text,out var email))throw new ArgumentException($"{field}: introduce una dirección como nombre@dominio.com, sin espacios ni puntos consecutivos (máximo 120 caracteres).");
        if(institutional && !Validation.InstitutionalEmail(email))
            throw new ArgumentException($"{field}: el dominio institucional debe ser @acapulco.tecnm.mx.");
        return email;
    }
}
