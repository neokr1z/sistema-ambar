-- AMBAR | 04/08 | Reinscripcion minima 2025/2026 + dos grupos de 2.º.
-- Crear la tabla definitiva directamente: sin respaldos ni migraciones de demostracion.
-- AMBAR · Reinscripción autónoma de alumnos (Semana 6)
-- Requiere los grupos y horarios de 07_grupos_y_fichas_2026.sql.
-- Este módulo guarda registros mínimos independientes del expediente completo.
USE AMBAR_Control_Escolar;
CREATE TABLE IF NOT EXISTS Reinscripcion (
    numero_control VARCHAR(20) NOT NULL PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50) NOT NULL,
    anio_ingreso SMALLINT UNSIGNED NOT NULL,
    semestre_destino TINYINT UNSIGNED NOT NULL,
    ID_grupo INT NOT NULL,
    periodo VARCHAR(50) NOT NULL DEFAULT 'Agosto-Diciembre 2026',
    correo_institucional VARCHAR(120) NULL UNIQUE,
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estatus VARCHAR(20) NOT NULL DEFAULT 'INSCRITO',
    CONSTRAINT ck_reins_generacion CHECK
       ((anio_ingreso=2026 AND semestre_destino=2)
        OR (anio_ingreso=2025 AND semestre_destino IN (2,3))),
    CONSTRAINT fk_reins_grupo FOREIGN KEY(ID_grupo) REFERENCES Grupo(ID_grupo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- No se insertan alumnos predefinidos. Los datos se capturan en la ventana de Reinscripción.
-- El número de control se valida por su FORMA y AÑO, no por existencia en Alumno.
SELECT COUNT(*) AS 'Reinscripciones'
FROM Reinscripcion;

-- 3. Crear grupos de segundo semestre (sin inventar profesores ni horarios).
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_grupos_segundo_2026 $$
CREATE PROCEDURE ambar_grupos_segundo_2026()
BEGIN
    DECLARE v_carrera INT;
    DECLARE v_periodo INT;
    DECLARE v_semestre INT;

    SELECT ID_Carrera INTO v_carrera FROM Carrera WHERE clave_carrera='ISC' LIMIT 1;
    SELECT ID_Periodo INTO v_periodo FROM Periodo_Escolar
    WHERE nombre_periodo='Agosto-Diciembre 2026' LIMIT 1;

    IF v_carrera IS NULL OR v_periodo IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Faltan carrera ISC o periodo Agosto-Diciembre 2026. Ejecuta la carga 07.';
    END IF;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(2,'Semestre 2')
    ON DUPLICATE KEY UPDATE numero_semestre=VALUES(numero_semestre);
    SELECT ID_Semestre INTO v_semestre FROM Semestre WHERE numero_semestre=2;

    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
    SELECT '2ISC1',v_carrera,v_semestre,v_periodo,25,'ACTIVO','MATUTINO',0
    WHERE NOT EXISTS(SELECT 1 FROM Grupo
      WHERE nombre_grupo='2ISC1' AND ID_Carrera=v_carrera AND ID_Periodo=v_periodo);
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
    SELECT '2ISC2',v_carrera,v_semestre,v_periodo,25,'ACTIVO','VESPERTINO',0
    WHERE NOT EXISTS(SELECT 1 FROM Grupo
      WHERE nombre_grupo='2ISC2' AND ID_Carrera=v_carrera AND ID_Periodo=v_periodo);

    IF EXISTS(SELECT 1 FROM Grupo g
        WHERE g.nombre_grupo IN ('2ISC1','2ISC2') AND g.ID_Carrera=v_carrera
        AND g.ID_Periodo=v_periodo AND g.ID_Semestre<>v_semestre) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Un grupo 2ISC existente tiene otro semestre. Revisa antes de continuar.';
    END IF;
END $$
CALL ambar_grupos_segundo_2026() $$
DROP PROCEDURE IF EXISTS ambar_grupos_segundo_2026 $$
DELIMITER ;

