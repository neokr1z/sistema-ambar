-- AMBAR | 07/08 | Vistas para consulta por semestre.
-- Tres vistas actualizadas automáticamente, una por semestre.
-- Incluyen alumnos de Inscripción y de Reinscripción autónoma.
-- Requiere Sql/16 y Sql/18.
USE AMBAR_Control_Escolar;
-- Vista de consulta unificada: mantiene las tres tablas transaccionales y evita
-- duplicar las UNION de Alumno e Reinscripcion en los formularios.
-- No crea tabla ni copia datos; combina expedientes y reinscripciones mínimas.
CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Unificados AS
SELECT a.Numero_control AS NumeroControl,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS NombreCompleto,
       a.curp AS Curp,ca.nombre_carrera AS Carrera,
       a.correo_institucional AS Correo,a.estatus AS Estatus,
       s.numero_semestre AS Semestre,g.nombre_grupo AS Grupo,
       'EXPEDIENTE' AS Tipo,pe.nombre_periodo AS Periodo
FROM Alumno a
JOIN Plan_Estudio pl ON pl.id_plan=a.ID_Plan
JOIN Carrera ca ON ca.clave_carrera=pl.clave_carrera
JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
WHERE s.numero_semestre IN (1,2,3) AND i.estatus='INSCRITO'
UNION ALL
SELECT r.numero_control,
       CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
       '',ca.nombre_carrera,COALESCE(r.correo_institucional,''),r.estatus,
       s.numero_semestre,g.nombre_grupo,'REINSCRIPCIÓN',pe.nombre_periodo
FROM Reinscripcion r
JOIN Grupo g ON g.ID_grupo=r.ID_grupo
JOIN Carrera ca ON ca.ID_Carrera=g.ID_Carrera
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
WHERE s.numero_semestre IN (2,3) AND r.estatus IN ('INSCRITO','BAJA');

CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_1 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=1 AND i.estatus='INSCRITO'
AND p.estatus='ACTIVO';

CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_2 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=2 AND i.estatus='INSCRITO'
AND p.estatus='ACTIVO'
UNION ALL
SELECT r.numero_control,CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
       g.nombre_grupo,p.nombre_periodo,'REINSCRIPCION'
FROM Reinscripcion r JOIN Grupo g ON g.ID_grupo=r.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=2 AND r.estatus='INSCRITO'
AND p.estatus='ACTIVO';

CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_3 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=3 AND i.estatus='INSCRITO'
AND p.estatus='ACTIVO'
UNION ALL
SELECT r.numero_control,CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
       g.nombre_grupo,p.nombre_periodo,'REINSCRIPCION'
FROM Reinscripcion r JOIN Grupo g ON g.ID_grupo=r.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=3 AND r.estatus='INSCRITO'
AND p.estatus='ACTIVO';

SELECT '1.º' AS semestre,COUNT(*) AS alumnos FROM vw_AMBAR_Alumnos_Semestre_1
UNION ALL SELECT '2.º',COUNT(*) FROM vw_AMBAR_Alumnos_Semestre_2
UNION ALL SELECT '3.º',COUNT(*) FROM vw_AMBAR_Alumnos_Semestre_3;
