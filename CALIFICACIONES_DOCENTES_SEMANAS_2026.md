# AMBAR | Módulo Docente: semanas 5, 9, 13 y 17

## Requisitos
Ejecutar **una sola vez** `Sql/25_calificaciones_semanas_5_9_13_17.sql` en MySQL Workbench **después** de las migraciones 10, 11, 20 y 24. Realiza antes un respaldo de `AMBAR_Control_Escolar`.

El script crea la tabla **`Calificacion_Corte`**, índices, restricciones, dos disparadores de protección y completa asignaciones y horarios del docente a partir de `Horario_Referencia`. No altera ni borra las tablas existentes `Calificacion`, `Unidad_Evaluacion`, `Periodo_Captura` ni `Registro_Captura` (datos históricos). El sistema debe tener aplicada la tabla definitiva `Reinscripcion`.

## Reglas
- El docente solo puede acceder a **sus propias materias y grupos**.
- Semanas **5, 9, 13**: captura manual, corrección permitida hasta el inicio de la semana 17.
- El primer corte debe estar completo para todo el grupo antes de comenzar el segundo; el segundo debe estar completo antes del tercero. Cada alumno debe tener todos los anteriores.
- Entrada numérica de 0 a 100, hasta dos decimales. **Cualquier valor menor de 70 se guarda como 0**; los valores de 70 a 100 se guardan sin redondear.
- **Semana 17**: se calcula automáticamente como promedio aritmético de los tres cortes guardados, **truncado a dos decimales sin redondeo**. Ejemplo 70, 70, 71 → 70.33.
- El cierre sucede al acceder al módulo Docente en o después de la semana 17, y al abrir explícitamente la semana 17 de Captura. No hay un evento en segundo plano: si nadie ejecuta AMBAR, las notas no se generan hasta la siguiente apertura.
- Después de comenzar semana 17, **no se permite capturar ni modificar** semanas 5/9/13 desde la aplicación. La nota final generada es inmutable mediante disparadores MySQL.
- Si falta uno de los tres cortes al cerrar, la nota final se mantiene **pendiente** en vez de inventar notas; los cortes anteriores tampoco podrán modificarse al haber comenzado semana 17. Resolver cualquier faltante antes de cerrar el periodo.

## Calendario de la práctica
Las semanas se cuentan desde `Periodo_Escolar.fecha_inicio`; en la carga de prueba del script 07 está fijada en **2026-08-01**. Por eso:
- Semana 5: 2026-08-29
- Semana 9: 2026-09-26
- Semana 13: 2026-10-24
- Semana 17: **2026-11-21** (cierre)

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
5. Capturar semanas 5 → 9 → 13, corregir una calificación y consultar el historial.
6. Verificar calificación final al llegar a semana 17 y que no se puedan modificar sus cortes.
7. Exportar lista del docente y el historial a PDF.
8. Confirmar rechazo de cambios desde otro docente.

**Estado:** cambios subidos a GitHub. No se ejecutó la migración en el MySQL local ni se verificó la compilación Windows Forms desde esta sesión.
