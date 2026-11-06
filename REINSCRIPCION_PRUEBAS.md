# Probar Reinscripción · AMBAR

Periodo de destino: **Agosto–diciembre 2026**. Sólo 2.º→3.º y 4.º→5.º.
La prueba NO otorga aprobaciones ni modifica las calificaciones de los docentes.

## Preparación
1. En MySQL Workbench, selecciona la base `AMBAR_Control_Escolar`.
2. Asegúrate de haber ejecutado `Sql/06_validaciones_y_personal.sql` y `Sql/07_grupos_y_fichas_2026.sql`.
3. Ejecuta **una vez** `Sql/13_reinscripcion_2026.sql`. Es reejecutable.
4. Compila y ejecuta AMBAR desde Visual Studio. En el menú **Reinscripción**, busca por **número de control**.

## Datos necesarios para una prueba válida
Un alumno con notas aprobadas de segundo o cuarto semestre requiere **historial académico real del periodo anterior**. El sistema nunca asume que un alumno actualmente en 1.º cursó 2.º.

Para un alumno de prueba cuyo antecedente sea **segundo semestre**, confirma primero que ese antecedente es correcto y registra:

```sql
USE AMBAR_Control_Escolar;
-- Cambia NUMERO_CONTROL por el número de un alumno que REALMENTE cursó 2.º.
INSERT INTO Historial_Semestre_Reinscripcion(numero_control,semestre_cursado,periodo_origen)
VALUES ('NUMERO_CONTROL',2,'Enero-Junio 2026');
```

Después registra cada calificación final del semestre anterior (0–100), sin omitir materias. Ejemplo para UNA materia:

```sql
INSERT INTO Calificacion_Final_Reinscripcion
  (numero_control,id_materia_reinscripcion,periodo_origen,calificacion)
SELECT 'NUMERO_CONTROL',id_materia_reinscripcion,'Enero-Junio 2026',85
FROM Materia_Reinscripcion
WHERE semestre_origen=2 AND nombre='Cálculo Integral';
```

**Para conocer las materias que faltan:**

```sql
SELECT m.nombre, f.calificacion
FROM Materia_Reinscripcion m
LEFT JOIN Calificacion_Final_Reinscripcion f
 ON f.id_materia_reinscripcion=m.id_materia_reinscripcion
 AND f.numero_control='NUMERO_CONTROL'
WHERE m.semestre_origen=2
ORDER BY m.nombre;
```

Para un alumno procedente de **cuarto semestre**, registra `semestre_cursado=4` y usa `semestre_origen=4` en las consultas.

## Reglas que debes probar
- Falta alguna nota final: muestra «Faltan calificaciones finales», no permite guardar.
- Una nota de 69.99: bloquea la reinscripción.
- Todas las notas >=70: habilita los grupos del siguiente semestre.
- Grupo lleno: no aparece para seleccionarlo y se comprueba nuevamente antes de insertar.
- Reinscripción existente en agosto–diciembre 2026: bloquea la duplicación.
- Alumno sin historial de segundo o cuarto: no se reinscribe.
- No se modifican CURP, NSS, número de control, ni expediente del alumno.

## Limitaciones actuales
El módulo exige un registro explícito de calificaciones **finales** de 2.º/4.º en la tabla nueva. El componente de calificaciones de docentes registra **notas por unidad** (no acreditación final completa del semestre histórico); vincular automáticamente estas dos fuentes sería incorrecto.
Para producir pruebas con alumnos ficticios, usa exclusivamente registros explícitamente creados para la demostración, sin falsificar historiales de estudiantes reales.
Los grupos 3ISC1/3ISC2 y 5ISC1/5ISC2 se reutilizan de la carga 07 y conservan profesores NULL donde no se han asignado.
