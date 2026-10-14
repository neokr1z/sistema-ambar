# AMBAR · Semana 4 — Sistema de Control Escolar

Proyecto de escritorio en **C# (.NET 8) / Windows Forms**, pensado para **Visual Studio Community 2022** y una base de datos **MySQL 8**. Está construido tomando como referencia los informes de las semanas 1, 2 y 3 y el script `tablas_ambar.sql` proporcionados por el equipo.

## 1. ¿Qué incluye realmente esta entrega?

| Requisito | Estado en el código |
|---|---|
| Pantalla de inicio, marca y logotipos del TecNM | Implementado |
| Inicio de sesión con credenciales MySQL | Implementado |
| Administrador precargado en base de datos | Script 02 |
| Alta de docentes y secretarías por el administrador | Implementado |
| Restricciones de acceso por rol | Implementado |
| Mensajes de error y validaciones de entrada | Implementado |
| Menú principal con iconos para cada rol | Implementado |
| Validación de aspirantes existentes y registro de inscripción | Implementado |
| Consulta digital de datos sin exponer la interfaz de MySQL | Implementado |
| Modificación de datos personales y de domicilio | Implementado |
| Ocultamiento lógico del expediente / recuperación por administrador | Implementado |
| Exportación del expediente a PDF con membrete y logotipos | Implementado en el código |
| Reinscripción, captura de calificaciones, correo y reportes | **Sólo menús visuales, todavía no funcionan** |

**Importante:** esta entrega es código fuente. No se compiló ni ejecutó la aplicación Windows Forms en este entorno Linux. Se deben restaurar los paquetes, compilar y completar las pruebas funcionales en una PC con Windows/Visual Studio 2022 antes de afirmar que está lista para producción o para una demostración sin supervisión.

## 2. Requisitos del equipo

- Windows 10/11.
- Visual Studio Community 2022 actualizado (17.8 o posterior recomendado) con la carga de trabajo **Desarrollo de escritorio con .NET**.
- .NET SDK 8 (si Visual Studio lo solicita).
- MySQL Server 8 y MySQL Workbench o cliente SQL compatible.
- Acceso a NuGet para restaurar `MySqlConnector` y `QuestPDF`.

El proyecto se abre desde **`AMBAR.Semana4.sln`** (también sirve abrir el `.csproj`). No es una aplicación web: corre como programa de escritorio en Windows.

## 3. Preparar MySQL

**Ejecutar en este orden** desde MySQL Workbench con permisos suficientes:

1. `Sql/01_tablas_ambar_original.sql`: es una copia **sin modificaciones** del SQL entregado por el equipo; crea `AMBAR_Control_Escolar` y sus 27 tablas.
2. `Sql/02_migracion_y_administrador.sql`: agrega campos que el modelo original omitió (`Personal_Administrativo.nombre`, `Alumno.correo_propio`, `Usuario.debe_cambiar_password`), carga los roles y precarga el usuario administrador con contraseña hasheada PBKDF2-SHA256. Se ha pensado para poder ejecutarse más de una vez sin resetear al administrador.
3. **Opcional, para una demo:** `Sql/03_datos_demostracion_opcional.sql`, crea una carrera/plan, semestre, periodo, grupo y ficha de aspirante **ficticios**. No utilizar en la base de datos real. Ficha de demostración: `F2026DEMO01`.
4. **Opcional, para configurar una cuenta de MySQL:** revisar y modificar `Sql/04_usuario_mysql_opcional.sql`, establecer contraseña propia y ejecutarlo con un usuario administrador de MySQL. La aplicación **no debe conectarse con root**.

La base de datos usada por AMBAR se llama `AMBAR_Control_Escolar`. Si ya existen las tablas, **no vuelvas a ejecutar el archivo 01 sin hacer un respaldo**, pues usa `CREATE TABLE` sin `IF NOT EXISTS`; ejecuta la migración 02 contra tu base existente después de comprobar su compatibilidad.

### Conexión de la aplicación

Abrir `appsettings.json`, ubicado junto al `.csproj`, y modificar el valor de `ConnectionString` con los datos de tu equipo. Ejemplo:

```json
{
  "ConnectionString": "Server=localhost;Port=3306;Database=AMBAR_Control_Escolar;User ID=ambar_app;Password=MI_CLAVE_PRIVADA;SslMode=Preferred;"
}
```

El fichero se copia junto al `.exe`. **No compartas un `appsettings.json` con una contraseña real en un repositorio público.** Evita otorgar permisos `CREATE`, `DROP` o `ALTER` a la cuenta normal de la aplicación. Las migraciones sólo las hace un DBA.

## 4. Iniciar en Visual Studio

1. Clonar o descargar el repositorio y abrir `AMBAR.Semana4.sln`.
2. Esperar a que Visual Studio restaure los paquetes NuGet.
3. Comprobar/editar `appsettings.json` y que el servicio MySQL esté iniciado.
4. Seleccionar la configuración **Debug**, proyecto de inicio `AMBAR.Semana4`, y pulsar **F5**.
5. Ingresar con el usuario inicial `admin` y la clave generada en `credenciales_iniciales.txt` del ZIP original entregado de forma privada. Ese archivo se excluye del repositorio público.
6. En el primer acceso se exige cambiar esa contraseña por una nueva. **Eliminar el archivo de credenciales iniciales después del cambio**. Si el administrador ya existía antes de ejecutar la migración, el script NO sustituye su contraseña.

## 5. Guía breve de uso y validaciones

**Administrador:** inicia sesión, usa **Alta de personal** y registra a un docente (RFC de 13 caracteres) o secretaria (número administrativo único), su nombre, correo institucional, usuario y contraseña. Cada operación crea los registros de empleado y cuenta dentro de una misma transacción y guarda una entrada de bitácora. El docente sólo recibe acceso a sus menús académicos (todavía pendientes); la secretaria accede a los módulos administrativos.

**Secretaría:** entra a **Alta e inscripción**. Primero busca una ficha cargada en `Aspirante`. El sistema sólo permite continuar si `resultado_examen` es `APROBADO`, `ACEPTADO` o `ACREDITADO` y el `estatus` está habilitado; no inventa un puntaje mínimo, que todavía no fue establecido formalmente en los reportes. Completa nombres, apellidos, fecha de nacimiento, teléfono de emergencia, domicilio y correo institucional. Selecciona un plan vigente y un grupo de primer semestre con cupo. Guarda para crear, en una transacción, las filas correspondientes a `Alumno`, `Domicilio`, `Inscripcion`, la actualización de `Aspirante` y `Bitacora`.

**Número de control:** se genera con un formato de demostración `AAAA + ID de aspirante con 6 dígitos`. Debe ajustarse cuando la institución defina su regla oficial.

**Correo institucional:** el sistema sugiere una dirección basada en ese número, que Secretaría debe **verificar o reemplazar**. La aplicación **no crea cuentas de correo**. El NSS se captura sólo si existe y fue verificado: AMBAR no debe inventar un NSS válido.

**Consulta:** en **Consulta de alumnos** hay búsqueda por número, nombre y CURP; doble clic para abrir el expediente visual, además de botones **Modificar**, **Exportar PDF** y **Ocultar registro**. El ocultamiento cambia sólo el valor `Alumno.estatus` a `OCULTO`; **no ejecuta DELETE**. Los expedientes ocultos desaparecen del listado de Secretaría, mientras el Administrador puede verlos, exportarlos y restaurarlos. Las inscripciones guardadas siguen ocupando el cupo del grupo, incluso si se ocultó el alumno.

**PDF:** el archivo se genera mediante `QuestPDF`, lleva el membrete/logo extraído del PDF de la semana 3, el nombre institucional, información del expediente, fecha de emisión y paginación. Incluye una advertencia explícita de que no sustituye una constancia escolar oficial.

## 6. Evidencias de pruebas propuestas para la semana 4

Las siguientes pruebas se deben realizar en Windows y guardar captura de pantalla del resultado (no se afirma aquí que ya se ejecutaron):

| Caso | Prueba | Resultado esperado |
|---|---|---|
| P01 | Ingresar contraseña incorrecta | Error de credenciales y permanencia en acceso |
| P02 | Usar administrador y cambiar contraseña inicial | Acceso a menú administrador tras cambio obligatorio |
| P03 | Registrar docente y secretaria | Ambos usuarios aparecen en `Usuario` y tablas de personal |
| P04 | Intentar alta con email, RFC o teléfono incorrecto | Mensaje de validación y sin transacción parcial |
| P05 | Inscribir ficha ficticia aprobada y grupo con cupo | Mensaje exitoso y registros consistentes en tablas |
| P06 | Intentar inscribir la misma ficha de nuevo | Bloqueo de registro duplicado |
| P07 | Intentar inscribir una ficha no aceptada | Mensaje de rechazo |
| P08 | Consultar, modificar y generar PDF | Se muestran cambios en vista digital y PDF con membrete |
| P09 | Ocultar un alumno desde Secretaría | Desaparece de su consulta |
| P10 | Consultar y restaurar el alumno como Administrador | Registro intacto y reaparece para Secretaría |
| P11 | Entrar como Docente | Visualiza su menú, sin habilitar alta de alumnos |
| P12 | Comprobar módulos pendientes | Aparecen con iconos y mensaje de módulo aún no implementado |

## 7. Alcance y decisiones pendientes

- **El correo propio y el nombre de Secretaría** se agregan en una migración: no figuraban como columnas en el script proporcionado.
- **No hay configuración institucional real de asignación de número de control, NSS o correos:** se implementa lo que se puede demostrar sin crear identidades oficiales inválidas.
- **No existe una regla explícita de aprobación numérica del examen**, por lo que sólo se aceptan los estados textuales indicados arriba.
- El PDF es un **comprobante informativo**, no un certificado oficial.
- El esquema original no almacena el turno del grupo ni la checklist de documentos entregados; esas funciones quedan para la próxima fase si se amplía el modelo.
- No se implementan aún los módulos completos de reinscripción, calificaciones, envío de documentos, estadísticas, auditoría visual ni generación de documentos distintos al PDF del expediente.

## 8. Archivos principales

`Program.cs` contiene configuración, conexión, autenticación, PBKDF2 y validaciones. `LoginForm.cs`, `MainForm.cs`, `UsersForm.cs`, `EnrollmentForm.cs`, `StudentsForm.cs`, `EditStudentForm.cs`, `ChangePasswordForm.cs` contienen las interfaces. `StudentModels.cs` y `StudentRepository.cs` manejan datos de alumnos con consultas parametrizadas y transacciones. `PdfService.cs` genera el PDF. `Theme.cs` define los colores y componentes visuales. `Assets/` conserva los dos recursos de identidad visual extraídos de los documentos proporcionados.

**Nota de seguridad:** los datos de estudiantes son información personal; para una instalación real se requieren controles adicionales de operación, copias de seguridad, cifrado en tránsito para servidores remotos, auditoría de accesos fallidos, límites de intentos, revisión de permisos e infraestructura antes de operar con alumnos reales.
