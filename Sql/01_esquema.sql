-- AMBAR | 01/08 | Esquema vigente (instalacion limpia)
-- Usar SOLO sobre una base nueva. No contiene tablas de captura antigua ni permisos individuales.

CREATE DATABASE IF NOT EXISTS AMBAR_Control_Escolar
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE AMBAR_Control_Escolar;

-- 1. ASPIRANTE
CREATE TABLE Aspirante (
    ID_Aspirante INT AUTO_INCREMENT PRIMARY KEY,
    numero_ficha VARCHAR(20) NOT NULL UNIQUE,
    curp CHAR(18) NOT NULL,
    promedio_preparatoria DECIMAL(4,2) NOT NULL,
    nombre_preparatoria VARCHAR(120) NOT NULL,
    resultado_examen VARCHAR(30) NOT NULL,
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estatus VARCHAR(20) NOT NULL
) ENGINE = InnoDB;

-- 2. CARRERA
CREATE TABLE Carrera (
    ID_Carrera INT AUTO_INCREMENT PRIMARY KEY,
    clave_carrera VARCHAR(20) NOT NULL UNIQUE,
    nombre_carrera VARCHAR(120) NOT NULL,
    duracion_semestres TINYINT UNSIGNED NOT NULL,
    estatus VARCHAR(20) NOT NULL
) ENGINE = InnoDB;

-- 3. SEMESTRE
CREATE TABLE Semestre (
    ID_Semestre INT AUTO_INCREMENT PRIMARY KEY,
    numero_semestre TINYINT UNSIGNED NOT NULL UNIQUE,
    descripcion VARCHAR(100) NOT NULL
) ENGINE = InnoDB;

-- 4. PERIODO ESCOLAR
CREATE TABLE Periodo_Escolar (
    ID_Periodo INT AUTO_INCREMENT PRIMARY KEY,
    nombre_periodo VARCHAR(50) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    estatus VARCHAR(20) NOT NULL
) ENGINE = InnoDB;

-- 5. DOCENTE
CREATE TABLE Docente (
    ID_docente INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50) NOT NULL,
    correo_institucional VARCHAR(120) NOT NULL UNIQUE,
    RFC CHAR(13) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    estatus VARCHAR(20) NOT NULL
) ENGINE = InnoDB;

-- 6. PERSONAL ADMINISTRATIVO
CREATE TABLE Personal_Administrativo (
    num_administrativo VARCHAR(20) PRIMARY KEY,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50) NOT NULL,
    correo_institucional VARCHAR(120) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    puesto VARCHAR(80) NOT NULL,
    estatus VARCHAR(20) NOT NULL
) ENGINE = InnoDB;

-- 7. PLAN DE ESTUDIO
CREATE TABLE Plan_Estudio (
    id_plan INT AUTO_INCREMENT PRIMARY KEY,
    clave_carrera VARCHAR(20) NOT NULL,
    nombre_plan VARCHAR(120) NOT NULL,
    version VARCHAR(20) NOT NULL,
    especialidad VARCHAR(100),
    fecha_inicio_vigencia DATE NOT NULL,
    fecha_fin_vigencia DATE,
    estatus VARCHAR(20) NOT NULL,

    CONSTRAINT fk_plan_carrera
        FOREIGN KEY (clave_carrera)
        REFERENCES Carrera(clave_carrera)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 8. MATERIA
CREATE TABLE Materia (
    ID_Materia INT AUTO_INCREMENT PRIMARY KEY,
    ID_Carrera INT NOT NULL,
    clave_materia VARCHAR(20) NOT NULL,
    nombre_materia VARCHAR(120) NOT NULL,
    creditos TINYINT UNSIGNED NOT NULL,
    horas_teoricas TINYINT UNSIGNED NOT NULL,
    horas_practicas TINYINT UNSIGNED NOT NULL,
    estatus VARCHAR(20) NOT NULL,

    CONSTRAINT uq_materia_carrera_clave
        UNIQUE (ID_Carrera, clave_materia),

    CONSTRAINT fk_materia_carrera
        FOREIGN KEY (ID_Carrera)
        REFERENCES Carrera(ID_Carrera)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 9. GRUPO
CREATE TABLE Grupo (
    ID_grupo INT AUTO_INCREMENT PRIMARY KEY,
    nombre_grupo VARCHAR(20) NOT NULL,
    ID_Carrera INT NOT NULL,
    ID_Semestre INT NOT NULL,
    ID_Periodo INT NOT NULL,
    Capacidad SMALLINT UNSIGNED NOT NULL,
    Estatus VARCHAR(20) NOT NULL,

    CONSTRAINT fk_grupo_carrera
        FOREIGN KEY (ID_Carrera)
        REFERENCES Carrera(ID_Carrera)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_grupo_semestre
        FOREIGN KEY (ID_Semestre)
        REFERENCES Semestre(ID_Semestre)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_grupo_periodo
        FOREIGN KEY (ID_Periodo)
        REFERENCES Periodo_Escolar(ID_Periodo)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 10. ALUMNO
CREATE TABLE Alumno (
    Numero_control VARCHAR(20) PRIMARY KEY,
    ID_Aspirante INT NOT NULL UNIQUE,
    ID_Plan INT NOT NULL,
    curp CHAR(18) NOT NULL UNIQUE,
    nombre VARCHAR(60) NOT NULL,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    nss VARCHAR(20),
    telefono VARCHAR(20),
    telefono_emergencia VARCHAR(20) NOT NULL,
    alergias TEXT,
    correo_institucional VARCHAR(120) NOT NULL UNIQUE,
    estatus VARCHAR(25) NOT NULL,

    CONSTRAINT fk_alumno_aspirante
        FOREIGN KEY (ID_Aspirante)
        REFERENCES Aspirante(ID_Aspirante)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_alumno_plan
        FOREIGN KEY (ID_Plan)
        REFERENCES Plan_Estudio(id_plan)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 11. DOMICILIO
CREATE TABLE Domicilio (
    ID_Domicilio INT AUTO_INCREMENT PRIMARY KEY,
    numero_control VARCHAR(20) NOT NULL UNIQUE,
    calle VARCHAR(100) NOT NULL,
    numero_exterior VARCHAR(15) NOT NULL,
    numero_interior VARCHAR(15),
    colonia VARCHAR(100) NOT NULL,
    localidad VARCHAR(100) NOT NULL,
    municipio VARCHAR(100) NOT NULL,
    estado VARCHAR(100) NOT NULL,
    codigo_postal CHAR(5) NOT NULL,

    CONSTRAINT fk_domicilio_alumno
        FOREIGN KEY (numero_control)
        REFERENCES Alumno(Numero_control)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 12. PLAN MATERIA
CREATE TABLE Plan_Materia (
    id_plan_materia INT AUTO_INCREMENT PRIMARY KEY,
    id_plan INT NOT NULL,
    ID_Materia INT NOT NULL,
    id_semestre INT NOT NULL,
    obligatoriedad VARCHAR(20) NOT NULL,
    estatus VARCHAR(20) NOT NULL,

    CONSTRAINT uq_plan_materia_semestre
        UNIQUE (id_plan, ID_Materia, id_semestre),

    CONSTRAINT fk_planmateria_plan
        FOREIGN KEY (id_plan)
        REFERENCES Plan_Estudio(id_plan)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_planmateria_materia
        FOREIGN KEY (ID_Materia)
        REFERENCES Materia(ID_Materia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_planmateria_semestre
        FOREIGN KEY (id_semestre)
        REFERENCES Semestre(ID_Semestre)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 13. INSCRIPCION
CREATE TABLE Inscripcion (
    Id_inscripcion INT AUTO_INCREMENT PRIMARY KEY,
    Numero_control VARCHAR(20) NOT NULL,
    ID_grupo INT NOT NULL,
    tipo_inscripcion VARCHAR(20) NOT NULL,
    fecha_inscripcion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estatus VARCHAR(20) NOT NULL,

    CONSTRAINT uq_inscripcion_alumno_grupo
        UNIQUE (Numero_control, ID_grupo),

    CONSTRAINT fk_inscripcion_alumno
        FOREIGN KEY (Numero_control)
        REFERENCES Alumno(Numero_control)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_inscripcion_grupo
        FOREIGN KEY (ID_grupo)
        REFERENCES Grupo(ID_grupo)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- ============================================================
-- 14. ASIGNACION DOCENTE
-- ============================================================

CREATE TABLE Asignacion_Docente (
    Id_asignacion INT AUTO_INCREMENT PRIMARY KEY,
    ID_docente INT NOT NULL,
    ID_Materia INT NOT NULL,
    ID_grupo INT NOT NULL,

    CONSTRAINT fk_asignacion_docente
        FOREIGN KEY (ID_docente)
        REFERENCES Docente(ID_docente)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_asignacion_materia
        FOREIGN KEY (ID_Materia)
        REFERENCES Materia(ID_Materia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_asignacion_grupo
        FOREIGN KEY (ID_grupo)
        REFERENCES Grupo(ID_grupo)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 15. HORARIO
CREATE TABLE Horario (
    Id_horario INT AUTO_INCREMENT PRIMARY KEY,
    Id_asignacion INT NOT NULL,
    dia VARCHAR(15) NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    aula VARCHAR(20) NOT NULL,

    CONSTRAINT fk_horario_asignacion
        FOREIGN KEY (Id_asignacion)
        REFERENCES Asignacion_Docente(Id_asignacion)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 19. ROL
CREATE TABLE Rol (
    id_rol INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(255)
) ENGINE = InnoDB;

-- 20. Usuario
CREATE TABLE Usuario (
    Id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    Id_rol INT NOT NULL,
    estatus VARCHAR(20) NOT NULL,
    ID_docente INT UNIQUE,
    num_administrativo VARCHAR(20) UNIQUE,

    CONSTRAINT fk_usuario_rol
        FOREIGN KEY (Id_rol)
        REFERENCES Rol(id_rol)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_usuario_docente
        FOREIGN KEY (ID_docente)
        REFERENCES Docente(ID_docente)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_usuario_administrativo
        FOREIGN KEY (num_administrativo)
        REFERENCES Personal_Administrativo(num_administrativo)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;


-- 25. DOCUMENTO
CREATE TABLE Documento (
    Id_documento INT AUTO_INCREMENT PRIMARY KEY,
    numero_control VARCHAR(20) NOT NULL,
    tipo_documento VARCHAR(50) NOT NULL,
    nombre_archivo VARCHAR(255) NOT NULL,
    ruta_archivo VARCHAR(500) NOT NULL,
    fecha_carga DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estatus VARCHAR(20) NOT NULL,

    CONSTRAINT fk_documento_alumno
        FOREIGN KEY (numero_control)
        REFERENCES Alumno(Numero_control)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 26. ENVIO DOCUMENTO
CREATE TABLE Envio_Documento (
    Id_envio INT AUTO_INCREMENT PRIMARY KEY,
    Id_documento INT NOT NULL,
    fecha_envio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    correo_destino VARCHAR(150) NOT NULL,
    estado_envio VARCHAR(30) NOT NULL,
    Id_usuario INT NOT NULL,

    CONSTRAINT fk_enviodocumento_documento
        FOREIGN KEY (Id_documento)
        REFERENCES Documento(Id_documento)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_enviodocumento_usuario
        FOREIGN KEY (Id_usuario)
        REFERENCES Usuario(Id_usuario)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

-- 27. BITACORA
CREATE TABLE Bitacora (
    Id_bitacora BIGINT AUTO_INCREMENT PRIMARY KEY,
    Id_usuario INT NOT NULL,
    fecha_hora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    tipo_evento VARCHAR(50) NOT NULL,
    descripcion TEXT NOT NULL,
    modulo_afectado VARCHAR(60) NOT NULL,
    id_registro_afectado VARCHAR(100),

    CONSTRAINT fk_bitacora_usuario
        FOREIGN KEY (Id_usuario)
        REFERENCES Usuario(Id_usuario)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;
