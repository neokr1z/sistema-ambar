# AMBAR | Modificar y dar de baja alumnos reinscritos

## Funciones habilitadas
En **Consulta de alumnos** (Administrador o Secretaría), al seleccionar un registro de tipo **REINSCRIPCIÓN** con estado `INSCRITO`:
- **Modificar**: abre formulario independiente con número de control, nombre(s), apellidos, semestre (2.º o 3.º según generación) y grupo. El correo institucional se conserva o regenera automáticamente si cambia el nombre; no se captura CURP, fecha de nacimiento, domicilio ni NSS.
- **Dar de baja** (reutiliza el botón «Ocultar registro»): solicita motivo obligatorio y confirmación; cambia `Reinscripcion_Demostracion.estatus` a `BAJA` y registra el movimiento en `Bitacora` dentro de una transacción. No se borran los datos.

Los registros con `BAJA` **permanecen visibles en Consulta de alumnos**, pero no se cuentan como inscritos ni aparecen en las listas de grupos activos o vistas por semestre. Su lugar en el grupo queda disponible. Los botones Modificar, Dar de baja y Ver horario se deshabilitan para estos registros. No se ha implementado reactivación.

Los expedientes completos de **Inscripción** conservan sus botones originales «Modificar» y «Ocultar registro» sin cambio de funcionamiento.

## Validaciones de modificación
- Número de control: exactamente 10 dígitos. Generación 2026 solamente a 2.º; 2025 a 2.º (repetidor) o 3.º.
- No se permite reutilizar números de control ya presentes en `Alumno` o en otra reinscripción, ni conflictos de inscripción activa del periodo.
- El grupo de destino debe pertenecer al semestre y periodo actual y contar con cupo. Se permite conservar el grupo actual incluso si está lleno.
- Las modificaciones y las bajas se ejecutan como transacciones con el bloqueo de `Control_Identidad`, y se registran en Bitácora.
- El correo se regenera sin duplicarse frente a `Alumno`, `Docente`, `Personal_Administrativo` y otras reinscripciones.

## Base de datos
**No se necesita una nueva migración SQL.** Se reutilizan:
- `Reinscripcion_Demostracion` (incluyendo `estatus` y `correo_institucional`);
- `Grupo`, `Semestre` y `Periodo_Escolar` para validar destino;
- `Bitacora` para registrar movimientos;
- `Control_Identidad` para serializar cambios.

Previamente deben estar instalados los scripts 06, 07, 16, 18, 19 y **22** (correo institucional), y los horarios del script 20. Se recomienda 23 para completar los correos de registros antiguos.

## Verificar en MySQL Workbench
```sql
USE AMBAR_Control_Escolar;

SELECT numero_control, nombre, apellido_paterno, apellido_materno,
       anio_ingreso, semestre_destino, ID_grupo, correo_institucional, estatus
FROM Reinscripcion_Demostracion
ORDER BY numero_control;

SELECT Id_bitacora, fecha_hora, tipo_evento, descripcion, id_registro_afectado
FROM Bitacora
WHERE tipo_evento IN ('MODIFICAR_REINSCRIPCION','BAJA_REINSCRIPCION')
ORDER BY fecha_hora DESC;
```

## Pruebas pendientes en Visual Studio / MySQL
1. Modificar nombre y comprobar regeneración de correo.
2. Cambiar de grupo a uno con espacio; verificar horarios y contador.
3. Intentar cambiar a un grupo sin cupo y confirmar el bloqueo.
4. Intentar asignar un número ya usado y comprobar que no se guardan cambios parciales.
5. Dar de baja con motivo, comprobar que sigue visible como `BAJA`, desaparece de listas de activos y libera una plaza.
6. Comprobar que un alumno dado de baja no puede modificarse accidentalmente.
7. Confirmar que la Inscripción de primer semestre sigue utilizando su editor y ocultación originales.

**Limitación:** el código está subido a GitHub, pero aún no se compiló ni probó contra el MySQL local del equipo.
