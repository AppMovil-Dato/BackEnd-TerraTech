# Validación de la implementación TB1

Ejecución: 2026-10-04T03:51:05.005794+00:00. SDK .NET 10.0.401; compilación y pruebas exclusivamente **Debug**. MySQL 9.5.0 aislado en localhost:33308; bases nuevas con UUID y bases independientes para migraciones antiguas. La base revisada anteriormente no fue borrada ni migrada.

- **49 pruebas aprobadas, 0 fallidas, 0 omitidas**.
- Líneas: **76.33 %** (1445/1893).
- Ramas: **42.85 %** (141/329).
- Migraciones y código generado excluidos; la cobertura abarca el backend completo, incluidos módulos heredados fuera del recorrido. Los comandos ejecutados como procesos separados se verifican funcionalmente, pero sus procesos hijos no se instrumentan con el colector del proceso principal.
- Build sin errores. Se conservan 35 advertencias de nulabilidad en código heredado; no hay advertencia NU1903 de Microsoft.OpenApi en la compilación validada.
- Recorrido TCP/HTTP real en `http://127.0.0.1:55023`: **15 solicitudes verificadas**, confirmación rechazada, cuenta propia, perfil, parcela, selección de dispositivos, asociación, ausencia inicial de lecturas y consulta posterior de **168 lecturas de siete días y 720 de treinta días**. Todas identificadas como `SIMULATED`; ID y fecha UTC disponibles para Room; detalle por ID idéntico al recurso descargado.
- Swagger exportado del servidor Debug: **60 operaciones**. Registro/login y lecturas públicas de catálogo/comunidad reflejan acceso anónimo; contratos nuevos incluidos, confirmPassword marcado como obligatorio y detalle por ID documentado.
- Dockerfile y configuración de despliegue preparados para revisión; imagen no construida y backend no publicado.

Evidencia: [resultados TRX](tb1.trx), [cobertura Cobertura XML](coverage.cobertura.xml), [resumen de pruebas](test-summary.json), [recorrido HTTP](http-validation.json), [OpenAPI](backend-openapi.snapshot.json). No se guardaron credenciales ni tokens del recorrido HTTP.

La cobertura de líneas de 76.33 % **no demuestra el 70 % funcional exigido por TB1**. Evaluar el alcance con [la matriz de historias y criterios](TB1_MATRIX.md); La implementación Android y sus pruebas se documentan en el repositorio App-Android-Terratech; la aceptación en teléfono físico permanece pendiente.

El cierre incluye pruebas separadas de confirmación exacta, edición persistente de perfil, selección vacía/propia, JWT inválidos con sujeto existente, detalle protegido y límites UTC. La última muestra se marca antigua estrictamente después de 30 minutos. Esta ampliación no agrega una migración.

## Reproducir

Usar el comando de `dotnet test` del README con un servidor MySQL exclusivo de pruebas. Para el recorrido TCP, levantar la API con variables de Development y una base aislada, y ejecutar:

```sh
python3 scripts/demo-journey.py --base-url http://localhost:8080
```

El script invoca comandos Debug explícitos, crea una cuenta temporal y elige uno de los cinco sensores de demostración disponibles. Se limita a localhost y exige Development. Conserva asociaciones previas; si todos los sensores están ocupados, solicita otra base aislada/provisión. Los correos y contraseñas de prueba no son cuentas sembradas ni se usan en producción.

Revalidación antes de subir a GitHub: **49 pruebas Debug aprobadas, cero fallos y cero omitidas**, sobre MySQL aislado y bases nuevas. [Resultado](push-validation.json). Esta ejecución no volvió a medir cobertura; los porcentajes anteriores corresponden a la ejecución instrumentada indicada arriba.
