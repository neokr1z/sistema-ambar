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
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estatus VARCHAR(20) NOT NULL DEFAULT 'INSCRITO',
    CONSTRAINT ck_reins_demo_generacion CHECK
       ((anio_ingreso=2025 AND semestre_destino=3)
        OR (anio_ingreso=2024 AND semestre_destino=5)),
    CONSTRAINT fk_reins_demo_grupo FOREIGN KEY(ID_grupo) REFERENCES Grupo(ID_grupo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- No se insertan alumnos predefinidos. Los datos se capturan en la ventana de Reinscripción.
-- El número de control se valida por su FORMA y AÑO, no por existencia en Alumno.
SELECT COUNT(*) AS 'Reinscripciones'
FROM Reinscripcion;
