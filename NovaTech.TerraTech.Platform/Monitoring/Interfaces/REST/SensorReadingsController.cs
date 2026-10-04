using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Application.Internal.CommandServices;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST;
public record RegisterSensorResource([Required, RegularExpression("^TT-[0-9A-Z]{6}$")] string SensorCode, [Range(1, int.MaxValue)] int FieldId, [Required, MaxLength(100)] string Name);
public record ReadingResource(int Id, int DeviceId, DateTime RecordedAt, double MoisturePercent, double SoilTemperatureC, double NitrogenPpm, double PhosphorusPpm, double PotassiumPpm, string Source);
public record LatestReadingResource(ReadingResource Reading, bool IsStale);
public record ReadingHistoryResource(int DeviceId, DateTime FromUtc, DateTime ToUtc, double MinimumMoisturePercent, IReadOnlyList<ReadingResource> Readings);
[ApiController, Route("api/v1/devices")]
public class SensorReadingsController(NovaTech.TerraTech.Platform.Monitoring.Application.Internal.QueryServices.SensorReadingQueryService queries, SensorRegistrationService registration) : ControllerBase
{
    [HttpPost("register"), ProducesResponseType<DeviceResource>(201)]
    [SwaggerOperation(Summary = "Associate a provisioned sensor", Description = "TT-XXXXXX must already exist in the catalog and be available. Own field required. Concurrent association is prevented by a unique database index.")]
    public async Task<ActionResult<DeviceResource>> Register(RegisterSensorResource r, CancellationToken ct)
    {
        var device = await registration.Register(r.SensorCode, null, r.FieldId, r.Name, ct: ct);
        return Created($"/api/v1/devices/{device.Id}", DeviceResourceFromEntityAssembler.ToResourceFromEntity(device));
    }
    [HttpGet("{id:int}/readings/latest")]
    [SwaggerOperation(Summary = "Latest soil reading", Description = "UTC timestamp; moisture in %, soil temperature in °C, N/P/K in ppm. SIMULATED identifies demo data. isStale becomes true after 30 minutes; NO_READINGS is returned when empty.")]
    public async Task<ActionResult<LatestReadingResource>> Latest(int id, CancellationToken ct)
    {
        var (reading, isStale) = await queries.Latest(id, ct);
        return new LatestReadingResource(Resource(reading), isStale);
    }
    [HttpGet("{id:int}/readings")]
    [SwaggerOperation(Summary = "7 or 30 day history", Description = "Default 7 days. Chronological UTC range, minimum moisture reference in %, and readings. Empty collection when no measurements. Independent of statistical reports.")]
    public async Task<ActionResult<ReadingHistoryResource>> History(int id, [FromQuery] int days = 7, CancellationToken ct = default)
    {
        var (from, to, minimumMoisture, readings) = await queries.History(id, days, ct);
        return new ReadingHistoryResource(id, from, to, minimumMoisture, readings.Select(Resource).ToList());
    }
    [HttpGet("{deviceId:int}/readings/{readingId:int}")]
    [SwaggerOperation(Summary = "Reading detail", Description = "A persisted reading belonging to an owned device, including readings older than 30 days. Returns READING_NOT_FOUND for missing or mismatched readings. Same units and resource as history.")]
    public async Task<ActionResult<ReadingResource>> Detail(int deviceId, int readingId, CancellationToken ct)
    {
        return Resource(await queries.Detail(deviceId, readingId, ct));
    }
    private static ReadingResource Resource(SensorReading x) => new(x.Id, x.DeviceId, DateTime.SpecifyKind(x.RecordedAt, DateTimeKind.Utc), x.MoisturePercent, x.SoilTemperatureC, x.NitrogenPpm, x.PhosphorusPpm, x.PotassiumPpm, x.Source);
}
