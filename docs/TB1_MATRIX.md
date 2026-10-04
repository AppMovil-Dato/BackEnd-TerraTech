# Cierre funcional del backend TB1

Historias fijadas por el equipo: **US06, US07, US09, US17, US11, US10, US12 y consulta sin conexión**. La tabla separa cada criterio del servidor. Todas las rutas llevan `/api/v1`.

La columna de prueba identifica el caso en [JourneyTests.cs](../TerraTech.IntegrationTests/JourneyTests.cs) o [ClosureTests.cs](../TerraTech.IntegrationTests/ClosureTests.cs). `Account`, `Sensor` y `AssertProblem` son helpers ejecutados por los casos, no pruebas independientes. La evidencia de aprobación se encuentra por nombre de caso en [tb1.trx](tb1.trx), con resumen en [VALIDATION.md](VALIDATION.md). El recorrido TCP adicional está en [http-validation.json](http-validation.json).

| Historia | Criterio backend cerrado | Endpoint / interfaz | Prueba y evidencia | Estado |
|---|---|---|---|---|
| US06 | Nombre/correo/contraseña y confirmación válidos; mínimos; respuesta pública 201 | `POST authentication/sign-up` | RegistrationAcceptsMinimumsNormalizesEmailAndDoesNotExposePasswords | Verificado |
| US06 | Validar campos ausentes e inválidos por separado; ninguna cuenta persiste | `POST authentication/sign-up` | RegistrationFieldsFailIndependentlyWithoutPersistence; InvalidRegistration | Verificado |
| US06 | Confirmación obligatoria y coincidencia exacta, sin recortar contraseñas | `POST authentication/sign-up` | ConfirmationIsExactAndRejectedSignupDoesNotReserveEmail; RegistrationFieldsFailIndependentlyWithoutPersistence | Verificado |
| US06 | Duplicado incluso con correo en otra capitalización; 409 EMAIL_EXISTS | `POST authentication/sign-up` | RegistrationAcceptsMinimumsNormalizesEmailAndDoesNotExposePasswords; RegistrationDuplicateAndWrongPassword | Verificado |
| US07 | Login válido; JWT firmado de ocho horas; nombre y vencimiento | `POST authentication/sign-in; GET users/me` | Account (recorrido utilizado por las pruebas); ProfileEditPersistsPersonalTerrainDataAndPreservesEmailAndThresholds | Verificado |
| US07 | Usuario o contraseña incorrectos: 401 INVALID_CREDENTIALS | `POST authentication/sign-in` | RegistrationDuplicateAndWrongPassword; RegistrationAcceptsMinimumsNormalizesEmailAndDoesNotExposePasswords | Verificado |
| US07 | JWT ausente, manipulado, expirado y usuario inexistente; sujeto existente para comprobar firma/expiración | `GET users/me` | InvalidJwt | Verificado |
| US07 | Exigir expiración, algoritmo aprobado, firma y notBefore | `GET users/me` | JwtRequiresExpirationAndApprovedAlgorithmForExistingUser | Verificado |
| US07 | Listar, leer, modificar y eliminar solamente recursos propios | `Rutas privadas existentes` | TwoAccountsCannotReadWriteDeleteOrListOthersResources; AncillaryPrivateResourcesAlsoEnforceOwnership; LegacyMacUsesSameRegistryAndOwnedCrudWorks | Verificado |
| US09 | Perfil pendiente 404; creación y edición propias con persistencia | `GET/PUT profiles/me` | Account; ProfileEditPersistsPersonalTerrainDataAndPreservesEmailAndThresholds | Verificado |
| US09 | Editar nombre, teléfono, fundo, ubicación y m²; reflejar nombre en cuenta/login | `PUT profiles/me; GET users/me` | ProfileEditPersistsPersonalTerrainDataAndPreservesEmailAndThresholds | Verificado |
| US09 | Correo inmutable e identidad obtenida de la sesión | `PUT profiles/me` | ProfileEditPersistsPersonalTerrainDataAndPreservesEmailAndThresholds | Verificado |
| US09 | Validación independiente y conservación de datos ante entradas inválidas | `PUT profiles/me` | InvalidProfileDoesNotChangePersistedData | Verificado |
| US09 | Preservar umbrales omitidos y contrato legado snake_case | `PUT profiles/me; PUT profiles/{id}` | ProfileThresholdsAndLegacySnakeCaseRemainCompatible; ProfileEditPersistsPersonalTerrainDataAndPreservesEmailAndThresholds | Verificado |
| US17 | Código válido provisionado y parcela propia: asociación 201 | `POST devices/register` | Sensor (recorrido utilizado por las pruebas); UnknownOccupiedAndConcurrentSensorRegistration | Verificado |
| US17 | Código mal formado o desconocido: 400 y sin asociación | `POST devices/register` | MalformedSensorCodesDoNotCreateAssociations; UnknownOccupiedAndConcurrentSensorRegistration | Verificado |
| US17 | Parcela inexistente/ajena: 404; sensor ocupado incluso por otra cuenta: 409 | `POST devices/register` | UnknownOccupiedAndConcurrentSensorRegistration; TwoAccountsCannotReadWriteDeleteOrListOthersResources | Verificado |
| US17 | Registro concurrente permite una sola asociación | `POST devices/register` | UnknownOccupiedAndConcurrentSensorRegistration | Verificado |
| US17 | Alta por MAC usa el mismo catálogo y conserva identidad | `POST/PUT devices` | LegacyMacUsesSameRegistryAndOwnedCrudWorks | Verificado |
| US11 | Crear y elegir parcela propia; incluir cultivo, ubicación y superficie | `POST/GET fields` | Account; ParcelSelectionReturnsOnlyItsDevicesAndSupportsEmptyParcel | Verificado |
| US11 | Consultar dispositivos de la parcela elegida; lista vacía en parcela sin sensores | `GET fields/{fieldId}/devices` | ParcelSelectionReturnsOnlyItsDevicesAndSupportsEmptyParcel | Verificado |
| US11 | Excluir parcelas y dispositivos ajenos de listas y consultas | `GET fields; GET devices; GET fields/{fieldId}/devices` | ParcelSelectionReturnsOnlyItsDevicesAndSupportsEmptyParcel; TwoAccountsCannotReadWriteDeleteOrListOthersResources | Verificado |
| US10 | Humedad %, temperatura °C, N/P/K ppm, UTC y etiqueta SIMULATED | `GET devices/{id}/readings/latest` | LatestStalenessUsesStrictThirtyMinuteBoundary; CompleteJourneyReadingsUnitsRangesAndPersistence | Verificado |
| US10 | Devolver la muestra más reciente y estado sin lecturas NO_READINGS | `GET devices/{id}/readings/latest` | CompleteJourneyReadingsUnitsRangesAndPersistence; LatestStalenessUsesStrictThirtyMinuteBoundary | Verificado |
| US10 | Aviso antiguo únicamente después de 30 minutos, con reloj controlado | `GET devices/{id}/readings/latest` | LatestStalenessUsesStrictThirtyMinuteBoundary (1799, 1800 y 1801 segundos) | Verificado |
| US12 | Siete días por defecto y cambio a treinta; intervalos UTC inclusivos | `GET devices/{id}/readings?days=7\|30` | HistoryIncludesUtcBoundariesExcludesFutureAndSupportsDefaultRange | Verificado |
| US12 | Orden cronológico; excluir fuera del intervalo y fechas futuras; rechazar rangos inválidos | `GET devices/{id}/readings` | HistoryIncludesUtcBoundariesExcludesFutureAndSupportsDefaultRange; CompleteJourneyReadingsUnitsRangesAndPersistence | Verificado |
| US12 | Histórico vacío en 7/30 y umbral mínimo como referencia | `GET devices/{id}/readings` | CompleteJourneyReadingsUnitsRangesAndPersistence | Verificado |
| US12 | Detalle persistido, también anterior a treinta días; sin efectos secundarios | `GET devices/{deviceId}/readings/{readingId}` | ReadingDetailPreservesCacheContractAndRejectsOtherDeviceAndOwner | Verificado |
| US12 | 404 ante lectura inexistente, ajena o de otro dispositivo; 401 sin JWT | `GET devices/{deviceId}/readings/{readingId}` | ReadingDetailPreservesCacheContractAndRejectsOtherDeviceAndOwner | Verificado |
| Offline — contrato backend | IDs/fechas/origen y mismo DTO entre última muestra, histórico y detalle; consultas repetibles | `Rutas de mediciones anteriores` | ReadingDetailPreservesCacheContractAndRejectsOtherDeviceAndOwner | Verificado |
| Transversal | FK, índices únicos y eliminación restringida, incluso directamente en MySQL | `Persistencia + DELETE` | ForeignKeysAndUniqueReadingTimestampAreEnforced; CompleteJourneyReadingsUnitsRangesAndPersistence | Verificado |
| Transversal | Migración válida conserva asociaciones/MAC; preflight rechaza huérfanos/duplicados sin borrar | `Migraciones incrementales` | IncrementalMigrationPreservesLegacyDevicesAndAssociations; PreflightRejectsOrphansAndDuplicatesWithoutDeletingData | Verificado |
| Transversal | Demo explícita repetible y bloqueada en Production | `--demo-catalog; --demo-readings` | ExplicitDemoCommandsAreRepeatableAndBlockedInProduction | Verificado |
| Transversal | Swagger público, contratos de las rutas y errores Problem Details | `Swagger y rutas HTTP` | MissingRoutesAndPublicSwagger; AssertProblem (aserción utilizada por pruebas de cierre) | Verificado |

## Trabajo restante en Android

- US06: formulario con confirmación local antes de enviar `confirmPassword` obligatorio.
- US07: sesión, presentación de errores, almacenamiento protegido del token y separación de datos por cuenta.
- US09: pantallas de perfil y edición; conversión de m² a hectáreas.
- US17 / US11: asociación y selección de parcela/sensor, con estados vacíos y errores.
- US10: indicadores, unidades, fecha y aviso de antigüedad; etiqueta visible SIMULATED.
- US12: gráfico de humedad 7/30 y detalle usando el mismo recurso recibido o cacheado.
- Nueva HU offline: Room para parcelas, dispositivos y mediciones; `downloadedAt` local, fecha de medición y recalcular antigüedad cuando no haya red. **La ejecución offline no se afirma probada en backend: requiere las pruebas futuras de Android.**

El servidor ya proporciona las dependencias funcionales de estas historias; no se requieren endpoints de dashboard, recomendaciones ni sincronización offline adicionales. No hay nuevas tablas ni migraciones en este cierre. El despliegue conserva la revisión separada acordada.

## Alcance y porcentaje

No hay IA, recomendaciones, verificación de correo ni recuperación de contraseña en TB1. Los reportes estadísticos existentes mantienen su significado. La confirmación ahora también se valida en el servidor: reemplaza la decisión previa de validarla solamente en Android.

Esta matriz documenta cierre del **alcance backend acordado**, no cumplimiento de toda la entrega. El porcentaje funcional oficial requiere el denominador y pesos de la rúbrica y las evidencias Android/despliegue. La cobertura de líneas o el número de endpoints no sustituyen esa evaluación.
