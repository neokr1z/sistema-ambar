# Panel principal, personal, bitácora y docencia

Cambios sobre `main`, base `3e685c0a75dd04a8b980a1a76f2924d44f4ecc6a` (V5).

## Panel y personal

- El perfil muestra nombre(s), apellido paterno y apellido materno, tanto de Secretaría como de Docencia. Para el administrador sin registro de empleado conserva el nombre de usuario.
- Se retiró la tabla de actividad reciente del inicio, su consulta SQL y el texto «Indicadores acumulados · Últimas 10 actividades».
- El botón ☰ sólo aparece en el panel principal. Contrae la barra de opciones, libera espacio y permanece visible para volver a abrirla.
- Alta de personal genera el correo al escribir el nombre y apellidos: `Michelle Pereda Perez` → `MichellePP@acapulco.tecnm.mx`. Nombres compuestos se unen sin espacios, con sus iniciales originales; se retiran acentos y separadores. Ejemplo: `José Luis Pérez García` → `JoseLuisPG@acapulco.tecnm.mx`.
- Al guardar, el backend vuelve a generar el correo y comprueba disponibilidad en docentes, secretarias y alumnos. Si existe, agrega `2`, `3`, etc., sin sobrepasar la longitud del correo. Usa el bloqueo transaccional de identidades para evitar colisiones entre altas simultáneas.
- Secretaría recibe un número `ADM000001`, `ADM000002`, etc., generado al guardar. En Docencia se muestra `DOC` más su identificador autogenerado de la tabla Docente. El número final y el correo se muestran al confirmar el alta. Los campos ya no se capturan manualmente.
- No cambia correos ni números de empleados existentes, ni crea buzones reales de correo.

## Bitácora

Sólo Administración puede abrirla. Al entrar, consultar nuevos filtros o pasar de página, solicita la contraseña actual del mismo administrador. El backend comprueba rol, estado de la cuenta y hash de contraseña antes de devolver datos. No conserva la contraseña para posteriores consultas.

Permite filtrar por fechas y buscar usuario, actividad o descripción. Muestra 100 actividades por página, fechas legibles, nombre completo, usuario, rol, actividad, módulo y descripción; las actividades conocidas tienen etiquetas como «Alta de personal» o «Captura de calificaciones». La tabla usa encabezados institucionales, filas alternadas y descripciones con ajuste de línea.

Cancelar o fallar la autenticación deja la tabla vacía. Cambiar filtros también retira el resultado anterior. Las consultas y los intentos fallidos quedan registrados sin guardar contraseñas.

## Módulos docentes

- **Materias y grupos:** listado de asignaciones propias por materia/grupo/periodo, alumnos inscritos y horario operativo. Incluye acceso a captura para la asignación seleccionada.
- **Captura de calificaciones:** filtros por periodo escolar, materia, grupo, unidad y ventana de captura. Unidades dinámicas: admite las que estén configuradas, no sólo cuatro. Carga alumnos reales y notas existentes; guarda altas y correcciones en MySQL.
- Las notas admiten 0–100 y hasta dos decimales. Las celdas vacías nuevas no crean calificaciones; no se permite borrar una nota existente dejándola vacía. El guardado confirma pertenencia de la asignación, inscripción del alumno, unidad de la materia y periodo abierto según la fecha del servidor MySQL.
- Cada lote es transaccional: si un alumno ya no pertenece al grupo, cambió una nota desde que se cargó o se cerró la captura, se rechaza todo el lote. Las correcciones generan entradas en Registro_Captura y un evento en Bitacora.
- **Historial:** consulta de notas por periodo/materia/grupo/unidad, promedio del filtro y número de notas ≥70 como indicador. Ese conteo no certifica aprobación final de la materia. «Ver cambios» conserva cada valor reportado; «Exportar PDF» genera el listado seleccionado. La exportación vuelve a validar acceso y consulta datos vigentes.
- Se comprueba la cuenta activa y la propiedad de la asignación en cada operación; un docente dado de baja no puede seguir consultando o capturando desde una sesión abierta.

## Base de datos necesaria

No se requiere una nueva migración estructural para estos cambios. Se mantienen los scripts 01–08 y los datos existentes. **Los docentes necesitan asignaciones operativas reales**:

1. Materia y Plan_Materia con el catálogo académico oficial.
2. Asignacion_Docente vinculando la cuenta del maestro con materia y grupo.
3. Unidad_Evaluacion para cada unidad de la materia.
4. Periodo_Captura del periodo escolar, con estado ACTIVO o ABIERTO y fechas vigentes.
5. Horario con el Id_asignacion correspondiente, para mostrar sus bloques.

`Sql/09_guia_asignaciones_docentes.sql` ofrece consultas de diagnóstico y plantillas comentadas para la configuración en Workbench. El script 07 carga grupos y Horario_Referencia, que conserva las imágenes; esa referencia no crea materias con créditos inventados ni vincula automáticamente cuentas de maestros. Cuando falta configuración, las pantallas muestran el estado vacío o sólo lectura correspondiente.

## Verificación

- Código de aplicación y pruebas compilado con Roslyn de .NET SDK 8.0.426 y referencias Windows Forms/.NET 8, sin errores de C#. La ejecución de MSBuild del entorno falló al consultar información de sus procesos; se comprobó el código con el compilador directamente.
- **101 verificaciones satisfactorias**: reglas de datos, inscripción, altas/bajas, generación de correo/número, colisiones, aislamiento por docente, captura y correcciones, conflictos, rollback, ventanas cerradas/vencidas y autenticación de bitácora.
- Pruebas de integración en MySQL 8.4.7, sobre una base temporal con datos ficticios exclusivos de pruebas. No se modificó el MySQL de la computadora del usuario.
- Los formularios mantienen `.Designer.cs` y `.resx`, incluidos los nuevos. La compilación completa con recursos, apertura visual del diseñador y ejecución gráfica requieren validación en Visual Studio/Windows; no se ejecutó la interfaz Windows en este entorno.

Para repetir las pruebas del backend: `dotnet run --project Tests/Backend.Tests.csproj --configuration Release`. La integración requiere la conexión a una base temporal terminada en `_test`, como se explica en `Tests/README.md`.
