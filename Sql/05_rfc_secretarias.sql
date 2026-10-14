-- Ejecutar después de 02_migracion_y_administrador.sql, antes de usar la nueva alta.
-- Conserva las claves administrativas y las cuentas existentes.
USE AMBAR_Control_Escolar;
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Personal_Administrativo' AND COLUMN_NAME='RFC')=0,
    'ALTER TABLE Personal_Administrativo ADD COLUMN RFC CHAR(13) NULL', 'SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Personal_Administrativo' AND INDEX_NAME='uq_administrativo_rfc')=0,
    'ALTER TABLE Personal_Administrativo ADD UNIQUE INDEX uq_administrativo_rfc (RFC)', 'SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;
-- NULL permite conservar personal anterior cuyo RFC aún no se ha capturado.
-- Las altas nuevas exigen un RFC válido en UsersForm antes de abrir la transacción.
