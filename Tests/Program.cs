using AMBAR.Semana4;
using System.Text.Json;

int passed=0;
void Check(bool condition,string name)
{
    if(!condition) throw new Exception("FALLÓ: "+name);
    passed++; Console.WriteLine("OK: "+name);
}
void Reject(Action action,string name)
{
    try { action(); } catch (ArgumentException) { Check(true,name); return; }
    throw new Exception("Aceptó datos inválidos: "+name);
}
void Deny(Action action,string name)
{
    try { action(); } catch (UnauthorizedAccessException) { Check(true,name); return; }
    throw new Exception("Permitió acceso: "+name);
}
Check(Validation.Name("  JOHny  ","nombre")=="Johny","capitalización");
Check(Validation.Name("MARÍA   josÉ","nombre")=="María José","acentos y espacios");
foreach(string s in new[]{"....","123","Juan2","-Juan","Juan--Luis","A"}) Reject(()=>Validation.Name(s,"nombre"),"nombre "+s);
Check(Validation.Name("ana-mARía","nombre")=="Ana-María","nombre con guion");
foreach(string s in new[]{"....","a...b","abc_","12345","a bcd","ab"}) Check(!Validation.Username(s),"usuario inválido "+s);
Check(Validation.Username("juan.perez_01"),"usuario válido");
foreach(string s in new[]{"...@...","a..b@acapulco.tecnm.mx",".a@acapulco.tecnm.mx","a@-dominio.mx","a@dominio..mx","a@dominio.x"}) Check(!Validation.Email(s),"correo inválido "+s);
Check(Validation.InstitutionalEmail("juan.perez@acapulco.tecnm.mx"),"correo personal institucional");
Check(Validation.InstitutionalEmail("202600001@alumnos.acapulco.tecnm.mx"),"correo alumno institucional");
Check(!Validation.InstitutionalEmail("juan@gmail.com") && Validation.Email("juan@gmail.com"),"distingue correo personal");
Check(!Validation.InstitutionalEmail("juan@acapulco.tecnm.mx.ejemplo.com"),"dominio suplantado");
foreach(string s in new[]{"0000000000","5555555555","1234567890","744123456","74412345678","74412abc67"}) Check(!Validation.Phone(s),"teléfono inválido "+s);
Check(Validation.Phone("7441234567"),"teléfono válido");
foreach(string s in new[]{"AAAA000000AAA","GODE990230AB1","GODE991332AB1","GODE900101ABZ"}) Check(!Validation.Rfc(s),"RFC inválido "+s);
Check(Validation.Rfc("GODE900101AB1"),"RFC con fecha real");
Check(Validation.Curp("GODE900101HGRMZN01"),"CURP con formato y fecha");
Check(!Validation.Curp("GODE000000HGRMZN01"),"CURP fecha imposible");
Check(!Validation.Curp("GODE900101HXXMZN01"),"CURP entidad inexistente");
Check(Validation.CurpMatchesBirth("GODE260101HGRMZN01",new DateTime(1926,1,1)),"límite inclusivo 1926");
Check(!Validation.CurpMatchesBirth("GODE251231HGRMZN01",new DateTime(1925,12,31)),"rechazo 1925");
Check(!Validation.CurpMatchesBirth("GODE900101HGRMZN01",new DateTime(1991,1,1)),"fecha distinta de CURP");

// Sólo base de pruebas desechable. Nunca lee la conexión del proyecto principal.
string? connection=Environment.GetEnvironmentVariable("AMBAR_TEST_CONNECTION");
if(string.IsNullOrEmpty(connection)) { Console.WriteLine($"{passed} verificaciones. Integración omitida: define AMBAR_TEST_CONNECTION con una BDD desechable preparada con 01,02,05,06,07."); return; }
var builder=new MySqlConnector.MySqlConnectionStringBuilder(connection);
if(!builder.Database.EndsWith("_test",StringComparison.OrdinalIgnoreCase)) throw new Exception("La BDD de pruebas debe terminar en _test.");
File.WriteAllText(Path.Combine(AppContext.BaseDirectory,"appsettings.json"),JsonSerializer.Serialize(new {ConnectionString=connection}));
object Scalar(string sql) { using var c=Db.Connect(); using var cmd=Db.Sql(c,sql);return cmd.ExecuteScalar()!; }
int Number(string sql)=>Convert.ToInt32(Scalar(sql));
Check(Number("SELECT COUNT(*) FROM Aspirante WHERE numero_ficha LIKE 'ISC26-%'")==50,"50 fichas");
Check(Number("SELECT COUNT(*) FROM Grupo WHERE Capacidad=25 AND nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2')")==6,"6 grupos con 25 lugares");
Check(Number("SELECT COUNT(*) FROM Horario_Referencia")==160,"160 bloques semanales de las imágenes");
var admin=new Sesion(Number("SELECT u.Id_usuario FROM Usuario u JOIN Rol r ON r.id_rol=u.Id_rol WHERE r.nombre='Administrador' LIMIT 1"),"admin","Administrador",false);
PersonalData Person(string rol,string name,string user,string curp,string rfc)=>new(rol,name,"Pérez","García",user+"@acapulco.tecnm.mx",user,rfc,curp,"7441234567",user,"PruebaClave123","PruebaClave123");
var secretary=Person("Secretaria","JOHny","test.secretaria","PEGJ900101HGRRHR01","PEGJ900101AB1");
var teacher=Person("Docente","MARÍA","test.docente","PEGM900101MGRRRC01","PEGM900101AB1");
RepositorioPersonal.Create(admin,secretary);
RepositorioPersonal.Create(admin,teacher);
Check((string)Scalar("SELECT nombre FROM Personal_Administrativo WHERE num_administrativo='TEST.SECRETARIA'")=="Johny","nombre normalizado en BDD");
Reject(()=>RepositorioPersonal.Create(admin,teacher with{Username="test.otro",Email="otro@acapulco.tecnm.mx",Curp="PEGM900102MGRRRC01",Rfc="PEGM900102AB1"}),"nombre completo duplicado");
Reject(()=>RepositorioPersonal.Create(admin,secretary with{Role="Docente",First="Pedro",Username="test.curp",Email="curp@acapulco.tecnm.mx",Rfc="XXXX900101AB1"}),"CURP entre secretaria y docente");
Reject(()=>RepositorioPersonal.Create(admin,secretary with{Role="Docente",First="Pedro",Username="test.rfc",Email="rfc@acapulco.tecnm.mx",Curp="PEGP900101HGRRRD01"}),"RFC entre módulos");
foreach(string rol in new[]{"Docente","Secretaria"})
    Reject(()=>RepositorioPersonal.Create(admin,teacher with{Role=rol,Rfc="AAAA000000AAA"}),"RFC inválido en "+rol);
var secSession=Auth.Login("test.secretaria","PruebaClave123")!;
Check(secSession is not null,"login secretaria activa");
Deny(()=>RepositorioPersonal.List(secSession!,"","",""),"consulta personal exclusiva de admin");
Deny(()=>RepositorioPersonal.Deactivate(secSession!,admin.Id),"baja exclusiva de admin");
Check(RepositorioPersonal.List(admin,"Johny","Secretaria","ACTIVO").Rows.Count==1,"filtro consulta");

// Dos altas simultáneas en tablas distintas: sólo una puede reservar el mismo nombre.
var candidate=Person("Docente","Concurrente","test.concurrente1","PEGC900101HGRRNC01","PEGC900101AB1");
var candidate2=Person("Secretaria","Concurrente","test.concurrente2","PEGC900102HGRRNC01","PEGC900102AB1");
var attempts=await Task.WhenAll(new[]{candidate,candidate2}.Select(p=>Task.Run(()=>
{
    try { RepositorioPersonal.Create(admin,p); return true; } catch(ArgumentException) { return false; }
})));
Check(attempts.Count(x=>x)==1,"duplicado concurrente entre tablas");
Reject(()=>RepositorioPersonal.Create(admin,teacher with{First="Johny",Username="test.nombre",Email="nombre@acapulco.tecnm.mx",Curp="PEGM900103MGRRRC01",Rfc="PEGM900103AB1"}),"nombre completo entre docente y secretaria");

int plan=RepositorioAlumnos.Plans().First().Id;
int group1=Number("SELECT ID_grupo FROM Grupo WHERE nombre_grupo='1ISC1'");
int group2=Number("SELECT ID_grupo FROM Grupo WHERE nombre_grupo='1ISC2'");
StudentData Student(int n,int group)
{
    var birth=new DateTime(2005,1,1).AddDays(n-1);
    var asp=RepositorioAlumnos.Aspirant($"ISC26-{n:D3}")!;
    string suffix=""+(char)('A'+(n/26))+(char)('A'+(n%26));
    return new StudentData{AspirantId=asp.Id,PlanId=plan,GroupId=group,Curp=$"GODE{birth:yyMMdd}HGRMZNA1",First="Alumno "+suffix,Paternal="Prueba",Maternal="Integración",Birth=birth,Phone="7441234567",Emergency="7447654321",Email=$"alumno{n}@alumnos.acapulco.tecnm.mx",PersonalEmail="",Street="Calle",Exterior="1",Neighborhood="Centro",Locality="Acapulco",Municipality="Acapulco",State="Guerrero",Postal="39300"};
}
var first=Student(1,group1);
string control=RepositorioAlumnos.Enroll(first,admin);
Check(RepositorioAlumnos.Aspirant("ISC26-001")!.Curp==first.Curp,"ficha adquiere CURP real al inscribir");
Reject(()=>RepositorioAlumnos.Enroll(first,admin),"ficha usada no se reutiliza");
var edited=RepositorioAlumnos.Detail(admin,control)!;
edited.First="aLUmNO AA";
RepositorioAlumnos.Update(edited,admin);
Check(RepositorioAlumnos.Detail(admin,control)!.First=="Alumno Aa","edición propia y normalización");
var duplicate=Student(2,group1);duplicate.First=edited.First;
Reject(()=>RepositorioAlumnos.Enroll(duplicate,admin),"nombre duplicado entre alumnos");
duplicate=Student(2,group1);duplicate.Curp=secretary.Curp;duplicate.Birth=new DateTime(1990,1,1);
Reject(()=>RepositorioAlumnos.Enroll(duplicate,admin),"CURP personal contra alumno");
for(int n=2;n<=25;n++) RepositorioAlumnos.Enroll(Student(n,group1),admin);
Reject(()=>RepositorioAlumnos.Enroll(Student(26,group1),admin),"26º alumno excede cupo");
Check(RepositorioAlumnos.Aspirant("ISC26-026")!.Status=="ACTIVO","rollback conserva ficha rechazada");
for(int n=26;n<=50;n++) RepositorioAlumnos.Enroll(Student(n,group2),admin);
Check(Number("SELECT COUNT(*) FROM Inscripcion WHERE estatus='INSCRITO'")==50,"50 alumnos: 25 por grupo");
Check(RepositorioAlumnos.Groups(plan,RepositorioAlumnos.Aspirant("ISC26-001")!.PeriodId).Count==0,"grupos llenos no se ofrecen");
RepositorioPersonal.Deactivate(admin,secSession!.Id);
Check(Auth.Login("test.secretaria","PruebaClave123")==null,"baja bloquea login");
Deny(()=>RepositorioAlumnos.ListStudents(secSession,""),"baja invalida sesión abierta");
Check(RepositorioPersonal.List(admin,"Johny","Secretaria","INACTIVO").Rows.Count==1,"historial de bajas consultable");
Reject(()=>RepositorioPersonal.Create(admin,secretary),"baja no libera identidad");
var teacherSession=Auth.Login("test.docente","PruebaClave123")!;
RepositorioPersonal.Deactivate(admin,teacherSession.Id);
Check(Auth.Login("test.docente","PruebaClave123")==null,"baja docente bloquea login");
Check(Number("SELECT COUNT(*) FROM Bitacora WHERE tipo_evento='BAJA_PERSONAL'")==2,"bajas auditadas");
Console.WriteLine($"{passed} verificaciones completadas.");

namespace AMBAR.Semana4 { internal sealed record Choice(int Id,string Label); }
