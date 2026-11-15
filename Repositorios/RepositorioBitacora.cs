using System.Data;
using System.Globalization;

namespace AMBAR.Semana4;

internal static class RepositorioBitacora
{
    public static DataTable Query(Sesion actor,string password,DateTime from,DateTime to,string filter,int page)
    {
        if(from.Date>to.Date || to.Year>=9999 || page<0 || page>100000)throw new ArgumentException("Revisa el intervalo de fechas.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor,true);
        using(var verify=Db.Sql(c,"SELECT password_hash FROM Usuario WHERE Id_usuario=@id"))
        {
            Db.Param(verify,"@id",actor.Id);
            string hash=Convert.ToString(verify.ExecuteScalar())??"";
            if(!PasswordHasher.Verify(password,hash))
            {
                Db.Audit(c,null,actor.Id,"BITACORA_DENEGADA","BITACORA","Contraseña incorrecta al consultar la bitácora");
                throw new UnauthorizedAccessException("Contraseña incorrecta. No se consultó la bitácora.");
            }
        }
        Db.Audit(c,null,actor.Id,"CONSULTA_BITACORA","BITACORA",$"Consulta del {from:dd/MM/yyyy} al {to:dd/MM/yyyy}, página {page+1}");
        using var cmd=Db.Sql(c,"""
            SELECT b.fecha_hora AS Fecha,
                COALESCE(NULLIF(TRIM(CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)),''),
                         NULLIF(TRIM(CONCAT_WS(' ',p.nombre,p.apellido_paterno,p.apellido_materno)),''),u.nombre_usuario) AS Persona,
                u.nombre_usuario AS Usuario,r.nombre AS Rol,b.tipo_evento AS Actividad,
                b.modulo_afectado AS Módulo,b.descripcion AS Detalle
            FROM Bitacora b JOIN Usuario u ON u.Id_usuario=b.Id_usuario JOIN Rol r ON r.id_rol=u.Id_rol
            LEFT JOIN Docente d ON d.ID_docente=u.ID_docente
            LEFT JOIN Personal_Administrativo p ON p.num_administrativo=u.num_administrativo
            WHERE b.fecha_hora>=@from AND b.fecha_hora<@until
              AND (@filter='' OR u.nombre_usuario LIKE @like OR b.descripcion LIKE @like OR b.tipo_evento LIKE @like)
            ORDER BY b.fecha_hora DESC,b.Id_bitacora DESC LIMIT 100 OFFSET @offset
            """);
        Db.Param(cmd,"@from",from.Date);Db.Param(cmd,"@until",to.Date.AddDays(1));Db.Param(cmd,"@filter",filter.Trim());
        Db.Param(cmd,"@like","%"+filter.Trim()+"%");Db.Param(cmd,"@offset",page*100);
        using var reader=cmd.ExecuteReader();var table=new DataTable();table.Load(reader);
        var titles=new Dictionary<string,string>{{"ACCESO","Inicio de sesión"},{"ALTA_USUARIO","Alta de personal"},{"BAJA_PERSONAL","Baja de personal"},
            {"INSCRIPCION","Inscripción de alumno"},{"CAPTURA_CALIFICACIONES","Captura de calificaciones"},{"CONSULTA_BITACORA","Consulta de bitácora"},
            {"BITACORA_DENEGADA","Consulta de bitácora denegada"},{"EDICION","Actualización de expediente"}};
        foreach(DataRow row in table.Rows)
        {
            string code=Convert.ToString(row["Actividad"])??"";
            row["Actividad"]=titles.TryGetValue(code,out var title)?title:Readable(code);
            row["Módulo"]=Readable(Convert.ToString(row["Módulo"])??"");
        }
        return table;
    }
    private static string Readable(string code)=>CultureInfo.GetCultureInfo("es-MX").TextInfo.ToTitleCase(code.ToLowerInvariant().Replace('_',' '));
}
