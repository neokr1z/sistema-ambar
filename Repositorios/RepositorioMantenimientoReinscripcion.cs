using System.Data;
using MySqlConnector;

namespace AMBAR.Semana4;

// Operaciones independientes del expediente completo; el alumno queda registrado
// en Reinscripcion y las acciones se registran en Bitacora.
internal sealed record ReinscritoEditable(string Control,string Nombre,string Paterno,
    string Materno,int Semestre,int Grupo,string Correo,string Estatus);

internal static class RepositorioMantenimientoReinscripcion
{
    public static ReinscritoEditable Obtener(Sesion actor,string numero)
    {
        if (!actor.CanSeeStudents) throw new UnauthorizedAccessException("Acceso no autorizado.");
        using var c=Db.Connect();
        Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT numero_control,nombre,apellido_paterno,apellido_materno,
                   semestre_destino,ID_grupo,COALESCE(correo_institucional,''),estatus
            FROM Reinscripcion WHERE numero_control=@numero
            """);
        Db.Param(cmd,"@numero",numero);
        using var rd=cmd.ExecuteReader();
        if(!rd.Read())throw new ArgumentException("No se encontró esta reinscripción.");
        return new ReinscritoEditable(rd.GetString(0),rd.GetString(1),rd.GetString(2),
            rd.GetString(3),rd.GetInt32(4),rd.GetInt32(5),rd.GetString(6),rd.GetString(7));
    }

    // Incluye el grupo actual incluso si está lleno: un alumno puede
    // corregir sus datos sin necesidad de cambiar de grupo.
    public static List<Choice> Grupos(Sesion actor,int semestre,int grupoActual)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        if(semestre is not (2 or 3))throw new ArgumentException("Semestre inválido.");
        using var c=Db.Connect();Identidad.RequireActive(c,null,actor);
        using var cmd=Db.Sql(c,"""
            SELECT g.ID_grupo,
                   CONCAT(g.nombre_grupo,' · ',COALESCE(g.turno,'SIN TURNO'),
                     ' · ',GREATEST(0,g.Capacidad-
                         (SELECT COUNT(*) FROM Reinscripcion r
                          WHERE r.ID_grupo=g.ID_grupo AND UPPER(r.estatus)='INSCRITO')-
                         (SELECT COUNT(*) FROM Inscripcion i
                          WHERE i.ID_grupo=g.ID_grupo AND UPPER(i.estatus)='INSCRITO')),
                     ' lugares') AS etiqueta
            FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE s.numero_semestre=@semestre
              AND p.nombre_periodo=@periodo
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO'
              AND (g.ID_grupo=@actual OR g.Capacidad>
                  (SELECT COUNT(*) FROM Reinscripcion r
                   WHERE r.ID_grupo=g.ID_grupo AND UPPER(r.estatus)='INSCRITO')+
                  (SELECT COUNT(*) FROM Inscripcion i
                   WHERE i.ID_grupo=g.ID_grupo AND UPPER(i.estatus)='INSCRITO'))
            ORDER BY g.turno,g.nombre_grupo
            """);
        Db.Param(cmd,"@semestre",semestre);
        Db.Param(cmd,"@actual",grupoActual);
        Db.Param(cmd,"@periodo",RepositorioReinscripcion.Periodo);
        var result=new List<Choice>();using var rd=cmd.ExecuteReader();
        while(rd.Read())result.Add(new Choice(rd.GetInt32(0),rd.GetString(1)));
        return result;
    }

    public static void Modificar(Sesion actor,string anterior,string nuevo,string nombre,
        string paterno,string materno,int semestre,int grupo)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        anterior=anterior.Trim();nuevo=nuevo.Trim();
        var (generacion,_) = RepositorioReinscripcion.Inferir(nuevo);
        RepositorioReinscripcion.ValidarSemestre(nuevo,semestre);
        nombre=Validation.Name(nombre,"nombre",60);
        paterno=Validation.Name(paterno,"apellido paterno");
        materno=Validation.Name(materno,"apellido materno");

        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c,tx,actor);
        using(var bloqueo=Db.Sql(c,"SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE",tx))
            if(bloqueo.ExecuteScalar() is null)
                throw new InvalidOperationException("Falta la migración de identidad 06.");

        string estado;int grupoAnterior;string originalNombre,originalPaterno,originalMaterno,correoAnterior;
        using(var cmd=Db.Sql(c,"""
            SELECT nombre,apellido_paterno,apellido_materno,ID_grupo,
                   COALESCE(correo_institucional,''),estatus
            FROM Reinscripcion
            WHERE numero_control=@anterior FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@anterior",anterior);
            using var rd=cmd.ExecuteReader();
            if(!rd.Read())throw new ArgumentException("La reinscripción ya no existe.");
            originalNombre=rd.GetString(0);originalPaterno=rd.GetString(1);
            originalMaterno=rd.GetString(2);grupoAnterior=rd.GetInt32(3);
            correoAnterior=rd.GetString(4);estado=rd.GetString(5);
        }
        if(!estado.Equals("INSCRITO",StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Solo se modifican reinscripciones activas.");

        if(nuevo!=anterior)
        {
            using(var chk=Db.Sql(c,"""
                SELECT (SELECT COUNT(*) FROM Reinscripcion WHERE numero_control=@numero)
                     + (SELECT COUNT(*) FROM Alumno WHERE Numero_control=@numero)
                """,tx))
            {
                Db.Param(chk,"@numero",nuevo);
                if(Convert.ToInt32(chk.ExecuteScalar())>0)
                    throw new ArgumentException("El nuevo número de control ya está registrado.");
            }
        }
        // Evitar doble inscripción en cualquier grupo del periodo.
        using(var chk=Db.Sql(c,"""
            SELECT COUNT(*) FROM Inscripcion i
            JOIN Grupo g ON g.ID_grupo=i.ID_grupo
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE i.Numero_control=@numero AND UPPER(i.estatus)='INSCRITO'
              AND p.nombre_periodo=@periodo
            """,tx))
        {
            Db.Param(chk,"@numero",nuevo);
            Db.Param(chk,"@periodo",RepositorioReinscripcion.Periodo);
            if(Convert.ToInt32(chk.ExecuteScalar())>0)
                throw new ArgumentException("Este número de control ya tiene una inscripción activa.");
        }

        int capacidad;
        using(var cmd=Db.Sql(c,"""
            SELECT g.Capacidad FROM Grupo g
            JOIN Semestre s ON s.ID_Semestre=g.ID_Semestre
            JOIN Periodo_Escolar p ON p.ID_Periodo=g.ID_Periodo
            WHERE g.ID_grupo=@grupo AND s.numero_semestre=@semestre
              AND p.nombre_periodo=@periodo
              AND UPPER(g.Estatus)='ACTIVO' AND UPPER(p.estatus)='ACTIVO'
            FOR UPDATE
            """,tx))
        {
            Db.Param(cmd,"@grupo",grupo);Db.Param(cmd,"@semestre",semestre);
            Db.Param(cmd,"@periodo",RepositorioReinscripcion.Periodo);
            var value=cmd.ExecuteScalar();
            if(value is null)throw new ArgumentException("El grupo elegido no corresponde al semestre.");
            capacidad=Convert.ToInt32(value);
        }
        if(grupo!=grupoAnterior)
        {
            using var cmd=Db.Sql(c,"""
                SELECT (SELECT COUNT(*) FROM Reinscripcion
                        WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
                     + (SELECT COUNT(*) FROM Inscripcion
                        WHERE ID_grupo=@grupo AND UPPER(estatus)='INSCRITO')
                """,tx);
            Db.Param(cmd,"@grupo",grupo);
            if(Convert.ToInt32(cmd.ExecuteScalar())>=capacidad)
                throw new ArgumentException("El grupo seleccionado ya está lleno.");
        }

        // Conservar correo existente si no cambiaron los nombres.
        // Si cambia la identidad escrita, generar uno disponible sin sobrescribir a otros.
        bool cambiarCorreo=originalNombre!=nombre || originalPaterno!=paterno ||
            originalMaterno!=materno || string.IsNullOrWhiteSpace(correoAnterior);
        string correo=cambiarCorreo
            ? RepositorioReinscripcion.GenerarCorreo(c,tx,nombre,paterno,materno,anterior)
            : correoAnterior;

        using(var cmd=Db.Sql(c,"""
            UPDATE Reinscripcion
            SET numero_control=@nuevo,nombre=@nombre,
                apellido_paterno=@paterno,apellido_materno=@materno,
                anio_ingreso=@anio,semestre_destino=@semestre,
                ID_grupo=@grupo,correo_institucional=@correo
            WHERE numero_control=@anterior AND UPPER(estatus)='INSCRITO'
            """,tx))
        {
            Db.Param(cmd,"@nuevo",nuevo);Db.Param(cmd,"@nombre",nombre);
            Db.Param(cmd,"@paterno",paterno);Db.Param(cmd,"@materno",materno);
            Db.Param(cmd,"@anio",generacion);Db.Param(cmd,"@semestre",semestre);
            Db.Param(cmd,"@grupo",grupo);Db.Param(cmd,"@correo",correo);
            Db.Param(cmd,"@anterior",anterior);
            if(cmd.ExecuteNonQuery()!=1)throw new InvalidOperationException("No se pudo guardar el cambio.");
        }
        Db.Audit(c,tx,actor.Id,"MODIFICAR_REINSCRIPCION","ALUMNOS",
            $"Actualización de reinscripción. Control {anterior} → {nuevo}; grupo {grupoAnterior} → {grupo}; semestre {semestre}.",
            nuevo);
        tx.Commit();
    }

    public static void Baja(Sesion actor,string control,string motivo)
    {
        if(!actor.CanSeeStudents)throw new UnauthorizedAccessException("Acceso no autorizado.");
        control=control.Trim();motivo=Validation.Mandatory(motivo,"motivo de baja");
        if(motivo.Length>250)throw new ArgumentException("El motivo debe tener como máximo 250 caracteres.");

        using var c=Db.Connect();
        using var tx=c.BeginTransaction(IsolationLevel.ReadCommitted);
        Identidad.RequireActive(c,tx,actor);
        using(var cmd=Db.Sql(c,"SELECT id FROM Control_Identidad WHERE id=1 FOR UPDATE",tx))
            if(cmd.ExecuteScalar() is null)throw new InvalidOperationException("Falta migración 06.");
        using(var cmd=Db.Sql(c,"""
            UPDATE Reinscripcion SET estatus='BAJA'
            WHERE numero_control=@control AND UPPER(estatus)='INSCRITO'
            """,tx))
        {
            Db.Param(cmd,"@control",control);
            if(cmd.ExecuteNonQuery()!=1)
                throw new ArgumentException("Este alumno no tiene una reinscripción activa para dar de baja.");
        }
        Db.Audit(c,tx,actor.Id,"BAJA_REINSCRIPCION","ALUMNOS",
            "Baja escolar de reinscripción. Motivo: "+motivo,control);
        tx.Commit();
    }
}
