using System.Net;
using System.Net.Http;
using AMBAR.Semana4;

internal static class ContactoTests
{
    private sealed class Handler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage,HttpResponseMessage> reply;
        public List<Uri> Seen {get;}=new();
        public Handler(Func<HttpRequestMessage,HttpResponseMessage> response){reply=response;}
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken token)
        {Seen.Add(request.RequestUri!);return Task.FromResult(reply(request));}
    }
    private static HttpResponseMessage Json(string value)=>new(HttpStatusCode.OK){Content=new StringContent(value)};
    private const string Mx="{\"Status\":0,\"Answer\":[{\"type\":15,\"data\":\"10 mx.test.\"}]}";
    private const string A="{\"Status\":0,\"Answer\":[{\"type\":1,\"data\":\"192.0.2.1\"}]}";
    private const string Nx="{\"Status\":3}";
    private const string Empty="{\"Status\":0}";
    internal static CorreoDominio WorkingResolver()=>new(new HttpClient(new Handler(r=>Json(r.RequestUri!.Query.EndsWith("type=15")?Mx:A))));
    internal static async Task<int> Run()
    {
        int count=0;
        void Check(bool test,string message){if(!test)throw new Exception("FALLÓ: "+message);count++;Console.WriteLine("OK: "+message);}
        async Task Reject(Func<Task> action,string name,bool unavailable=false)
        {
            try{await action();}
            catch(ArgumentException) when(!unavailable){Check(true,name);return;}
            catch(InvalidOperationException) when(unavailable){Check(true,name);return;}
            throw new Exception("Aceptó: "+name);
        }
        foreach(string phone in new[]{"5512345678","3312345678","8112345678","7441234567","6641234567","9991234567","2221234567","9981234567"})
            Check(Validation.Phone(phone),"número nacional: "+phone);
        foreach(string phone in new[]{"2101234567","5201234567","+12025550123","+517441234567","+5217441234567","017441234567","0447441234567","911","8001234567","9001234567","7441234567 ext 1","74412abc67","٧٤٤١٢٣٤٥٦٧"})
            Check(!Validation.Phone(phone),"rechazo telefónico: "+phone);
        Check(Contacto.Phone("+52 (744) 123-4567","Teléfono")=="7441234567","normaliza +52 y formato visual a 10 dígitos");
        Check(Contacto.Phone(" ","Teléfono",optional:true)=="","opcional vacío");
        foreach(string email in new[]{"...@...","a@@gmail.com","a..b@gmail.com","a.@gmail.com",".a@gmail.com","a b@gmail.com","Persona <a@gmail.com>","a@-gmail.com","a@gmail.com.","a@123.456","a@gmail.com\r\nBcc:x@y.mx"})
            Check(!Validation.Email(email),"rechazo de correo: "+email.Replace("\r\n"," "));
        Check(Contacto.Email(" Nombre.Apellido+escuela@GMAIL.COM ","Correo")=="Nombre.Apellido+escuela@gmail.com","normaliza dominio y conserva buzón");
        Check(Validation.Email("ana_maria@outlook.com"),"correo con guion bajo");
        Check(Contacto.TryEmail("ana@bücher.de",out var idn) && idn=="ana@xn--bcher-kva.de","dominio internacionalizado");
        Check(!Validation.Email(new string('a',65)+"@gmail.com"),"límite local 64");
        Check(!Validation.Email(new string('a',64)+"@"+new string('b',60)+".com"),"límite de columna 120");
        var validHandler=new Handler(r=>Json(r.RequestUri!.Query.EndsWith("type=15")?Mx:A));
        await new CorreoDominio(new HttpClient(validHandler)).RequireAsync("privado@gmail.com","Correo");
        Check(validHandler.Seen.All(u=>!u.AbsoluteUri.Contains("privado") && !u.AbsoluteUri.Contains("%40")),"DNS recibe dominio, nunca el buzón");
        var nx=new CorreoDominio(new HttpClient(new Handler(_=>Json(Nx))));
        await Reject(()=>nx.RequireAsync("a@no-existe.invalid","Correo"),"NXDOMAIN bloquea guardado");
        var noMail=new CorreoDominio(new HttpClient(new Handler(_=>Json("{\"Status\":0,\"Answer\":[{\"type\":15,\"data\":\"0 .\"}]}"))));
        await Reject(()=>noMail.RequireAsync("a@example.com","Correo"),"Null MX rechaza dominio sin correo");
        var deadMx=new CorreoDominio(new HttpClient(new Handler(r=>Json(r.RequestUri!.Query.EndsWith("type=15")?Mx:Nx))));
        await Reject(()=>deadMx.RequireAsync("a@dominio.mx","Correo"),"MX cuyo servidor no existe");
        var fallback=new CorreoDominio(new HttpClient(new Handler(r=>Json(r.RequestUri!.Query.EndsWith("type=15")?Empty:A))));
        await fallback.RequireAsync("a@dominio.mx","Correo");Check(true,"MX implícito con A válido");
        var ipv6=new CorreoDominio(new HttpClient(new Handler(r=>Json(r.RequestUri!.Query.EndsWith("type=28")?"{\"Status\":0,\"Answer\":[{\"type\":28,\"data\":\"2001:db8::1\"}]}":Empty))));
        await ipv6.RequireAsync("a@dominio.mx","Correo");Check(true,"ruta IPv6 sin IPv4");
        var noRoute=new CorreoDominio(new HttpClient(new Handler(_=>Json(Empty))));
        await Reject(()=>noRoute.RequireAsync("a@dominio.mx","Correo"),"sin MX ni A ni AAAA");
        var failoverHandler=new Handler(r=>r.RequestUri!.Host=="cloudflare-dns.com"?new(HttpStatusCode.ServiceUnavailable):Json(r.RequestUri.Query.EndsWith("type=15")?Mx:A));
        await new CorreoDominio(new HttpClient(failoverHandler)).RequireAsync("a@dominio.mx","Correo");
        Check(failoverHandler.Seen.Any(u=>u.Host=="dns.google"),"resolvedor secundario ante falla");
        foreach(string response in new[]{"{\"Status\":2}","{\"Status\":0,\"TC\":true}","{\"Status\":0,\"Answer\":{}}","no-json"})
            await Reject(()=>new CorreoDominio(new HttpClient(new Handler(_=>Json(response)))).RequireAsync("a@dominio.mx","Correo"),"falla temporal no se confunde con dirección inválida",true);
        var offline=new CorreoDominio(new HttpClient(new Handler(_=>throw new HttpRequestException("Sin conexión"))));
        await Reject(()=>offline.RequireAsync("a@dominio.mx","Correo"),"sin Internet no guarda ni acepta silenciosamente",true);
        var timeout=new CorreoDominio(new HttpClient(new Handler(_=>throw new TaskCanceledException("Timeout"))));
        await Reject(()=>timeout.RequireAsync("a@dominio.mx","Correo"),"timeout permite reintentar",true);
        // Comprueba los repositorios reales; el rechazo ocurre antes de abrir MySQL.
        var actor=new Sesion(1,"admin","Administrador",false);
        StudentData Student()=>new(){AspirantId=1,PlanId=1,GroupId=1,First="Concepción",Paternal="Salgado",Maternal="Briseño",Curp="SABC560626MDFLRN01",Birth=new(1956,6,26),Phone="7441234567",Emergency="7447654321",Email="persona@acapulco.tecnm.mx",PersonalEmail="a@gmail.com",Street="Calle",Exterior="1",Neighborhood="Centro",Locality="Acapulco",Municipality="Acapulco",State="Guerrero",Postal="39300"};
        CorreoDominio saved=CorreoDominio.Current;
        try
        {
            CorreoDominio.Current=nx;
            await Reject(()=>RepositorioAlumnos.EnrollAsync(Student(),actor),"inscripción exige dominio real");
            await Reject(()=>RepositorioAlumnos.UpdateAsync(Student(),actor),"edición exige dominio real");
            CorreoDominio.Current=new(new HttpClient(new Handler(r=>Json(r.RequestUri!.Query.Contains("name=gmail.com")?Nx:r.RequestUri.Query.EndsWith("type=15")?Mx:A))));
            await Reject(()=>RepositorioAlumnos.EnrollAsync(Student(),actor),"correo personal también se consulta");
            foreach(string role in new[]{"Docente","Secretaria"})
            {
                var person=new PersonalData(role,"Concepción","Salgado","Briseño","","prueba.user","SABC560626AB1","SABC560626MDFLRN01","7441234567","","PruebaClave123","PruebaClave123",new(1956,6,26));
                CorreoDominio.Current=nx;
                await Reject(()=>RepositorioPersonal.CreateAsync(actor,person),"dominio institucional en alta "+role);
                CorreoDominio.Current=WorkingResolver();
                await Reject(()=>RepositorioPersonal.CreateAsync(actor,person with{Phone="2101234567"}),"teléfono en alta "+role);
            }
            var wrongPhone=Student();wrongPhone.Phone="2101234567";
            await Reject(()=>RepositorioAlumnos.EnrollAsync(wrongPhone,actor),"teléfono de alumno se rechaza en repositorio");
            var wrongEmergency=Student();wrongEmergency.Emergency="+12025550123";
            await Reject(()=>RepositorioAlumnos.UpdateAsync(wrongEmergency,actor),"emergencia extranjera se rechaza en edición");
            var normalized=Student();normalized.Phone="+52 (744) 123-4567";normalized.Validate();
            Check(normalized.Phone=="7441234567","modelo persiste teléfono normalizado");
        }
        finally{CorreoDominio.Current=saved;}
        // Opcional: comprobación real explícita, nunca envía mensajes ni consulta buzones.
        if(Environment.GetEnvironmentVariable("AMBAR_TEST_DNS_LIVE")=="1")
        {
            await saved.RequireAsync("prueba@gmail.com","Correo");
            await saved.RequireAsync("prueba@acapulco.tecnm.mx","Correo");
            await Reject(()=>saved.RequireAsync("prueba@dominio-inexistente.invalid","Correo"),"DNS real NXDOMAIN");
            await Reject(()=>saved.RequireAsync("prueba@example.com","Correo"),"DNS real Null MX");
            Check(true,"DNS real Gmail y dominio institucional");
        }
        return count;
    }
}
