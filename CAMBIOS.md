# Cambios de la limpieza rápida

## Base de datos
- `01_esquema.sql`: se quitan `Plan_Materia`, `Documento` y `Envio_Documento` (sin uso en la app); `Usuario` incluye `debe_cambiar_password`, `intentos_fallidos` y `bloqueado_hasta`; encabezados renumerados. 17 tablas en este script, 23 en total.
- `07_vistas.sql`: periodo por `estatus='ACTIVO'` en lugar de un nombre fijo; sin `UPPER(estatus)`.
- Nuevo `10_actualizar_base_existente.sql` para quien ya tiene la base instalada.

## Código
- `PeriodoActual.cs` (nuevo): único lugar donde se decide el periodo vigente. Reemplaza `'Agosto-Diciembre 2026'` escrito a mano en MainForm, ConsultaA, ConsultaGruposForm, HorarioGrupoForm, InscripcionForm, RepositorioAlumnos, RepositorioConsultaSemestres y RepositorioReinscripcion.
- `Auth.Login`: bloqueo de 15 min tras 5 contraseñas incorrectas; el intento fallido queda en bitácora. `LoginForm` muestra el aviso de bloqueo.
- Se elimina el generador de NSS de prueba (método, botón y `SeguridadSocial.GenerarPrueba`).
- Se eliminan `RepositorioAlumnos.Enroll` y `RepositorioPersonal.Create` (envoltorios sincrónicos que nadie llamaba).
- `CanSeeStudents` pasa a `PuedeGestionarAlumnos` (Administrador y Secretaría inscriben, reinscriben y consultan).
- 71 `UPPER(estatus)` eliminados (la colación `utf8mb4_unicode_ci` ya ignora mayúsculas y así se pueden usar índices).
- Handlers vacíos `logo_Click` y `title_Click` eliminados, junto con su conexión en los Designer.
- `ModulosDocenteActivos` (siempre `true`) eliminado.
- Reportes, Documentos y Envío de documentos quedan ocultos en el menú.

## Pendiente (siguiente etapa)
- Esquema sin `ALTER` de parche, fusionar `Horario` con `Horario_Referencia`, unificar `Alumno` y `Reinscripcion`.
- Regla de generaciones 2025/2026 en `Inferir` y `ck_reins_generacion`.
- Pasar nombres de clases y archivos a español.
