# Actualización de validaciones, personal y grupos

Base: rama `actualizacion-zip-octubre-2026`, commit `104ff4ec31656cf3773c62432566966293a6e57c`.

## Aplicar a una instalación existente

1. Actualiza/descarga **esta rama** y cierra AMBAR.
2. En MySQL Workbench, sobre `AMBAR_Control_Escolar`, ejecuta `Sql/05_rfc_secretarias.sql` si aún no lo habías aplicado.
3. Ejecuta completo `Sql/06_validaciones_y_personal.sql`.
4. Ejecuta completo `Sql/07_grupos_y_fichas_2026.sql`.
5. Abre la solución en Visual Studio 2022 y recompila. Conserva tu conexión local en `appsettings.json`.

No vuelvas a ejecutar `01_tablas_ambar_original.sql` sobre una base existente. Para una base nueva, el orden es **01 → 02 → 05 → 06 → 07**. El script 03 sólo contiene datos antiguos de demostración y no hace falta para esta carga.

Los scripts 06 y 07 se pueden repetir. No borran alumnos ni cuentas, no duplican la carga y no reactivan fichas inscritas. El 07 detecta grupos/periodos ambiguos o grupos con más de 25 inscritos y revierte su carga. Los registros que ya estuvieran inactivos conservan ese estado.

La carga se verificó en una base MySQL temporal. **No se ha ejecutado en el MySQL de tu computadora**; los pasos anteriores son necesarios para aplicar los datos allí.

## Cambios de uso

- En Administración aparece **Consulta / baja de personal**. Permite buscar por nombre completo, usuario, CURP o RFC y filtrar por rol y estado. La baja desactiva al empleado y a su cuenta en una transacción, conserva el historial y registra el evento en bitácora. No permite desactivar administradores ni la propia cuenta.
- El alta de docentes y secretarias pide CURP. Los registros anteriores conservan CURP nula si nunca se capturó; no se les inventa una.
- Nombres y apellidos admiten letras, espacios internos, guiones y apóstrofos entre palabras. Al guardar se normalizan espacios y capitalización: `JOHny` → `Johny`.
- Se rechazan **nombres completos** y CURP duplicados entre alumnos, docentes y secretarias, incluso en bajas; se puede compartir un nombre de pila o apellido. También se comprueba el RFC entre los dos tipos de personal. Las fichas no pueden compartir CURP con otra ficha. El bloqueo transaccional evita que dos altas simultáneas eludan la comprobación. No se fusionan ni corrigen automáticamente personas históricas.
- Usuarios nuevos: 4–50 caracteres, inicio con letra, letras/números y separadores `.`, `_`, `-` internos no consecutivos. Los usuarios existentes conservan su acceso.
- Correo institucional: formato válido y dominio exacto `acapulco.tecnm.mx` o `alumnos.acapulco.tecnm.mx`, coherentes con los correos usados por el proyecto. El correo personal puede usar otros dominios. No se comprueba la existencia del buzón.
- Teléfonos: 10 dígitos, inicio de 2 a 9; se rechazan letras, longitudes incorrectas y números con todos los dígitos iguales. El teléfono personal conserva su carácter opcional; el de emergencia del alumno es obligatorio.
- RFC de persona física: 13 caracteres, fecha posible y estructura de homoclave. La fecha debe coincidir con la CURP del empleado. CURP: formato, entidad y fecha posibles; en alumnos también debe coincidir con su nacimiento. Estas comprobaciones son locales, **no certifican registro ante SAT/RENAPO ni verifican sus dígitos de control**.
- Nacimiento del alumno: desde **01/01/1926**, inclusive, hasta ayer. Se aplica tanto al alta como a la modificación y al mínimo del selector visual.

Las reglas están en `Validation.cs`, las altas y bajas en `RepositorioPersonal.cs` y las comprobaciones de identidad/acceso en `Identidad.cs`. Se aplican al guardar en MySQL desde AMBAR; las escrituras SQL externas no pasan por estas validaciones. Los formularios conservan sus archivos `.Designer.cs`; la pantalla nueva también es editable con el diseñador. Su apertura visual en Visual Studio requiere comprobarse en Windows.

## Grupos y fichas

Datos transcritos de las tres imágenes de horarios de Ingeniería en Sistemas Computacionales, periodo **Agosto-Diciembre 2026**:

| Semestre | Matutino | Vespertino | Capacidad por grupo |
|---|---|---|---|
| 1 | 1ISC1 | 1ISC2 | 25 |
| 3 | 3ISC1 | 3ISC2 | 25 |
| 5 | 5ISC1 | 5ISC2 | 25 |

Los **160 bloques semanales** de las 36 filas de asignaturas seleccionadas se guardan en `Horario_Referencia`, incluyendo día, hora, aula y nombre del profesor. «Por asignar» se representa con profesor nulo. Esta tabla conserva la fuente; no sustituye las asignaciones operativas de `Asignacion_Docente`/`Horario`, que requieren docentes registrados y materias con sus claves y créditos oficiales. No se inventaron esos datos ausentes en las imágenes.

El script reutiliza la carrera `ISC` y su plan activo. Si no existe un plan activo, crea uno auxiliar con versión `AMBAR-2026`, que no pretende ser una clave oficial. Las fechas 01/08/2026–31/12/2026 abarcan los meses del encabezado; no son las fechas oficiales de inicio y fin de clases, que no aparecen en las imágenes.

Fichas disponibles: **`ISC26-001` a `ISC26-050`**. Son 50 folios habilitados para inscripción (`APROBADO`/`ACTIVO`), sin CURP, nombre de preparatoria ni promedio ficticios. No equivalen a resultados de examen reales. Al consultar uno en Inscripción, captura los datos del alumno y su CURP; el formulario habilita su edición para las fichas vacías. Al guardar, vincula la CURP y marca la ficha como `INSCRITO`.

Estas fichas sólo permiten los grupos habilitados de primer semestre del periodo correspondiente. Puedes distribuirlas entre `1ISC1` y `1ISC2`; el sistema rechaza al alumno 26 de cualquiera de ellos. Los grupos de tercero y quinto quedan cargados, sin fichas de nuevo ingreso.

## Verificación realizada

- Compilación Release de la aplicación .NET 8 para Windows, sin errores; permanecen advertencias previas del diseñador.
- 44 comprobaciones de validación y 30 de integración: **74 verificaciones satisfactorias**.
- Integración en MySQL **8.4.7**: creación de ambos tipos de personal, normalización persistida, duplicados entre módulos y altas simultáneas, consulta por filtros, bajas, bloqueo de login y de sesiones de secretarias ya abiertas.
- Inscripción de 50 alumnos de prueba, 25 por grupo, rechazo del exceso de cupo, rechazo de ficha reutilizada y conservación de ficha ante una operación fallida.
- Migraciones repetidas y carga repetida después de usar las 50 fichas: conserva las inscripciones y los 160 horarios.

Los datos de prueba sólo se crearon en una base temporal. No forman parte de la carga 07. Consulta `Tests/README.md` para repetir las pruebas.
