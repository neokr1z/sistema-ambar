-- AMBAR | 10 | Actualización de una base YA INSTALADA (no es parte de la instalación desde cero).
-- 1) Exporta un respaldo antes de ejecutarlo.
-- 2) Es reejecutable: cada paso se omite si ya se aplicó.
-- Cambios: bloqueo de login por intentos fallidos y eliminación de tablas sin uso
--          (Documento, Envio_Documento y Plan_Materia no las usa ninguna parte de la aplicación).
USE AMBAR_Control_Escolar;

-- ---- Bloqueo por intentos fallidos de inicio de sesión ----
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Usuario' AND COLUMN_NAME='intentos_fallidos')=0,
    'ALTER TABLE Usuario ADD COLUMN intentos_fallidos TINYINT UNSIGNED NOT NULL DEFAULT 0', 'SELECT 1');
PREPARE migration FROM @ddl; EXECUTE migration; DEALLOCATE PREPARE migration;

SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Usuario' AND COLUMN_NAME='bloqueado_hasta')=0,
    'ALTER TABLE Usuario ADD COLUMN bloqueado_hasta DATETIME NULL', 'SELECT 1');
PREPARE migration FROM @ddl; EXECUTE migration; DEALLOCATE PREPARE migration;

-- ---- Tablas antiguas de módulos todavía no implementados ----
-- Reejecutable; se omiten tablas inexistentes y se conserva cualquier dato antiguo.
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_limpiar_tablas_antiguas$$
CREATE PROCEDURE ambar_limpiar_tablas_antiguas()
BEGIN
    DECLARE fin INT DEFAULT 0;
    DECLARE tabla VARCHAR(64);
    DECLARE n BIGINT DEFAULT 0;
    DECLARE tablas CURSOR FOR
      SELECT table_name FROM information_schema.tables
      WHERE table_schema=DATABASE() AND table_name IN ('Envio_Documento','Documento','Plan_Materia');
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET fin=1;
    OPEN tablas;
    ciclo: LOOP
      FETCH tablas INTO tabla;
      IF fin=1 THEN LEAVE ciclo; END IF;
      SET @sql=CONCAT('SELECT COUNT(*) INTO @filas_antiguas FROM `',tabla,'`');
      PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
      SET n=@filas_antiguas;
      IF n>0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay datos en una tabla antigua. No se elimina: respalda y revisa primero.';
      END IF;
    END LOOP;
    CLOSE tablas;
END$$
CALL ambar_limpiar_tablas_antiguas()$$
DROP PROCEDURE ambar_limpiar_tablas_antiguas$$
DELIMITER ;
DROP TABLE IF EXISTS Envio_Documento;
DROP TABLE IF EXISTS Documento;
DROP TABLE IF EXISTS Plan_Materia;
