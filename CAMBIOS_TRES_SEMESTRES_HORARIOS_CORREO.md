# AMBAR · Actualización académica de tres semestres (Semana 6)

Se mantiene el módulo de Reinscripción autónoma con las reglas 2025 → 2.º (repetidor) o 3.º, 2026 → solo 2.º. Si un alumno ya tiene inscripción activa de primero en agosto-diciembre de 2026, se bloquea la reinscripción.

## Orden de los scripts

En una base AMBAR existente con Sql/06 y Sql/07 ejecutados:
1. `Sql/16_reinscripcion_autonoma.sql` — tabla de demostración (si aún no existe).
2. `Sql/18_reinscripcion_2026_semestres_1_2_3.sql` — crea **solo** los dos grupos de segundo, sin volver a crear tablas de respaldo históricas.
3. `Sql/19_reinscripcion_repetidores_2025.sql` — permite que generación 2025 seleccione 2.º o 3.º.
4. **`Sql/20_horarios_tres_semestres_14_docentes.sql`** — completa los huecos docentes en primero y tercero y genera el horario de **2ISC1 y 2ISC2**.
5. **`Sql/21_vistas_alumnos_tres_semestres.sql`** — crea las tres vistas de alumnado por semestre.

**No vuelvas a ejecutar el script 07 después de 20**, ya que 07 tiene la carga original de los horarios de primero/tercero, que podría dejar campos sin profesor otra vez.

## Horarios y maestros

Los horarios de 1.º/3.º se basan en las cargas originales. Se reutilizan docentes existentes, escogiendo de preferencia materias afines. Los horarios de 2.º se proponen para esta práctica porque no se recibió una tabla oficial de clases de ese semestre.
- Grupos: 1ISC1, 1ISC2, **2ISC1, 2ISC2**, 3ISC1, 3ISC2.
- **18 materias distintas:** seis por semestre.
- **14 docentes distintos** en las asignaciones para esos grupos.
- El SQL revisa choques de horario por docente e interrumpe la operación si detecta alguno.
- No se eliminan otros docentes existentes de la base de datos; solo se limita a 14 el personal asignado a los seis grupos de esta práctica.

## Listas de alumnos de cada semestre

En MySQL Workbench:

```sql
SELECT * FROM vw_AMBAR_Alumnos_Semestre_1 ORDER BY numero_control;
SELECT * FROM vw_AMBAR_Alumnos_Semestre_2 ORDER BY numero_control;
SELECT * FROM vw_AMBAR_Alumnos_Semestre_3 ORDER BY numero_control;
```

Son **vistas**, no tablas duplicadas; muestran la información actual de `Alumno/Inscripcion` y `Reinscripcion` sin copiar expedientes.

## Horarios

Se mantiene el botón **Ver horario** del grupo, sin captura de correo y sin envío SMTP.

## Limitaciones
- La compilación y las pruebas de MySQL deben verificarse en Windows.
