using AMBAR.Semana4;
internal static class NssTests
{
    public static int Run()
    {
        int n=0;
        void Check(bool test){if(!test)throw new Exception("Falló validación NSS");n++;}
        void Reject(string value){try{SeguridadSocial.Validate(value);}catch(ArgumentException){n++;return;}throw new Exception("NSS inválido aceptado");}
        Check(SeguridadSocial.Validate("")=="");
        Check(SeguridadSocial.Validate("  ")=="");
        // Vector estructural de prueba. No representa una afiliación verificada.
        Check(SeguridadSocial.Validate(" 12345678903 ")=="12345678903");
        foreach(string value in new[]{"12345678904","00000000000","11111111111","1234567890","123456789012","12345a78903","１２３４５６７８９０３","12345 78903"})Reject(value);
        for(int i=0;i<11;i++)
        {
            char[] v="12345678903".ToCharArray();v[i]=(char)('0'+(v[i]-'0'+1)%10);Reject(new string(v));
        }
        Check(SeguridadSocial.Display("").Contains("Pendiente"));
        Check(SeguridadSocial.Display("12345678903")=="12345678903");
        try{RepositorioListas.Obtener(new Sesion(0,"test","Sin permiso",false),1);throw new Exception("Permitió lista sin rol");}
        catch(UnauthorizedAccessException){n++;}
        Console.WriteLine($"{n} verificaciones NSS/permisos satisfactorias.");return n;
    }
}
