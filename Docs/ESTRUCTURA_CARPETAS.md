# Organización de archivos de AMBAR

## Objetivo

Facilitar la búsqueda de código en Visual Studio sin alterar funcionalidad, nombres de clases, tablas SQL ni la configuración de conexión.

## Estructura

- `Program.cs`: punto de entrada; permanece en la raíz.
- `Nucleo/`: `Datos.cs` (conexión, sesión y autenticación) e `Identidad.cs`.
- `Formularios/Acceso/`: inicio de sesión y cambio/confirmación de contraseñas.
- `Formularios/Principal/`: ventana principal.
- `Formularios/Alumnos/`: inscripción, reinscripción, consulta, expedientes, horarios y listas de alumnos.
- `Formularios/Docentes/`: materias, filtros, captura, historial y detalles de calificaciones, horarios.
- `Formularios/Administracion/`: usuarios, personal y bitácora.
- `Formularios/Documentos/`: consultas del módulo de documentos.
- `Repositorios/`: todas las clases `Repositorio*.cs`, separadas por responsabilidad.
- `Modelos/`: clases que representan datos de estudiantes, contactos y bloques de horario.
- `Servicios/`: validaciones, catálogo postal, correo, NSS y automatización de datos.
- `Interfaz/`: colores, botones, paneles y navegación visual.
- `PDF/`: generación de reportes y constancias.
- `Assets/`, `Properties/`, `Sql/`, `Tools/`, `.github/`: se conservan en sus ubicaciones previas.

## Reglas de mantenimiento

1. Crear nuevos formularios junto con su `.Designer.cs` y su `.resx` cuando existan.
2. Evitar editar manualmente los archivos `.Designer.cs`: usar el diseñador visual de Visual Studio.
3. Mantener las consultas a MySQL en `Repositorios/` y las reglas auxiliares en `Servicios/`.
4. Los archivos usan el mismo namespace `AMBAR.Semana4`; moverlos de carpeta no cambia los nombres de las clases ni sus permisos.
5. Al agregar formularios nuevos, verificar el vínculo de `Compile Update` y `EmbeddedResource Update` en el archivo `.csproj`.
6. No subir `appsettings.json` ni contraseñas al repositorio.

## Verificación

- Abrir `AMBAR.Semana4.sln` en Visual Studio 2022.
- Restaurar paquetes NuGet y ejecutar **Compilar → Recompilar solución**.
- Abrir desde el diseñador los formularios principales y comprobar imágenes y recursos.
- Comprobar el inicio de sesión, alta de alumnos, reinscripción, captura de calificaciones y generación de PDF con la base MySQL de prueba.
- Los scripts `Sql/` no fueron cambiados ni ejecutados como parte de esta reorganización.

La reorganización conserva los contenidos originales de los archivos de código: los cambios son de ubicación, referencias del proyecto y documentación. No implica una validación de ejecución local sobre Windows/MySQL.
