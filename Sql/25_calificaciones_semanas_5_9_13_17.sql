-- AMBAR | Corte docente de las semanas 5, 9, 13 y resultado final 17.
-- Ejecutar UNA VEZ en MySQL Workbench DESPUES de scripts 10, 11, 20 y 24.
-- Conserva Calificacion/Registro_Captura historicos por unidad; no los modifica.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4;

CREATE TABLE IF NOT EXISTS Calificacion_Corte (
    id_calificacion_corte BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    numero_control VARCHAR(20) NOT NULL,
    Id_asignacion INT NOT NULL,
    semana TINYINT UNSIGNED NOT NULL,
    calificacion DECIMAL(5,2) NOT NULL,
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uq_corte_alumno_asignacion_semana(numero_control,Id_asignacion,semana),
    KEY idx_corte_asignacion_semana(Id_asignacion,semana),
    CONSTRAINT fk_corte_asignacion FOREIGN KEY (Id_asignacion)
       REFERENCES Asignacion_Docente(Id_asignacion),
    CONSTRAINT ck_corte_semana CHECK(semana IN (5,9,13,17)),
    CONSTRAINT ck_corte_valor CHECK(
       calificacion=0 OR (calificacion>=70 AND calificacion<=100)
    )
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Evita correcciones después de un cierre individual y hace inmutable semana 17.
DELIMITER $$
DROP TRIGGER IF EXISTS ambar_corte_antes_actualizar $$
CREATE TRIGGER ambar_corte_antes_actualizar BEFORE UPDATE ON Calificacion_Corte
FOR EACH ROW
BEGIN
    IF OLD.semana=17 OR NEW.semana=17 OR
        OLD.numero_control<>NEW.numero_control OR OLD.Id_asignacion<>NEW.Id_asignacion OR
        OLD.semana<>NEW.semana OR
        EXISTS(SELECT 1 FROM Calificacion_Corte f
          WHERE f.numero_control=OLD.numero_control AND f.Id_asignacion=OLD.Id_asignacion
            AND f.semana=17) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
          'La calificacion final es definitiva; no se permite modificar un corte cerrado.';
    END IF;
END $$
DROP TRIGGER IF EXISTS ambar_corte_antes_borrar $$
CREATE TRIGGER ambar_corte_antes_borrar BEFORE DELETE ON Calificacion_Corte
FOR EACH ROW
BEGIN
    IF OLD.semana=17 OR EXISTS(SELECT 1 FROM Calificacion_Corte f
       WHERE f.numero_control=OLD.numero_control
         AND f.Id_asignacion=OLD.Id_asignacion AND f.semana=17) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
          'No se pueden eliminar calificaciones de una materia cerrada.';
    END IF;
END $$
DELIMITER ;

-- Relaciona las materias de los horarios con sus docentes reales. No crea personas ficticias.
-- Las materias faltantes reciben una clave TECNICA, no una clave oficial.
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_preparar_asignaciones_cortes $$
CREATE PROCEDURE ambar_preparar_asignaciones_cortes()
BEGIN
    DECLARE v_faltantes INT DEFAULT 0;
    DECLARE v_ambiguos INT DEFAULT 0;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;

    SELECT COUNT(*) INTO v_faltantes FROM (
        SELECT DISTINCT h.profesor
        FROM Horario_Referencia h
        JOIN Grupo g ON g.ID_grupo=h.ID_grupo
        JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
        JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
        WHERE s.numero_semestre IN (1,2,3)
          AND p.nombre_periodo='Agosto-Diciembre 2026'
          AND COALESCE(TRIM(h.profesor),'')=''
    ) faltas;
    IF v_faltantes>0 THEN
       SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
         'Hay horarios sin docente; completa el script 20 antes de asignar.';
    END IF;

    SELECT COUNT(*) INTO v_faltantes FROM (
        SELECT DISTINCT h.profesor
        FROM Horario_Referencia h
        JOIN Grupo g ON g.ID_grupo=h.ID_grupo
        JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
        JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
        LEFT JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=h.profesor
            AND UPPER(d.estatus)='ACTIVO'
        WHERE s.numero_semestre IN (1,2,3)
          AND p.nombre_periodo='Agosto-Diciembre 2026'
          AND d.ID_docente IS NULL
    ) faltas;
    IF v_faltantes>0 THEN
       SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=
         'Hay profesores de horario sin registro Docente activo; revisa scripts 11 y 20.';
    END IF;

    SELECT COUNT(*) INTO v_ambiguos FROM (
        SELECT h.profesor
        FROM Horario_Referencia h
        JOIN Grupo g ON g.ID_grupo=h.ID_grupo
        JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
        JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
        JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=h.profesor
        WHERE s.numero_semestre IN (1,2,3)
          AND p.nombre_periodo='Agosto-Diciembre 2026'
          AND UPPER(d.estatus)='ACTIVO'
        GROUP BY h.profesor
        HAVING COUNT(DISTINCT d.ID_docente)>1
    ) amb;
    IF v_ambiguos>0 THEN
       SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay docentes con nombres ambiguos; revisa antes de asignar.';
    END IF;

    INSERT INTO Materia(ID_Carrera,clave_materia,nombre_materia,creditos,horas_teoricas,horas_practicas,estatus)
    SELECT DISTINCT g.ID_Carrera,CONCAT('WC26-',LEFT(MD5(h.asignatura),12)),
      h.asignatura,NULL,NULL,NULL,'ACTIVO'
    FROM Horario_Referencia h
    JOIN Grupo g ON g.ID_grupo=h.ID_grupo
    JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
    JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
    LEFT JOIN Materia m ON m.ID_Carrera=g.ID_Carrera
       AND m.nombre_materia=h.asignatura
    WHERE s.numero_semestre IN (1,2,3)
      AND p.nombre_periodo='Agosto-Diciembre 2026'
      AND m.ID_Materia IS NULL;

    SELECT COUNT(*) INTO v_ambiguos FROM (
        SELECT g.ID_grupo,h.asignatura,COUNT(DISTINCT d.ID_docente) AS docentes
        FROM Horario_Referencia h
        JOIN Grupo g ON g.ID_grupo=h.ID_grupo
        JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
        JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
        JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=h.profesor
        WHERE s.numero_semestre IN (1,2,3)
          AND p.nombre_periodo='Agosto-Diciembre 2026'
          AND UPPER(d.estatus)='ACTIVO'
        GROUP BY g.ID_grupo,h.asignatura HAVING docentes<>1
    ) amb;
    IF v_ambiguos>0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Una materia tiene mas de un docente en el mismo grupo.';
    END IF;

    SELECT COUNT(*) INTO v_ambiguos FROM (
        SELECT g.ID_grupo,h.asignatura,COUNT(DISTINCT m.ID_Materia) AS materias
        FROM Horario_Referencia h
        JOIN Grupo g ON g.ID_grupo=h.ID_grupo
        JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
        JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
        JOIN Materia m ON m.ID_Carrera=g.ID_Carrera AND m.nombre_materia=h.asignatura
        WHERE s.numero_semestre IN (1,2,3)
          AND p.nombre_periodo='Agosto-Diciembre 2026'
        GROUP BY g.ID_grupo,h.asignatura HAVING materias<>1
    ) amb;
    IF v_ambiguos>0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay materias duplicadas por nombre en la misma carrera.';
    END IF;

    -- Evitar asignaciones contradictorias con otras ya existentes.
    SELECT COUNT(*) INTO v_ambiguos
    FROM Asignacion_Docente ad
    JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
    JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
    JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
    JOIN Materia m ON m.ID_Materia=ad.ID_Materia
    JOIN Horario_Referencia h ON h.ID_grupo=g.ID_grupo AND h.asignatura=m.nombre_materia
    JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=h.profesor
    WHERE s.numero_semestre IN (1,2,3)
      AND p.nombre_periodo='Agosto-Diciembre 2026'
      AND ad.ID_docente<>d.ID_docente;
    IF v_ambiguos>0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Existe asignacion docente distinta al horario, requiere revision.';
    END IF;

    INSERT INTO Asignacion_Docente(ID_docente,ID_Materia,ID_grupo)
    SELECT DISTINCT d.ID_docente,m.ID_Materia,g.ID_grupo
    FROM Horario_Referencia h
    JOIN Grupo g ON g.ID_grupo=h.ID_grupo
    JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
    JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
    JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=h.profesor
    JOIN Materia m ON m.ID_Carrera=g.ID_Carrera AND m.nombre_materia=h.asignatura
    WHERE s.numero_semestre IN (1,2,3)
      AND p.nombre_periodo='Agosto-Diciembre 2026'
      AND UPPER(d.estatus)='ACTIVO'
      AND NOT EXISTS(SELECT 1 FROM Asignacion_Docente ad
        WHERE ad.ID_grupo=g.ID_grupo AND ad.ID_Materia=m.ID_Materia);

    INSERT INTO Horario(Id_asignacion,dia,hora_inicio,hora_fin,aula)
    SELECT DISTINCT ad.Id_asignacion,h.dia,h.hora_inicio,h.hora_fin,h.aula
    FROM Horario_Referencia h
    JOIN Grupo g ON g.ID_grupo=h.ID_grupo
    JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
    JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
    JOIN Materia m ON m.ID_Carrera=g.ID_Carrera AND m.nombre_materia=h.asignatura
    JOIN Asignacion_Docente ad ON ad.ID_grupo=g.ID_grupo AND ad.ID_Materia=m.ID_Materia
    WHERE s.numero_semestre IN (1,2,3)
      AND p.nombre_periodo='Agosto-Diciembre 2026'
      AND NOT EXISTS(SELECT 1 FROM Horario ho WHERE ho.Id_asignacion=ad.Id_asignacion
         AND ho.dia=h.dia AND ho.hora_inicio=h.hora_inicio AND ho.hora_fin=h.hora_fin);

    COMMIT;
END $$
CALL ambar_preparar_asignaciones_cortes() $$
DROP PROCEDURE IF EXISTS ambar_preparar_asignaciones_cortes $$
DELIMITER ;

-- Confirmar asignaciones de los grupos del periodo.
SELECT g.nombre_grupo,m.nombre_materia,
       CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno) AS docente
FROM Asignacion_Docente ad
JOIN Grupo g ON g.ID_grupo=ad.ID_grupo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
JOIN Materia m ON m.ID_Materia=ad.ID_Materia
JOIN Docente d ON d.ID_docente=ad.ID_docente
WHERE s.numero_semestre IN (1,2,3) AND p.nombre_periodo='Agosto-Diciembre 2026'
ORDER BY g.nombre_grupo,m.nombre_materia;
