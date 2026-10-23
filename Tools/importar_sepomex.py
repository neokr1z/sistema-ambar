#!/usr/bin/env python3
"""Genera SQL para importar un TXT delimitado por | de Correos de México.
Uso: python importar_sepomex.py CPdescarga.txt catalogo_postal_datos.sql
"""
import csv
import sys
from pathlib import Path

def q(value):
    return "'" + str(value or "").strip().replace("\\", "\\\\").replace("'", "''") + "'"

def main(source, target):
    # Los archivos TXT de SEPOMEX pueden estar codificados como Latin-1.
    data = Path(source).read_bytes()
    text = None
    for enc in ("utf-8-sig", "cp1252", "latin-1"):
        try:
            text = data.decode(enc)
            break
        except UnicodeDecodeError:
            pass
    if text is None:
        raise ValueError("Codificación no compatible")
    rows = list(csv.reader(text.splitlines(), delimiter="|"))
    header_at = next((i for i,row in enumerate(rows)
        if "d_codigo" in [cell.lower().strip() for cell in row]), None)
    if header_at is None:
        raise ValueError("No se encontró encabezado d_codigo. Descarga TXT de SEPOMEX.")
    names = [x.strip().lower() for x in rows[header_at]]
    def idx(name): return names.index(name.lower())
    required = ("d_codigo","d_asenta","d_mnpio","d_estado")
    for name in required: idx(name)
    city = names.index("d_ciudad") if "d_ciudad" in names else None
    records=set()
    for row in rows[header_at+1:]:
        if len(row)<len(names): continue
        cp = row[idx("d_codigo")].strip().zfill(5)
        if len(cp)!=5 or not cp.isdigit(): continue
        colony=row[idx("d_asenta")].strip()
        mun=row[idx("d_mnpio")].strip()
        state=row[idx("d_estado")].strip()
        locality=(row[city].strip() if city is not None else "") or mun
        if not colony or not mun or not state: continue
        records.add((cp,colony,locality,mun,state))
    with open(target,"w",encoding="utf-8",newline="\n") as out:
        out.write("USE AMBAR_Control_Escolar;\nSET NAMES utf8mb4;\nSTART TRANSACTION;\n")
        ordered=sorted(records)
        for start in range(0,len(ordered),500):
            batch=ordered[start:start+500]
            out.write("INSERT INTO Catalogo_Postal (codigo_postal,colonia,localidad,municipio,estado) VALUES\n")
            out.write(",\n".join("("+",".join(map(q,row))+")" for row in batch))
            out.write("\nON DUPLICATE KEY UPDATE localidad=VALUES(localidad);\n")
        out.write("COMMIT;\n")
    print(f"{len(records)} registros exportados a {target}. Ejecuta SQL 08 primero.")

if __name__=="__main__":
    if len(sys.argv)!=3:
        raise SystemExit("Uso: python importar_sepomex.py CPdescarga.txt catalogo_postal_datos.sql")
    main(sys.argv[1],sys.argv[2])
