-- AMBAR | Ajuste de Reinscripción autónoma: 2026 → 2.º y 2025 → 3.º.
-- Ejecutar una vez DESPUÉS de Sql/16_reinscripcion_autonoma.sql y Sql/07_grupos_y_fichas_2026.sql.
-- Periodo de destino: Agosto-Diciembre 2026.
-- El histórico de generación 2024 se conserva en una tabla de respaldo (no se cuenta en el nuevo módulo).
USE AMBAR_Control_Escolar;

-- 1. El histórico de 2024 ya no participa en este módulo.
-- No se crea ni se vuelve a generar una tabla de respaldo 2024.
-- La limpieza del respaldo antiguo se hace una sola vez con el script 24.

-- 2. Actualizar la restricción de generación.
ALTER TABLE Reinscripcion
    DROP CHECK ck_reins_demo_generacion;
ALTER TABLE Reinscripcion
    ADD CONSTRAINT ck_reins_demo_generacion CHECK
    ((anio_ingreso=2026 AND semestre_destino=2)
      OR (anio_ingreso=2025 AND semestre_destino IN (2,3)));

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

-- 4. Comprobar grupos actuales y que se bloqueó la generación 2024.
SELECT s.numero_semestre, g.nombre_grupo, g.turno, g.Capacidad
FROM Grupo g JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
WHERE pe.nombre_periodo='Agosto-Diciembre 2026'
  AND s.numero_semestre IN (1,2,3)
ORDER BY s.numero_semestre,g.nombre_grupo;
