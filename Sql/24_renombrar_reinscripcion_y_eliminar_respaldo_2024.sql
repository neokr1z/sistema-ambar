-- AMBAR | Migracion final: Reinscripcion_Demostracion -> Reinscripcion.
-- IMPORTANTE: haz un respaldo de MySQL antes de ejecutar.
-- Requiere las migraciones 16, 18, 19 y 22 aplicadas en la instalacion anterior.
-- Renombra la tabla en el servidor; CONSERVA sus filas, estatus y correos.
-- Elimina el respaldo 2024 solicitado y recrea las tres vistas dependientes.
USE AMBAR_Control_Escolar;

DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_renombrar_reinscripcion_2026 $$
CREATE PROCEDURE ambar_renombrar_reinscripcion_2026()
BEGIN
    DECLARE v_vieja INT DEFAULT 0;
    DECLARE v_nueva INT DEFAULT 0;
    DECLARE v_invalidos INT DEFAULT 0;

    SELECT COUNT(*) INTO v_vieja FROM information_schema.TABLES
     WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Reinscripcion_Demostracion' AND TABLE_TYPE='BASE TABLE';
    SELECT COUNT(*) INTO v_nueva FROM information_schema.TABLES
     WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Reinscripcion' AND TABLE_TYPE='BASE TABLE';

    IF v_vieja=1 AND v_nueva=1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
          'Existen las dos tablas: revisa datos y duplicados antes de renombrar.';
    ELSEIF v_vieja=0 AND v_nueva=0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
          'No existe tabla de reinscripcion; ejecuta los scripts previos 16-22.';
    END IF;

    IF v_vieja=1 THEN
        SELECT COUNT(*) INTO v_invalidos
          FROM Reinscripcion_Demostracion WHERE anio_ingreso NOT IN (2025,2026);
        IF v_invalidos>0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
              'Existen reinscripciones fuera de 2025-2026: revisalas antes de migrar.';
        END IF;
        RENAME TABLE Reinscripcion_Demostracion TO Reinscripcion;
    END IF;
END $$
CALL ambar_renombrar_reinscripcion_2026() $$
DROP PROCEDURE IF EXISTS ambar_renombrar_reinscripcion_2026 $$
DELIMITER ;

-- Respaldo antiguo: no se necesita para las generaciones operativas 2025-2026.
-- Asegurate de tener un respaldo externo antes de eliminarlo definitivamente.
DROP TABLE IF EXISTS Reinscripcion_Demostracion_Respaldo_2024;

-- Recrear vistas para que apunten al nuevo nombre fisico.
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


-- Comprobaciones de la instalacion final.
SELECT numero_control,anio_ingreso,semestre_destino,ID_grupo,estatus,correo_institucional
FROM Reinscripcion ORDER BY numero_control;
SELECT TABLE_NAME FROM information_schema.TABLES
WHERE TABLE_SCHEMA=DATABASE()
 AND TABLE_NAME IN ('Reinscripcion','Reinscripcion_Demostracion',
                    'Reinscripcion_Demostracion_Respaldo_2024')
ORDER BY TABLE_NAME;
