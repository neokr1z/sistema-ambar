# Generación de NSS para demostración escolar

En Inscripción, pestaña Datos personales, pulsa **Generar NSS** debajo de Alergias. Primero selecciona la fecha de nacimiento. Si ya hay un número, el botón no lo reemplaza; vacía el campo para generar otro.

Se generan once dígitos: prefijo local 99, año actual (dos cifras), año de nacimiento (dos cifras), serie de cuatro dígitos y dígito verificador. El prefijo es una convención didáctica, no una serie reservada por IMSS; el resultado no acredita existencia, titularidad ni afiliación. La pantalla identifica esta función como prueba escolar.

Se omiten los NSS ya guardados en Alumno y se conserva la comprobación transaccional de duplicados al guardar. La generación no reserva el número; si dos capturas simultáneas eligen el mismo, la segunda se rechaza al guardar y debe generar otro. No modifica NSS existentes ni requiere SQL nuevo.

Pruebas: generación de las 10 000 series sin repetición, formato y dígito verificador de cada resultado, agotamiento, nacimiento mínimo y rechazo de fechas inválidas. Suite de lógica sin MySQL y compilación C#; queda pendiente probar el formulario y la consulta MySQL en Windows.
