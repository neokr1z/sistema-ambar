-- AMBAR | Completar correos institucionales en reinscripciones anteriores.
-- Ejecutar DESPUÉS de Sql/22_correo_reinscripcion_minima.sql.
-- Preserva correos ya existentes y evita duplicados con Alumno/Docente/Personal.
-- Regla: nombre(s) sin espacios + inicial de apellido paterno + inicial materno.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4;

DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_completar_correos_reinscripcion $$
CREATE PROCEDURE ambar_completar_correos_reinscripcion()
BEGIN
    DECLARE fin INT DEFAULT 0;
    DECLARE v_control VARCHAR(20);
    DECLARE v_nombre VARCHAR(60);
    DECLARE v_ap VARCHAR(50);
    DECLARE v_am VARCHAR(50);
    DECLARE v_local VARCHAR(100);
    DECLARE v_base VARCHAR(100);
    DECLARE v_candidato VARCHAR(120);
    DECLARE v_sufijo VARCHAR(10);
    DECLARE v_ocupado INT DEFAULT 0;
    DECLARE v_n INT;
    DECLARE cur CURSOR FOR
        SELECT numero_control,nombre,apellido_paterno,apellido_materno
        FROM Reinscripcion_Demostracion
        WHERE correo_institucional IS NULL OR TRIM(correo_institucional)=''
        ORDER BY numero_control;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET fin=1;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;

    START TRANSACTION;
    OPEN cur;
    bucle: LOOP
        FETCH cur INTO v_control,v_nombre,v_ap,v_am;
        IF fin=1 THEN LEAVE bucle; END IF;
        SET v_local=LOWER(CONCAT(TRIM(v_nombre),
            LEFT(TRIM(v_ap),1),LEFT(TRIM(v_am),1)));
        -- Normalizar letras acentuadas y espacios sin alterar el nombre almacenado.
        SET v_local=REPLACE(v_local,'á','a');
        SET v_local=REPLACE(v_local,'é','e');
        SET v_local=REPLACE(v_local,'í','i');
        SET v_local=REPLACE(v_local,'ó','o');
        SET v_local=REPLACE(v_local,'ú','u');
        SET v_local=REPLACE(v_local,'ü','u');
        SET v_local=REPLACE(v_local,'ñ','n');
        SET v_local=REGEXP_REPLACE(v_local,'[^a-z0-9]','');
        IF CHAR_LENGTH(v_local)<3 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Nombre insuficiente para generar correo; revisa el registro.';
        END IF;
        SET v_base=LEFT(v_local,64);
        SET v_n=1;
        buscar: LOOP
            SET v_sufijo=IF(v_n=1,'',CAST(v_n AS CHAR));
            SET v_candidato=CONCAT(LEFT(v_base,64-CHAR_LENGTH(v_sufijo)),
                v_sufijo,'@acapulco.tecnm.mx');
            SELECT
              (SELECT COUNT(*) FROM Alumno WHERE correo_institucional=v_candidato)
             +(SELECT COUNT(*) FROM Docente WHERE correo_institucional=v_candidato)
             +(SELECT COUNT(*) FROM Personal_Administrativo WHERE correo_institucional=v_candidato)
             +(SELECT COUNT(*) FROM Reinscripcion_Demostracion WHERE correo_institucional=v_candidato)
            INTO v_ocupado;
            IF v_ocupado=0 THEN LEAVE buscar; END IF;
            SET v_n=v_n+1;
            IF v_n>99999 THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se pudo asignar un correo único.';
            END IF;
        END LOOP;
        UPDATE Reinscripcion_Demostracion SET correo_institucional=v_candidato
        WHERE numero_control=v_control
          AND (correo_institucional IS NULL OR TRIM(correo_institucional)='');
    END LOOP;
    CLOSE cur;
    COMMIT;
END $$
CALL ambar_completar_correos_reinscripcion() $$
DROP PROCEDURE IF EXISTS ambar_completar_correos_reinscripcion $$
DELIMITER ;

SELECT numero_control, CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno) AS alumno,
       correo_institucional
FROM Reinscripcion_Demostracion
ORDER BY numero_control;
