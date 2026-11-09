# Reinscripción con persistencia en base de datos
Esta rama es independiente de `feature/reinscripcion-autonoma-pruebas-2026` y todavía requiere pruebas de integración en Windows y MySQL.

## Flujo
- El número de control debe tener 10 dígitos y empezar en 2025 o 2026.
- Generación 2025: segundo (repetidor) o tercero. Generación 2026: segundo.
- Si el número ya existe en `Alumno`, se reutiliza y se precargan sus datos (solo lectura), sin duplicar expediente.
- Si no existe, se solicitan nombre, apellidos, CURP, fecha de nacimiento, teléfono de emergencia y correo institucional antes de darlo de alta.
- Se verifica CURP y congruencia con los nombres/fecha; teléfono mexicano; correo institucional válido con catálogo local.
- Se bloquea cualquier número con inscripción activa en el periodo de destino; incluidos los alumnos recién inscritos a primer semestre.
- El guardado utiliza una transacción: alta en `Aspirante` si no existe el alumno (ficha administrativa `REINS-<control>`, no es un proceso de admisión), alta en `Alumno` si no existe, e inserción en `Inscripcion` con `REINSCRIPCION`. Si ocurre un error, se revierte todo.
- Los registros en la tabla de demostración anterior se detectan para evitar duplicarlos. No se migran automáticamente.
- Las listas de grupo ya consultan `Inscripcion`; el nuevo alumno quedará disponible en Consulta de alumnos.

## Requisitos
Scripts de AMBAR ya aplicados (06, 07, 16, 18, 19, 20 y 21 para las funciones auxiliares). No hay migración nueva para el guardado real: usa `Aspirante`, `Alumno` e `Inscripcion`.

## Comprobación MySQL
```sql
SELECT a.Numero_control,a.nombre,a.apellido_paterno,g.nombre_grupo,i.tipo_inscripcion,
       p.nombre_periodo
FROM Alumno a
JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
WHERE i.tipo_inscripcion='REINSCRIPCION'
  AND p.nombre_periodo='Agosto-Diciembre 2026';
```

**Advertencia:** La captura de alumnos que no existen en BD es una modalidad académica. No se puede verificar que un número de control sea realmente suyo ni que haya aprobado materias solo por el año: deben verificarse contra registros institucionales cuando la aplicación deje de ser demostrativa.
