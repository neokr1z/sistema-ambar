# AMBAR | Reinscripción mínima y consulta por semestre

## Alcance
Esta versión mantiene independiente la reinscripción: pide número de control, nombre(s), apellidos, semestre y grupo. **No solicita CURP, fecha de nacimiento, domicilio ni expediente**. Conserva las reglas de generación 2026 → 2.º; 2025 → 2.º (repetidor) o 3.º. Impide reinscribir a un alumno de primer semestre del mismo periodo y bloquea registros duplicados en la tabla autónoma o con inscripción activa en agosto–diciembre 2026.

La información se conserva en MySQL en `Reinscripcion`, una tabla de inscripción reducida, **independiente de `Alumno`**. El nombre definitivo de la tabla es `Reinscripcion` y la interfaz indica «Reinscripción guardada correctamente». Los expedientes originales no se crean ni se alteran.

El correo se genera **sin pedirlo al alumno** con la regla compartida de AMBAR (`PersonalAutomatico.EmailBase`): nombre(s) + inicial del apellido paterno + inicial del materno + `@acapulco.tecnm.mx`. Ante coincidencias se añade sufijo numérico. Se verifica disponibilidad local frente a correos de alumnado, personal y otras reinscripciones. **No se crea un buzón real ni se envía ningún correo.** El correo queda guardado sin mostrarse en Reinscripción ni Consulta.

## Migración obligatoria
Ejecutar en **MySQL Workbench** `Sql/22_correo_reinscripcion_minima.sql`. Requiere haber aplicado 06, 07, 16, 18 y 19, y el script 20 para los horarios de segundo semestre. Es reejecutable y no modifica los expedientes existentes. Las reinscripciones anteriores quedan con correo NULL, sin inventar un correo retroactivo.

## Consulta de alumnos
En «Consulta de alumnos», elegir «Todos (1.º a 3.º)», «1.º», «2.º» o «3.º». La tabla combina altas reales de `Alumno` + `Inscripcion` y reinscripciones autónomas, ordenadas por número de control. El botón «Ver datos» muestra el número, nombre, semestre y grupo de los reinscritos; «Ver horario» consulta su grupo. Los botones que necesitan expediente completo se deshabilitan en estos registros. Los alumnos con expediente completo conservan sus acciones habituales.

Las tres vistas SQL `vw_AMBAR_Alumnos_Semestre_1/2/3` del script 21 siguen disponibles para consultas MySQL; no se crean tres copias físicas de alumnos.

## Verificación SQL
```sql
SELECT numero_control, nombre, apellido_paterno, apellido_materno,
       semestre_destino, ID_grupo, correo_institucional
FROM Reinscripcion
ORDER BY numero_control;

SELECT COUNT(*) AS primero FROM vw_AMBAR_Alumnos_Semestre_1;
SELECT COUNT(*) AS segundo FROM vw_AMBAR_Alumnos_Semestre_2;
SELECT COUNT(*) AS tercero FROM vw_AMBAR_Alumnos_Semestre_3;
```

### Pendiente de validar en el equipo
No se compiló Windows Forms ni se probó contra una base de datos MySQL real desde este entorno. Verificar guardar un alumno nuevo, intentar guardar nuevamente el mismo control, bloqueo de inscripción activa, correo duplicado, filtros de consulta y lista PDF.
