# Semana 6 · Reinscripción autónoma (modo demostración)

**Objetivo:** permitir comprobar reglas del número de control y capturar la reinscripción sin alumnos precargados. No se exige historial académico de 2.º o 4.º ni notas para esta modalidad.

## Preparar base de datos
Ejecutar en MySQL Workbench:
1. Scripts previos de AMBAR, incluyendo `06_validaciones_y_personal.sql` y `07_grupos_y_fichas_2026.sql` (creación de grupos de 3.º y 5.º).
2. **`Sql/16_reinscripcion_autonoma.sql`**, y a continuación **`Sql/18_reinscripcion_2026_semestres_1_2_3.sql`** para crear los grupos de segundo semestre y actualizar la restricción de generaciones. Este último respalda los datos de demostración de 2024 antes de quitarlos de la tabla activa.
3. Abrir la rama `feature/reinscripcion-autonoma-pruebas-2026` y compilar en Visual Studio.

No hay que ejecutar los scripts 13, 14 o 15 para probar esta modalidad autónoma.

## Probar el formulario
- `2025000001`: número de 10 dígitos, comienza con 2025 → **3.º semestre**.
- `2026000001`: comienza con 2026 → **2.º semestre**, salvo que tenga inscripción activa en primero en el mismo periodo.
- `2024000001`: rechazar (generación no incluida en este ejercicio).
- `2025ABC001`: rechazar (no son 10 dígitos numéricos).
- Después de validar: nombre y apellidos en blanco, seleccionar grupo matutino/vespertino, pulsar **Ver horario**, y guardar.
- Volver a escribir el mismo número: debe impedir duplicación.
- Nombre `....` o vacío: debe impedir el guardado.
- Grupo lleno: debe bloquearlo.

## Consulta de grupo y PDF
Desde **Reinscripción → Grupos / PDF**, seleccionar grupo 2ISC1/2ISC2 o 3ISC1/3ISC2, abrir **Ver lista / PDF** y exportar.
Los registros de demostración se muestran con estatus `DEMO`. El horario viene del grupo: los nombres de docente `NULL` se muestran vacíos.

## Alcance importante
Esta funcionalidad es una **demostración autónoma**. La interpretación `2025 → 3.º` y `2026 → 2.º` es una regla simplificada para el periodo agosto–diciembre 2026; no acredita identidad ni materias aprobadas. Los expedientes reales de `Alumno` y el flujo anterior de inscripción permanecen intactos. El contador principal incluye los registros de demostración, pero `Consulta de alumnos` continúa mostrando exclusivamente los expedientes reales.

El script SQL crea la tabla nueva `Reinscripcion_Demostracion` para separar el flujo de pruebas.

## Regla importante de primer semestre
Si un número tiene inscripción activa de primer semestre en `Agosto-Diciembre 2026`, el módulo lo rechaza tanto al validar como al guardar, aunque comience con 2026. El segundo semestre se crea con dos grupos sin horarios de referencia, hasta que se proporcionen horarios reales. No inventar horarios.
