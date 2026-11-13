# AMBAR · Sistema de Control Escolar

Aplicación de escritorio **C# / Windows Forms (.NET 8)** y **MySQL** para la carrera de Ingeniería en Sistemas Computacionales. Proyecto académico, periodo **Agosto–Diciembre 2026**.

## Módulos vigentes

- **Administrador:** control de usuarios (docentes y secretarías), consulta y modificación de alumnos, bitácora e indicadores.
- **Secretaría:** inscripción de primer semestre con expediente completo, ficha y domicilio validado por catálogo postal; reinscripción independiente de 2.º y 3.º semestre; consulta, modificación y baja lógica.
- **Docente:** grupos y materias asignadas; horarios, listas digitales y PDF; calificaciones secuenciales de **semanas 5, 9, 13 y 17**. La semana 17 se captura manualmente y, al guardar el grupo completo, se **cierran los cuatro cortes**; se genera una lista final imprimible en PDF.

Se mantienen solo **semestres 1.º, 2.º y 3.º**, con **dos grupos de 25 plazas por semestre** y **14 docentes**. La generación 2026 solo se reinscribe a 2.º; la 2025 a 2.º (repetidor) o 3.º. Los registros de Reinscripción son mínimos e independientes de la tabla Alumno.

> Los menús de **Reportes**, **Documentos** y **Enviar documentos** todavía contienen opciones pendientes. Las tablas de Documentos se reservan para su futura implementación: no confundir esquema existente con funcionalidad terminada.

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

**Catálogo postal incluido:** \`Sql/09_catalogo_postal_datos_SEPOMEX.sql\` contiene los **158,370 registros** adjuntados (11.7 MB). Ejecútalo después del paso 2 (o después del paso 8), usando MySQL Workbench. Puede volver a ejecutarse para incorporar cambios porque utiliza \`ON DUPLICATE KEY UPDATE\`; no crea ni elimina tablas. Ya no necesitas descargar el TXT SEPOMEX para instalar esta versión.

No ejecutar sobre una instalación vieja sin migración o respaldo: estos archivos están diseñados para instalar la versión final **desde cero**, no para alterar arbitrariamente una base existente.

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
SELECT COUNT(*) AS bloques FROM Horario_Referencia; -- 170
SELECT COUNT(*) AS fichas FROM Aspirante WHERE numero_ficha LIKE 'ISC26-%'; -- 50
SELECT COUNT(*) AS reinscripciones FROM Reinscripcion; -- 0 inicialmente
SELECT COUNT(*) AS calificaciones FROM Calificacion_Corte; -- 0 inicialmente
\`\`\`

## Estructura de carpetas

La aplicación se organiza por responsabilidad sin cambiar los espacios de nombres (`AMBAR.Semana4`) ni combinar clases. Los archivos `.cs`, `.Designer.cs` y `.resx` de un mismo formulario permanecen juntos.

| Carpeta | Contenido |
|---|---|
| `Nucleo/` | Conexión MySQL, sesiones, autenticación y seguridad de identidad |
| `Formularios/Acceso/` | Inicio de sesión y contraseñas |
| `Formularios/Principal/` | Panel y navegación principal |
| `Formularios/Alumnos/` | Inscripción, reinscripción, consultas, expedientes y listas |
| `Formularios/Docentes/` | Materias, horarios, captura e historial de calificaciones |
| `Formularios/Administracion/` | Cuentas, personal y bitácora |
| `Formularios/Documentos/` | Formularios de documentos |
| `Repositorios/` | Acceso y consultas de MySQL por módulo |
| `Modelos/` | Clases de datos compartidas |
| `Servicios/` | Validación, correo, NSS y catálogo postal |
| `Interfaz/` | Tema, controles y navegación visual reutilizable |
| `PDF/` | Generación de documentos y reportes en PDF |
| `Assets/`, `Properties/` | Recursos visuales y recursos .NET |
| `Sql/` | Scripts oficiales de instalación y catálogo SEPOMEX |
| `Docs/`, `Tools/`, `.github/` | Documentación, herramientas y verificaciones automáticas |

`Program.cs`, el proyecto `.csproj`, la solución `.sln` y `appsettings.example.json` permanecen en la raíz. En `AMBAR.Semana4.csproj` se actualizaron las rutas de archivos dependientes para que Visual Studio reconozca los formularios y sus recursos. Más detalles: [guía de carpetas](Docs/ESTRUCTURA_CARPETAS.md).

## Estado de validación

Las modificaciones se revisan mediante un workflow de compilación; para comprobar el comportamiento de WinForms y los datos debes ejecutar la aplicación en Windows con MySQL. **No asumir que Reportes y envío de documentos están implementados** por el solo hecho de tener tablas.
