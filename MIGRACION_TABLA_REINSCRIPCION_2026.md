# AMBAR | Tabla definitiva Reinscripcion (2025 y 2026)

## Cambios en C#
- Se corrigió el error de compilación CS7036 en `ConsultaA.cs`: el `Padding` de la ventana de baja ahora tiene los cuatro argumentos exigidos por el constructor.
- Reinscripción, consulta de alumnos, listas de grupos/PDF, panel de indicadores, edición y baja lógica utilizan ahora la tabla **`Reinscripcion`**.
- La tabla sigue siendo independiente de `Alumno`, no pide CURP ni expediente completo y conserva los correos generados.

## Migración obligatoria en MySQL Workbench

**No basta con descargar la nueva rama.** La tabla en tu servidor sigue teniendo el nombre anterior hasta ejecutar la migración.

1. **Realiza un respaldo de la base de datos `AMBAR_Control_Escolar`** desde MySQL Workbench (Data Export), incluyendo estructura y datos. Guarda el archivo fuera del proyecto.
2. Comprueba que los scripts previos 16, 18, 19 y 22 están aplicados. El script 23 es opcional para completar correos antiguos.
3. Ejecuta exclusivamente **`Sql/24_renombrar_reinscripcion_y_eliminar_respaldo_2024.sql`** en esa base. No vuelvas a ejecutar las migraciones anteriores sobre la base que ya está al día.
4. Confirma que aparece `Reinscripcion` y desaparecen `Reinscripcion_Demostracion` y `Reinscripcion_Demostracion_Respaldo_2024`.
5. Descarga la rama actualizada, compila y prueba Consulta, Modificar, Dar de baja, listas/PDF y el contador del panel principal.

La migración:
- **Renombra** la tabla sin copiar o borrar las reinscripciones de 2025/2026 y conserva sus índices, estatus, correo y relaciones.
- **Detiene la operación** si existen ambas tablas o si todavía hay registros activos de generaciones distintas de 2025/2026.
- Elimina de forma explícita y definitiva el respaldo 2024, como se solicitó.
- Recrea las tres vistas `vw_AMBAR_Alumnos_Semestre_1`, `_2` y `_3` con referencias nuevas.
- Puede ejecutarse de nuevo para reconstruir las vistas; no vuelve a renombrar una tabla que ya tenga el nombre final.

> Si sale un error de MySQL, no elimines ninguna tabla manualmente: copia el mensaje para revisar el estado. DDL de MySQL hace confirmaciones implícitas, por lo que un error después de renombrar puede requerir reejecutar este script.

## Verificación

```sql
USE AMBAR_Control_Escolar;

SHOW TABLES LIKE 'Reinscripcion';
SHOW TABLES LIKE 'Reinscripcion_Demostracion%';

SELECT numero_control,nombre,apellido_paterno,apellido_materno,
       anio_ingreso,semestre_destino,ID_grupo,correo_institucional,estatus
FROM Reinscripcion
ORDER BY numero_control;

SELECT * FROM vw_AMBAR_Alumnos_Semestre_2;
SELECT * FROM vw_AMBAR_Alumnos_Semestre_3;
```

## Nota sobre otras advertencias
Las siete advertencias anteriores de Visual Studio son independientes de CS7036. La alerta de seguridad de `MySqlConnector 2.4.0` debe atenderse actualizando la dependencia de manera controlada y probando compatibilidad; no se modificó NuGet en este cambio.

## Estado de verificación
Los cambios están confirmados en el repositorio. No hay acceso a tu instancia de MySQL ni a Visual Studio para ejecutar la migración o confirmar la compilación local.
