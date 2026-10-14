-- Ejecutar con una cuenta DBA en MySQL, SOLO si no existe el usuario ambar_app.
-- Cambiar la contraseña ANTES de ejecutar. Ajustar host según la instalación.
CREATE USER IF NOT EXISTS 'ambar_app'@'localhost'
    IDENTIFIED BY 'CAMBIAR_ESTA_CONTRASENA';
GRANT SELECT, INSERT, UPDATE, DELETE
    ON AMBAR_Control_Escolar.* TO 'ambar_app'@'localhost';
-- El DDL/migraciones sólo las ejecuta el DBA; la aplicación NO necesita esos permisos.
