# Revisión del modelo relacional de AMBAR — rama experimental

## Qué se modificó

| Antes | Después | Archivos afectados |
|---|---|---|
| `Horario` y `Horario_Referencia` | **Una sola tabla `Horario`** con `ID_grupo` y `Id_asignacion` opcional | SQL 01, 02, 03, 05, 06; repositorio y formularios de horarios |
| `Dominio_Correo` (23 dominios) | **HashSet local** para validar dominios sin Internet | `Servicios/CorreoDominio.cs`, SQL 02 |
| `Aspirante.promedio_preparatoria`, `nombre_preparatoria` | Eliminados; no se capturan | SQL 01, 02, 03 |
| `Materia.creditos`, `horas_teoricas`, `horas_practicas` | Eliminados; no se utilizan para el horario real | SQL 01, 02, 06 |
| `Personal_Administrativo.puesto` | Eliminado; el rol se identifica por `Usuario.Id_rol` | SQL 01, `RepositorioPersonal.cs` |
| `Plan_Estudio.especialidad`, fechas de vigencia | Eliminados; se conservan ID, carrera, nombre, versión y estatus | SQL 01, 03 |
| `Rol.descripcion` | Eliminado; los permisos dependen de C# y `nombre` | SQL 01, 02 |
| `Aspirante.periodo_fichas` sin FK | FK a `Periodo_Escolar.ID_Periodo` | SQL 02 |
| `Domicilio` solo con CP y asentamiento en texto | También FK `id_catalogo_postal` a la fila exacta de `Catalogo_Postal` | SQL 02, `RepositorioAlumnos.cs` |
| Consulta de alumnos y reinscritos repetida en C# | Vista `vw_AMBAR_Alumnos_Unificados` y repositorio simplificado | SQL 07, `RepositorioConsultaSemestres.cs` |

**No se borran datos de alumnos, inscripciones, reinscripciones ni calificaciones.** Las columnas eliminadas sí perderán su información histórica; respaldar antes de migrar.

## Por qué no se juntaron todas las tablas

- `Aspirante` controla las fichas y el examen **antes** de que exista un alumno matriculado.
- `Alumno` es el expediente completo de ingreso; `Inscripcion` registra la relación con un grupo y permite guardar historial de inscripciones. Unir las tres generaría repetición y permitiría errores de estado.
- `Reinscripcion` es independiente para permitir capturar alumnos históricos sin expediente completo. Para exponerlos juntos sin duplicar expedientes se creó `vw_AMBAR_Alumnos_Unificados`.
- `Calificacion_Corte.numero_control` puede pertenecer a `Alumno` **o** `Reinscripcion`; no se agregó una FK inválida a una sola tabla. Una FK rígida requeriría un rediseño mayor hacia una identidad estudiantil centralizada y migración total.
- `Control_Identidad` es un candado transaccional y no necesita FK. `Catalogo_Postal` es un catálogo de referencia: se relaciona con `Domicilio`, pero no debe estar escrito como miles de líneas C#.
- `Usuario.debe_cambiar_password` se **conserva**: las reglas de la contraseña están en código, pero la bandera persiste en BD para sobrevivir reinicios y evitar usar contraseñas temporales indefinidamente.

## Instalación limpia

Ejecutar SQL 01 a SQL 08 en orden; importar SQL 09 (SEPOMEX). No ejecutar SQL 10 ni SQL 11 después de una instalación limpia. Resultado previsto: **21 tablas físicas** y las vistas de SQL 07.

## Migración de una base anterior

1. Exportar respaldos de esquema y datos de `AMBAR_Control_Escolar`.
2. Restaurar ese respaldo en **una base de pruebas aislada**.
3. Verificar consultas de SQL 11 y ejecutar **una sola vez** en la copia. DDL no es transaccional.
4. Conservar temporalmente `Horario_Antes_11`, comparar los registros y verificar los 170 bloques y sus asignaciones.
5. Reejecutar SQL 07 para crear `vw_AMBAR_Alumnos_Unificados`.
6. Revisar `SELECT COUNT(*) FROM Domicilio WHERE id_catalogo_postal IS NULL;` y corregir direcciones históricas si procede.
7. Probar inicio de sesión, formulario de inscripción, cambio de domicilio, alumnos de primer a tercer semestre, horarios docente/alumno, calificaciones, PDF y bitácora.
8. Solo después del respaldo y la validación, considerar eliminar `Horario_Antes_11`.

**Importante:** el script SQL 11 detiene la migración si detecta horarios operativos no representados en la referencia, periodos inválidos o dominios personalizados. No garantiza recuperabilidad sin respaldo, y no es idempotente.

## Estado de validación

Los cambios fueron inspeccionados estáticamente. Sin ejecutar .NET 8 + MySQL con datos reales no se garantiza la compilación o compatibilidad de una instalación histórica. Mantener esta rama experimental separada de `main` hasta probarla completamente.
