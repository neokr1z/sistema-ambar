-- AMBAR | Reinscripción autónoma: generación 2025 puede ir a 2.º (repetidor) o 3.º.
-- Generación 2026 solo puede ir a 2.º.
-- Ejecutar DESPUÉS de Sql/16_reinscripcion_autonoma.sql y Sql/18_reinscripcion_2026_semestres_1_2_3.sql.
USE AMBAR_Control_Escolar;

-- Actualizar la restricción sin borrar alumnos existentes.
ALTER TABLE Reinscripcion
    DROP CHECK ck_reins_demo_generacion;

ALTER TABLE Reinscripcion
    ADD CONSTRAINT ck_reins_demo_generacion CHECK
       ((anio_ingreso=2026 AND semestre_destino=2)
        OR (anio_ingreso=2025 AND semestre_destino IN (2,3)));

-- Confirmar el estado actual.
SELECT numero_control,anio_ingreso,semestre_destino,
       CASE WHEN anio_ingreso=2025 AND semestre_destino=2
            THEN 'REPETIDOR' ELSE 'REGULAR' END AS situacion,
       periodo,estatus
FROM Reinscripcion
ORDER BY numero_control;
