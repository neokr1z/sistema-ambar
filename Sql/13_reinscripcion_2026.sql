-- AMBAR · Reinscripciones de 2.º a 3.º y de 4.º a 5.º.
-- Ejecutar DESPUÉS de Sql/07_grupos_y_fichas_2026.sql. No borra datos existentes.
USE AMBAR_Control_Escolar;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Catálogo de materias que deben tener CALIFICACIÓN FINAL del semestre ANTERIOR.
-- Estas son las asignaturas legibles en los horarios entregados (2.º y 4.º).
CREATE TABLE IF NOT EXISTS Materia_Reinscripcion (
    id_materia_reinscripcion INT AUTO_INCREMENT PRIMARY KEY,
    semestre_origen TINYINT UNSIGNED NOT NULL,
    nombre VARCHAR(120) NOT NULL,
    UNIQUE KEY uq_materia_reinscripcion(semestre_origen,nombre),
    CONSTRAINT ck_materia_reinscripcion_sem CHECK (semestre_origen IN (2,4))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO Materia_Reinscripcion(semestre_origen,nombre) VALUES
(2,'Cálculo Integral'),
(2,'Programación Orientada a Objetos'),
(2,'Contabilidad Financiera'),
(2,'Química'),
(2,'Probabilidad y Estadística'),
(4,'Principios Eléctricos y Aplicaciones Digitales'),
(4,'Tópicos Avanzados de Programación'),
(4,'Métodos Numéricos'),
(4,'Ecuaciones Diferenciales'),
(4,'Fundamentos de Bases de Datos'),
(4,'Simulación')
ON DUPLICATE KEY UPDATE nombre=VALUES(nombre);

-- Historial explícito del último semestre cursado, antes del periodo de reinscripción.
-- No se deduce de grupos 1.º, 3.º o 5.º del periodo actual.
CREATE TABLE IF NOT EXISTS Historial_Semestre_Reinscripcion (
    numero_control VARCHAR(20) NOT NULL PRIMARY KEY,
    semestre_cursado TINYINT UNSIGNED NOT NULL,
    periodo_origen VARCHAR(50) NOT NULL DEFAULT 'Enero-Junio 2026',
    CONSTRAINT fk_reins_hist_alumno FOREIGN KEY(numero_control) REFERENCES Alumno(Numero_control),
    CONSTRAINT ck_reins_hist_sem CHECK(semestre_cursado IN (2,4))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Evidencia de notas finales del periodo ANTERIOR. Sin evidencia completa, NO se reinscribe.
-- El sistema no crea ni aprueba calificaciones automáticamente.
CREATE TABLE IF NOT EXISTS Calificacion_Final_Reinscripcion (
    numero_control VARCHAR(20) NOT NULL,
    id_materia_reinscripcion INT NOT NULL,
    periodo_origen VARCHAR(50) NOT NULL DEFAULT 'Enero-Junio 2026',
    calificacion DECIMAL(5,2) NOT NULL,
    PRIMARY KEY (numero_control,id_materia_reinscripcion),
    CONSTRAINT fk_reins_cal_alumno FOREIGN KEY(numero_control) REFERENCES Alumno(Numero_control),
    CONSTRAINT fk_reins_cal_materia FOREIGN KEY(id_materia_reinscripcion) REFERENCES Materia_Reinscripcion(id_materia_reinscripcion),
    CONSTRAINT ck_reins_cal_valor CHECK(calificacion BETWEEN 0 AND 100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Los grupos 3ISC1/3ISC2 y 5ISC1/5ISC2 ya están precargados en 07.
-- Reutilizarlos: crear otros duplicaría los dos grupos permitidos por semestre.
-- Los docentes sin asignar permanecen NULL en Horario_Referencia.
SELECT g.nombre_grupo,s.numero_semestre,g.turno,g.Capacidad,pe.nombre_periodo
FROM Grupo g
JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
WHERE pe.nombre_periodo='Agosto-Diciembre 2026' AND s.numero_semestre IN (3,5)
ORDER BY s.numero_semestre,g.nombre_grupo;

-- Primero registra el semestre realmente cursado, SOLO para alumnos con ese antecedente:
-- INSERT INTO Historial_Semestre_Reinscripcion(numero_control,semestre_cursado,periodo_origen)
-- VALUES('NUMERO_CONTROL',2,'Enero-Junio 2026');
--
-- Para CAPTURAR notas finales de un alumno existente, usar un INSERT por materia:
-- INSERT INTO Calificacion_Final_Reinscripcion(numero_control,id_materia_reinscripcion,periodo_origen,calificacion)
-- SELECT 'NUMERO_CONTROL',id_materia_reinscripcion,'Enero-Junio 2026',85
-- FROM Materia_Reinscripcion WHERE semestre_origen=2 AND nombre='Cálculo Integral';
-- NO inventar calificaciones; capturar las finales reales del periodo anterior.
