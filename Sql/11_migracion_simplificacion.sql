-- AMBAR | 11 | Migracion DEL MODELO ANTERIOR al simplificado.
-- NO ejecutar en instalacion limpia (01-08 nuevos), ni sin respaldo completo.
-- DDL en MySQL provoca commit implicito; respaldar y ejecutar sobre una copia primero.
-- NO elimina alumnos, inscripciones, reinscripciones ni calificaciones.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;

DELIMITER $$
DROP PROCEDURE IF EXISTS ambar_validar_pre_migracion_11$$
CREATE PROCEDURE ambar_validar_pre_migracion_11()
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.tables
      WHERE table_schema=DATABASE() AND table_name='Horario_Referencia')
      OR NOT EXISTS (SELECT 1 FROM information_schema.tables
      WHERE table_schema=DATABASE() AND table_name='Horario') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Se requieren ambas tablas antiguas Horario y Horario_Referencia. No ejecutar dos veces.';
    END IF;
    IF EXISTS (
      SELECT 1 FROM Horario h WHERE NOT EXISTS (
        SELECT 1 FROM Asignacion_Docente ad
        JOIN Materia m ON m.ID_Materia=ad.ID_Materia
        JOIN Horario_Referencia hr ON hr.ID_grupo=ad.ID_grupo AND hr.asignatura=m.nombre_materia
        WHERE ad.Id_asignacion=h.Id_asignacion AND hr.dia=h.dia
          AND hr.hora_inicio=h.hora_inicio AND hr.hora_fin=h.hora_fin AND hr.aula=h.aula
      )
    ) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay bloques en Horario sin equivalencia en Horario_Referencia; conservar datos y migrar manualmente.';
    END IF;
    IF EXISTS (
      SELECT 1 FROM Aspirante a LEFT JOIN Periodo_Escolar p ON p.ID_Periodo=a.periodo_fichas
      WHERE a.periodo_fichas IS NOT NULL AND p.ID_Periodo IS NULL
    ) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Hay periodos_fichas sin periodo escolar; corregir antes de migrar.';
    END IF;
    -- Evitar perder dominios personalizados agregados fuera de la lista original.
    IF EXISTS (SELECT 1 FROM information_schema.tables
       WHERE table_schema=DATABASE() AND table_name='Dominio_Correo') THEN
      IF EXISTS (SELECT 1 FROM Dominio_Correo WHERE activo=1 AND dominio NOT IN (
       'gmail.com','googlemail.com','outlook.com','hotmail.com','hotmail.com.mx',
       'live.com','live.com.mx','msn.com','yahoo.com','yahoo.com.mx','ymail.com',
       'icloud.com','me.com','mac.com','aol.com','proton.me','protonmail.com',
       'zoho.com','mail.com','gmx.com','gmx.es','acapulco.tecnm.mx','tecnm.mx'
      )) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Dominio_Correo tiene dominios activos adicionales. Agregarlos a CorreoDominio.cs antes de migrar.';
      END IF;
    END IF;
END$$
CALL ambar_validar_pre_migracion_11()$$
DROP PROCEDURE ambar_validar_pre_migracion_11$$
DELIMITER ;

-- Cambios de estructura sin afectar datos operativos (pero sí eliminan esos atributos).
ALTER TABLE Aspirante DROP COLUMN promedio_preparatoria, DROP COLUMN nombre_preparatoria;
ALTER TABLE Personal_Administrativo DROP COLUMN puesto;
ALTER TABLE Plan_Estudio DROP COLUMN especialidad, DROP COLUMN fecha_inicio_vigencia,
 DROP COLUMN fecha_fin_vigencia;
ALTER TABLE Materia DROP COLUMN creditos, DROP COLUMN horas_teoricas, DROP COLUMN horas_practicas;
ALTER TABLE Rol DROP COLUMN descripcion;

-- La FK vincula la ficha con el periodo ya existente.
ALTER TABLE Aspirante ADD CONSTRAINT fk_aspirante_periodo
 FOREIGN KEY (periodo_fichas) REFERENCES Periodo_Escolar(ID_Periodo);

-- Un solo horario. El original de asignaciones se conserva hasta comprobar que
-- sus sesiones están representadas en la antigua tabla de referencia (arriba).
RENAME TABLE Horario TO Horario_Antes_11, Horario_Referencia TO Horario;
ALTER TABLE Horario ADD COLUMN Id_asignacion INT NULL;
UPDATE Horario h
JOIN Asignacion_Docente ad ON ad.ID_grupo=h.ID_grupo
JOIN Materia m ON m.ID_Materia=ad.ID_Materia AND m.nombre_materia=h.asignatura
JOIN Docente d ON d.ID_docente=ad.ID_docente
 SET h.Id_asignacion=ad.Id_asignacion
WHERE h.profesor=TRIM(CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno));
ALTER TABLE Horario ADD CONSTRAINT fk_horario_asignacion_unificado
 FOREIGN KEY (Id_asignacion) REFERENCES Asignacion_Docente(Id_asignacion);
-- Comprueba las 170 sesiones antes de eliminar la tabla antigua:
SELECT COUNT(*) AS bloques_totales,
 SUM(Id_asignacion IS NOT NULL) AS bloques_con_asignacion
 FROM Horario;
-- La tabla Horario_Antes_11 se mantiene como respaldo temporal por seguridad.
-- Tras validar en la aplicación y comparar datos, se puede eliminar manualmente:
-- DROP TABLE Horario_Antes_11;

-- Relacionar el domicilio con el asentamiento exacto, sin perder el texto histórico.
ALTER TABLE Domicilio ADD COLUMN id_catalogo_postal BIGINT NULL;
UPDATE Domicilio d SET id_catalogo_postal=(
 SELECT MIN(cp.id) FROM Catalogo_Postal cp
 WHERE cp.codigo_postal=d.codigo_postal AND cp.colonia=d.colonia
   AND cp.localidad=d.localidad AND cp.municipio=d.municipio AND cp.estado=d.estado
);
ALTER TABLE Domicilio ADD CONSTRAINT fk_domicilio_catalogo_postal
 FOREIGN KEY (id_catalogo_postal) REFERENCES Catalogo_Postal(id)
 ON UPDATE CASCADE ON DELETE RESTRICT;
SELECT COUNT(*) AS domicilios_sin_match FROM Domicilio WHERE id_catalogo_postal IS NULL;
-- Si este conteo no es cero, revisar las direcciones históricas antes de imponer NOT NULL.

-- Dominios locales ahora configurados en Servicios/CorreoDominio.cs.
DROP TABLE IF EXISTS Dominio_Correo;

-- Se mantiene Usuario.debe_cambiar_password: la regla está en C# y el estado
-- debe persistir en MySQL entre sesiones, aun después de cerrar el programa.
