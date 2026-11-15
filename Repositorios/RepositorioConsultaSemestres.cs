using System.Data;

namespace AMBAR.Semana4;

// Consulta por semestre sin exigir CURP ni expediente a las reinscripciones autónomas.
internal static class RepositorioConsultaSemestres
{
    public static DataTable Listar(Sesion actor,int semestre,string busqueda,bool ascendente)
    {
        if(!actor.PuedeGestionarAlumnos)throw new UnauthorizedAccessException("Acceso no autorizado.");
        if(semestre is <0 or >3)throw new ArgumentException("Semestre inválido.");
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT v.NumeroControl AS 'No. control',v.NombreCompleto AS 'Nombre completo',
                v.Curp AS 'CURP',v.Carrera,v.Correo,v.Estatus,
                v.Semestre,v.Grupo,v.Tipo
            FROM vw_AMBAR_Alumnos_Unificados v
            WHERE v.Periodo=@periodo
              AND (@admin=1 OR v.Estatus<>'OCULTO')
              AND (@semestre=0 OR v.Semestre=@semestre)
              AND (@buscar='' OR v.NumeroControl LIKE @like OR v.NombreCompleto LIKE @like)
            ORDER BY CASE WHEN @asc=1 THEN v.NumeroControl END ASC,
                     CASE WHEN @asc=0 THEN v.NumeroControl END DESC
            """);
        Db.Param(cmd,"@admin",actor.IsAdmin?1:0);
        Db.Param(cmd,"@periodo",PeriodoActual.Nombre);
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
        if(!actor.PuedeGestionarAlumnos)throw new UnauthorizedAccessException("Acceso no autorizado.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT r.ID_grupo,g.nombre_grupo,pe.nombre_periodo
            FROM Reinscripcion r
            JOIN Grupo g ON g.ID_grupo=r.ID_grupo
            JOIN Periodo_Escolar pe ON pe.ID_Periodo=g.ID_Periodo
            WHERE r.numero_control=@control AND r.estatus='INSCRITO'
            """);
        Db.Param(cmd,"@control",numero);
        using var rd=cmd.ExecuteReader();
        return rd.Read() ? (rd.GetInt32(0),rd.GetString(1),rd.GetString(2)) : null;
    }
}
