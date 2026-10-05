# Sesión de registro y parcelas dibujadas — TB1

`POST /api/v1/authentication/sign-up` devuelve 201 con `id`, `emailAddress`, `fullName`, `token` y `expiresAt`. El JWT emitido utiliza las mismas validaciones y duración de ocho horas del login. Android puede entrar directamente, sin una segunda solicitud ni almacenamiento de contraseña. Registro duplicado sigue devolviendo 409.

Los contratos de crear, consultar y actualizar parcelas añaden `boundary`, una colección opcional de `{latitude, longitude}`. Se guarda el polígono completo, sin repetir el vértice de cierre. Se admiten 3–100 vértices distintos; coordenadas válidas, contorno sin cruces y área entre 0.01 y 9,999,999 m². El servidor calcula el área esférica en m² y la ubicación de referencia como promedio de los vértices; esta ubicación no es un centroide topográfico. Android convierte a hectáreas.

La actualización legacy sin `boundary` conserva el contorno. `boundary: []` lo elimina y conserva el punto/área enviados. El dato es aproximado y no representa una medición catastral. Las autorizaciones de perfil y parcela no cambian.

Migración incremental `20261005012206_FieldBoundary`: únicamente añade la columna nullable `fields.boundary` (longtext JSON). No borra filas ni recrea tablas. Registros anteriores conservan su área y ubicación y no tienen contorno. `Database__MigrateOnStartup=true` aplica esta migración en el despliegue existente.

Pruebas: sesión utilizable inmediatamente mediante `/users/me`, vencimiento, persistencia de vértices/área, normalización del cierre, actualización legacy, eliminación explícita del contorno, validación de geometría, propiedad con dos cuentas, preservación de base antigua y formato Problem Details. La suite completa Debug utiliza MySQL local aislado. Evidencia de esta actualización en `docs/ux-validation.json` y `docs/ux-coverage.cobertura.xml`.
