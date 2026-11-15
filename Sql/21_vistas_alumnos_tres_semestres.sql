-- Tres vistas actualizadas automáticamente, una por semestre.
-- Incluyen alumnos de Inscripción y de Reinscripción autónoma.
-- Requiere Sql/16 y Sql/18.
USE AMBAR_Control_Escolar;
CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_1 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=1 AND UPPER(i.estatus)='INSCRITO'
AND p.nombre_periodo='Agosto-Diciembre 2026';

CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_2 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=2 AND UPPER(i.estatus)='INSCRITO'
AND p.nombre_periodo='Agosto-Diciembre 2026'
UNION ALL
SELECT r.numero_control,CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
       g.nombre_grupo,p.nombre_periodo,'REINSCRIPCION'
FROM Reinscripcion r JOIN Grupo g ON g.ID_grupo=r.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=2 AND UPPER(r.estatus)='INSCRITO'
AND p.nombre_periodo='Agosto-Diciembre 2026';

CREATE OR REPLACE VIEW vw_AMBAR_Alumnos_Semestre_3 AS
SELECT a.Numero_control AS numero_control,
       CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS alumno,
       g.nombre_grupo AS grupo,p.nombre_periodo AS periodo,
       'INSCRIPCION' AS origen
FROM Alumno a JOIN Inscripcion i ON i.Numero_control=a.Numero_control
JOIN Grupo g ON g.ID_grupo=i.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=3 AND UPPER(i.estatus)='INSCRITO'
AND p.nombre_periodo='Agosto-Diciembre 2026'
UNION ALL
SELECT r.numero_control,CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
       g.nombre_grupo,p.nombre_periodo,'REINSCRIPCION'
FROM Reinscripcion r JOIN Grupo g ON g.ID_grupo=r.ID_grupo
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
WHERE s.numero_semestre=3 AND UPPER(r.estatus)='INSCRITO'
AND p.nombre_periodo='Agosto-Diciembre 2026';

SELECT '1.º' AS semestre,COUNT(*) AS alumnos FROM vw_AMBAR_Alumnos_Semestre_1
UNION ALL SELECT '2.º',COUNT(*) FROM vw_AMBAR_Alumnos_Semestre_2
UNION ALL SELECT '3.º',COUNT(*) FROM vw_AMBAR_Alumnos_Semestre_3;
