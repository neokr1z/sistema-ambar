"""Prepara una base NUEVA en un servidor MySQL local desechable (root sin contraseña).
No usa appsettings del proyecto. Uso: python preparar_mysql.py --mysql /ruta/mysql --port 33306
"""
import argparse
import pathlib
import subprocess
import time

parser = argparse.ArgumentParser()
parser.add_argument('--mysql', required=True)
parser.add_argument('--port', type=int, default=33306)
a = parser.parse_args()
name = f'AMBAR_integracion_{time.time_ns()}_test'
root = pathlib.Path(__file__).resolve().parents[1]
client = [a.mysql, '--no-defaults', '--protocol=TCP', '--host=127.0.0.1', '--port='+str(a.port), '--user=root', '--default-character-set=utf8mb4']
for filename in ['01_tablas_ambar_original.sql', '02_migracion_y_administrador.sql',
                 '05_rfc_secretarias.sql', '06_validaciones_y_personal.sql',
                 '07_grupos_y_fichas_2026.sql', '06_validaciones_y_personal.sql',
                 '07_grupos_y_fichas_2026.sql', '08_catalogo_postal.sql']:
    sql = (root/'Sql'/filename).read_text().replace('AMBAR_Control_Escolar', name)
    result = subprocess.run(client, input=sql, text=True, capture_output=True)
    if result.returncode:
        raise RuntimeError(filename+': '+result.stderr)
    print('OK: '+filename)
print('Base de pruebas: '+name)
