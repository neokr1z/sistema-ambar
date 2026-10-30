# Listas de grupo y diseño unificado

Se integra en main la rama visual que se probó en Windows, y se añaden los siguientes cambios.

## Uso

- Después de guardar una inscripción aparece una confirmación con el número de control. **Generar lista de grupo** abre la consulta digital; **Guardar lista PDF** permite elegir el destino. La inscripción ya está confirmada: cancelar el PDF no la revierte ni vuelve a guardarla.
- En **Materias y grupos**, el docente selecciona una asignación y pulsa **Generar lista de grupo**. Sólo puede consultar o exportar sus grupos. El repositorio comprueba rol, cuenta activa y propiedad de la asignación; vuelve a comprobarlos antes de exportar.
- Las listas incluyen número de control, nombre y estatus. Incluyen alumnos e inscripciones con estatus INSCRITO, ordenados por apellidos; no incluyen expedientes ocultos. El PDF muestra número consecutivo, control y nombre.
- Se corrigió el hover del menú: fondo y texto ya no usan el mismo color. Los botones de acción son azules, con estados hover y pulsado de contraste legible.
- Las tablas usan encabezados institucionales, filas alternadas, separación horizontal, relleno y ajuste de altura para textos largos. El estilo queda en propiedades de los controles en cada Designer.cs, sin aplicar un tema al cargar que borre tus cambios.
- El expediente usa cuatro tarjetas: identificación, inscripción, contacto/salud y domicilio. El horario mantiene horas y días en columnas, incluye sábado y permite exportar PDF.

## NSS

El campo sigue siendo opcional. Si está vacío se guarda NULL, se muestra pendiente en el expediente y está disponible **Tramitar NSS en IMSS**. No se genera un número inventado ni se afirma que se haya realizado una afiliación.

Al capturar un NSS, inscripción y edición validan 11 dígitos ASCII y el dígito de control, rechazan secuencias de un solo dígito y evitan duplicados entre alumnos. La comprobación de duplicados se ejecuta dentro de la transacción bajo el bloqueo compartido de identidad. No se puede confirmar existencia, titularidad ni vigencia ante IMSS mediante el dígito de control. No se imponen reglas de fecha sobre NSS históricos o convencionales.

Fuentes oficiales:
- https://www.imss.gob.mx/tramites/imss02008
- https://www.imss.gob.mx/sites/all/statics/detPrima/2026/manual-usuario.pdf

## Diseñador

ListaGrupoForm, InscripcionResultadoForm, HorarioAlumnoForm y ConsultaDForm tienen interfaz separada en Designer.cs y recursos asociados. HorarioAlumnoForm tiene un constructor vacío sin acceso a MySQL. La tabla del panel docente también está en MainForm.Designer.cs, dentro de teacherPanel (Visible=false hasta que inicia sesión un docente). El botón Ver horario está en el diseñador de ConsultaA.

Editar colores/fuentes en DefaultCellStyle, AlternatingRowsDefaultCellStyle y ColumnHeadersDefaultCellStyle; tamaños en RowTemplate y ColumnHeadersHeight. Los campos dinámicos conservan sus permisos de edición al cargar datos.

## Verificación y límites

- Compilación C# con Roslyn y referencias .NET 8/Windows Forms: sin errores. Persisten advertencias de nulabilidad/campos del diseñador. No sustituye una compilación MSBuild completa en Windows.
- 144 comprobaciones de lógica y validaciones satisfactorias, sin MySQL y con DNS simulado.
- PDF generado y revisado con 0, 25 y 60 alumnos, encabezados repetidos y paginación; la muestra de 25 alumnos ocupa una página.
- Se añaden pruebas de integración para permisos de listas y NSS duplicado en Tests/Program.cs. No se ejecutaron: este entorno no dispone de MySQL.
- Pendiente en Windows: abrir todos los diseñadores; probar hover y escalado 100/125/150%; inscribir y editar contra MySQL; probar lista y PDF con los roles administrador, secretaria y docente.

No se requieren migraciones SQL nuevas. Se mantienen los scripts y la configuración existentes.
