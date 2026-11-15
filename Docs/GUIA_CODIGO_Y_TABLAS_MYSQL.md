# AMBAR · Guía para explicar todo el código (versión limpia)

> Fuente: ZIP `sistema-ambar-limpio_113038(1).zip` recibido el 13/11/2026. Esta guía documenta **los archivos de esa versión**; no sustituye ejecutar el proyecto en Visual Studio con MySQL. Las rutas describen la versión reorganizada, pero los números de línea de los `.cs` no cambian.

## 1. Cómo se conecta el sistema

`Program.Main()` → `LoginForm` → `Auth.Login()` (`Nucleo/Datos.cs`) → `Sesion` → `MainForm` → formulario de módulo → repositorio o consulta SQL directa → `Db.Connect()` → MySQL → tabla/resultado/PDF.

**Archivos clave de infraestructura:** `Program.cs` inicia; `Nucleo/Datos.cs` autentica y conecta; `Nucleo/Identidad.cs` valida permisos e identidades; `Nucleo/PeriodoActual.cs` identifica el periodo escolar ACTIVO. Las ventanas WinForms tienen tres archivos: `.cs` (eventos y lógica), `.Designer.cs` (controles y posiciones) y `.resx` (recursos).

## 2. Los 23 objetos de tabla y su papel en AMBAR

| Tabla | ¿Qué guarda? | ¿Dónde la consulta o modifica el código C#? |
|---|---|---|
| `Aspirante` | Ficha de ingreso, CURP y resultado del examen; se vincula al alumno admitido. | `Nucleo/Identidad.cs` (líneas 47; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 255; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 62, 93, 159, 207, 293; SELECT/JOIN/UPDATE) |
| `Carrera` | Catálogo de carreras del instituto; relaciona planes, materias y grupos. | `Repositorios/RepositorioAlumnos.cs` (líneas 23, 40, 111, 179, 209; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 25; SELECT/JOIN) |
| `Semestre` | Catálogo de 1.º, 2.º y 3.º semestre usado para clasificar grupos. | `Formularios/Alumnos/ConsultaGruposForm.cs` (líneas 55; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 42, 113; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 28, 42; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 47, 133; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 48, 86, 166; SELECT/JOIN) |
| `Periodo_Escolar` | Periodos, fechas de inicio/fin y cuál está ACTIVO. | `Formularios/Alumnos/ConsultaGruposForm.cs` (líneas 56; SELECT/JOIN); `Formularios/Alumnos/HorarioAlumnoForm.cs` (líneas 49; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 256, 268; SELECT/JOIN); `Nucleo/PeriodoActual.cs` (líneas 18; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 43, 114, 214; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 29, 43, 73; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 23, 41; SELECT/JOIN); `Repositorios/RepositorioDocente.cs` (líneas 26, 43; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 25; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 48, 119, 134; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 49, 87, 153, 167; SELECT/JOIN) |
| `Docente` | Datos laborales y personales de los profesores; se conecta con asignaciones y usuarios. | `Nucleo/Identidad.cs` (líneas 11, 35; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 307; SELECT/JOIN); `Servicios/PersonalAutomatico.cs` (líneas 28; SELECT/JOIN); `Repositorios/RepositorioBitacora.cs` (líneas 30; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 21; SELECT/JOIN); `Repositorios/RepositorioPersonal.cs` (líneas 57, 128, 161; INSERT/SELECT/JOIN/UPDATE); `Repositorios/RepositorioReinscripcion.cs` (líneas 116; SELECT/JOIN) |
| `Personal_Administrativo` | Personal de Secretaría y administrativo, con identificador y estado. | `Nucleo/Identidad.cs` (líneas 12, 36; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 309; SELECT/JOIN); `Servicios/PersonalAutomatico.cs` (líneas 29, 41; SELECT/JOIN); `Repositorios/RepositorioBitacora.cs` (líneas 31; SELECT/JOIN); `Repositorios/RepositorioPersonal.cs` (líneas 74, 129, 162; INSERT/SELECT/JOIN/UPDATE); `Repositorios/RepositorioReinscripcion.cs` (líneas 117; SELECT/JOIN) |
| `Plan_Estudio` | Versiones de planes académicos asociados a una carrera. | `Repositorios/RepositorioAlumnos.cs` (líneas 23, 41, 112, 178, 208; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 24; SELECT/JOIN) |
| `Materia` | Catálogo de asignaturas por carrera. | `Formularios/Principal/MainForm.cs` (líneas 441, 507; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 24; SELECT/JOIN); `Repositorios/RepositorioDocente.cs` (líneas 25, 44; SELECT/JOIN) |
| `Grupo` | Grupos académicos, semestre, carrera, periodo, cupo y estado. | `Formularios/Alumnos/ConsultaGruposForm.cs` (líneas 54; SELECT/JOIN); `Formularios/Alumnos/HorarioAlumnoForm.cs` (líneas 47; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 267, 443; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 39, 110, 213; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 27, 41, 72; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 22, 40; SELECT/JOIN); `Repositorios/RepositorioDocente.cs` (líneas 25, 43; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 25; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 46, 118, 132; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 47, 67, 85, 152, 165; SELECT/JOIN) |
| `Alumno` | Expediente de nuevo ingreso (número de control, CURP y datos de contacto). | `Nucleo/Identidad.cs` (líneas 34; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 259, 497; SELECT/JOIN); `Servicios/PersonalAutomatico.cs` (líneas 30; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 11, 105, 136, 177, 206, 263, 267…; SELECT/JOIN/INSERT/UPDATE); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 23; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 69, 147, 187; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 36; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 107; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 115; SELECT/JOIN) |
| `Domicilio` | Datos postales vinculados al número de control del alumno. | `Repositorios/RepositorioAlumnos.cs` (líneas 147, 210, 286; INSERT/SELECT/JOIN/UPDATE) |
| `Inscripcion` | Vínculo entre expediente Alumno y grupo; contiene tipo, fecha y estatus. | `Formularios/Alumnos/ConsultaGruposForm.cs` (líneas 57; SELECT/JOIN); `Formularios/Alumnos/HorarioAlumnoForm.cs` (líneas 48; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 249, 266, 432; SELECT/JOIN); `Repositorios/RepositorioAlumnos.cs` (líneas 44, 128, 155, 211, 212; SELECT/JOIN/INSERT); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 26; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 68; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 36; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 43, 55, 117, 152; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 46, 89, 152, 181; SELECT/JOIN) |
| `Asignacion_Docente` | Relaciona a un docente con una materia y un grupo. | `Formularios/Principal/MainForm.cs` (líneas 439, 505; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 19, 39, 67, 75; SELECT/JOIN); `Repositorios/RepositorioDocente.cs` (líneas 24, 42; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 16; SELECT/JOIN) |
| `Rol` | Catálogo de roles: Administrador, Secretaría y Docente. | `Nucleo/Datos.cs` (líneas 64; SELECT/JOIN); `Nucleo/Identidad.cs` (líneas 10; SELECT/JOIN); `Repositorios/RepositorioBitacora.cs` (líneas 29; SELECT/JOIN); `Repositorios/RepositorioPersonal.cs` (líneas 91, 127, 150; SELECT/JOIN) |
| `Usuario` | Cuentas de acceso, hash de contraseña, rol, bloqueo de intentos y estatus. | `Formularios/Acceso/CambiarPasswordForm.cs` (líneas 30, 35; SELECT/JOIN/UPDATE); `Nucleo/Datos.cs` (líneas 64, 93, 107; SELECT/JOIN/UPDATE); `Nucleo/Identidad.cs` (líneas 10; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 306, 438, 506; SELECT/JOIN); `Repositorios/RepositorioBitacora.cs` (líneas 12, 29; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 20; SELECT/JOIN); `Repositorios/RepositorioDocente.cs` (líneas 24, 42; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 17; SELECT/JOIN); `Repositorios/RepositorioPersonal.cs` (líneas 99, 127, 150, 159; INSERT/SELECT/JOIN/UPDATE) |
| `Bitacora` | Registro de acciones: usuario, fecha, módulo, evento, descripción y registro afectado. | `Nucleo/Datos.cs` (líneas 34; INSERT); `Repositorios/RepositorioBitacora.cs` (líneas 29; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 204; SELECT/JOIN) |
| `Reinscripcion` | Registro autónomo de reinscritos con datos mínimos y grupo, independiente de Alumno. | `Formularios/Alumnos/ConsultaGruposForm.cs` (líneas 58; SELECT/JOIN); `Formularios/Principal/MainForm.cs` (líneas 275, 435, 499; SELECT/JOIN); `Repositorios/RepositorioConsultaSemestres.cs` (líneas 40, 71; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 76, 148, 188; SELECT/JOIN); `Repositorios/RepositorioListas.cs` (líneas 42; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 21, 41, 53, 89, 106, 150, 169…; SELECT/JOIN/UPDATE); `Repositorios/RepositorioReinscripcion.cs` (líneas 67, 88, 114, 142, 179, 190, 212; SELECT/JOIN/INSERT) |
| `Control_Identidad` | Fila de control de concurrencia para evitar altas simultáneas de personas duplicadas. | `Nucleo/Identidad.cs` (líneas 28; SELECT/JOIN); `Repositorios/RepositorioMantenimientoReinscripcion.cs` (líneas 81, 199; SELECT/JOIN); `Repositorios/RepositorioReinscripcion.cs` (líneas 139; SELECT/JOIN) |
| `Horario` | Horario único de grupo, vinculado opcionalmente a asignación docente. | `Formularios/Alumnos/HorarioAlumnoForm.cs` (líneas 46; SELECT/JOIN); `Formularios/Docentes/HorarioGrupoForm.cs` (líneas 131; SELECT/JOIN) |
| `Catalogo_Postal` | Catálogo de códigos postales, colonias, municipio, estado y localidad (SEPOMEX). | `Servicios/CatalogoPostal.cs` (líneas 15; SELECT/JOIN) |
| Lista de dominios en C# (sin tabla) | Dominios admitidos localmente. | `Servicios/CorreoDominio.cs` |
| `Calificacion_Corte` | Notas por alumno, asignación docente y semana (5, 9, 13 o 17). | `Formularios/Principal/MainForm.cs` (líneas 504; SELECT/JOIN); `Repositorios/RepositorioCortesDocente.cs` (líneas 79, 98, 146, 186, 260, 275, 290; SELECT/JOIN/INSERT) |

**Cómo leer la tercera columna:** `SELECT/JOIN` = lectura; `INSERT` = alta; `UPDATE` = modificación; `DELETE` = borrado. Se detectaron apariciones SQL explícitas y sus líneas. Cuando una tabla se consulta mediante otra clase, debes seguir también la llamada al método del repositorio. Esta tabla no es un analizador formal de C# y SQL dinámico.

### Vistas SQL que existen

- `vw_AMBAR_Alumnos_Semestre_1`: vista definida en `Sql/07_vistas.sql` para consulta por semestre.
- `vw_AMBAR_Alumnos_Semestre_2`: vista definida en `Sql/07_vistas.sql` para consulta por semestre.
- `vw_AMBAR_Alumnos_Semestre_3`: vista definida en `Sql/07_vistas.sql` para consulta por semestre.
En esta revisión **no se detectaron referencias directas a esas vistas en los archivos `.cs`**; `RepositorioConsultaSemestres.Listar()` hace un `SELECT` a tablas y un `UNION ALL` directamente.

## 3. ¿Para qué sirve cada archivo `.cs`?

| Archivo organizado | Función principal | Tablas mencionadas directamente en su SQL |
|---|---|---|
| `Formularios/Acceso/CambiarPasswordForm.cs` | Solicita nueva contraseña y actualiza el hash almacenado de Usuario. | `Usuario` |
| `Formularios/Acceso/ConfirmarPasswordForm.cs` | Diálogo auxiliar de confirmación para operaciones de contraseña. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Acceso/LoginForm.cs` | Interfaz para capturar usuario y contraseña; llama Auth.Login y abre el panel principal. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Administracion/BitacoraForm.cs` | Consulta bitácora con filtros y paginación. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Administracion/PersonalForm.cs` | Consulta y desactiva personal mediante repositorio. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Administracion/UsuariosForm.cs` | Captura creación de cuentas y personal según rol; prepara correo institucional. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/ConsultaA.cs` | Pantalla de consulta, búsqueda, filtrado por semestre, expediente, modificación, baja y exportación. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/ConsultaGruposForm.cs` | Muestra los grupos, cupos y acceso a la lista correspondiente. | `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Reinscripcion`, `Semestre` |
| `Formularios/Alumnos/ExpedienteMForm.cs` | Formulario para visualizar y modificar datos de expediente de alumnos existentes. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/HorarioAlumnoForm.cs` | Consulta bloques de horario por alumno y genera el PDF correspondiente. | `Grupo`, `Horario`, `Inscripcion`, `Periodo_Escolar` |
| `Formularios/Alumnos/InscripcionForm.cs` | Captura ficha, datos personales, domicilio, plan y grupo; envía datos a RepositorioAlumnos.EnrollAsync. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/InscripcionResultadoForm.cs` | Muestra el resultado de inscripción y permite abrir lista/constancia. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/ListaGrupoForm.cs` | Visualiza la lista de un grupo y permite exportarla. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/ModificarReinscripcionForm.cs` | Permite editar los datos de una reinscripción y cambiar su grupo. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Alumnos/ReinscripcionForm.cs` | Captura reinscripción autónoma y selección de semestre y grupo. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Docentes/CapturaCLForm.cs` | Pantalla de captura/edición de calificaciones y aplicación de controles de celda. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Docentes/DetalleCapturaForm.cs` | Diálogo de lectura de eventos/cambios de calificaciones. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Docentes/FiltrosDocente.cs` | Carga y coordina los filtros de periodo/materia/grupo de las pantallas docentes. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Docentes/HistorialdocForm.cs` | Filtros y tabla histórica de calificaciones; abre detalle de cambios y exporta PDF. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Docentes/HorarioGrupoForm.cs` | Consulta y presenta horarios de un grupo y permite exportarlos. | `Horario` |
| `Formularios/Docentes/MateriasDocenteForm.cs` | Vista de asignaciones, materias y grupos del docente. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Documentos/ConsultaDForm.cs` | Interfaz del módulo de documentos; el menú principal de este módulo permanece oculto. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Formularios/Principal/MainForm.cs` | Panel principal y menú por rol; abre formularios, consulta indicadores y muestra resumen docente. | `Alumno`, `Asignacion_Docente`, `Aspirante`, `Calificacion_Corte`, `Docente`, `Grupo`, `Inscripcion`, `Materia`, `Periodo_Escolar`, `Personal_Administrativo`, `Reinscripcion`, `Usuario` |
| `Interfaz/BotonRedondeado.cs` | Define botón personalizado de esquinas redondeadas. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Interfaz/ComponentesVisuales.cs` | Construye elementos reutilizables, encabezados, etiquetas y tablas. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Interfaz/NavegacionEnter.cs` | Instala navegación de controles al pulsar Enter. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Interfaz/PanelRedondeado.cs` | Define panel personalizado de esquinas redondeadas. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Interfaz/Theme.cs` | Paleta institucional y fábricas de controles comunes; contiene el tipo Choice. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Modelos/BloqueHorario.cs` | Representa un bloque de horario como tipo de datos. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Modelos/Contacto.cs` | Normaliza/verifica teléfono y correo electrónico. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Modelos/ModelosEstudiantes.cs` | Modelos StudentData y validaciones para capturar y transferir expedientes. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Nucleo/Datos.cs` | Db.Connect y Db.Sql crean conexiones/comandos; Auth.Login autentica y bloquea intentos; Sesion define roles; PasswordHasher usa PBKDF2. | `Bitacora`, `Rol`, `Usuario` |
| `Nucleo/Identidad.cs` | Comprueba usuario activo y controla duplicidades de identidad entre alumno, docente y administrativo dentro de transacciones. | `Alumno`, `Aspirante`, `Control_Identidad`, `Docente`, `Personal_Administrativo`, `Rol`, `Usuario` |
| `Nucleo/PeriodoActual.cs` | Consulta el Periodo_Escolar ACTIVO más reciente y conserva su nombre en caché. | `Periodo_Escolar` |
| `PDF/CalificacionesFinalesPdf.cs` | Crea documento PDF de calificaciones finales. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `PDF/ListaGrupoPdf.cs` | Crea un PDF de lista de grupo. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `PDF/PdfService.cs` | Compone constancia de registro/inscripción con QuestPDF. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `PDF/ReporteDocentePdf.cs` | Crea PDF del historial de calificaciones. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Program.cs` | Punto de entrada (Main); inicializa WinForms y QuestPDF y abre LoginForm. Incluye Problems.Show para errores. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Repositorios/RepositorioAlumnos.cs` | Consultas y operaciones transaccionales de inscripción, expediente, domicilio, ficha y visibilidad. | `Alumno`, `Aspirante`, `Carrera`, `Domicilio`, `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Plan_Estudio`, `Semestre` |
| `Repositorios/RepositorioBitacora.cs` | Consulta paginada de eventos con roles y personal. | `Bitacora`, `Docente`, `Personal_Administrativo`, `Rol`, `Usuario` |
| `Repositorios/RepositorioConsultaSemestres.cs` | Combina alumnos inscritos y reinscritos para consultar por semestre. | `Alumno`, `Carrera`, `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Plan_Estudio`, `Reinscripcion`, `Semestre` |
| `Repositorios/RepositorioCortesDocente.cs` | Valida asignaciones/semanas; lee, guarda y cierra calificaciones; lee cambios de Bitacora. | `Alumno`, `Asignacion_Docente`, `Bitacora`, `Calificacion_Corte`, `Docente`, `Grupo`, `Inscripcion`, `Materia`, `Periodo_Escolar`, `Reinscripcion`, `Usuario` |
| `Repositorios/RepositorioDocente.cs` | Consulta las asignaciones y horarios que corresponden al docente autenticado. | `Asignacion_Docente`, `Grupo`, `Horario`, `Materia`, `Periodo_Escolar`, `Usuario` |
| `Repositorios/RepositorioListas.cs` | Construye listas de grupos combinando alumnos y reinscritos. | `Alumno`, `Asignacion_Docente`, `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Reinscripcion`, `Usuario` |
| `Repositorios/RepositorioMantenimientoReinscripcion.cs` | Consulta, edita y da de baja registros de reinscripción. | `Alumno`, `Control_Identidad`, `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Reinscripcion`, `Semestre` |
| `Repositorios/RepositorioPersonal.cs` | Da de alta, lista y desactiva docentes, administrativos y cuentas de usuario. | `Docente`, `Personal_Administrativo`, `Rol`, `Usuario` |
| `Repositorios/RepositorioReinscripcion.cs` | Valida generación y semestre, obtiene grupos, genera correo y guarda reinscripción. | `Alumno`, `Control_Identidad`, `Docente`, `Grupo`, `Inscripcion`, `Periodo_Escolar`, `Personal_Administrativo`, `Reinscripcion`, `Semestre` |
| `Servicios/CatalogoPostal.cs` | Busca y valida colonias y domicilios por código postal desde Catalogo_Postal. | `Catalogo_Postal` |
| `Servicios/CorreoDominio.cs` | Valida correo con un `HashSet` local de dominios. | Sin consulta MySQL |
| `Servicios/PersonalAutomatico.cs` | Calcula correo disponible y número administrativo al crear personal. | `Alumno`, `Docente`, `Personal_Administrativo` |
| `Servicios/SeguridadSocial.cs` | Valida el NSS; ya no genera NSS ficticio en esta versión. | Ninguna (utiliza clases de servicio, modelos o interfaz) |
| `Servicios/Validation.cs` | Valida nombre, CURP, RFC, nacimiento, contraseña, etc. | Ninguna (utiliza clases de servicio, modelos o interfaz) |

### 3.1 Métodos localizables en Visual Studio

Para encontrar un método, abre el archivo `.cs`, pulsa `Ctrl+F` y busca el nombre; con `Ctrl+G` puedes ir a la línea indicada. Se listan declaraciones detectadas por patrones, por lo que métodos divididos en varias líneas o constructores pueden no figurar aquí.

**`Formularios/Acceso/CambiarPasswordForm.cs`:** `CambiarPasswordForm()` (L7), `CambiarPasswordForm()` (L8), `OnLoad()` (L9), `PasswordClosing()` (L15), `Save()` (L21).

**`Formularios/Acceso/ConfirmarPasswordForm.cs`:** `ConfirmarPasswordForm()` (L4).

**`Formularios/Acceso/LoginForm.cs`:** `LoginForm()` (L6), `RegisterClick()` (L8), `TryLogin()` (L11).

**`Formularios/Administracion/BitacoraForm.cs`:** `BitacoraForm()` (L6), `ClearQuery()` (L13), `BitacoraForm()` (L18), `OnShown()` (L23), `Consult()` (L29), `SearchClick()` (L45), `PreviousClick()` (L46), `NextClick()` (L47), `CloseClick()` (L48).

**`Formularios/Administracion/PersonalForm.cs`:** `PersonalForm()` (L6), `PersonalForm()` (L7), `OnLoad()` (L12), `SearchClick()` (L21), `CloseClick()` (L22), `Reload()` (L23), `DeactivateClick()` (L36).

**`Formularios/Administracion/UsuariosForm.cs`:** `UsuariosForm()` (L7), `UsuariosForm()` (L15), `OnLoad()` (L28), `ActionsResize()` (L45), `HeaderResize()` (L54), `RoleChanged()` (L59), `PreviewEmail()` (L65), `OnFormClosing()` (L72), `Save()` (L78).

**`Formularios/Alumnos/ConsultaA.cs`:** `ConsultaA()` (L12), `ConsultaA()` (L21), `OnLoad()` (L31), `PrepararSemestres()` (L39), `EsReinscripcionSeleccionada()` (L70), `VerReinscripcion()` (L73), `PrepararOrden()` (L88), `MenuButton_Click()` (L113), `RefreshClick()` (L114), `ViewClick()` (L115), `EditClick()` (L116), `PdfClick()` (L117), `HideClick()` (L118), `RestoreClick()` (L123), `GridDoubleClick()` (L124), `SelectedControl()` (L126), `LoadData()` (L133), `ActualizarAcciones()` (L147), `SelectedStudent()` (L165), `View()` (L169), `Edit()` (L176), `Pdf()` (L195), `DarDeBajaReinscripcion()` (L214), `ChangeVisibility()` (L309), `grid_CellContentClick()` (L328), `HorarioClick()` (L332).

**`Formularios/Alumnos/ConsultaGruposForm.cs`:** `ConsultaGruposForm()` (L13), `Cargar()` (L40), `AbrirLista()` (L76).

**`Formularios/Alumnos/ExpedienteMForm.cs`:** `ExpedienteMForm()` (L11), `CargarLogo()` (L20), `ExpedienteMForm()` (L33), `OnLoad()` (L66), `CancelClick()` (L74), `F()` (L76), `OnFormClosing()` (L79), `Save()` (L85), `NssClick()` (L122).

**`Formularios/Alumnos/HorarioAlumnoForm.cs`:** `HorarioAlumnoForm()` (L21), `HorarioAlumnoForm()` (L22), `CloseClick()` (L27), `CargarHorario()` (L28), `LlenarTablaHorario()` (L96), `DescargarPdf()` (L141).

**`Formularios/Alumnos/InscripcionForm.cs`:** `InscripcionForm()` (L11), `ConfigurarOrdenCaptura()` (L27), `AgregarBotonGrupos()` (L36), `ActivarEstiloPasos()` (L72), `Tabs_DrawItem()` (L80), `MenuButton_Click()` (L96), `InscripcionForm()` (L98), `OnLoad()` (L106), `FindClick()` (L121), `PlanChanged()` (L122), `CancelClick()` (L123), `LoadGroups()` (L125), `Lookup()` (L138), `PostalChanged()` (L165), `ColoniaChanged()` (L185), `Collect()` (L195), `OnFormClosing()` (L226), `Save()` (L232).

**`Formularios/Alumnos/InscripcionResultadoForm.cs`:** `InscripcionResultadoForm()` (L5), `InscripcionResultadoForm()` (L6), `ListClick()` (L8), `CloseClick()` (L10).

**`Formularios/Alumnos/ListaGrupoForm.cs`:** `ListaGrupoForm()` (L8), `ListaGrupoForm()` (L9), `OnLoad()` (L11), `RefreshList()` (L17), `RefreshClick()` (L31), `CloseClick()` (L32), `ExportClick()` (L33).

**`Formularios/Alumnos/ModificarReinscripcionForm.cs`:** `ModificarReinscripcionForm()` (L16), `ValidarGeneracionVisual()` (L105), `ActualizarGrupos()` (L130), `Guardar()` (L147).

**`Formularios/Alumnos/ReinscripcionForm.cs`:** `ReinscripcionForm()` (L22), `Boton()` (L131), `AgregarCampo()` (L140), `LimpiarCaptura()` (L150), `ValidarNumero()` (L162), `CambiarSemestre()` (L199), `RefrescarGrupos()` (L215), `VerHorario()` (L234), `Guardar()` (L241).

**`Formularios/Docentes/CapturaCLForm.cs`:** `CapturaCLForm()` (L16), `CapturaCLForm()` (L43), `OnLoad()` (L50), `AssignmentChanged()` (L65), `SelectionChanged()` (L98), `Discard()` (L107), `LoadClick()` (L111), `LoadRoster()` (L113), `PaintGradeField()` (L161), `ValidateGrade()` (L190), `ValueChanged()` (L201), `UpdateSummary()` (L206), `SaveClick()` (L217), `ImprimirFinalClick()` (L270), `CheckClosing()` (L289), `CloseClick()` (L295), `GridError()` (L296).

**`Formularios/Docentes/DetalleCapturaForm.cs`:** `DetalleCapturaForm()` (L5), `DetalleCapturaForm()` (L6), `CloseClick()` (L12).

**`Formularios/Docentes/FiltrosDocente.cs`:** `FiltrosDocente()` (L10), `Subjects()` (L28), `Groups()` (L36).

**`Formularios/Docentes/HistorialdocForm.cs`:** `HistorialdocForm()` (L11), `HistorialdocForm()` (L12), `OnLoad()` (L13), `Clear()` (L20), `AssignmentChanged()` (L21), `UnitChanged()` (L27), `SearchClick()` (L28), `DetailClick()` (L41), `ExportClick()` (L46), `CloseClick()` (L58).

**`Formularios/Docentes/HorarioGrupoForm.cs`:** `HorarioGrupoForm()` (L19), `Cargar()` (L120), `DescargarPdf()` (L179), `NormalizarDia()` (L280).

**`Formularios/Docentes/MateriasDocenteForm.cs`:** `MateriasDocenteForm()` (L5), `MateriasDocenteForm()` (L6), `OnLoad()` (L7), `SelectionChanged()` (L19), `CaptureClick()` (L30), `ListClick()` (L31), `CloseClick()` (L36).

**`Formularios/Documentos/ConsultaDForm.cs`:** `ConsultaDForm()` (L7), `ConsultaDForm()` (L8), `OnLoad()` (L16), `ExportClick()` (L23), `CloseClick()` (L32), `CardSizeChanged()` (L33).

**`Formularios/Principal/MainForm.cs`:** `MainForm()` (L8), `MainForm()` (L13), `OnLoad()` (L35), `ToggleMenu()` (L53), `LogoutClick()` (L63), `SidebarPaint()` (L68), `SelectMenu()` (L81), `Open()` (L107), `Placeholder()` (L115), `HomeClick()` (L125), `EnrollClick()` (L131), `RenewClick()` (L137), `ReportsClick()` (L146), `DocumentsClick()` (L152), `StudentsClick()` (L158), `SendClick()` (L164), `UsersClick()` (L170), `PersonnelClick()` (L176), `AuditClick()` (L182), `PasswordClick()` (L188), `Home()` (L194), `LoadProfile()` (L288), `SetupTeacherInterface()` (L333), `AddTeacherOption()` (L352), `LoadTeacherDashboard()` (L406).

**`Interfaz/BotonRedondeado.cs`:** `OnSizeChanged()` (L9), `OnHandleCreated()` (L10), `UpdateShape()` (L11).

**`Interfaz/ComponentesVisuales.cs`:** `Path()` (L7), `Round()` (L15), `Label()` (L25), `Input()` (L32), `Grid()` (L38), `Stat()` (L77).

**`Interfaz/NavegacionEnter.cs`:** `Instalar()` (L6).

**`Interfaz/PanelRedondeado.cs`:** `PanelRedondeado()` (L35), `OnSizeChanged()` (L41), `OnHandleCreated()` (L47), `UpdateShape()` (L53), `OnPaint()` (L68).

**`Interfaz/Theme.cs`:** `Setup()` (L14), `Text()` (L24), `Button()` (L28), `Input()` (L34), `Combo()` (L38), `Fields()` (L42), `Logo()` (L57), `Heading()` (L66), `Message()` (L70), `Choice()` (L75), `ToString()` (L77).

**`Modelos/BloqueHorario.cs`:** `BloqueHorario()` (L3).

**`Modelos/Contacto.cs`:** `TryPhone()` (L11), `Phone()` (L32), `TryEmail()` (L38), `Email()` (L55).

**`Modelos/ModelosEstudiantes.cs`:** `Validate()` (L37), `AspirantInfo()` (L62).

**`Nucleo/Datos.cs`:** `Connect()` (L10), `Sql()` (L24), `Param()` (L27), `Audit()` (L30), `Sesion()` (L44), `Login()` (L58), `RegistrarFallo()` (L102), `Hash()` (L125), `Verify()` (L132).

**`Nucleo/Identidad.cs`:** `RequireActive()` (L7), `Check()` (L24).

**`Nucleo/PeriodoActual.cs`:** `Reiniciar()` (L12), `Consultar()` (L14).

**`PDF/CalificacionesFinalesPdf.cs`:** `Create()` (L11).

**`PDF/ListaGrupoPdf.cs`:** `Create()` (L7).

**`PDF/PdfService.cs`:** `Create()` (L10), `Section()` (L74).

**`PDF/ReporteDocentePdf.cs`:** `Create()` (L8).

**`Program.cs`:** `Show()` (L19).

**`Repositorios/RepositorioAlumnos.cs`:** `CheckNss()` (L8), `Plans()` (L17), `Groups()` (L32), `Aspirant()` (L57), `Eligible()` (L70), `EnrollAsync()` (L78), `ListStudents()` (L166), `Detail()` (L194), `StudentParams()` (L232), `AddressParams()` (L245), `Update()` (L254), `UpdateAsync()` (L255), `Visibility()` (L299).

**`Repositorios/RepositorioBitacora.cs`:** `Query()` (L8), `Readable()` (L50).

**`Repositorios/RepositorioConsultaSemestres.cs`:** `Listar()` (L8), `Reinscrito()` (L65).

**`Repositorios/RepositorioCortesDocente.cs`:** `Autorizar()` (L12), `Periodo()` (L35), `Calendario()` (L50), `Lista()` (L60), `Obtener()` (L88), `Cerrada()` (L96), `EstaCerrada()` (L103), `Estado()` (L110), `Finales()` (L129), `Historial()` (L171), `Cambios()` (L196), `Guardar()` (L220).

**`Repositorios/RepositorioDocente.cs`:** `Asignacion()` (L6), `ToString()` (L8), `NotaCambio()` (L10), `Teacher()` (L14), `Assignments()` (L19), `Own()` (L38), `Schedule()` (L51), `Table()` (L59).

**`Repositorios/RepositorioListas.cs`:** `ListaGrupo()` (L4), `Obtener()` (L7).

**`Repositorios/RepositorioMantenimientoReinscripcion.cs`:** `ReinscritoEditable()` (L8), `Obtener()` (L13), `Grupos()` (L32), `Modificar()` (L67), `Baja()` (L190).

**`Repositorios/RepositorioPersonal.cs`:** `CreateAsync()` (L7), `List()` (L116), `Deactivate()` (L143), `PersonalData()` (L173), `PersonalCreated()` (L177).

**`Repositorios/RepositorioReinscripcion.cs`:** `ReinscripcionBasica()` (L8), `Inferir()` (L15), `ValidarSemestre()` (L28), `VerificarNoPrimerSemestre()` (L36), `VerificarNoPrimerSemestre()` (L43), `Existente()` (L60), `Grupos()` (L77), `GenerarCorreo()` (L102), `Guardar()` (L126), `Lista()` (L205).

**`Servicios/CatalogoPostal.cs`:** `RegistroPostal()` (L3), `Buscar()` (L7), `Validar()` (L24).

**`Servicios/CorreoDominio.cs`:** `RequireAsync()`.

**`Servicios/PersonalAutomatico.cs`:** `EmailBase()` (L9), `AvailableEmail()` (L19), `NextNumber()` (L37).

**`Servicios/SeguridadSocial.cs`:** `Validate()` (L6), `Display()` (L22).

**`Servicios/Validation.cs`:** `Mandatory()` (L10), `Name()` (L15), `Username()` (L23), `Email()` (L24), `InstitutionalEmail()` (L25), `Phone()` (L27), `Password()` (L28), `Postal()` (L29), `BirthCode()` (L30), `Rfc()` (L38), `Curp()` (L43), `CurpMatchesBirth()` (L49), `CurpIdentity()` (L54), `CurpWord()` (L80).

## 4. Recorridos completos para exponer el proyecto

### Inicio de sesión

1. `Program.Main()` ejecuta `new LoginForm()`. 2. `LoginForm.TryLogin` llama `Auth.Login`. 3. `Auth.Login` lee `Usuario` y `Rol`, verifica hash con `PasswordHasher`, comprueba bloqueo de 5 intentos/15 minutos y actualiza `Usuario`. 4. `Identidad.RequireActive` confirma la cuenta activa, usando también `Docente` o `Personal_Administrativo` si corresponde. 5. `Db.Audit` registra en `Bitacora`. 6. Se abre `MainForm` y `Sesion` limita el menú.

### Inscripción de primer semestre

1. `MainForm` abre `InscripcionForm`. 2. El formulario busca ficha (`RepositorioAlumnos.Aspirant`, tabla `Aspirante`), carga planes (`Plan_Estudio`/`Carrera`) y grupos disponibles (`Grupo`, `Inscripcion`, `Semestre`, `Periodo_Escolar`). 3. `InscripcionForm.Collect` prepara `StudentData`, validado por `Validation`, `Contacto`, `CatalogoPostal`, `SeguridadSocial` y `CorreoDominio`. 4. `RepositorioAlumnos.EnrollAsync` usa transacción para insertar `Alumno`, `Domicilio`, `Inscripcion` y actualizar `Aspirante` (y auditar la operación). 5. Se muestra `InscripcionResultadoForm`, se puede generar comprobante con `PdfService`.

### Reinscripción de 2.º o 3.º semestre

1. `ReinscripcionForm` captura control, datos y grupo. 2. `RepositorioReinscripcion.Inferir`/`ValidarSemestre` comprueban las reglas del número de control y generación. 3. Se consulta si existe inscripción previa o reinscripción en `Alumno`, `Inscripcion` y `Reinscripcion`; se comprueba grupo (`Grupo`, `Semestre`, `Periodo_Escolar`). 4. `Guardar` inserta en `Reinscripcion` y registra el evento. 5. `RepositorioMantenimientoReinscripcion.Modificar` y `Baja` actualizan el registro cuando corresponde; la consulta combinada está en `RepositorioConsultaSemestres`.

### Calificaciones e historial docente

1. `MateriasDocenteForm`/`FiltrosDocente` obtienen asignaciones con `RepositorioDocente` (`Usuario`, `Docente`, `Asignacion_Docente`, `Materia`, `Grupo`, `Periodo_Escolar`). 2. `CapturaCLForm` solicita lista a `RepositorioCortesDocente.Obtener` (`Inscripcion`/`Alumno` y `Reinscripcion`, unidos por grupo). 3. `Guardar` valida que sean enteras, respeta orden 5→9→13→17, transforma las notas menores de 70 a 0, inserta/actualiza `Calificacion_Corte` y audita en `Bitacora`. 4. La semana 17 cierra la asignación. 5. `HistorialdocForm` usa `Historial` y `Cambios`; este último consulta `Bitacora` con un filtro SQL y corresponde al botón «Ver cambios». 6. `ReporteDocentePdf` y `CalificacionesFinalesPdf` convierten los resultados a PDF.

### Personal y bitácora

1. `UsuariosForm` prepara datos y utiliza `RepositorioPersonal.CreateAsync`. 2. Según el rol, este crea `Docente` o `Personal_Administrativo` y luego `Usuario` con su `Rol`. `PersonalAutomatico` determina correo/número disponibles; `Identidad` comprueba duplicados. 3. `PersonalForm` lista o desactiva cuentas. 4. `BitacoraForm` usa `RepositorioBitacora.Query` para buscar eventos en `Bitacora`, relacionándolos con `Usuario`, `Rol` y personal.

## 5. Ejemplos reales de la relación código ↔ tabla

```csharp
// Nucleo/PeriodoActual.cs: recupera el periodo vigente
SELECT nombre_periodo FROM Periodo_Escolar
WHERE estatus='ACTIVO' ORDER BY fecha_inicio DESC LIMIT 1

// Servicios/CatalogoPostal.cs: busca colonias para un CP
SELECT colonia, localidad, municipio, estado
FROM Catalogo_Postal WHERE codigo_postal=@cp ORDER BY colonia

// Repositorios/RepositorioCortesDocente.cs: guarda nota
INSERT INTO Calificacion_Corte
(numero_control,Id_asignacion,semana,calificacion)
VALUES (@control,@asignacion,@semana,@nota)
ON DUPLICATE KEY UPDATE calificacion=VALUES(calificacion)
```

La variable `@cp`, igual que `@control` y `@nota`, representa un parámetro SQL. Se asigna usando `Db.Param` para evitar concatenar directamente los datos introducidos por el usuario dentro de la consulta.

## 6. Scripts de MySQL y su orden

- **`Sql/01_esquema.sql`**: Crea el esquema principal y 17 tablas de control escolar.
- **`Sql/02_configuracion.sql`**: Realiza ajustes de columnas, catálogo postal, dominios, roles, usuario de arranque y controles auxiliares.
- **`Sql/03_grupos_fichas.sql`**: Carga carrera, grupos y fichas de aspirantes.
- **`Sql/04_reinscripcion.sql`**: Crea `Reinscripcion` y configura los grupos de segundo semestre.
- **`Sql/05_horarios.sql`**: Carga referencias y bloques de horario.
- **`Sql/06_docentes.sql`**: Carga los docentes, cuentas y asignaciones iniciales.
- **`Sql/07_vistas.sql`**: Crea tres vistas por semestre.
- **`Sql/08_calificaciones.sql`**: Crea `Calificacion_Corte` e instala triggers que restringen cambios después del cierre.
- **`Sql/09_catalogo_postal_datos_SEPOMEX.sql`**: Carga registros postales para `Catalogo_Postal`.
- **`Sql/10_actualizar_base_existente.sql`**: Migración para una base previa: columnas de bloqueo y eliminación de tablas obsoletas. No forma parte de la instalación desde cero.

## 7. Hallazgos a revisar antes de presentar

1. **El script `Sql/10_actualizar_base_existente.sql` declara ser reejecutable, pero incluye un `SELECT ... FROM Documento UNION ALL ... FROM Envio_Documento ... FROM Plan_Materia` sin verificar si esas tablas existen.** Si ya se eliminaron, esa consulta falla. No lo ejecutes de nuevo sobre una base con datos sin revisar y respaldar. Esto no se modificó porque la tarea es organizar, no migrar datos.

2. **`Sql/07_vistas.sql` tiene un comentario antiguo que dice «Requiere Sql/16 y Sql/18».** En esta versión la instalación documentada usa `01`–`08`; el comentario debe actualizarse para evitar confusión, pero las vistas sí están definidas.

3. **`RepositorioCortesDocente.Cambios()` es la consulta del botón «Ver cambios».** Si persiste el error de collation observado en otra versión, ocultar el botón no corrige esa SQL; comprueba funcionamiento en tu base real.

4. El código de esta edición sigue conservando clases y archivos de interfaz para Documentos, aunque el menú esté oculto; no equivale a tener ese módulo implementado.

5. Las clases de WinForms conservan el namespace `AMBAR.Semana4` aunque estén en subcarpetas. **No cambies los namespaces** por la ubicación del archivo sin revisar todas las referencias y los recursos del diseñador.

6. La reorganización no ejecutó MySQL, no cargó las fichas ni verificó operaciones reales. Verifica el proyecto desde Visual Studio con la BD respaldada antes de sustituir tu versión funcional.

## 8. Recomendaciones para exponer

Puedes explicar **un módulo completo**, enseñando: (a) evento de botón en `.cs`, (b) método del repositorio, (c) SQL y tablas afectadas, (d) validación o transacción, (e) resultado en pantalla o PDF. Después demuestra permisos por rol y describe qué funciones están pendientes.

> Nota: este inventario registra referencias SQL identificables textualmente en el código. Para una auditoría exhaustiva de dependencias indirectas y comportamiento dinámico, compila y prueba la solución.
