"""Verifica 11 sobre una BDD desechable ya preparada. No utiliza appsettings.
Uso: python verificar_carga_docentes.py --mysql /ruta/mysql --database AMBAR_..._test
"""
import argparse, pathlib, subprocess
p=argparse.ArgumentParser()
p.add_argument('--mysql',required=True);p.add_argument('--port',type=int,default=33306)
p.add_argument('--database',required=True);a=p.parse_args()
if not a.database.endswith('_test') or not a.database.replace('_','').isalnum():
    raise ValueError('Sólo se admiten bases de pruebas terminadas en _test')
client=[a.mysql,'--no-defaults','--protocol=TCP','--host=127.0.0.1','--port='+str(a.port),'--user=root','--default-character-set=utf8mb4','-N','-B',a.database]
def query(sql,ok=True):
    r=subprocess.run(client,input=sql,text=True,capture_output=True)
    if ok and r.returncode: raise RuntimeError(r.stderr)
    if not ok and not r.returncode: raise AssertionError('Debió rechazar el conflicto')
    return r.stdout.strip()
script=(pathlib.Path(__file__).resolve().parents[1]/'Sql/11_docentes_y_asignaciones_2026.sql').read_text().replace('AMBAR_Control_Escolar',a.database)
assert query('SELECT COUNT(*) FROM Docente')=='25'
assert query('SELECT COUNT(*) FROM Materia')=='18'
assert query('SELECT COUNT(*) FROM Asignacion_Docente')=='29'
assert query('SELECT COUNT(*) FROM Horario')=='128'
assert query("SELECT COUNT(*) FROM Horario_Referencia WHERE profesor IS NULL")=='32'
# Cada bloque operativo debe reproducir exactamente el profesor, materia, grupo y hora de la referencia.
assert query('''SELECT COUNT(*) FROM Horario h JOIN Asignacion_Docente a ON a.Id_asignacion=h.Id_asignacion
 JOIN Docente d ON d.ID_docente=a.ID_docente JOIN Materia m ON m.ID_Materia=a.ID_Materia
 JOIN Horario_Referencia r ON r.ID_grupo=a.ID_grupo AND r.asignatura=m.nombre_materia
 AND r.profesor=CONCAT_WS(' ',d.nombre,d.apellido_paterno,d.apellido_materno)
 AND r.dia=h.dia AND r.hora_inicio=h.hora_inicio AND r.hora_fin=h.hora_fin AND r.aula=h.aula''')=='128'
def snapshot():
    return '\n'.join(query('SELECT * FROM '+t+' ORDER BY 1') for t in ['Docente','Usuario','Materia','Asignacion_Docente','Horario','Bitacora'])
before=snapshot();query(script);assert snapshot()==before
row=query('SELECT Id_usuario,password_hash FROM Usuario WHERE ID_docente IS NOT NULL ORDER BY Id_usuario LIMIT 1').split('\t');uid,old=row
query(f"UPDATE Usuario SET password_hash='hash_cambiado_por_usuario' WHERE Id_usuario={uid}")
query(script)
assert query(f'SELECT password_hash FROM Usuario WHERE Id_usuario={uid}')=='hash_cambiado_por_usuario'
query(f"UPDATE Usuario SET password_hash='{old}' WHERE Id_usuario={uid}")
# Conflicto: nunca reasigna un grupo a otro docente ni deja cambios parciales.
aid,did=query('SELECT Id_asignacion,ID_docente FROM Asignacion_Docente ORDER BY Id_asignacion LIMIT 1').split('\t')
other=query(f'SELECT MIN(ID_docente) FROM Docente WHERE ID_docente<>{did}')
query(f'UPDATE Asignacion_Docente SET ID_docente={other} WHERE Id_asignacion={aid}')
before=snapshot();query(script,ok=False);assert snapshot()==before
query(f'UPDATE Asignacion_Docente SET ID_docente={did} WHERE Id_asignacion={aid}')
print('OK: carga 25 docentes / 18 materias / 29 asignaciones / 128 bloques; correspondencia, reejecución, contraseña existente y conflicto transaccional.')
