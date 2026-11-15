# AMBAR | Módulo Docente: semanas 5, 9, 13 y 17

## Requisitos
Ejecutar **una sola vez** `Sql/25_calificaciones_semanas_5_9_13_17.sql` en MySQL Workbench **después** de las migraciones 10, 11, 20 y 24. Realiza antes un respaldo de `AMBAR_Control_Escolar`.

El script crea la tabla **`Calificacion_Corte`**, índices, restricciones, dos disparadores de protección y completa asignaciones y horarios del docente a partir de `Horario_Referencia`. No altera ni borra las tablas existentes `Calificacion`, `Unidad_Evaluacion`, `Periodo_Captura` ni `Registro_Captura` (datos históricos). El sistema debe tener aplicada la tabla definitiva `Reinscripcion`.

## Reglas definitivas
- Los cuatro cortes **se capturan manualmente** en el orden **5 → 9 → 13 → 17**. No se puede capturar el siguiente hasta completar para **todo el grupo** el inmediatamente anterior.
- Se aceptan valores de 0 a 100 con máximo dos decimales. **Cualquier nota inferior a 70 se guarda como 0**; las demás no se redondean.
- Las semanas 5, 9 y 13 pueden corregirse mientras no se haya completado la 17.
- La **semana 17 es la cuarta evaluación manual** y se captura de todos los alumnos del grupo en una sola operación; el guardado exige confirmación de **cierre irreversible**.
- Después de guardar la semana 17, ya no se pueden modificar **ninguno de los cuatro cortes** de esa materia/grupo. El cierre se registra en Bitácora.
- El promedio final se calcula automáticamente al consultar la lista final, a partir de las **cuatro** notas; se trunca a dos decimales, sin redondeo. Si es inferior a 70, el resultado definitivo es 0.
- Tras el cierre, la aplicación muestra la lista definitiva y permite exportarla a **PDF** para imprimirla.
- El inicio de la semana 17 **por fecha no cierra automáticamente** el sistema. No se generan notas sin intervención del docente. El orden de captura se valida sobre los registros ya guardados, no sobre el reloj.
- La lista digital, horarios y exportación de listas de grupo a PDF continúan funcionando.

## Calendario de la práctica
Las semanas se cuentan desde `Periodo_Escolar.fecha_inicio`; en la carga de prueba del script 07 está fijada en **2026-08-01**. Por eso:
- Semana 5: 2026-08-29
- Semana 9: 2026-09-26
- Semana 13: 2026-10-24
- Semana 17: **2026-11-21** (referencia de calendario; el cierre depende de guardar la captura)

**No se ha verificado que estas fechas correspondan al calendario oficial del plantel.** Si las semanas se refieren a otro calendario, modificar primero `Periodo_Escolar.fecha_inicio` de manera planificada, sin alterar sin revisión otros procesos.

## Alumnos y horarios
- El catálogo digital incluye tanto **`Alumno` + `Inscripcion`** como registros mínimos de **`Reinscripcion`** de 2.º y 3.º.
- `Calificacion_Corte` almacena el número de control y la asignación docente. No es una FK directa con `Alumno`, porque los reinscritos no tienen expediente en esa tabla.
- El script 25 crea asignaciones faltantes en `Asignacion_Docente`, reutilizando nombres de docentes y materias de `Horario_Referencia`; se detiene con error si encuentra docentes faltantes, identidades ambiguas, asignaciones incompatibles o materias duplicadas.
- El docente conserva la vista de horario asociado a su materia/grupo desde `Horario` y la lista digital y PDF de `ListaGrupoForm`.
- En **Historial docente** aparecen los cortes semanales, las correcciones auditadas y la exportación PDF.

## Consultas para validar
```sql
USE AMBAR_Control_Escolar;

SELECT g.nombre_grupo, m.nombre_materia,
       CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno) AS docente,
       COUNT(DISTINCT h.Id_horario) AS bloques
FROM Asignacion_Docente ad
JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
JOIN Materia m ON m.ID_Materia=ad.ID_Materia
JOIN Docente d ON d.ID_docente=ad.ID_docente
LEFT JOIN Horario h ON h.Id_asignacion=ad.Id_asignacion
GROUP BY ad.Id_asignacion,g.nombre_grupo,m.nombre_materia,d.nombre,d.apellido_paterno,d.apellido_materno
ORDER BY g.nombre_grupo,m.nombre_materia;

SELECT numero_control,Id_asignacion,
       MAX(CASE WHEN semana=5 THEN calificacion END) AS semana5,
       MAX(CASE WHEN semana=9 THEN calificacion END) AS semana9,
       MAX(CASE WHEN semana=13 THEN calificacion END) AS semana13,
       MAX(CASE WHEN semana=17 THEN calificacion END) AS final17
FROM Calificacion_Corte
GROUP BY numero_control,Id_asignacion
ORDER BY Id_asignacion,numero_control;

SELECT fecha_hora,tipo_evento,descripcion
FROM Bitacora WHERE tipo_evento IN ('CAPTURA_CORTE','CIERRE_CALIFICACIONES')
ORDER BY fecha_hora DESC;
```

## Pruebas pendientes en Windows
1. Iniciar sesión como docente con materias asignadas y consultar su lista/horario.
2. Verificar que la lista incluya inscritos y reinscritos activos.
3. Capturar 69, 70 y 99.75 en semana 5; comprobar 0, 70 y 99.75.
4. Intentar capturar semana 9 antes de completar 5 para todo el grupo; comprobar bloqueo.
5. Capturar semanas 5 → 9 → 13 y corregir una calificación.
6. Capturar semana 17 de todo el grupo, confirmar el cierre y comprobar promedio de cuatro cortes.
7. Verificar que tras el cierre no se puedan editar las semanas 5, 9, 13 o 17.
8. Exportar el PDF de calificaciones finales, la lista del docente y el historial.
9. Confirmar rechazo de cambios desde otro docente.

**Estado:** cambios subidos a GitHub. No se ejecutó la migración en el MySQL local ni se verificó la compilación Windows Forms desde esta sesión.
