-- Carga de las tres imágenes de horarios ISC, agosto-diciembre 2026.
-- Selección: grupos 1 y 2 de los semestres 1, 3 y 5. Capacidad solicitada: 25.
-- Ejecutar después de 06. Reejecutable, transaccional; conserva fichas ya utilizadas.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_carga_2026$$
CREATE PROCEDURE ambar_carga_2026()
BEGIN
    DECLARE carrera INT;
    DECLARE periodo INT;
    DECLARE grupo INT;
    DECLARE semestre INT;
    DECLARE n INT DEFAULT 1;
    DECLARE gate INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    SELECT id INTO gate FROM Control_Identidad WHERE id=1 FOR UPDATE;
    INSERT INTO Carrera(clave_carrera,nombre_carrera,duracion_semestres,estatus)
        SELECT 'ISC','Ingeniería en Sistemas Computacionales',9,'ACTIVO'
        WHERE NOT EXISTS(SELECT 1 FROM Carrera WHERE clave_carrera='ISC');
    SELECT ID_Carrera INTO carrera FROM Carrera WHERE clave_carrera='ISC';
    IF (SELECT COUNT(*) FROM Periodo_Escolar WHERE nombre_periodo='Agosto-Diciembre 2026')>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay periodos Agosto-Diciembre 2026 duplicados; revisarlos antes de cargar.';
    END IF;
    -- Las imágenes sólo indican meses; estos límites abarcan el periodo, no son el calendario oficial.
    INSERT INTO Periodo_Escolar(nombre_periodo,fecha_inicio,fecha_fin,estatus)
        SELECT 'Agosto-Diciembre 2026','2026-08-01','2026-12-31','ACTIVO'
        WHERE NOT EXISTS(SELECT 1 FROM Periodo_Escolar WHERE nombre_periodo='Agosto-Diciembre 2026');
    SELECT ID_Periodo INTO periodo FROM Periodo_Escolar WHERE nombre_periodo='Agosto-Diciembre 2026';
    -- Reutiliza un plan activo si ya existe. El plan auxiliar no representa una clave oficial.
    INSERT INTO Plan_Estudio(clave_carrera,nombre_plan,version,fecha_inicio_vigencia,estatus)
        SELECT 'ISC','Carga académica agosto-diciembre 2026','AMBAR-2026','2026-08-01','ACTIVO'
        WHERE NOT EXISTS(SELECT 1 FROM Plan_Estudio WHERE clave_carrera='ISC' AND UPPER(estatus)='ACTIVO');

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(1,'Semestre 1')
        ON DUPLICATE KEY UPDATE numero_semestre=1;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=1;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='1ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 1ISC1 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '1ISC1',carrera,semestre,periodo,25,'ACTIVO','MATUTINO',1
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='1ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='1ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 1ISC1 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 1ISC1 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='MATUTINO',habilitado_fichas=1 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Taller de Ética','Martes','07:00:00','08:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Taller de Ética','Miércoles','07:00:00','08:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Taller de Ética','Jueves','07:00:00','08:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Taller de Ética','Viernes','07:00:00','08:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Fundamentos de Programación','Lunes','08:00:00','09:00:00','G301',NULL),
        (grupo,'Fundamentos de Programación','Martes','08:00:00','09:00:00','G301',NULL),
        (grupo,'Fundamentos de Programación','Miércoles','08:00:00','09:00:00','G301',NULL),
        (grupo,'Fundamentos de Programación','Jueves','08:00:00','09:00:00','G301',NULL),
        (grupo,'Fundamentos de Programación','Viernes','08:00:00','09:00:00','G301',NULL),
        (grupo,'Cálculo Diferencial','Lunes','12:00:00','13:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Martes','12:00:00','13:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Miércoles','12:00:00','13:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Jueves','12:00:00','13:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Viernes','12:00:00','13:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Matemáticas Discretas','Lunes','10:00:00','11:00:00','G301',NULL),
        (grupo,'Matemáticas Discretas','Martes','10:00:00','11:00:00','G301',NULL),
        (grupo,'Matemáticas Discretas','Miércoles','10:00:00','11:00:00','G301',NULL),
        (grupo,'Matemáticas Discretas','Jueves','10:00:00','11:00:00','G301',NULL),
        (grupo,'Matemáticas Discretas','Viernes','10:00:00','11:00:00','G301',NULL),
        (grupo,'Taller de Administración','Lunes','11:00:00','12:00:00','G301','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Taller de Administración','Martes','11:00:00','12:00:00','G301','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Taller de Administración','Miércoles','11:00:00','12:00:00','G301','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Taller de Administración','Jueves','11:00:00','12:00:00','G301','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Fundamentos de Investigación','Lunes','09:00:00','10:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Fundamentos de Investigación','Martes','09:00:00','10:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Fundamentos de Investigación','Jueves','09:00:00','10:00:00','G301','José Trinidad Quintana Castellanos'),
        (grupo,'Fundamentos de Investigación','Viernes','09:00:00','10:00:00','G301','José Trinidad Quintana Castellanos')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(1,'Semestre 1')
        ON DUPLICATE KEY UPDATE numero_semestre=1;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=1;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='1ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 1ISC2 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '1ISC2',carrera,semestre,periodo,25,'ACTIVO','VESPERTINO',1
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='1ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='1ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 1ISC2 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 1ISC2 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='VESPERTINO',habilitado_fichas=1 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Taller de Ética','Lunes','17:00:00','18:00:00','G301','Martha Hernández Najera'),
        (grupo,'Taller de Ética','Martes','17:00:00','18:00:00','G301','Martha Hernández Najera'),
        (grupo,'Taller de Ética','Miércoles','17:00:00','18:00:00','G301','Martha Hernández Najera'),
        (grupo,'Taller de Ética','Jueves','17:00:00','18:00:00','G301','Martha Hernández Najera'),
        (grupo,'Fundamentos de Programación','Lunes','12:00:00','13:00:00','CB1',NULL),
        (grupo,'Fundamentos de Programación','Martes','12:00:00','13:00:00','CB1',NULL),
        (grupo,'Fundamentos de Programación','Miércoles','12:00:00','13:00:00','CB1',NULL),
        (grupo,'Fundamentos de Programación','Jueves','12:00:00','13:00:00','CB1',NULL),
        (grupo,'Fundamentos de Programación','Viernes','12:00:00','13:00:00','CB1',NULL),
        (grupo,'Cálculo Diferencial','Lunes','13:00:00','14:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Martes','13:00:00','14:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Miércoles','13:00:00','14:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Jueves','13:00:00','14:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Cálculo Diferencial','Viernes','13:00:00','14:00:00','G301','Antonio Rodríguez Román'),
        (grupo,'Matemáticas Discretas','Lunes','15:00:00','16:00:00','G301','Raúl Rumbo Rojas'),
        (grupo,'Matemáticas Discretas','Martes','15:00:00','16:00:00','G301','Raúl Rumbo Rojas'),
        (grupo,'Matemáticas Discretas','Miércoles','15:00:00','16:00:00','G301','Raúl Rumbo Rojas'),
        (grupo,'Matemáticas Discretas','Jueves','15:00:00','16:00:00','G301','Raúl Rumbo Rojas'),
        (grupo,'Matemáticas Discretas','Viernes','15:00:00','16:00:00','G301','Raúl Rumbo Rojas'),
        (grupo,'Taller de Administración','Lunes','14:00:00','15:00:00','R701','Diana Gozos Guadalupe Peña Guzmán'),
        (grupo,'Taller de Administración','Martes','14:00:00','15:00:00','R701','Diana Gozos Guadalupe Peña Guzmán'),
        (grupo,'Taller de Administración','Miércoles','14:00:00','15:00:00','R701','Diana Gozos Guadalupe Peña Guzmán'),
        (grupo,'Taller de Administración','Jueves','14:00:00','15:00:00','R701','Diana Gozos Guadalupe Peña Guzmán'),
        (grupo,'Fundamentos de Investigación','Lunes','16:00:00','17:00:00','G301','Silvestre Bedolla Solano'),
        (grupo,'Fundamentos de Investigación','Martes','16:00:00','17:00:00','G301','Silvestre Bedolla Solano'),
        (grupo,'Fundamentos de Investigación','Miércoles','16:00:00','17:00:00','G301','Silvestre Bedolla Solano'),
        (grupo,'Fundamentos de Investigación','Jueves','16:00:00','17:00:00','G301','Silvestre Bedolla Solano')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(3,'Semestre 3')
        ON DUPLICATE KEY UPDATE numero_semestre=3;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=3;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='3ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 3ISC1 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '3ISC1',carrera,semestre,periodo,25,'ACTIVO','MATUTINO',0
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='3ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='3ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 3ISC1 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 3ISC1 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='MATUTINO',habilitado_fichas=0 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Física General','Lunes','07:00:00','08:00:00','R702','Miguel Ángel Cabello Quintero'),
        (grupo,'Física General','Martes','07:00:00','08:00:00','R702','Miguel Ángel Cabello Quintero'),
        (grupo,'Física General','Miércoles','07:00:00','08:00:00','R702','Miguel Ángel Cabello Quintero'),
        (grupo,'Física General','Jueves','07:00:00','08:00:00','R702','Miguel Ángel Cabello Quintero'),
        (grupo,'Física General','Viernes','07:00:00','08:00:00','R702','Miguel Ángel Cabello Quintero'),
        (grupo,'Estructura de Datos','Lunes','08:00:00','09:00:00','LVS4','Daniel Solís Chávez'),
        (grupo,'Estructura de Datos','Martes','08:00:00','09:00:00','LVS4','Daniel Solís Chávez'),
        (grupo,'Estructura de Datos','Miércoles','08:00:00','09:00:00','LVS4','Daniel Solís Chávez'),
        (grupo,'Estructura de Datos','Jueves','08:00:00','09:00:00','LVS4','Daniel Solís Chávez'),
        (grupo,'Estructura de Datos','Viernes','08:00:00','09:00:00','LVS4','Daniel Solís Chávez'),
        (grupo,'Investigación de Operaciones','Lunes','09:00:00','10:00:00','R702','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Martes','09:00:00','10:00:00','R702','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Miércoles','09:00:00','10:00:00','R702','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Jueves','09:00:00','10:00:00','R702','Gilberto Vales Ortiz'),
        (grupo,'Desarrollo Sustentable','Lunes','10:00:00','11:00:00','R702','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Desarrollo Sustentable','Martes','10:00:00','11:00:00','R702','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Desarrollo Sustentable','Miércoles','10:00:00','11:00:00','R702','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Desarrollo Sustentable','Jueves','10:00:00','11:00:00','R702','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Desarrollo Sustentable','Viernes','10:00:00','11:00:00','R702','Lizeth Julieta Mendoza Vázquez'),
        (grupo,'Cálculo Vectorial','Lunes','11:00:00','12:00:00','R702','Luis Alberto Bedolla Melendez'),
        (grupo,'Cálculo Vectorial','Martes','11:00:00','12:00:00','R702','Luis Alberto Bedolla Melendez'),
        (grupo,'Cálculo Vectorial','Miércoles','11:00:00','12:00:00','R702','Luis Alberto Bedolla Melendez'),
        (grupo,'Cálculo Vectorial','Jueves','11:00:00','12:00:00','R702','Luis Alberto Bedolla Melendez'),
        (grupo,'Cálculo Vectorial','Viernes','11:00:00','12:00:00','R702','Luis Alberto Bedolla Melendez'),
        (grupo,'Cultura Empresarial','Lunes','12:00:00','13:00:00','R702','Yaerd Zarate Liquidano'),
        (grupo,'Cultura Empresarial','Martes','12:00:00','13:00:00','R702','Yaerd Zarate Liquidano'),
        (grupo,'Cultura Empresarial','Miércoles','12:00:00','13:00:00','R702','Yaerd Zarate Liquidano'),
        (grupo,'Cultura Empresarial','Jueves','12:00:00','13:00:00','R702','Yaerd Zarate Liquidano')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(3,'Semestre 3')
        ON DUPLICATE KEY UPDATE numero_semestre=3;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=3;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='3ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 3ISC2 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '3ISC2',carrera,semestre,periodo,25,'ACTIVO','VESPERTINO',0
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='3ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='3ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 3ISC2 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 3ISC2 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='VESPERTINO',habilitado_fichas=0 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Física General','Lunes','16:00:00','17:00:00','G306','Esther Gallardo Cabrera'),
        (grupo,'Física General','Martes','16:00:00','17:00:00','G306','Esther Gallardo Cabrera'),
        (grupo,'Física General','Miércoles','16:00:00','17:00:00','G306','Esther Gallardo Cabrera'),
        (grupo,'Física General','Jueves','16:00:00','17:00:00','G306','Esther Gallardo Cabrera'),
        (grupo,'Física General','Viernes','16:00:00','17:00:00','G306','Esther Gallardo Cabrera'),
        (grupo,'Estructura de Datos','Lunes','14:00:00','15:00:00','G301','Juan Miguel Hernández Bravo'),
        (grupo,'Estructura de Datos','Martes','14:00:00','15:00:00','G301','Juan Miguel Hernández Bravo'),
        (grupo,'Estructura de Datos','Miércoles','14:00:00','15:00:00','G301','Juan Miguel Hernández Bravo'),
        (grupo,'Estructura de Datos','Jueves','14:00:00','15:00:00','G301','Juan Miguel Hernández Bravo'),
        (grupo,'Estructura de Datos','Viernes','14:00:00','15:00:00','G301','Juan Miguel Hernández Bravo'),
        (grupo,'Investigación de Operaciones','Lunes','13:00:00','14:00:00','G306','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Martes','13:00:00','14:00:00','G306','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Miércoles','13:00:00','14:00:00','G306','Gilberto Vales Ortiz'),
        (grupo,'Investigación de Operaciones','Jueves','13:00:00','14:00:00','G306','Gilberto Vales Ortiz'),
        (grupo,'Desarrollo Sustentable','Lunes','15:00:00','16:00:00','R701','José Luis Quiroz Cisneros'),
        (grupo,'Desarrollo Sustentable','Martes','15:00:00','16:00:00','R701','José Luis Quiroz Cisneros'),
        (grupo,'Desarrollo Sustentable','Miércoles','15:00:00','16:00:00','R701','José Luis Quiroz Cisneros'),
        (grupo,'Desarrollo Sustentable','Jueves','15:00:00','16:00:00','R701','José Luis Quiroz Cisneros'),
        (grupo,'Desarrollo Sustentable','Viernes','15:00:00','16:00:00','R701','José Luis Quiroz Cisneros'),
        (grupo,'Cálculo Vectorial','Lunes','17:00:00','18:00:00','R704','Diana Maritza Navarrete Rodríguez'),
        (grupo,'Cálculo Vectorial','Martes','17:00:00','18:00:00','R704','Diana Maritza Navarrete Rodríguez'),
        (grupo,'Cálculo Vectorial','Miércoles','17:00:00','18:00:00','R704','Diana Maritza Navarrete Rodríguez'),
        (grupo,'Cálculo Vectorial','Jueves','17:00:00','18:00:00','R704','Diana Maritza Navarrete Rodríguez'),
        (grupo,'Cálculo Vectorial','Viernes','17:00:00','18:00:00','R704','Diana Maritza Navarrete Rodríguez'),
        (grupo,'Cultura Empresarial','Lunes','18:00:00','19:00:00','R701','Miriam Janet Gallardo Santos'),
        (grupo,'Cultura Empresarial','Martes','18:00:00','19:00:00','R701','Miriam Janet Gallardo Santos'),
        (grupo,'Cultura Empresarial','Miércoles','18:00:00','19:00:00','R701','Miriam Janet Gallardo Santos'),
        (grupo,'Cultura Empresarial','Jueves','18:00:00','19:00:00','R701','Miriam Janet Gallardo Santos')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(5,'Semestre 5')
        ON DUPLICATE KEY UPDATE numero_semestre=5;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=5;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='5ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 5ISC1 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '5ISC1',carrera,semestre,periodo,25,'ACTIVO','MATUTINO',0
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='5ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='5ISC1' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 5ISC1 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 5ISC1 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='MATUTINO',habilitado_fichas=0 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Gráficación','Lunes','10:00:00','11:00:00','R706',NULL),
        (grupo,'Gráficación','Martes','10:00:00','11:00:00','R706',NULL),
        (grupo,'Gráficación','Miércoles','10:00:00','11:00:00','R706',NULL),
        (grupo,'Gráficación','Jueves','10:00:00','11:00:00','R706',NULL),
        (grupo,'Fund. de Telecomunicaciones','Lunes','12:00:00','13:00:00','R706','Francisco Javier Gutierrez Mata'),
        (grupo,'Fund. de Telecomunicaciones','Martes','12:00:00','13:00:00','R706','Francisco Javier Gutierrez Mata'),
        (grupo,'Fund. de Telecomunicaciones','Miércoles','12:00:00','13:00:00','R706','Francisco Javier Gutierrez Mata'),
        (grupo,'Fund. de Telecomunicaciones','Jueves','12:00:00','13:00:00','R706','Francisco Javier Gutierrez Mata'),
        (grupo,'Taller de Bases de Datos','Lunes','09:00:00','10:00:00','R706',NULL),
        (grupo,'Taller de Bases de Datos','Martes','09:00:00','10:00:00','R706',NULL),
        (grupo,'Taller de Bases de Datos','Miércoles','09:00:00','10:00:00','R706',NULL),
        (grupo,'Taller de Bases de Datos','Jueves','09:00:00','10:00:00','R706',NULL),
        (grupo,'Fund. de Ingeniería de Software','Lunes','07:00:00','08:00:00','R706',NULL),
        (grupo,'Fund. de Ingeniería de Software','Martes','07:00:00','08:00:00','R706',NULL),
        (grupo,'Fund. de Ingeniería de Software','Miércoles','07:00:00','08:00:00','R706',NULL),
        (grupo,'Fund. de Ingeniería de Software','Jueves','07:00:00','08:00:00','R706',NULL),
        (grupo,'Arquitectura de Computadoras','Lunes','11:00:00','12:00:00','R706','Oscar Arzeta Armenta'),
        (grupo,'Arquitectura de Computadoras','Martes','11:00:00','12:00:00','R706','Oscar Arzeta Armenta'),
        (grupo,'Arquitectura de Computadoras','Miércoles','11:00:00','12:00:00','R706','Oscar Arzeta Armenta'),
        (grupo,'Arquitectura de Computadoras','Jueves','11:00:00','12:00:00','R706','Oscar Arzeta Armenta'),
        (grupo,'Arquitectura de Computadoras','Viernes','11:00:00','12:00:00','R706','Oscar Arzeta Armenta'),
        (grupo,'Sistemas Operativos','Lunes','08:00:00','09:00:00','R711','Juan Baños Montes'),
        (grupo,'Sistemas Operativos','Martes','08:00:00','09:00:00','R711','Juan Baños Montes'),
        (grupo,'Sistemas Operativos','Miércoles','08:00:00','09:00:00','R711','Juan Baños Montes'),
        (grupo,'Sistemas Operativos','Jueves','08:00:00','09:00:00','R711','Juan Baños Montes')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    INSERT INTO Semestre(numero_semestre,descripcion) VALUES(5,'Semestre 5')
        ON DUPLICATE KEY UPDATE numero_semestre=5;
    SELECT ID_Semestre INTO semestre FROM Semestre WHERE numero_semestre=5;
    IF (SELECT COUNT(*) FROM Grupo WHERE nombre_grupo='5ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo)>1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Grupo 5ISC2 duplicado; revisar antes de cargar.';
    END IF;
    INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus,turno,habilitado_fichas)
        SELECT '5ISC2',carrera,semestre,periodo,25,'ACTIVO','VESPERTINO',0
        WHERE NOT EXISTS(SELECT 1 FROM Grupo WHERE nombre_grupo='5ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo);
    SELECT ID_grupo INTO grupo FROM Grupo WHERE nombre_grupo='5ISC2' AND ID_Carrera=carrera AND ID_Periodo=periodo;
    IF (SELECT COUNT(*) FROM Inscripcion WHERE ID_grupo=grupo AND UPPER(estatus)='INSCRITO')>25 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 5ISC2 ya supera 25 inscritos; no se redujo el cupo.';
    END IF;
    IF (SELECT ID_Semestre FROM Grupo WHERE ID_grupo=grupo)<>semestre THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El grupo 5ISC2 existente pertenece a otro semestre.';
    END IF;
    UPDATE Grupo SET Capacidad=25,turno='VESPERTINO',habilitado_fichas=0 WHERE ID_grupo=grupo;
    INSERT INTO Horario_Referencia(ID_grupo,asignatura,dia,hora_inicio,hora_fin,aula,profesor) VALUES
        (grupo,'Gráficación','Lunes','15:00:00','16:00:00','R706','Juan Carlos Gómez Ramírez'),
        (grupo,'Gráficación','Martes','15:00:00','16:00:00','R706','Juan Carlos Gómez Ramírez'),
        (grupo,'Gráficación','Miércoles','15:00:00','16:00:00','R706','Juan Carlos Gómez Ramírez'),
        (grupo,'Gráficación','Jueves','15:00:00','16:00:00','R706','Juan Carlos Gómez Ramírez'),
        (grupo,'Fund. de Telecomunicaciones','Lunes','16:00:00','17:00:00','R706','Fernando Ríos Escalera'),
        (grupo,'Fund. de Telecomunicaciones','Martes','16:00:00','17:00:00','R706','Fernando Ríos Escalera'),
        (grupo,'Fund. de Telecomunicaciones','Miércoles','16:00:00','17:00:00','R706','Fernando Ríos Escalera'),
        (grupo,'Fund. de Telecomunicaciones','Jueves','16:00:00','17:00:00','R706','Fernando Ríos Escalera'),
        (grupo,'Taller de Bases de Datos','Lunes','13:00:00','14:00:00','R711','Eleazar Pacheco Salazar'),
        (grupo,'Taller de Bases de Datos','Martes','13:00:00','14:00:00','R711','Eleazar Pacheco Salazar'),
        (grupo,'Taller de Bases de Datos','Miércoles','13:00:00','14:00:00','R711','Eleazar Pacheco Salazar'),
        (grupo,'Taller de Bases de Datos','Jueves','13:00:00','14:00:00','R711','Eleazar Pacheco Salazar'),
        (grupo,'Fund. de Ingeniería de Software','Lunes','17:00:00','18:00:00','R706','Carolina Astudillo Hernández'),
        (grupo,'Fund. de Ingeniería de Software','Martes','17:00:00','18:00:00','R706','Carolina Astudillo Hernández'),
        (grupo,'Fund. de Ingeniería de Software','Miércoles','17:00:00','18:00:00','R706','Carolina Astudillo Hernández'),
        (grupo,'Fund. de Ingeniería de Software','Jueves','17:00:00','18:00:00','R706','Carolina Astudillo Hernández'),
        (grupo,'Arquitectura de Computadoras','Lunes','14:00:00','15:00:00','R706',NULL),
        (grupo,'Arquitectura de Computadoras','Martes','14:00:00','15:00:00','R706',NULL),
        (grupo,'Arquitectura de Computadoras','Miércoles','14:00:00','15:00:00','R706',NULL),
        (grupo,'Arquitectura de Computadoras','Jueves','14:00:00','15:00:00','R706',NULL),
        (grupo,'Arquitectura de Computadoras','Viernes','14:00:00','15:00:00','R706',NULL),
        (grupo,'Sistemas Operativos','Lunes','12:00:00','13:00:00','LVS5','Adrián Morales Gálvez'),
        (grupo,'Sistemas Operativos','Martes','12:00:00','13:00:00','LVS5','Adrián Morales Gálvez'),
        (grupo,'Sistemas Operativos','Miércoles','12:00:00','13:00:00','LVS5','Adrián Morales Gálvez'),
        (grupo,'Sistemas Operativos','Jueves','12:00:00','13:00:00','LVS5','Adrián Morales Gálvez')
    ON DUPLICATE KEY UPDATE id=Horario_Referencia.id;

    WHILE n<=50 DO
        IF EXISTS(SELECT 1 FROM Aspirante WHERE numero_ficha=CONCAT('ISC26-',LPAD(n,3,'0'))
            AND (periodo_fichas IS NULL OR periodo_fichas<>periodo)) THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Un folio ISC26 ya existe fuera de esta carga; no se sobrescribió.';
        END IF;
        INSERT INTO Aspirante(numero_ficha,curp,promedio_preparatoria,nombre_preparatoria,
            resultado_examen,estatus,periodo_fichas)
            SELECT CONCAT('ISC26-',LPAD(n,3,'0')),NULL,NULL,NULL,'APROBADO','ACTIVO',periodo
            WHERE NOT EXISTS(SELECT 1 FROM Aspirante WHERE numero_ficha=CONCAT('ISC26-',LPAD(n,3,'0')));
        SET n=n+1;
    END WHILE;
    COMMIT;
END$$
DELIMITER ;
CALL ambar_carga_2026();
DROP PROCEDURE ambar_carga_2026;

SELECT g.nombre_grupo,s.numero_semestre,g.turno,g.Capacidad,p.nombre_periodo
FROM Grupo g JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
WHERE p.nombre_periodo='Agosto-Diciembre 2026' AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2')
ORDER BY g.nombre_grupo;
SELECT numero_ficha,estatus,curp FROM Aspirante
WHERE numero_ficha BETWEEN 'ISC26-001' AND 'ISC26-050' ORDER BY numero_ficha;
