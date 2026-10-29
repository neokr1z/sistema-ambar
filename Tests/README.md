# Pruebas del backend

`Backend.Tests.csproj` enlaza directamente las clases del programa. No depende de Windows Forms ni copia su lógica. Requiere .NET 8.

Sólo validaciones, sin base de datos:

```sh
dotnet run --project Tests/Backend.Tests.csproj --configuration Release
```

Integración: utiliza exclusivamente un MySQL local **desechable**, accesible en 127.0.0.1:33306 con root sin contraseña. El preparador no usa `appsettings.json`; crea una base nueva terminada en `_test` cada vez y ejecuta 01,02,05,06,07,06,07,08. Requiere Python 3 y el cliente mysql:

```sh
python Tests/preparar_mysql.py --mysql /ruta/al/mysql --port 33306
```

Toma el nombre de la base que imprime el preparador y define `AMBAR_TEST_CONNECTION` para esa base. Ejemplo PowerShell (reemplaza NOMBRE_IMPRESO_test):

```powershell
$env:AMBAR_TEST_CONNECTION = "Server=127.0.0.1;Port=33306;User ID=root;Database=NOMBRE_IMPRESO_test;SSL Mode=None;"
dotnet run --project Tests/Backend.Tests.csproj --configuration Release
```

El ejecutable se niega a usar una base cuyo nombre no termine en `_test`. Sus pruebas de integración escriben cuentas y 50 alumnos ficticios; nunca deben apuntar a datos reales. Cada ejecución requiere una base recién preparada. No usa ni cambia la configuración de conexión de la aplicación principal.

Comprobación adicional de idempotencia tras inscribir: en una copia de `07_grupos_y_fichas_2026.sql`, sustituye **únicamente** `USE AMBAR_Control_Escolar` por `USE nombre_de_la_base_test`, ejecuta la copia sobre esa base temporal y comprueba que siguen existiendo 50 fichas `INSCRITO` y 160 filas de `Horario_Referencia`.

La suite incluye 101 verificaciones, con pruebas de correo/número automáticos, permisos de bitácora, acceso por docente, unidades dinámicas, captura, conflictos, rollback e historial. El catálogo postal usado en la prueba es una fila ficticia exclusiva de esa base temporal.
