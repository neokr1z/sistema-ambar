-- Docentes y asignaciones de los seis grupos de 07; ejecutar después de 10.
-- Reejecutable: no reinicia contraseñas, no reactiva bajas ni cambia asignaciones existentes.
-- Nuevos usuarios: contraseña solicitada, almacenada como PBKDF2-SHA256 con sal individual.
-- RFC/CURP/fecha/telefono pendientes: NULL. Claves HOR26-* internas, créditos/horas pendientes.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;
DROP TEMPORARY TABLE IF EXISTS ambar_docentes_carga;
CREATE TEMPORARY TABLE ambar_docentes_carga(
 completo VARCHAR(160) PRIMARY KEY,nombre VARCHAR(60),ap VARCHAR(50),am VARCHAR(50),
 usuario VARCHAR(50),correo_base VARCHAR(64),hash_clave VARCHAR(255)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
INSERT INTO ambar_docentes_carga VALUES
('Adrián Morales Gálvez','Adrián','Morales','Gálvez','adrian.morales.galvez','AdrianMG','AMBAR:250000:Xa57rPEC+ImrmX2WY70f1w==:kl5zI/d3wJBbGdIov3nlmnVJd3WO4AXterUJeEXVoB4='),
('Antonio Rodríguez Román','Antonio','Rodríguez','Román','antonio.rodriguez.roman','AntonioRR','AMBAR:250000:MDtyT/57OwoAg/mTZjnvGA==:KDTfjlvt/2R9jq7K+Pk/1l71j9CQlSYaV8nhOKrZ8yM='),
('Carolina Astudillo Hernández','Carolina','Astudillo','Hernández','carolina.astudillo.hernandez','CarolinaAH','AMBAR:250000:CqQcxPKdM90lik8OpmGL2g==:wOHHVUvSuZ5tw7mYC1qAWx82/PAkp7zUibeEeBldK8Y='),
('Daniel Solís Chávez','Daniel','Solís','Chávez','daniel.solis.chavez','DanielSC','AMBAR:250000:7SK+E2fxLAIQhcjRgOVKVg==:hf2gye73HLYVyKrlEa/1EKq1Zx6zAwx/iXFXfFQjIis='),
('Diana Gozos Guadalupe Peña Guzmán','Diana Gozos Guadalupe','Peña','Guzmán','diana.pena.guzman','DianaGozosGuadalupePG','AMBAR:250000:hZ2kj+K2nb6p3Nr6+XcpAQ==:MdnEBDTS6saOoEfPyS683+x13HQtlLgXIBEfdxA0S0U='),
('Diana Maritza Navarrete Rodríguez','Diana Maritza','Navarrete','Rodríguez','diana.navarrete.rodriguez','DianaMaritzaNR','AMBAR:250000:bQnHd/SIqKW5rtm7X/WpOQ==:QCba7tahYDHgtprQCIAC/iZbFcj/pzn0xHNrdrDkDr4='),
('Eleazar Pacheco Salazar','Eleazar','Pacheco','Salazar','eleazar.pacheco.salazar','EleazarPS','AMBAR:250000:iOJhSQ0iXCZTwQQGUkP8nQ==:FKG2ik6JAsE+CNNJrecFdM8J4KOXwoZCpirc5l/2cho='),
('Esther Gallardo Cabrera','Esther','Gallardo','Cabrera','esther.gallardo.cabrera','EstherGC','AMBAR:250000:cH6dWIyLknPD7J3ysgOvhw==:gRWbT5ArdIzu7AcUCWs4hvJA/swfeHjiPufhsnzMCD0='),
('Fernando Ríos Escalera','Fernando','Ríos','Escalera','fernando.rios.escalera','FernandoRE','AMBAR:250000:ATNKH4eP0aKGTJSya1zEoQ==:R6f1whEL7T2J3mrE/wM2kglY1UUQSD49xXsk+JqaGUU='),
('Francisco Javier Gutierrez Mata','Francisco Javier','Gutierrez','Mata','francisco.gutierrez.mata','FranciscoJavierGM','AMBAR:250000:V2P4//eJhCwhfVc0mlJwZw==:Kx9ojrHYiB6gSoNWO3v5u3Go/jQXd6EmD4KyJASEq5w='),
('Gilberto Vales Ortiz','Gilberto','Vales','Ortiz','gilberto.vales.ortiz','GilbertoVO','AMBAR:250000:raK5/sKRXo2JqqSQL1y0bQ==:lnHMddabHTR0Y/Dpy6MqLdx8StRA3sGBGTaA29S/kLw='),
('José Luis Quiroz Cisneros','José Luis','Quiroz','Cisneros','jose.quiroz.cisneros','JoseLuisQC','AMBAR:250000:3Sc2S7vx5u9WknbkugYm4w==:g/lio2LnfnkFGdLFvDeb7cxmFvJvlWB+qTr0XRVWZGk='),
('José Trinidad Quintana Castellanos','José Trinidad','Quintana','Castellanos','jose.quintana.castellanos','JoseTrinidadQC','AMBAR:250000:rsP+AcghLolQHTzQ/yFOog==:dPkEVh0fpti8H5jnp0hKlEGTtJwS2wXmofHRwqRRu5c='),
('Juan Baños Montes','Juan','Baños','Montes','juan.banos.montes','JuanBM','AMBAR:250000:QjkT3VCgyAKgdFl3KuYGqA==:9IvWJud03q/6HPOV9R/mHXfE4bP+rGUmyYxvJQm5KY0='),
('Juan Carlos Gómez Ramírez','Juan Carlos','Gómez','Ramírez','juan.gomez.ramirez','JuanCarlosGR','AMBAR:250000:ZRKFqBSdLfFBq4mQYR+sYA==:B8Vd6lKNTUexWeVt3dK/Q2ihNEQ1CbguxZgO+EiaZao='),
('Juan Miguel Hernández Bravo','Juan Miguel','Hernández','Bravo','juan.hernandez.bravo','JuanMiguelHB','AMBAR:250000:tVVD5QNgE20PAhxhaI2HHg==:7O+AAqNiSCwxplRN3U8NkXczjeckEPtqnaJRTbBkZbI='),
('Lizeth Julieta Mendoza Vázquez','Lizeth Julieta','Mendoza','Vázquez','lizeth.mendoza.vazquez','LizethJulietaMV','AMBAR:250000:ejdVUynFSXdIhzQawCrzuQ==:JNOHGrJ7WDfO7Dwn9tvTQHQdvW5T4BC0agiIdeJPNhY='),
('Luis Alberto Bedolla Melendez','Luis Alberto','Bedolla','Melendez','luis.bedolla.melendez','LuisAlbertoBM','AMBAR:250000:W2LsaZBvxP+WNxff51omkg==:kuMPXjQ5Tl/hFvLZgksAnGNtGPQQ/KWQSRQ3oqTBSxs='),
('Martha Hernández Najera','Martha','Hernández','Najera','martha.hernandez.najera','MarthaHN','AMBAR:250000:1QOvGCXkQ5SsO+InuqdTrw==:BR2ctdi9VFJWpb0+hehh7xeRWdY+IOPpfpsBvNxog3M='),
('Miguel Ángel Cabello Quintero','Miguel Ángel','Cabello','Quintero','miguel.cabello.quintero','MiguelAngelCQ','AMBAR:250000:SH3PAZXEE2byu4eiVxbJBQ==:m6/W53IGUGQUD19RQH9ouHJU7NANjKG7L1tbxHBlN8g='),
('Miriam Janet Gallardo Santos','Miriam Janet','Gallardo','Santos','miriam.gallardo.santos','MiriamJanetGS','AMBAR:250000:GlcTLPno8Jmsvi9SRNOxlg==:8Rj5e6CKz58dQhvNcVQPYsLxgzt5xJ85FzFF7QAprWY='),
('Oscar Arzeta Armenta','Oscar','Arzeta','Armenta','oscar.arzeta.armenta','OscarAA','AMBAR:250000:jfRuCG6MAzmYZ0awoViHew==:/iDJ9ZC4yIG7spdakF9n5UvH0cy8bd5DEA362qsuNt0='),
('Raúl Rumbo Rojas','Raúl','Rumbo','Rojas','raul.rumbo.rojas','RaulRR','AMBAR:250000:Bk1RzRhLzZBBX4DbIgbKlw==:bC/VK0tFK0zFwPHGZSoKN8Zbb6ip3He4+SSwHf2dC+Q='),
('Silvestre Bedolla Solano','Silvestre','Bedolla','Solano','silvestre.bedolla.solano','SilvestreBS','AMBAR:250000:z1BANiz0qk3k3NZ97nB/7w==:5X91b+TBKiijwTnrvbbLB9s7WDqmo6C/Wiy/ioXxhn8='),
('Yaerd Zarate Liquidano','Yaerd','Zarate','Liquidano','yaerd.zarate.liquidano','YaerdZL','AMBAR:250000:dRdmBdGnH1qxTqk3Snkbrw==:dm5OuMH9X8f05WTcx6WrmTrwCszlzzpK8V1TatBkRrw=');
DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_asignar_docentes_2026$$
CREATE PROCEDURE ambar_asignar_docentes_2026()
BEGIN
 DECLARE done INT DEFAULT 0;
 DECLARE gate, carrera, periodo, rol, doc, mat, grp, asign, n, actor INT;
 DECLARE full_name, given_name, ap_name, am_name, user_base, email_base, hash_value VARCHAR(255);
 DECLARE user_value VARCHAR(50);
 DECLARE email_value VARCHAR(120);
 DECLARE subject_name VARCHAR(120);
 DECLARE internal_key VARCHAR(20);
 DECLARE teachers CURSOR FOR SELECT completo,nombre,ap,am,usuario,correo_base,hash_clave FROM ambar_docentes_carga ORDER BY completo;
 DECLARE subjects CURSOR FOR
  SELECT DISTINCT h.ID_grupo,h.asignatura,h.profesor FROM Horario_Referencia h
  JOIN Grupo g ON g.ID_grupo=h.ID_grupo
  WHERE g.ID_Carrera=carrera AND g.ID_Periodo=periodo
   AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2')
  ORDER BY h.asignatura,h.ID_grupo;
 DECLARE CONTINUE HANDLER FOR NOT FOUND SET done=1;
 DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
 START TRANSACTION;
 SELECT id INTO gate FROM Control_Identidad WHERE id=1 FOR UPDATE;
 SET carrera=(SELECT ID_Carrera FROM Carrera WHERE clave_carrera='ISC');
 SET periodo=(SELECT ID_Periodo FROM Periodo_Escolar WHERE nombre_periodo='Agosto-Diciembre 2026');
 SET rol=(SELECT id_rol FROM Rol WHERE nombre='Docente');
 SET actor=(SELECT MIN(u.Id_usuario) FROM Usuario u JOIN Rol r ON r.id_rol=u.Id_rol WHERE r.nombre='Administrador' AND UPPER(u.estatus)='ACTIVO');
 IF carrera IS NULL OR periodo IS NULL OR rol IS NULL OR gate IS NULL OR actor IS NULL THEN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Faltan catálogo, administrador o carga 06/07. Ejecutar los scripts previos.';
 END IF;
 IF (SELECT COUNT(*) FROM Grupo WHERE ID_Carrera=carrera AND ID_Periodo=periodo AND nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2'))<>6
 OR (SELECT COUNT(*) FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo WHERE g.ID_Carrera=carrera AND g.ID_Periodo=periodo AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2'))<>160 THEN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La carga de horarios no coincide con los seis grupos y 160 bloques de 07.';
 END IF;
 IF EXISTS(SELECT 1 FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
  LEFT JOIN ambar_docentes_carga t ON t.completo=h.profesor
  WHERE g.ID_Carrera=carrera AND g.ID_Periodo=periodo AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2') AND h.profesor IS NOT NULL AND t.completo IS NULL) THEN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Un profesor del horario no está en el mapa de nombres. Revisar antes de importar.';
 END IF;
 OPEN teachers;
 teacher_loop: LOOP
  FETCH teachers INTO full_name,given_name,ap_name,am_name,user_base,email_base,hash_value;
  IF done=1 THEN LEAVE teacher_loop; END IF;
  IF NOT EXISTS(SELECT 1 FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo WHERE h.profesor=full_name AND g.ID_Carrera=carrera AND g.ID_Periodo=periodo AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2')) THEN ITERATE teacher_loop; END IF;
  IF (SELECT COUNT(*) FROM Docente WHERE REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)),'[[:space:]]+',' ')=full_name)>1 THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay docentes homónimos duplicados; resolver antes de importar.';
  END IF;
  SET doc=(SELECT ID_docente FROM Docente WHERE REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)),'[[:space:]]+',' ')=full_name);
  IF doc IS NULL THEN
   IF EXISTS(SELECT 1 FROM Personal_Administrativo WHERE REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)),'[[:space:]]+',' ')=full_name)
    OR EXISTS(SELECT 1 FROM Alumno WHERE REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)),'[[:space:]]+',' ')=full_name) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Un nombre docente ya existe como alumno o personal; revisar identidad.';
   END IF;
   SET n=1;
   SET email_value=CONCAT(email_base,'@acapulco.tecnm.mx');
   WHILE EXISTS(SELECT 1 FROM Docente WHERE correo_institucional=email_value)
     OR EXISTS(SELECT 1 FROM Personal_Administrativo WHERE correo_institucional=email_value)
     OR EXISTS(SELECT 1 FROM Alumno WHERE correo_institucional=email_value) DO
    SET n=n+1; SET email_value=CONCAT(email_base,n,'@acapulco.tecnm.mx');
   END WHILE;
   INSERT INTO Docente(nombre,apellido_paterno,apellido_materno,correo_institucional,RFC,curp,fecha_nacimiento,telefono,estatus)
    VALUES(given_name,ap_name,am_name,email_value,NULL,NULL,NULL,NULL,'ACTIVO');
   SET doc=LAST_INSERT_ID();
  ELSEIF (SELECT UPPER(estatus) FROM Docente WHERE ID_docente=doc)<>'ACTIVO' THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Un docente del horario está dado de baja; no se reactivó automáticamente.';
  END IF;
  IF EXISTS(SELECT 1 FROM Usuario WHERE ID_docente=doc AND (Id_rol<>rol OR UPPER(estatus)<>'ACTIVO')) THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Cuenta docente inactiva o con rol incompatible; revisar manualmente.';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM Usuario WHERE ID_docente=doc) THEN
   SET n=1; SET user_value=user_base;
   WHILE EXISTS(SELECT 1 FROM Usuario WHERE nombre_usuario=user_value) DO
    SET n=n+1; SET user_value=CONCAT(LEFT(user_base,42),'.',n);
   END WHILE;
   INSERT INTO Usuario(nombre_usuario,password_hash,Id_rol,estatus,ID_docente,debe_cambiar_password)
    VALUES(user_value,hash_value,rol,'ACTIVO',doc,0);
   INSERT INTO Bitacora(Id_usuario,tipo_evento,modulo_afectado,descripcion,id_registro_afectado)
    VALUES(actor,'ALTA_USUARIO','USUARIOS',CONCAT('Importación de docente desde horario: ',full_name),user_value);
  END IF;
 END LOOP;
 CLOSE teachers;
 SET done=0;
 OPEN subjects;
 subject_loop: LOOP
  FETCH subjects INTO grp,subject_name,full_name;
  IF done=1 THEN LEAVE subject_loop; END IF;
  IF (SELECT COUNT(*) FROM Materia WHERE ID_Carrera=carrera AND nombre_materia=subject_name)>1 THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Materias con nombre duplicado: resolver el catálogo antes de importar.';
  END IF;
  SET mat=(SELECT ID_Materia FROM Materia WHERE ID_Carrera=carrera AND nombre_materia=subject_name);
  IF mat IS NULL THEN
   SET internal_key=CONCAT('HOR26-',LEFT(SHA2(subject_name,256),12));
   IF EXISTS(SELECT 1 FROM Materia WHERE ID_Carrera=carrera AND clave_materia=internal_key) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Colisión de clave interna de materia; revisar catálogo.';
   END IF;
   INSERT INTO Materia(ID_Carrera,clave_materia,nombre_materia,creditos,horas_teoricas,horas_practicas,estatus)
    VALUES(carrera,internal_key,subject_name,NULL,NULL,NULL,'ACTIVO');
   SET mat=LAST_INSERT_ID();
  ELSEIF (SELECT UPPER(estatus) FROM Materia WHERE ID_Materia=mat)<>'ACTIVO' THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Una materia del horario está inactiva; no se reactivó.';
  END IF;
  IF full_name IS NULL THEN ITERATE subject_loop; END IF;
  SET doc=(SELECT ID_docente FROM Docente WHERE REGEXP_REPLACE(TRIM(CONCAT_WS(' ',nombre,apellido_paterno,apellido_materno)),'[[:space:]]+',' ')=full_name);
  IF (SELECT COUNT(*) FROM Asignacion_Docente WHERE ID_grupo=grp AND ID_Materia=mat)>1
   OR EXISTS(SELECT 1 FROM Asignacion_Docente WHERE ID_grupo=grp AND ID_Materia=mat AND ID_docente<>doc) THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Asignación existente incompatible con el horario: no se sobrescribió.';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM Asignacion_Docente WHERE ID_grupo=grp AND ID_Materia=mat) THEN
   INSERT INTO Asignacion_Docente(ID_docente,ID_Materia,ID_grupo) VALUES(doc,mat,grp);
   INSERT INTO Bitacora(Id_usuario,tipo_evento,modulo_afectado,descripcion,id_registro_afectado)
    VALUES(actor,'ASIGNACION_DOCENTE','DOCENTES',CONCAT(full_name,' / ',subject_name),CAST(grp AS CHAR));
  END IF;
  SET asign=(SELECT Id_asignacion FROM Asignacion_Docente WHERE ID_grupo=grp AND ID_Materia=mat);
  IF EXISTS(SELECT 1 FROM Horario x WHERE x.Id_asignacion=asign AND NOT EXISTS(
   SELECT 1 FROM Horario_Referencia h WHERE h.ID_grupo=grp AND h.asignatura=subject_name AND h.profesor=full_name
    AND x.dia=h.dia AND x.hora_inicio=h.hora_inicio AND x.hora_fin=h.hora_fin AND x.aula<=>h.aula)) THEN
   SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Horario operativo distinto de la referencia; resolver antes de importar.';
  END IF;
  INSERT INTO Horario(Id_asignacion,dia,hora_inicio,hora_fin,aula)
   SELECT asign,h.dia,h.hora_inicio,h.hora_fin,h.aula FROM Horario_Referencia h
   WHERE h.ID_grupo=grp AND h.asignatura=subject_name AND h.profesor=full_name
    AND NOT EXISTS(SELECT 1 FROM Horario x WHERE x.Id_asignacion=asign AND x.dia=h.dia AND x.hora_inicio=h.hora_inicio AND x.hora_fin=h.hora_fin AND x.aula<=>h.aula);
 END LOOP;
 CLOSE subjects;
 COMMIT;
END$$
CALL ambar_asignar_docentes_2026()$$
DROP PROCEDURE ambar_asignar_docentes_2026$$
DELIMITER ;
-- Cuentas efectivas: las existentes conservan su contraseña anterior.
SELECT t.completo AS Docente,u.nombre_usuario AS Usuario,d.correo_institucional AS Correo,
 IF(d.curp IS NULL OR d.RFC IS NULL OR d.fecha_nacimiento IS NULL,'Pendiente','Capturada') AS Identidad
 FROM ambar_docentes_carga t JOIN Docente d ON CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)=t.completo
 JOIN Usuario u ON u.ID_docente=d.ID_docente ORDER BY t.completo;
SELECT g.nombre_grupo AS Grupo,h.asignatura AS Materia,COALESCE(h.profesor,'Por asignar') AS Docente
 FROM Horario_Referencia h JOIN Grupo g ON g.ID_grupo=h.ID_grupo
 JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo JOIN Carrera c ON c.ID_Carrera=g.ID_Carrera
 WHERE c.clave_carrera='ISC' AND p.nombre_periodo='Agosto-Diciembre 2026'
 AND g.nombre_grupo IN ('1ISC1','1ISC2','3ISC1','3ISC2','5ISC1','5ISC2')
 GROUP BY g.nombre_grupo,h.asignatura,h.profesor ORDER BY Grupo,Materia;
DROP TEMPORARY TABLE ambar_docentes_carga;
