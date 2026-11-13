-- AMBAR | 08/08 | Cuatro cortes docentes: semanas 5/9/13/17.
-- La semana 17 se CAPTURA manualmente; al guardarla se bloquean los cuatro cortes.
-- Los registros de Materia, Asignacion_Docente y Horario ya fueron creados por el 06.
-- Requiere schema nuevo y asignaciones de docentes cargadas.
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

