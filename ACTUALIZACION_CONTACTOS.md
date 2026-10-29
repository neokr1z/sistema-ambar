# Validación de teléfonos y correos

## Instalar

Actualiza el proyecto desde `main`, restaura los paquetes NuGet y recompila en Visual Studio 2022. Se agregó `libphonenumber-csharp` 9.0.42 (compatible con .NET 8). **Esta actualización no requiere un script SQL nuevo**; conserva las migraciones anteriores, incluida 10 para el nacimiento del personal.

## Dónde se aplica

- Inscripción: teléfono personal, teléfono de emergencia, correo institucional y correo personal.
- Edición del expediente: los mismos campos.
- Alta de docentes y secretarias: teléfono y dominio del correo institucional generado.

Las validaciones están en los repositorios y en las reglas compartidas, por lo que también se ejecutan si se llama al backend sin utilizar el formulario. Se mantienen opcionales los campos que ya lo eran. Emergencia y correo institucional siguen siendo obligatorios.

## Teléfonos

Se valida la longitud y la numeración mexicana de teléfonos fijos/móviles con los metadatos de libphonenumber, no sólo que existan diez dígitos. Se admite el formato nacional y `+52`, con espacios, guiones o paréntesis; por ejemplo, `+52 (744) 123-4567` se guarda como `7441234567`.

Se rechazan claves no contempladas por el catálogo, países distintos de México, los prefijos antiguos 01/044/045, extensiones, letras, números cortos y secuencias de un único dígito repetido. Los campos son de contacto personal/emergencia: no admiten servicios 800/900 ni otros servicios especiales.

Esto comprueba conformidad con el plan de numeración, **no que la línea esté activa ni que pertenezca a la persona**. No realiza llamadas ni envía SMS. Las reglas evolucionan; el paquete de numeración debe mantenerse actualizado cuando cambien las asignaciones.

## Correos, sin enviar mensajes

1. Se valida una dirección individual, sin espacios internos, puntos consecutivos, nombres de presentación ni listas. Se comprueban las longitudes del buzón, dominio y columna de MySQL (120 caracteres).
2. El dominio se normaliza a minúsculas y los dominios internacionalizados se convierten a su representación DNS. Se conserva la escritura de la parte anterior a `@`.
3. Al guardar se consulta DNS: el dominio debe existir y disponer de una ruta de correo válida. Se revisan los registros MX y que sus servidores resuelvan a una dirección; también se admite el MX implícito A/AAAA previsto por SMTP.
4. Se rechazan dominios inexistentes, MX que apuntan a servidores inexistentes y Null MX (dominios que declaran no recibir correo).

La consulta usa DNS sobre HTTPS de Cloudflare y, ante fallas temporales, Google. **Sólo se envían nombres de dominio, nunca el correo completo ni datos personales.** No se envían correos de verificación y no se conecta al servidor SMTP del destinatario.

La comprobación no garantiza la existencia de un buzón individual. Una dirección inventada dentro de un dominio real podría pasar; tampoco confirma propiedad, entrega o lectura del correo.

Si no hay Internet, la consulta falla o excede su plazo, el programa muestra un error temporal y no guarda. Conserva los campos para reintentar. Los formularios guardan de forma asíncrona y evitan duplicar operaciones o cambiar campos mientras se valida. Cada comprobación DNS tiene un límite total de 20 segundos, con intentos de 4 segundos por resolvedor.

### Dominio institucional

El dominio permitido es `acapulco.tecnm.mx`, cuya ruta de correo fue comprobada. El antiguo `alumnos.acapulco.tecnm.mx` respondió NXDOMAIN durante esta revisión y dejó de aceptarse. La sugerencia de inscripción utiliza ahora `@acapulco.tecnm.mx`.

Los registros anteriores no se modifican masivamente. Al editar un expediente que contenga el subdominio anterior, deberá capturarse el correo institucional correcto. Generar una dirección en AMBAR sigue sin crear un buzón en el servicio de correo de la institución.

## Pruebas realizadas

Se compilaron todas las fuentes C# de la aplicación y las pruebas mediante Roslyn con referencias .NET 8/Windows Forms. Pasaron 122 comprobaciones sin conexión MySQL: incluyen validaciones previas, teléfonos de distintas regiones, normalización, correos, respuestas DNS simuladas, caída/timeout/fallback, aplicación en los repositorios y consultas DNS reales a Gmail, el dominio institucional, NXDOMAIN y Null MX.

Esta revisión no ejecutó nuevamente la suite de escritura en MySQL ni el diseñador de Visual Studio. Las pruebas de integración existentes conservan un resolvedor DNS simulado para que dependan únicamente de la base de prueba.

## Referencias técnicas

- [Consulta de numeración nacional IFT](https://sns.ift.org.mx/sns-frontend/consulta-numeracion/numeracion-geografica.xhtml).
- [libphonenumber y límites de su validación](https://github.com/google/libphonenumber/blob/master/FAQ.md).
- [Paquete C# 9.0.42](https://www.nuget.org/packages/libphonenumber-csharp/9.0.42).
- [DNS JSON de Cloudflare](https://developers.cloudflare.com/1.1.1.1/encryption/dns-over-https/make-api-requests/dns-json/).
- [DNS JSON de Google](https://developers.google.com/speed/public-dns/docs/doh/json).
- [SMTP, RFC 5321](https://www.rfc-editor.org/rfc/rfc5321).
- [Null MX, RFC 7505](https://www.rfc-editor.org/rfc/rfc7505).
