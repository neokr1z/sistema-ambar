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
