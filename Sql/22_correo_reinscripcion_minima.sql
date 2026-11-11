-- AMBAR | Correo institucional automático para reinscripción autónoma.
-- Ejecutar DESPUÉS de Sql/16, 18 y 19. No convierte registros en expedientes completos.
USE AMBAR_Control_Escolar;

DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_correo_reinscripcion_minima $$
CREATE PROCEDURE ambar_correo_reinscripcion_minima()
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Reinscripcion'
        AND COLUMN_NAME='correo_institucional'
    ) THEN
        ALTER TABLE Reinscripcion
            ADD COLUMN correo_institucional VARCHAR(120) NULL;
    END IF;
    IF NOT EXISTS(
        SELECT 1 FROM information_schema.STATISTICS
        WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Reinscripcion'
        AND INDEX_NAME='uq_reinscripcion_correo_institucional'
    ) THEN
        ALTER TABLE Reinscripcion
            ADD UNIQUE KEY uq_reinscripcion_correo_institucional(correo_institucional);
    END IF;
END $$
CALL ambar_correo_reinscripcion_minima() $$
DROP PROCEDURE IF EXISTS ambar_correo_reinscripcion_minima $$
DELIMITER ;

-- Los registros anteriores pueden conservar NULL: no inventamos ni enviamos correos.
-- Los registros nuevos generan el correo automáticamente desde nombre y apellidos.
SELECT numero_control,nombre,apellido_paterno,apellido_materno,
       semestre_destino,correo_institucional
FROM Reinscripcion ORDER BY numero_control;
