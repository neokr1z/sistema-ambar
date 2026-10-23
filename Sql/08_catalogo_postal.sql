-- Ejecutar en MySQL antes de utilizar el domicilio por código postal.
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
