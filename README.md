# AMBAR · Sistema de Control Escolar

Aplicación de escritorio **C# / Windows Forms (.NET 8)** y **MySQL** para la carrera de Ingeniería en Sistemas Computacionales. Proyecto académico, periodo **Agosto–Diciembre 2026**.

## Módulos vigentes

- **Administrador:** control de usuarios (docentes y secretarías), consulta y modificación de alumnos, bitácora e indicadores.
- **Secretaría:** inscripción de primer semestre con expediente completo, ficha y domicilio validado por catálogo postal; reinscripción independiente de 2.º y 3.º semestre; consulta, modificación y baja lógica.
- **Docente:** grupos y materias asignadas; horarios, listas digitales y PDF; calificaciones secuenciales de **semanas 5, 9, 13 y 17**. La semana 17 se captura manualmente y, al guardar el grupo completo, se **cierran los cuatro cortes**; se genera una lista final imprimible en PDF.

Se mantienen solo **semestres 1.º, 2.º y 3.º**, con **dos grupos de 25 plazas por semestre** y **14 docentes**. La generación 2026 solo se reinscribe a 2.º; la 2025 a 2.º (repetidor) o 3.º. Los registros de Reinscripción son mínimos e independientes de la tabla Alumno.

> Los menús de **Reportes**, **Documentos** y **Enviar documentos** están ocultos hasta que se implementen. Sus tablas se eliminaron del esquema y se crearán junto con cada módulo.

## Requisitos

- Windows 10/11 con Visual Studio 2022 actualizado y el workload «Desarrollo de escritorio con .NET».
- .NET SDK 8, MySQL 8 o superior y MySQL Workbench.
- Restauración de paquetes NuGet (incluidos MySqlConnector y QuestPDF).

## Instalación de la base de datos desde cero

**Antes de recrear la base, exporta un respaldo de los datos actuales.** Ejecuta los **8 scripts de estructura y configuración** de \`Sql/\` en orden. Después importa el noveno archivo de datos SEPOMEX:

| Paso | Script | Contenido |
|---|---|---|
| 1 | \`01_esquema.sql\` | Base de datos, claves y tablas vigentes |
| 2 | \`02_configuracion.sql\` | Roles, usuario admin, validaciones, correo y tablas de catálogo |
| 3 | \`03_grupos_fichas.sql\` | Carrera, primero/tercero y 50 fichas de ingreso |
| 4 | \`04_reinscripcion.sql\` | Tabla Reinscripcion y grupos de segundo |
| 5 | \`05_horarios.sql\` | Horarios definitivos de los seis grupos; valida 14 docentes sin empalmes |
| 6 | \`06_docentes.sql\` | Alta de los 14 docentes, sus cuentas, materias y asignaciones |
| 7 | \`07_vistas.sql\` | Vistas de consulta para 1.º, 2.º y 3.º |
| 8 | \`08_calificaciones.sql\` | Tabla Calificacion_Corte y protección de cierre final |

**Modelo relacional simplificado:** 21 tablas físicas (dos menos que la versión anterior), horario único `Horario`, FK de domicilio a catálogo postal, catálogo de dominios en `CorreoDominio.cs` y vista `vw_AMBAR_Alumnos_Unificados`. Ver `Docs/REFORMA_MODELO.md`.

**Catálogo postal incluido:** \`Sql/09_catalogo_postal_datos_SEPOMEX.sql\` contiene los **158,370 registros** adjuntados (11.7 MB). Ejecútalo después del paso 2 (o después del paso 8), usando MySQL Workbench. Puede volver a ejecutarse para incorporar cambios porque utiliza \`ON DUPLICATE KEY UPDATE\`; no crea ni elimina tablas. Ya no necesitas descargar el TXT SEPOMEX para instalar esta versión.

No ejecutar sobre una instalación vieja sin migración o respaldo: estos archivos están diseñados para instalar la versión final **desde cero**, no para alterar arbitrariamente una base existente.

**Base existente:** tras respaldarla, revisar y ejecutar `Sql/11_migracion_simplificacion.sql` **solo sobre un clon de la instalación anterior**. El SQL 11 no es reejecutable y conserva temporalmente `Horario_Antes_11` para verificar que no se perdieron horarios. Reejecutar `Sql/07_vistas.sql` para crear la vista unificada. No ejecutar `Sql/10_actualizar_base_existente.sql` sin revisar primero el contenido de las tablas antiguas de documentos; ese script pertenece a la etapa anterior.

## Actualizar una base que ya está instalada

Si tu base se instaló con una versión anterior, **respáldala** y ejecuta `Sql/10_actualizar_base_existente.sql` antes de abrir la nueva versión de la aplicación (agrega las columnas del bloqueo de inicio de sesión y elimina tablas sin uso). No es parte de la instalación desde cero.

## Conexión y ejecución

El repositorio público **no incluye contraseñas**. Copia \`appsettings.example.json\` como \`appsettings.json\` en la raíz del proyecto y completa tu usuario, contraseña y conexión MySQL privados. El archivo local se ignora por Git.

En PowerShell:

\`\`\`powershell
Copy-Item appsettings.example.json appsettings.json
\`\`\`

Después abre \`AMBAR.Semana4.sln\` en Visual Studio, restaura paquetes y recompila. El usuario administrador inicial se crea en el script 02 con una contraseña temporal establecida por el proyecto; cámbiala en el primer inicio. Nunca subas ese secreto a GitHub. Si utilizaste contraseñas antiguas compartidas en commits anteriores, **cámbialas también en MySQL**.

El archivo \`Sql/09_catalogo_postal_datos_SEPOMEX.sql\` ya está incluido en GitHub. Si en el futuro necesitas generar un catálogo nuevo desde un TXT oficial, puedes usar el script auxiliar:

\`\`\`powershell
python Tools/importar_sepomex.py CPdescarga.txt catalogo_postal_datos.sql
\`\`\`

## Verificaciones después de instalar

\`\`\`sql
USE AMBAR_Control_Escolar;
SELECT COUNT(*) AS docentes FROM Docente; -- 14
SELECT COUNT(*) AS grupos FROM Grupo; -- 6
SELECT COUNT(*) AS bloques FROM Horario; -- 170
SELECT COUNT(*) AS fichas FROM Aspirante WHERE numero_ficha LIKE 'ISC26-%'; -- 50
SELECT COUNT(*) AS reinscripciones FROM Reinscripcion; -- 0 inicialmente
SELECT COUNT(*) AS calificaciones FROM Calificacion_Corte; -- 0 inicialmente
\`\`\`

## Estructura

- \`*.cs\`, \`*.Designer.cs\`, \`*.resx\`: aplicación y formularios de WinForms.
- \`Assets/\`, \`Properties/\`: recursos visuales y compilados.
- \`Sql/\`: **8 scripts de estructura** y **1 script de datos SEPOMEX**, sin respaldos ni migraciones históricas.
- \`Tools/importar_sepomex.py\`: utilidad opcional para reconstruir el catálogo desde un TXT SEPOMEX actualizado.
- \`Docs/\`: arquitectura de datos, alcance y decisiones de limpieza.
- \`.github/workflows/\`: validación de compilación .NET.

Los archivos históricos fueron eliminados de la rama limpia; siguen disponibles en el historial de Git, no en la versión distribuida.

## Estado de validación

Las modificaciones se revisan mediante un workflow de compilación; para comprobar el comportamiento de WinForms y los datos debes ejecutar la aplicación en Windows con MySQL. **No asumir que Reportes y envío de documentos están implementados** por el solo hecho de tener tablas.


## Organización por carpetas de esta entrega

Los archivos C# se agrupan en `Nucleo/`, `Formularios/`, `Repositorios/`, `Modelos/`, `Servicios/`, `Interfaz/` y `PDF/`. Se mantuvieron los namespaces, el contenido de las clases, los scripts `Sql/`, `Assets/` y `Properties/`. Las rutas de `.Designer.cs` y `.resx` están actualizadas en el proyecto.

**Guía para exponer:** [`Docs/GUIA_CODIGO_Y_TABLAS_MYSQL.md`](Docs/GUIA_CODIGO_Y_TABLAS_MYSQL.md) contiene la explicación de los 53 archivos de código, 23 tablas y ejemplos de flujos funcionales.
