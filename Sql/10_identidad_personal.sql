-- Ejecutar antes de usar la nueva alta y antes de 11. No inventa identidad ni créditos.
USE AMBAR_Control_Escolar;
SET @ddl=IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Docente' AND COLUMN_NAME='fecha_nacimiento')=0,
 'ALTER TABLE Docente ADD COLUMN fecha_nacimiento DATE NULL','SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;
SET @ddl=IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Personal_Administrativo' AND COLUMN_NAME='fecha_nacimiento')=0,
 'ALTER TABLE Personal_Administrativo ADD COLUMN fecha_nacimiento DATE NULL','SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;
-- NULL distingue información pendiente de datos falsos. El alta manual sigue exigiendo identidad.
ALTER TABLE Docente MODIFY RFC CHAR(13) NULL;
ALTER TABLE Materia MODIFY creditos TINYINT UNSIGNED NULL,
 MODIFY horas_teoricas TINYINT UNSIGNED NULL, MODIFY horas_practicas TINYINT UNSIGNED NULL;
