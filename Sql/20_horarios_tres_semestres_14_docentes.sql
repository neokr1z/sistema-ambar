-- AMBAR / Semana 6. Horarios completos de 1.º, 2.º y 3.º / 14 profesores sin empalmes.
-- Aplicar tras 06, 07, 16, 18 y 19. Solo se modifica la asignación de docentes
-- de seis materias preexistentes en primero/tercero y se carga el horario de segundo.
-- No crea nuevos grupos de 1.º/3.º. El 18 crea 2ISC1 y 2ISC2.
-- Los horarios de segundo son una propuesta didáctica (lunes a viernes).
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4;
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_horarios_tres_semestres_2026 $$
CREATE PROCEDURE ambar_horarios_tres_semestres_2026()
BEGIN
    DECLARE v_periodo INT;
    DECLARE v_grupo INT;
    DECLARE v_nombre VARCHAR(20);
    DECLARE v_conflictos INT DEFAULT 0;
    DECLARE v_docentes INT DEFAULT 0;
    DECLARE v_materias INT DEFAULT 0;
    DECLARE v_total_grupos INT DEFAULT 0;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    SELECT ID_Periodo INTO v_periodo FROM Periodo_Escolar
      WHERE nombre_periodo='Agosto-Diciembre 2026' LIMIT 1;
    IF v_periodo IS NULL THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Falta periodo agosto-diciembre 2026.'; END IF;
    SELECT COUNT(*) INTO v_total_grupos FROM Grupo g JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
      WHERE g.ID_Periodo=v_periodo AND s.numero_semestre IN (1,2,3)
      AND g.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2');
    IF v_total_grupos<>6 THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Faltan los seis grupos 1ISC1-3ISC2; ejecuta scripts 07 y 18.'; END IF;

    -- Profesores existentes, reutilizados según materias afines.
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='Juan Miguel Hernández Bravo'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='1ISC1' AND h.asignatura='Fundamentos de Programación';
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='Raúl Rumbo Rojas'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='1ISC1' AND h.asignatura='Matemáticas Discretas';
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='Daniel Solís Chávez'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='1ISC2' AND h.asignatura='Fundamentos de Programación';
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='José Trinidad Quintana Castellanos'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='1ISC2' AND h.asignatura='Fundamentos de Investigación';
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='Luis Alberto Bedolla Melendez'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='3ISC2' AND h.asignatura='Cálculo Vectorial';
    UPDATE Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      SET h.profesor='Yaerd Zarate Liquidano'
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo='3ISC2' AND h.asignatura='Cultura Empresarial';

    -- Segundo semestre, los dos grupos creados por el script 18.
    -- Solo se reemplaza el horario de estos dos grupos, no el de primero o tercero.
    DELETE h FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
      WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo IN ('2ISC1','2ISC2');
    SELECT ID_grupo INTO v_grupo FROM Grupo WHERE ID_Periodo=v_periodo AND nombre_grupo='2ISC1' LIMIT 1;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
      (v_grupo,'Cálculo Integral','Lunes','07:00:00','08:00:00','G302','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Martes','07:00:00','08:00:00','G302','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Miércoles','07:00:00','08:00:00','G302','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Jueves','07:00:00','08:00:00','G302','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Viernes','07:00:00','08:00:00','G302','Antonio Rodríguez Román'),
      (v_grupo,'Programación Orientada a Objetos','Lunes','09:00:00','10:00:00','G302','Juan Miguel Hernández Bravo'),
      (v_grupo,'Programación Orientada a Objetos','Martes','09:00:00','10:00:00','G302','Juan Miguel Hernández Bravo'),
      (v_grupo,'Programación Orientada a Objetos','Miércoles','09:00:00','10:00:00','G302','Juan Miguel Hernández Bravo'),
      (v_grupo,'Programación Orientada a Objetos','Jueves','09:00:00','10:00:00','G302','Juan Miguel Hernández Bravo'),
      (v_grupo,'Programación Orientada a Objetos','Viernes','09:00:00','10:00:00','G302','Juan Miguel Hernández Bravo'),
      (v_grupo,'Probabilidad y Estadística','Lunes','10:00:00','11:00:00','G302','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Martes','10:00:00','11:00:00','G302','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Miércoles','10:00:00','11:00:00','G302','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Jueves','10:00:00','11:00:00','G302','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Viernes','10:00:00','11:00:00','G302','Gilberto Vales Ortiz'),
      (v_grupo,'Contabilidad Financiera','Lunes','08:00:00','09:00:00','G302','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Martes','08:00:00','09:00:00','G302','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Miércoles','08:00:00','09:00:00','G302','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Jueves','08:00:00','09:00:00','G302','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Viernes','08:00:00','09:00:00','G302','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Química','Lunes','11:00:00','12:00:00','G302','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Martes','11:00:00','12:00:00','G302','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Miércoles','11:00:00','12:00:00','G302','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Jueves','11:00:00','12:00:00','G302','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Viernes','11:00:00','12:00:00','G302','Esther Gallardo Cabrera'),
      (v_grupo,'Álgebra Lineal','Lunes','12:00:00','13:00:00','G302','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Martes','12:00:00','13:00:00','G302','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Miércoles','12:00:00','13:00:00','G302','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Jueves','12:00:00','13:00:00','G302','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Viernes','12:00:00','13:00:00','G302','Luis Alberto Bedolla Melendez');
    SELECT ID_grupo INTO v_grupo FROM Grupo WHERE ID_Periodo=v_periodo AND nombre_grupo='2ISC2' LIMIT 1;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
      (v_grupo,'Cálculo Integral','Lunes','14:00:00','15:00:00','G303','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Martes','14:00:00','15:00:00','G303','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Miércoles','14:00:00','15:00:00','G303','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Jueves','14:00:00','15:00:00','G303','Antonio Rodríguez Román'),
      (v_grupo,'Cálculo Integral','Viernes','14:00:00','15:00:00','G303','Antonio Rodríguez Román'),
      (v_grupo,'Programación Orientada a Objetos','Lunes','13:00:00','14:00:00','G303','Daniel Solís Chávez'),
      (v_grupo,'Programación Orientada a Objetos','Martes','13:00:00','14:00:00','G303','Daniel Solís Chávez'),
      (v_grupo,'Programación Orientada a Objetos','Miércoles','13:00:00','14:00:00','G303','Daniel Solís Chávez'),
      (v_grupo,'Programación Orientada a Objetos','Jueves','13:00:00','14:00:00','G303','Daniel Solís Chávez'),
      (v_grupo,'Programación Orientada a Objetos','Viernes','13:00:00','14:00:00','G303','Daniel Solís Chávez'),
      (v_grupo,'Probabilidad y Estadística','Lunes','15:00:00','16:00:00','G303','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Martes','15:00:00','16:00:00','G303','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Miércoles','15:00:00','16:00:00','G303','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Jueves','15:00:00','16:00:00','G303','Gilberto Vales Ortiz'),
      (v_grupo,'Probabilidad y Estadística','Viernes','15:00:00','16:00:00','G303','Gilberto Vales Ortiz'),
      (v_grupo,'Contabilidad Financiera','Lunes','16:00:00','17:00:00','G303','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Martes','16:00:00','17:00:00','G303','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Miércoles','16:00:00','17:00:00','G303','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Jueves','16:00:00','17:00:00','G303','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Contabilidad Financiera','Viernes','16:00:00','17:00:00','G303','Diana Gozos Guadalupe Peña Guzmán'),
      (v_grupo,'Química','Lunes','17:00:00','18:00:00','G303','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Martes','17:00:00','18:00:00','G303','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Miércoles','17:00:00','18:00:00','G303','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Jueves','17:00:00','18:00:00','G303','Esther Gallardo Cabrera'),
      (v_grupo,'Química','Viernes','17:00:00','18:00:00','G303','Esther Gallardo Cabrera'),
      (v_grupo,'Álgebra Lineal','Lunes','18:00:00','19:00:00','G303','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Martes','18:00:00','19:00:00','G303','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Miércoles','18:00:00','19:00:00','G303','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Jueves','18:00:00','19:00:00','G303','Luis Alberto Bedolla Melendez'),
      (v_grupo,'Álgebra Lineal','Viernes','18:00:00','19:00:00','G303','Luis Alberto Bedolla Melendez');

    SELECT COUNT(DISTINCT h.profesor) INTO v_docentes
    FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
    WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2');
    IF v_docentes<>14 THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La asignación no tiene exactamente 14 docentes; operación cancelada.'; END IF;

    SELECT COUNT(DISTINCT CONCAT(s.numero_semestre,'/',h.asignatura)) INTO v_materias
    FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
    JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
    WHERE g.ID_Periodo=v_periodo AND g.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2');
    IF v_materias<>18 THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No hay exactamente 18 asignaturas repartidas entre los tres semestres.'; END IF;

    SELECT COUNT(*) INTO v_conflictos
    FROM Horario_Referencia a JOIN Grupo ga ON ga.ID_grupo=a.ID_grupo
    JOIN Horario_Referencia b ON b.id>a.id
        AND b.profesor=a.profesor AND b.dia=a.dia
        AND b.hora_inicio<a.hora_fin AND a.hora_inicio<b.hora_fin
    JOIN Grupo gb ON gb.ID_grupo=b.ID_grupo
    WHERE ga.ID_Periodo=v_periodo AND gb.ID_Periodo=v_periodo
       AND ga.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2')
       AND gb.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2')
       AND a.profesor IS NOT NULL AND a.profesor<>'';
    IF v_conflictos>0 THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Choque de horarios de docentes; operación cancelada.'; END IF;
    COMMIT;
END $$
CALL ambar_horarios_tres_semestres_2026() $$
DROP PROCEDURE IF EXISTS ambar_horarios_tres_semestres_2026 $$
DELIMITER ;

-- Verificación: 14 docentes únicos, 18 materias y cero empalmes dentro de estos seis grupos.
SELECT s.numero_semestre,g.nombre_grupo,h.asignatura,h.profesor,
       h.dia,h.hora_inicio,h.hora_fin
FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
WHERE p.nombre_periodo='Agosto-Diciembre 2026'
AND g.nombre_grupo IN ('1ISC1','1ISC2','2ISC1','2ISC2','3ISC1','3ISC2')
ORDER BY s.numero_semestre,g.nombre_grupo,FIELD(h.dia,'Lunes','Martes','Miércoles','Jueves','Viernes'),h.hora_inicio;
