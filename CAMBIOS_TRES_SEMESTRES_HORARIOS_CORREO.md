# AMBAR · Actualización académica de tres semestres (Semana 6)

Se mantiene el módulo de Reinscripción autónoma con las reglas 2025 → 2.º (repetidor) o 3.º, 2026 → solo 2.º. Si un alumno ya tiene inscripción activa de primero en agosto-diciembre de 2026, se bloquea la reinscripción.

## Orden de los scripts

En una base AMBAR existente con Sql/06 y Sql/07 ejecutados:
1. `Sql/16_reinscripcion_autonoma.sql` — tabla de demostración (si aún no existe).
2. `Sql/18_reinscripcion_2026_semestres_1_2_3.sql` — crea **solo** los dos grupos de segundo, respaldando los antiguos registros de demo de 2024.
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

Son **vistas**, no tablas duplicadas; muestran la información actual de `Alumno/Inscripcion` y `Reinscripcion_Demostracion` sin copiar expedientes.

## Enviar horario por correo

Después de guardar una reinscripción en el formulario autónomo:
1. Captura el **correo del alumno**.
2. Pulsa **Enviar horario** y confirma el destinatario.
3. AMBAR valida el correo y su dominio con el catálogo local, obtiene el horario del grupo de MySQL, crea un PDF y lo adjunta a un mensaje SMTP.

Para habilitar el envío real, configura cuatro **variables de entorno en Windows** antes de iniciar Visual Studio:

```powershell
[Environment]::SetEnvironmentVariable("AMBAR_SMTP_HOST","smtp.ejemplo.com","User")
[Environment]::SetEnvironmentVariable("AMBAR_SMTP_PORT","587","User")
[Environment]::SetEnvironmentVariable("AMBAR_SMTP_USER","cuenta@ejemplo.com","User")
[Environment]::SetEnvironmentVariable("AMBAR_SMTP_PASSWORD","CLAVE_DE_APLICACION","User")
```

Sustituye los ejemplos por el servidor, dirección y contraseña de aplicación de un buzón autorizado. **Nunca guardes contraseñas en GitHub, scripts SQL, capturas o appsettings.json.** Para mayor seguridad, introduce el secreto mediante la configuración de Windows en tu propio equipo. Una cuenta SMTP debe permitir autenticación por contraseña de aplicación y STARTTLS en el puerto seleccionado; si tu proveedor exige OAuth2, la integración debe modificarse para usar ese método.

**Requisitos:** `Sql/12_dominios_correo_local.sql` ejecutado y conectividad al servidor SMTP. La validación del dominio es completamente local y **no verifica que la dirección exista**; el envío necesita red. La modalidad anterior de Inscripción y sus expedientes no cambian.

## Limitaciones
- La cuenta del alumno no se guarda todavía como dato persistente en `Reinscripcion_Demostracion`: se captura antes de enviar el correo.
- El envío solo está habilitado inmediatamente después de guardar la reinscripción en esa ventana.
- Los horarios del segundo semestre son propuestos para fines escolares, no horarios oficiales confirmados.
- La modificación del repositorio no equivale a una compilación o prueba con MySQL/SMTP. Validar en Visual Studio antes de usar con alumnado real.
