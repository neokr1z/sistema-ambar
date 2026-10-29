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

// Segmento del ejemplo SEGOB; el dígito se ajusta al algoritmo (la imagen es ilustrativa).
Validation.CurpIdentity("SABC560626MDFLRN01","Concepción","Salgado","Briseño",new DateTime(1956,6,26));
Check(true,"segmento de ejemplo SEGOB con dígito calculado");
string Digit(string root)
{
 const string symbols="0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
 int total=root.Take(17).Select((c,i)=>symbols.IndexOf(c)*(18-i)).Sum();
 return root[..17]+((10-total%10)%10);
}
void Identity(string curp,string name="Concepción",string ap="Salgado",string am="Briseño") =>
 Validation.CurpIdentity(curp,name,ap,am,new DateTime(1956,6,26));
Reject(()=>Identity("SABC560626MDFLRN08"),"dígito verificador incorrecto");
Reject(()=>Identity(Digit("PABC560626MDFLRN00")),"inicial paterna incorrecta aunque checksum coincide");
Reject(()=>Identity(Digit("SOBC560626MDFLRN00")),"vocal interna incorrecta");
Reject(()=>Identity(Digit("SAGC560626MDFLRN00")),"inicial materna incorrecta");
Reject(()=>Identity(Digit("SABJ560626MDFLRN00")),"inicial del nombre incorrecta");
Reject(()=>Identity(Digit("SABC560626MDFLLN00")),"consonante interna incorrecta");
Identity("SABC560626MDFLRN01","María Concepción");
Check(true,"omisión María en nombre compuesto");
Identity("SABC560626MDFLRN01","Concepción","de Salgado","Briseño");
Check(true,"partícula de en apellido");
Validation.CurpIdentity(Digit("PEGL900101HGRRRS00"),"José Luis","Pérez","García",new DateTime(1990,1,1));
Check(true,"José Luis utiliza Luis");
Validation.CurpIdentity(Digit("XAGJ900101HGRRRL00"),"Julia","Ñarez","García",new DateTime(1990,1,1));
Check(true,"Ñ inicial sustituida por X");
Validation.CurpIdentity(Digit("BXCA900101HGRCRN00"),"Ana","Baca","Cruz",new DateTime(1990,1,1));
Check(true,"palabra inconveniente reemplaza segunda posición");

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
PersonalData Person(string rol,string name,string user,string curp,string rfc)=>new(rol,name,"Pérez","García",user+"@acapulco.tecnm.mx",user,rfc,Digit(curp),"7441234567",user,"PruebaClave123","PruebaClave123",DateTime.ParseExact("19"+curp.Substring(4,6),"yyyyMMdd",System.Globalization.CultureInfo.InvariantCulture));
var secretary=Person("Secretaria","JOHny","test.secretaria","PEGJ900101HGRRRH01","PEGJ900101AB1");
var teacher=Person("Docente","MARÍA","test.docente","PEGM900101MGRRRR01","PEGM900101AB1");
foreach(var person in new[]{teacher,secretary})
{
 Reject(()=>RepositorioPersonal.Create(admin,person with{First="Otro"}),"nombre/CURP en alta "+person.Role);
 Reject(()=>RepositorioPersonal.Create(admin,person with{Birth=null}),"fecha obligatoria en alta "+person.Role);
 Reject(()=>RepositorioPersonal.Create(admin,person with{Birth=new DateTime(1991,1,1)}),"fecha/CURP en alta "+person.Role);
 Reject(()=>RepositorioPersonal.Create(admin,person with{Rfc="PEGJ900102AB1"}),"RFC/CURP en alta "+person.Role);
}
var secretaryCreated=RepositorioPersonal.Create(admin,secretary);
RepositorioPersonal.Create(admin,teacher);
Check((string)Scalar("SELECT nombre FROM Personal_Administrativo WHERE nombre='Johny'")=="Johny","nombre normalizado en BDD");
Reject(()=>RepositorioPersonal.Create(admin,teacher with{Username="test.otro",Email="otro@acapulco.tecnm.mx",Curp=Digit("PEGM900102MGRRRR00"),Rfc="PEGM900102AB1",Birth=new DateTime(1990,1,2)}),"nombre completo duplicado");
Reject(()=>RepositorioPersonal.Create(admin,secretary with{Role="Docente",First="Johana",Username="test.curp",Email="curp@acapulco.tecnm.mx",Rfc="XXXX900101AB1"}),"CURP entre secretaria y docente");
Reject(()=>RepositorioPersonal.Create(admin,secretary with{Role="Docente",First="Pedro",Username="test.rfc",Email="rfc@acapulco.tecnm.mx",Curp=Digit("PEGP900101HGRRRD00")}),"RFC entre módulos");
foreach(string rol in new[]{"Docente","Secretaria"})
    Reject(()=>RepositorioPersonal.Create(admin,teacher with{Role=rol,Rfc="AAAA000000AAA"}),"RFC inválido en "+rol);
var secSession=Auth.Login("test.secretaria","PruebaClave123")!;
Check(secSession is not null,"login secretaria activa");
Deny(()=>RepositorioPersonal.List(secSession!,"","",""),"consulta personal exclusiva de admin");
Deny(()=>RepositorioPersonal.Deactivate(secSession!,admin.Id),"baja exclusiva de admin");
Check(RepositorioPersonal.List(admin,"Johny","Secretaria","ACTIVO").Rows.Count==1,"filtro consulta");

// Dos altas simultáneas en tablas distintas: sólo una puede reservar el mismo nombre.
var candidate=Person("Docente","Concurrente","test.concurrente1","PEGC900101HGRRRN01","PEGC900101AB1");
var candidate2=Person("Secretaria","Concurrente","test.concurrente2","PEGC900102HGRRRN01","PEGC900102AB1");
var attempts=await Task.WhenAll(new[]{candidate,candidate2}.Select(p=>Task.Run(()=>
{
    try { RepositorioPersonal.Create(admin,p); return true; } catch(ArgumentException) { return false; }
})));
Check(attempts.Count(x=>x)==1,"duplicado concurrente entre tablas");
Reject(()=>RepositorioPersonal.Create(admin,teacher with{First="Johny",Username="test.nombre",Email="nombre@acapulco.tecnm.mx",Curp=Digit("PEGJ900103HGRRRH00"),Rfc="PEGJ900103AB1",Birth=new DateTime(1990,1,3)}),"nombre completo entre docente y secretaria");

using(var setup=Db.Connect())
using(var cmd=Db.Sql(setup,"INSERT INTO Catalogo_Postal(codigo_postal,colonia,localidad,municipio,estado) VALUES('39300','Centro','Acapulco','Acapulco','Guerrero')"))cmd.ExecuteNonQuery();
int plan=RepositorioAlumnos.Plans().First().Id;
int group1=Number("SELECT ID_grupo FROM Grupo WHERE nombre_grupo='1ISC1'");
int group2=Number("SELECT ID_grupo FROM Grupo WHERE nombre_grupo='1ISC2'");
StudentData Student(int n,int group)
{
    var birth=new DateTime(2005,1,1).AddDays(n-1);
    var asp=RepositorioAlumnos.Aspirant($"ISC26-{n:D3}")!;
    string suffix=""+(char)('A'+(n/26))+(char)('A'+(n%26));
    return new StudentData{AspirantId=asp.Id,PlanId=plan,GroupId=group,Curp=Digit($"PUIA{birth:yyMMdd}HGRRNLA0"),First="Alumno "+suffix,Paternal="Prueba",Maternal="Integración",Birth=birth,Phone="7441234567",Emergency="7447654321",Email=$"alumno{n}@alumnos.acapulco.tecnm.mx",PersonalEmail="",Street="Calle",Exterior="1",Neighborhood="Centro",Locality="Acapulco",Municipality="Acapulco",State="Guerrero",Postal="39300"};
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
Check(PersonalAutomatico.EmailBase("Michelle","Pereda","Perez")=="MichellePP@acapulco.tecnm.mx","correo solicitado MichellePP");
Check(PersonalAutomatico.EmailBase("José Luis","Pérez","García")=="JoseLuisPG@acapulco.tecnm.mx","correo sin acentos con nombres compuestos");
Check(secretaryCreated.Email=="JohnyPG@acapulco.tecnm.mx" && secretaryCreated.Number=="ADM000001","datos automáticos al guardar");
var collision=RepositorioPersonal.Create(admin,teacher with{Username="test.colision",Paternal="Paz",Maternal="Gómez",Curp=Digit("PAGM900104MGRZMR00"),Rfc="PAGM900104AB1",Birth=new DateTime(1990,1,4)});
Check(collision.Email=="MariaPG2@acapulco.tecnm.mx","correo duplicado obtiene sufijo");
void Exec(string sql){using var c=Db.Connect();using var cmd=Db.Sql(c,sql);cmd.ExecuteNonQuery();}
Exec("INSERT INTO Materia(ID_Carrera,clave_materia,nombre_materia,creditos,horas_teoricas,horas_practicas,estatus) SELECT ID_Carrera,'TEST-MAT','Materia de prueba',5,2,3,'ACTIVO' FROM Carrera WHERE clave_carrera='ISC'");
int materia=Number("SELECT ID_Materia FROM Materia WHERE clave_materia='TEST-MAT'");
int docente=Number("SELECT ID_docente FROM Usuario WHERE nombre_usuario='test.docente'");
int periodo=Number($"SELECT ID_Periodo FROM Grupo WHERE ID_grupo={group1}");
Exec($"INSERT INTO Asignacion_Docente(ID_docente,ID_Materia,ID_grupo) VALUES({docente},{materia},{group1})");
int asignacion=Number($"SELECT Id_asignacion FROM Asignacion_Docente WHERE ID_docente={docente}");
Exec($"INSERT INTO Unidad_Evaluacion(ID_Materia,numero_unidad,descripcion,estatus) VALUES({materia},5,'Quinta unidad','ACTIVO')");
int unidad=Number($"SELECT ID_Unidad FROM Unidad_Evaluacion WHERE ID_Materia={materia}");
Exec($"INSERT INTO Periodo_Captura(id_periodo,nombre_captura,semana_programada,fecha_inicio,fecha_limite,estatus) VALUES({periodo},'Captura de prueba',1,CURDATE()-INTERVAL 1 DAY,CURDATE()+INTERVAL 1 DAY,'ACTIVO')");
int ventana=Number("SELECT MAX(id_periodo_captura) FROM Periodo_Captura");
Exec($"INSERT INTO Horario(Id_asignacion,dia,hora_inicio,hora_fin,aula) VALUES({asignacion},'Lunes','08:00','09:00','TEST')");
Check(RepositorioDocente.Assignments(teacherSession).Count==1,"asignaciones propias");
Check(RepositorioDocente.Roster(teacherSession,asignacion,unidad).Rows.Count==25,"lista de alumnos del grupo asignado");
Check(RepositorioDocente.Schedule(teacherSession,asignacion).Rows.Count==1,"horario operativo");
Check(RepositorioDocente.Units(teacherSession,asignacion).Single().Id==unidad,"unidades dinámicas incluso quinta unidad");
Check(RepositorioDocente.Windows(teacherSession,asignacion).Single().Id==ventana,"captura abierta");
var otro=Auth.Login("test.colision","PruebaClave123")!;
Deny(()=>RepositorioDocente.History(otro,asignacion),"no consulta calificaciones ajenas");
Deny(()=>RepositorioDocente.Roster(admin,asignacion),"módulo exclusivo de docente");
Deny(()=>RepositorioDocente.Save(otro,asignacion,unidad,ventana,new[]{new NotaCambio(control,90,null)}),"no modifica calificaciones ajenas");
RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,85.5m,null)});
Check(Convert.ToDecimal(RepositorioDocente.History(teacherSession,asignacion).Rows[0]["Calificación"])==85.5m,"persistencia de calificación");
RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,90,85.5m)});
Check(RepositorioDocente.Changes(teacherSession,asignacion).Rows.Count==2,"historial conserva valor anterior y corrección");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,95,85.5m)}),"conflicto de captura obsoleta");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,101,90)}),"rechaza nota mayor de 100");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,-1,90)}),"rechaza nota negativa");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,90.123m,90)}),"rechaza más de dos decimales");
string outsider=Convert.ToString(Scalar($"SELECT Numero_control FROM Inscripcion WHERE ID_grupo={group2} LIMIT 1"))!;
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,92,90),new NotaCambio(outsider,80,null)}),"lote con alumno ajeno se rechaza completo");
Check(Convert.ToDecimal(RepositorioDocente.History(teacherSession,asignacion).Rows[0]["Calificación"])==90,"rollback conserva nota anterior");
Exec($"UPDATE Periodo_Captura SET estatus='CERRADO' WHERE id_periodo_captura={ventana}");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,92,90)}),"periodo cerrado impide guardar");
Exec($"UPDATE Periodo_Captura SET estatus='ACTIVO',fecha_limite=CURDATE()-INTERVAL 1 DAY WHERE id_periodo_captura={ventana}");
Reject(()=>RepositorioDocente.Save(teacherSession,asignacion,unidad,ventana,new[]{new NotaCambio(control,92,90)}),"captura vencida impide guardar");
using(var c=Db.Connect())using(var cmd=Db.Sql(c,"UPDATE Usuario SET password_hash=@hash WHERE Id_usuario=@id"))
{Db.Param(cmd,"@hash",PasswordHasher.Hash("AdminTest12345"));Db.Param(cmd,"@id",admin.Id);cmd.ExecuteNonQuery();}
Deny(()=>RepositorioBitacora.Query(teacherSession,"PruebaClave123",DateTime.Today.AddDays(-1),DateTime.Today,"",0),"bitácora exclusiva del administrador");
Deny(()=>RepositorioBitacora.Query(admin,"incorrecta",DateTime.Today.AddDays(-1),DateTime.Today,"",0),"bitácora rechaza contraseña incorrecta");
var audit=RepositorioBitacora.Query(admin,"AdminTest12345",DateTime.Today.AddDays(-1),DateTime.Today.AddDays(1),"",0);
Check(audit.Rows.Count>0 && audit.Columns.Contains("Persona") && audit.Columns.Contains("Actividad"),"bitácora presenta nombres y columnas legibles");
Deny(()=>RepositorioBitacora.Query(admin,"",DateTime.Today.AddDays(-1),DateTime.Today,"",0),"otra consulta exige contraseña nuevamente");

RepositorioPersonal.Deactivate(admin,teacherSession.Id);
Check(Auth.Login("test.docente","PruebaClave123")==null,"baja docente bloquea login");
Deny(()=>RepositorioDocente.History(teacherSession,asignacion),"baja docente invalida sesión abierta");
Check(Number("SELECT COUNT(*) FROM Bitacora WHERE tipo_evento='BAJA_PERSONAL'")==2,"bajas auditadas");
Check(Number("SELECT COUNT(*) FROM Docente WHERE fecha_nacimiento='1990-01-01' AND nombre='María'") == 1,"fecha de personal persistida");
var imported=new List<(string User,string Hash)>();
using(var c=Db.Connect())using(var cmd=Db.Sql(c,"SELECT u.nombre_usuario,u.password_hash FROM Usuario u JOIN Docente d ON d.ID_docente=u.ID_docente WHERE d.curp IS NULL"))
using(var r=cmd.ExecuteReader())while(r.Read()) imported.Add((r.GetString(0),r.GetString(1)));
Check(imported.Count==25,"25 docentes importados sin identidad ficticia");
foreach(var account in imported)
{
 Check(PasswordHasher.Verify("holaruben12",account.Hash),"contraseña importada: "+account.User);
 var session=Auth.Login(account.User,"holaruben12")!;
 Check(session is not null && RepositorioDocente.Assignments(session).Count>0,"asignación visible: "+account.User);
}
Console.WriteLine($"{passed} verificaciones completadas.");

namespace AMBAR.Semana4 { internal sealed record Choice(int Id,string Label); }
