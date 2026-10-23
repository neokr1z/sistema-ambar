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
