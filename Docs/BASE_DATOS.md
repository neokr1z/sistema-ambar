# Base de datos final de AMBAR

## Principios

- Una sola base: \`AMBAR_Control_Escolar\`.
- **21 tablas físicas de instalación limpia**: 16 del esquema principal, 3 de configuración (`Control_Identidad`, `Horario`, `Catalogo_Postal`), `Reinscripcion` y `Calificacion_Corte`. Las vistas de semestre y `vw_AMBAR_Alumnos_Unificados` no son tablas.
- Semestres operativos 1.º, 2.º y 3.º; seis grupos; 14 docentes y 170 bloques.
- No se utilizan respaldos de 2024 ni \`Reinscripcion_Demostracion\`.
- Se eliminan las estructuras viejas de calificaciones por unidades \`Unidad_Evaluacion\`, \`Calificacion\`, \`Periodo_Captura\`, \`Registro_Captura\`, y las tablas de permisos individuales \`Permiso\`, \`Rol_Permiso\` y \`Usuario_Permiso\`; la autorización vigente depende de \`Rol\` y controles en C#.
- \`Plan_Materia\` se mantiene como relación del plan académico, aun cuando la carga actual de docentes es por \`Asignacion_Docente\`.

## Inscripción frente a Reinscripción

**Primer semestre:** \`Aspirante\` → \`Alumno\` → \`Domicilio\` + \`Inscripcion\`, ligado a \`Plan_Estudio\` y \`Grupo\`. Usa ficha precreada \`ISC26-001\` a \`ISC26-050\` y domicilio por \`Catalogo_Postal\`.

**Segundo y tercero:** \`Reinscripcion\` guarda número de control, nombres y apellidos, generación, semestre, grupo, periodo, estatus y correo generado. Se relaciona con \`Grupo\` mediante \`ID_grupo\`, pero no necesita expediente completo en \`Alumno\`. Grupos de 25 cupos; las bajas conservan el registro y liberan la plaza.

## Calificaciones

\`Calificacion_Corte\` guarda \`numero_control\`, \`Id_asignacion\` y \`semana\` (5, 9, 13 o 17). El grupo y profesor se determinan desde \`Asignacion_Docente\`.

- Se requiere completar toda la semana anterior para avanzar.
- Solo se aceptan calificaciones **enteras** de 0 a 100 (sin decimales que redondear). Menor de 70 se almacena como 0; de 70 a 100 se conserva tal cual.
- La semana 17 se **captura**, no se genera automáticamente.
- Cuando se guarda la semana 17 completa, ningún corte de esa asignación puede modificarse. Se genera lista digital y PDF con la media de los cuatro cortes, truncada a entero y con 0 para promedios menores de 70.
- El sistema valida la secuencia y el cierre transaccionalmente; los triggers SQL impiden cambios directos en cortes finales.

## Orden de instalación

Ver la tabla de **8 scripts** en la raíz \`README.md\`. No ejecutar scripts de migración del historial después de instalar el esquema nuevo.

## Acceso y bloqueo de cuentas

Tras 5 contraseñas incorrectas seguidas, la cuenta se bloquea 15 minutos (`Usuario.intentos_fallidos` y `Usuario.bloqueado_hasta`). Un inicio de sesión correcto reinicia el contador. Cada intento fallido queda en `Bitacora` como `ACCESO_FALLIDO`.

## Periodo vigente

La aplicación toma el periodo del registro de `Periodo_Escolar` con `estatus='ACTIVO'` más reciente (clase `PeriodoActual`). Al iniciar un semestre nuevo basta crear y activar el periodo en la BD; ya no hay que editar código. Pendiente: la regla de generaciones de `Reinscripcion` (años 2025/2026) todavía está fija en `RepositorioReinscripcion.Inferir` y en el `CHECK ck_reins_generacion`.

## Nota de seguridad

\`appsettings.json\` es local y debe permanecer fuera de Git. Las claves antiguas pudieron haberse guardado en el historial del repositorio; borrar un archivo de \`main\` no borra su historial. Rotar las credenciales expuestas.
