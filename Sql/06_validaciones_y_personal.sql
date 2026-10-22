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
