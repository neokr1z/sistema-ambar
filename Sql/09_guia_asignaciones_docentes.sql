-- CONSULTAS DE CONFIGURACIÓN. Este archivo no crea alumnos, maestros ni notas de ejemplo.
-- La funcionalidad docente utiliza las tablas originales; no necesita una migración de esquema.
USE AMBAR_Control_Escolar;

-- 1. Docentes con acceso activo y su identificador para Asignacion_Docente.
SELECT d.ID_docente,CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno) AS Docente,u.nombre_usuario
FROM Docente d JOIN Usuario u ON u.ID_docente=d.ID_docente
WHERE UPPER(d.estatus)='ACTIVO' AND UPPER(u.estatus)='ACTIVO';

-- 2. Grupos, carrera, semestre y periodo.
SELECT g.ID_grupo,g.nombre_grupo,g.ID_Carrera,s.numero_semestre,p.ID_Periodo,p.nombre_periodo,
       p.fecha_inicio,p.fecha_fin,g.Estatus
FROM Grupo g JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
ORDER BY p.fecha_inicio DESC,g.nombre_grupo;

-- 3. Materias operativas registradas. Si está vacío, captura primero el catálogo oficial
-- en Materia y Plan_Materia: claves, créditos y horas. Esos datos no se deducen de los horarios.
SELECT ID_Materia,ID_Carrera,clave_materia,nombre_materia,estatus FROM Materia ORDER BY nombre_materia;

-- 4. Configuración actual: permite detectar materias sin unidades o grupos sin captura abierta.
SELECT ad.Id_asignacion,d.nombre AS Docente,m.nombre_materia,g.nombre_grupo,p.nombre_periodo,
    (SELECT COUNT(*) FROM Unidad_Evaluacion ue WHERE ue.ID_Materia=m.ID_Materia AND UPPER(ue.estatus)='ACTIVO') AS Unidades,
    (SELECT COUNT(*) FROM Periodo_Captura pc WHERE pc.id_periodo=p.ID_Periodo
     AND UPPER(pc.estatus) IN ('ACTIVO','ABIERTO') AND CURDATE() BETWEEN pc.fecha_inicio AND pc.fecha_limite) AS Capturas_abiertas
FROM Asignacion_Docente ad JOIN Docente d ON d.ID_docente=ad.ID_docente
JOIN Materia m ON m.ID_Materia=ad.ID_Materia JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo;

-- PLANTILLAS PARA CONTROL ESCOLAR: reemplaza las variables por IDs/fechas reales antes
-- de ejecutar. Usa un docente activo, una materia de la carrera del grupo y el periodo correcto.
-- No se ejecutan automáticamente, para no inventar asignaciones de profesores.
--
-- SET @docente = ...;
-- SET @materia = ...;
-- SET @grupo = ...;
-- INSERT INTO Asignacion_Docente(ID_docente,ID_Materia,ID_grupo)
-- SELECT @docente,@materia,@grupo WHERE NOT EXISTS (
--   SELECT 1 FROM Asignacion_Docente WHERE ID_docente=@docente AND ID_Materia=@materia AND ID_grupo=@grupo);
--
-- Por cada unidad real del programa de la materia:
-- INSERT INTO Unidad_Evaluacion(ID_Materia,numero_unidad,descripcion,estatus)
-- VALUES(@materia, ..., 'Descripción oficial de la unidad', 'ACTIVO');
--
-- Para habilitar una ventana de captura autorizada:
-- INSERT INTO Periodo_Captura(id_periodo,nombre_captura,semana_programada,fecha_inicio,fecha_limite,estatus)
-- VALUES(..., 'Nombre de la captura', ..., 'AAAA-MM-DD', 'AAAA-MM-DD', 'ACTIVO');
--
-- Horario operativo (cada bloque requiere la asignación creada arriba):
-- INSERT INTO Horario(Id_asignacion,dia,hora_inicio,hora_fin,aula)
-- VALUES(..., 'Lunes', 'HH:MM:SS', 'HH:MM:SS', 'Aula real');
-- Horario_Referencia conserva las imágenes originales; no asigna automáticamente cuentas de docentes.
