# Docentes de los horarios y validación de identidad

## Actualizar una instalación existente

1. Actualiza el proyecto desde `main` y cierra el programa.
2. En MySQL Workbench, sobre tu base habitual, ejecuta completo `Sql/10_identidad_personal.sql`.
3. Ejecuta completo `Sql/11_docentes_y_asignaciones_2026.sql`.
4. Compila y ejecuta el proyecto actualizado en Visual Studio 2022.

Se presupone que ya ejecutaste las migraciones 01, 02, 05, 06, 07 y 08. No vuelvas a ejecutar 01 sobre tu base existente. El script 09 es una guía manual, no un requisito para esta carga. Los scripts 10 y 11 se pueden repetir. Publicar estos archivos en GitHub no ejecuta SQL en tu computadora.

## Carga de docentes

La fuente es `Horario_Referencia`, previamente transcrita de las imágenes mediante 07: ISC, agosto-diciembre 2026, grupos 1ISC1, 1ISC2, 3ISC1, 3ISC2, 5ISC1 y 5ISC2.

En una instalación sin docentes/asignaciones previas, 11 crea 25 docentes, sus 25 cuentas, 18 materias, 29 asignaciones y 128 bloques operativos. Conserva sin asignar las siete combinaciones materia/grupo (32 bloques) cuyo docente no figuraba en las imágenes. No modifica las 50 fichas ni las inscripciones.

Contraseña de todas las **cuentas nuevas**: `holaruben12`. Se almacena mediante PBKDF2-SHA256, 250000 iteraciones, con sal aleatoria individual; nunca como contraseña en texto plano en la tabla Usuario. El script contiene hashes compatibles con el login del programa.

El usuario se forma con primer nombre y los dos apellidos, separados por puntos, sin acentos. Ejemplos: `jose.quintana.castellanos`, `antonio.rodriguez.roman`. Si un usuario está ocupado, se añade un sufijo numérico. **La primera tabla de resultados de 11 muestra los usuarios y correos efectivos.**

Si ya existe un docente con el mismo nombre completo, se reutiliza. Si ya tiene cuenta activa, conserva su usuario y contraseña. Una cuenta existente no cambia de contraseña al repetir el script. Las bajas, identidades ambiguas, asignaciones incompatibles y horarios operativos diferentes provocan un error y revierten toda la carga de 11. No se reactivan ni reasignan personas silenciosamente.

Las imágenes no contienen RFC, CURP, nacimiento ni teléfono de esos profesores: quedan `NULL`, con identidad «Pendiente» en el reporte de resultados. Estos campos no se rellenan con datos inventados. Las materias nuevas tienen una clave interna `HOR26-*`; créditos y horas teóricas/prácticas quedan pendientes, porque no pueden deducirse de un horario. Se reutilizan materias existentes del mismo nombre y carrera sin cambiar sus datos. La migración 10 permite esos valores pendientes sin borrar los existentes.

Las materias y horarios aparecen en los módulos de cada docente al iniciar sesión. Para capturar calificaciones también deben existir las unidades de evaluación y un periodo de captura abierto; esos datos académicos no están en las imágenes y no se generan por suposición. Tampoco se crean buzones reales de correo.

## Validación compartida

El alta de docentes y secretarias incluye un selector de nacimiento obligatorio, editable visualmente desde el diseñador. El repositorio vuelve a validar antes de escribir en MySQL, de modo que no depende sólo de la interfaz.

La misma comprobación se aplica a inscripción y edición de alumnos:

- Formato de CURP, código de entidad y fecha real desde 01/01/1926 hasta ayer.
- Coincidencia del nacimiento y del siglo con la fecha capturada.
- Inicial paterna, primera vocal interna, inicial materna y nombre.
- Consonantes internas de apellidos y nombre.
- Dígito verificador.
- Tratamiento de acentos, Ñ, partículas, nombres compuestos que comienzan por María/José y palabras inconvenientes.

Se conservan la comparación de fecha RFC/CURP en personal, validación de teléfono y usuario, correo automático, normalización de nombres, detección de duplicados y transacciones. El alta manual exige identidad completa, aunque la importación de horarios permita datos pendientes.

Esta es una validación local de congruencia, no una consulta o certificación ante RENAPO/SAT. Sexo y entidad se verifican en formato dentro de la CURP; no se contrastan contra campos independientes porque estos formularios no los capturan. Ante una CURP oficial con una excepción documental, hay que revisar la constancia y la regla concreta; no sustituir datos reales para superar el formulario.

Referencia de estructura: [Instructivo de SEGOB](https://sidof.segob.gob.mx/notas/docFuente/5526717). El ejemplo ilustrativo de su imagen se usa para las letras y fecha; las pruebas calculan el dígito verificador por separado.

## Verificación

- Compilación directa de todas las fuentes C# con Roslyn y referencias .NET 8/Windows Forms. No se pudo ejecutar MSBuild ni el diseñador de Visual Studio en este entorno Linux.
- 173 comprobaciones de validación, persistencia, permisos, inscripción, calificaciones, bitácora y acceso/asignaciones de docentes en MySQL 8 desechable.
- `Tests/verificar_carga_docentes.py`: cantidades, correspondencia de los 128 bloques con la referencia, reejecución sin cambios, conservación de contraseña modificada y reversión ante conflicto.
- `Tests/preparar_mysql.py` prepara una base desechable y ejecuta 10/11 dos veces. No lee credenciales del proyecto.

Para repetir las pruebas, usar una base aislada cuyo nombre termine en `_test`; las instrucciones de conexión del resto de pruebas están en `Tests`.
