-- OPCIONAL: ejecutar después del script 02 sólo para una demostración académica.
-- Fechas, CURP y grupo SON FICTICIOS; reemplázalos con datos reales autorizados.
USE AMBAR_Control_Escolar;

INSERT INTO Carrera(clave_carrera,nombre_carrera,duracion_semestres,estatus)
VALUES ('ISC','Ingeniería en Sistemas Computacionales',9,'ACTIVO')
ON DUPLICATE KEY UPDATE nombre_carrera=VALUES(nombre_carrera);

INSERT INTO Semestre(numero_semestre,descripcion) VALUES(1,'Primer semestre')
ON DUPLICATE KEY UPDATE descripcion=VALUES(descripcion);

INSERT INTO Periodo_Escolar(nombre_periodo,fecha_inicio,fecha_fin,estatus)
SELECT 'Ago-Dic 2026 (DEMO)','2026-08-17','2026-12-18','ACTIVO'
WHERE NOT EXISTS (SELECT 1 FROM Periodo_Escolar WHERE nombre_periodo='Ago-Dic 2026 (DEMO)');

INSERT INTO Plan_Estudio(clave_carrera,nombre_plan,version,especialidad,fecha_inicio_vigencia,fecha_fin_vigencia,estatus)
SELECT 'ISC','Plan de demostración','2026-D',NULL,'2026-08-17',NULL,'ACTIVO'
WHERE NOT EXISTS (SELECT 1 FROM Plan_Estudio WHERE clave_carrera='ISC' AND nombre_plan='Plan de demostración');

INSERT INTO Grupo(nombre_grupo,ID_Carrera,ID_Semestre,ID_Periodo,Capacidad,Estatus)
SELECT '1A-DEMO',c.ID_Carrera,s.ID_Semestre,pe.ID_Periodo,40,'ACTIVO'
FROM Carrera c CROSS JOIN Semestre s CROSS JOIN Periodo_Escolar pe
WHERE c.clave_carrera='ISC' AND s.numero_semestre=1 AND pe.nombre_periodo='Ago-Dic 2026 (DEMO)'
 AND NOT EXISTS (SELECT 1 FROM Grupo g WHERE g.nombre_grupo='1A-DEMO' AND g.ID_Periodo=pe.ID_Periodo);

-- Aspirante ficticio (se consulta desde AMBAR con su ficha; no representa a un alumno real).
INSERT IGNORE INTO Aspirante(numero_ficha,curp,promedio_preparatoria,nombre_preparatoria,resultado_examen,estatus)
VALUES ('F2026DEMO01','XXXX000101HGRXXX01',92.50,'PREPARATORIA DEMO','APROBADO','ACEPTADO');
