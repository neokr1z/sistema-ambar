# Interfaz del ZIP sobre la versión actual

Base: `352e96da7392bf9540e14f8714bb08a323c68fef`.

Se incorporan la distribución, tamaños, fuentes, colores, líneas y recursos del ZIP en BitacoraForm, CapturaCLForm, ConsultaDForm, DetalleCapturaForm, HistorialdocForm, InscripcionForm, MateriasDocenteForm, PersonalForm y UsuariosForm. Los otros formularios, el horario y los componentes compartidos ya coinciden visualmente; se conserva su versión actual.

Los archivos de lógica, repositorios, validaciones, SQL y configuración no se sustituyen. Se mantienen los límites de captura actuales (incluido teléfono con +52), los eventos existentes, la CURP vinculada a la identidad, el correo automático, la comprobación del dominio, los permisos y las operaciones asíncronas de guardado. Los eventos vacíos añadidos por el diseñador del ZIP no se importan.

## Verificación

- Comparación de la base local con los hashes de GitHub; la configuración local no se publica.
- Comparación de todos los formularios, recursos y código del ZIP.
- Compilación de C# con Roslyn y referencias de .NET 8/Windows Forms: sin errores; hay advertencias de nulabilidad y un campo sin uso en los diseñadores. Esta comprobación no sustituye una compilación completa con MSBuild ni prueba la carga visual de los recursos.
- 119 comprobaciones existentes de validaciones satisfactorias, con respuestas DNS simuladas. No se ejecutó integración con MySQL ni consultas DNS reales en esta revisión.
- Los archivos de lógica y los enlaces a eventos se conservan respecto a la base.

## Prueba en Windows

Cambiar a esta rama, restaurar paquetes y recompilar la solución en Visual Studio. Abrir los formularios con Shift+F7 y revisar los tamaños a la escala de pantalla utilizada. Probar inscripción, edición, alta de personal, consulta de bitácora y captura de calificaciones con la base de pruebas habitual. No se requieren nuevos scripts SQL por esta mezcla visual.
