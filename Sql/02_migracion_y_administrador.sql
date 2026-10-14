-- SEMANA 4. Ejecutar DESPUÉS de 01_tablas_ambar_original.sql.
-- Operaciones idempotentes: migración de dos campos y alta del administrador.
USE AMBAR_Control_Escolar;

-- Campo requerido por el alta de personal. El esquema original sólo incluye apellidos.
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Personal_Administrativo' AND COLUMN_NAME='nombre')=0,
    'ALTER TABLE Personal_Administrativo ADD COLUMN nombre VARCHAR(60) NOT NULL DEFAULT '''' AFTER num_administrativo',
    'SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;

-- Correo personal de alumnos, ausente en la versión SQL original.
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Alumno' AND COLUMN_NAME='correo_propio')=0,
    'ALTER TABLE Alumno ADD COLUMN correo_propio VARCHAR(120) NULL AFTER correo_institucional',
    'SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;

-- La primera sesión del administrador exige cambiar la contraseña temporal.
SET @ddl = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='Usuario' AND COLUMN_NAME='debe_cambiar_password')=0,
    'ALTER TABLE Usuario ADD COLUMN debe_cambiar_password TINYINT(1) NOT NULL DEFAULT 0',
    'SELECT 1');
PREPARE migration FROM @ddl;
EXECUTE migration;
DEALLOCATE PREPARE migration;

INSERT INTO Rol(nombre,descripcion) VALUES
    ('Administrador','Control total del sistema y recuperación de registros'),
    ('Secretaria','Inscripción, consulta y mantenimiento de alumnos'),
    ('Docente','Gestión académica y calificaciones')
ON DUPLICATE KEY UPDATE descripcion=VALUES(descripcion);

-- La cuenta admin se precarga con contraseña PBKDF2-SHA256, no en texto plano.
-- INSERT IGNORE conserva su contraseña cuando el script se vuelve a ejecutar.
INSERT IGNORE INTO Usuario(nombre_usuario,password_hash,Id_rol,estatus,ID_docente,num_administrativo,debe_cambiar_password)
VALUES ('admin','AMBAR:250000:KsmFPzz4qFCqhlrKI3zCTA==:FjJdXiWDuDtnorokQQnoZFQpFpqxhVml0YjePFEasdU=',(SELECT id_rol FROM Rol WHERE nombre='Administrador'),
    'ACTIVO',NULL,NULL,1);

-- IMPORTANTE: cambiar clave admin después del primer ingreso y eliminar el archivo de claves iniciales.
