-- AMBAR | 02/08 | Seguridad, validaciones y catalogos
-- Incluye antiguos scripts 02,05,06,08,10,12. Catalogo_Postal requiere datos SEPOMEX adicionales.

-- ====== 02_migracion_y_administrador.sql ======
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


-- ====== 05_rfc_secretarias.sql ======
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


-- ====== 06_validaciones_y_personal.sql ======
-- MySQL 8.0+. Ejecutar después de 02 y 05. Reejecutable; no borra registros existentes.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE TABLE IF NOT EXISTS Control_Identidad (
    id INT PRIMARY KEY
) ENGINE=InnoDB;
INSERT INTO Control_Identidad(id) VALUES(1) ON DUPLICATE KEY UPDATE id=1;

DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_agregar_columna$$
CREATE PROCEDURE ambar_agregar_columna(IN tabla VARCHAR(64), IN columna VARCHAR(64), IN definicion VARCHAR(255))
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=tabla AND COLUMN_NAME=columna) THEN
        SET @ddl=CONCAT('ALTER TABLE `',tabla,'` ADD COLUMN `',columna,'` ',definicion);
        PREPARE stmt FROM @ddl; EXECUTE stmt; DEALLOCATE PREPARE stmt;
    END IF;
END$$
DELIMITER ;
CALL ambar_agregar_columna('Docente','curp','CHAR(18) NULL');
CALL ambar_agregar_columna('Personal_Administrativo','curp','CHAR(18) NULL');
CALL ambar_agregar_columna('Aspirante','periodo_fichas','INT NULL');
CALL ambar_agregar_columna('Grupo','turno','VARCHAR(15) NULL');
CALL ambar_agregar_columna('Grupo','habilitado_fichas','TINYINT(1) NOT NULL DEFAULT 0');
DROP PROCEDURE ambar_agregar_columna;
-- Las fichas disponibles no son personas ficticias. Su CURP se captura al inscribir.
ALTER TABLE Aspirante MODIFY curp CHAR(18) NULL,
    MODIFY promedio_preparatoria DECIMAL(4,2) NULL,
    MODIFY nombre_preparatoria VARCHAR(120) NULL;

-- Referencia fiel de las imágenes: no crea docentes con RFC/credenciales inventados
-- ni inventa créditos, claves de materias o distribución teórica/práctica.
CREATE TABLE IF NOT EXISTS Horario_Referencia (
    id INT AUTO_INCREMENT PRIMARY KEY,
    ID_grupo INT NOT NULL,
    asignatura VARCHAR(120) NOT NULL,
    dia VARCHAR(15) NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    aula VARCHAR(20) NOT NULL,
    profesor VARCHAR(160) NULL,
    UNIQUE KEY uq_horario_referencia (ID_grupo,asignatura,dia,hora_inicio),
    FOREIGN KEY (ID_grupo) REFERENCES Grupo(ID_grupo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
-- Los nombres se normalizan en Validation.Name al guardar desde AMBAR.
-- Identidad.Check evita duplicados entre alumnos/docentes/secretarias (incluye bajas),
-- bajo un bloqueo transaccional compartido. No se fusionan personas históricas.


-- ====== 08_catalogo_postal.sql ======

-- Catálogo: importar los registros oficiales SEPOMEX desde Correos de México.
-- https://www.correosdemexico.gob.mx/sslservicios/consultacp/CodigoPostal_Exportar.aspx
USE AMBAR_Control_Escolar;
CREATE TABLE IF NOT EXISTS Catalogo_Postal (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  codigo_postal CHAR(5) NOT NULL,
  colonia VARCHAR(120) NOT NULL,
  localidad VARCHAR(120) NOT NULL,
  municipio VARCHAR(120) NOT NULL,
  estado VARCHAR(120) NOT NULL,
  KEY idx_cp (codigo_postal),
  UNIQUE KEY uq_postal (codigo_postal,colonia,municipio,estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- IMPORTANTE: esta estructura no incluye datos ficticios.
-- Importar el catálogo de códigos postales antes de registrar alumnos.
-- Columnas SEPOMEX: d_codigo -> codigo_postal; d_asenta -> colonia;
-- d_ciudad -> localidad (si no existe, usar D_mnpio);
-- D_mnpio -> municipio; d_estado -> estado.
-- Ante un CP inexistente en el catálogo cargado, AMBAR NO permitirá guardar.

-- Importación recomendada: descargar TXT desde Correos de México (separador |).
-- MySQL Workbench: primero importar a tabla temporal o transformar el TXT a CSV
-- con columnas codigo_postal,colonia,localidad,municipio,estado.
-- En localidades donde d_ciudad sea vacío, usar D_mnpio como localidad.
-- Ejemplo para archivo CSV UTF-8 preparado con encabezado:
-- LOAD DATA LOCAL INFILE 'C:/ruta/catalogo_postal.csv'
-- INTO TABLE Catalogo_Postal
-- CHARACTER SET utf8mb4
-- FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
-- LINES TERMINATED BY '\\n'
-- IGNORE 1 LINES
-- (codigo_postal,colonia,localidad,municipio,estado);


-- ====== 10_identidad_personal.sql ======
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


-- ====== 12_dominios_correo_local.sql ======
-- Ejecutar una sola vez en la base de datos AMBAR, sin requerir Internet.
CREATE TABLE IF NOT EXISTS Dominio_Correo (
    dominio VARCHAR(253) NOT NULL PRIMARY KEY,
    activo TINYINT(1) NOT NULL DEFAULT 1
);
INSERT INTO Dominio_Correo (dominio,activo) VALUES
('gmail.com',1),('googlemail.com',1),('outlook.com',1),('hotmail.com',1),
('hotmail.com.mx',1),('live.com',1),('live.com.mx',1),('msn.com',1),
('yahoo.com',1),('yahoo.com.mx',1),('ymail.com',1),('icloud.com',1),
('me.com',1),('mac.com',1),('aol.com',1),('proton.me',1),('protonmail.com',1),
('zoho.com',1),('mail.com',1),('gmx.com',1),('gmx.es',1),
('acapulco.tecnm.mx',1),('tecnm.mx',1)
ON DUPLICATE KEY UPDATE dominio=VALUES(dominio);
-- Para agregar un dominio legítimo:
-- INSERT INTO Dominio_Correo (dominio, activo) VALUES ('universidad.edu.mx',1)
-- ON DUPLICATE KEY UPDATE activo=1;
