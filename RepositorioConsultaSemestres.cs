using System.Data;

namespace AMBAR.Semana4;

// Consulta por semestre sin exigir CURP ni expediente a las reinscripciones autónomas.
internal static class RepositorioConsultaSemestres
{
    public static DataTable Listar(Sesion actor,int semestre,string busqueda,bool ascendente)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        if(semestre is <0 or >3)throw new ArgumentException("Semestre inválido.");
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT *
            FROM (
                SELECT a.Numero_control AS 'No. control',
                  CONCAT_WS(' ',a.nombre,a.apellido_paterno,a.apellido_materno) AS 'Nombre completo',
                  a.curp AS 'CURP',ca.nombre_carrera AS 'Carrera',
                  a.correo_institucional AS 'Correo',a.estatus AS 'Estatus',
                  s.numero_semestre AS 'Semestre',g.nombre_grupo AS 'Grupo',
                  'EXPEDIENTE' AS 'Tipo'
                FROM Alumno a
                JOIN Plan_Estudio pl ON pl.id_plan=a.ID_Plan
                JOIN Carrera ca ON ca.clave_carrera=pl.clave_carrera
                JOIN Inscripcion i ON i.Numero_control=a.Numero_control
                JOIN Grupo g ON g.ID_grupo=i.ID_grupo
                JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
                JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
                WHERE s.numero_semestre IN (1,2,3)
                  AND UPPER(i.estatus)='INSCRITO'
                  AND pe.nombre_periodo='Agosto-Diciembre 2026'
                  AND (@admin=1 OR UPPER(a.estatus)<>'OCULTO')
                UNION ALL
                SELECT r.numero_control,
                  CONCAT_WS(' ',r.nombre,r.apellido_paterno,r.apellido_materno),
                  '', 'Ingeniería en Sistemas Computacionales',
                  COALESCE(r.correo_institucional,''),r.estatus,
                  s.numero_semestre,g.nombre_grupo,'REINSCRIPCIÓN'
                FROM Reinscripcion r
                JOIN Grupo g ON g.ID_grupo=r.ID_grupo
                JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
                JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
                WHERE s.numero_semestre IN (2,3)
                  AND UPPER(r.estatus) IN ('INSCRITO','BAJA')
                  AND pe.nombre_periodo='Agosto-Diciembre 2026'
            ) AS registros
            WHERE (@semestre=0 OR registros.`Semestre`=@semestre)
              AND (@buscar='' OR registros.`No. control` LIKE @like
                   OR registros.`Nombre completo` LIKE @like)
            ORDER BY CASE WHEN @asc=1 THEN registros.`No. control` END ASC,
                     CASE WHEN @asc=0 THEN registros.`No. control` END DESC
            """);
        Db.Param(cmd,"@admin",actor.IsAdmin?1:0);
        Db.Param(cmd,"@semestre",semestre);
        Db.Param(cmd,"@buscar",busqueda.Trim());
        Db.Param(cmd,"@like","%"+busqueda.Trim()+"%");
        Db.Param(cmd,"@asc",ascendente?1:0);
        using var reader=cmd.ExecuteReader();
        var datos=new DataTable();datos.Load(reader);
        return datos;
    }

    public static (int GrupoId,string Nombre,string Periodo)? Reinscrito(Sesion actor,string numero)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT r.ID_grupo,g.nombre_grupo,pe.nombre_periodo
            FROM Reinscripcion r
            JOIN Grupo g ON g.ID_grupo=r.ID_grupo
            JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            WHERE r.numero_control=@control AND UPPER(r.estatus)='INSCRITO'
            """);
        Db.Param(cmd,"@control",numero);
        using var rd=cmd.ExecuteReader();
        return rd.Read() ? (rd.GetInt32(0),rd.GetString(1),rd.GetString(2)) : null;
    }
}
