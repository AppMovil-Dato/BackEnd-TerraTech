using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Repositories;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Repositories;
namespace NovaTech.TerraTech.Platform.Monitoring.Application.Internal.QueryServices;
public class SensorReadingQueryService(ISensorDataRepository readings, IDeviceRepository devices, IFieldRepository fields, IProfileRepository profiles, TimeProvider clock)
{
    public async Task<(SensorReading reading, bool isStale)> Latest(int id, CancellationToken ct)
    {
        await Device(id, ct);
        var reading = await readings.Latest(id, ct) ?? throw new ApiFailure(404, "NO_READINGS", "Sensor has no readings.");
        return (reading, clock.GetUtcNow().UtcDateTime - reading.RecordedAt > TimeSpan.FromMinutes(30));
    }
    public async Task<SensorReading> Detail(int deviceId, int readingId, CancellationToken ct)
    {
        await Device(deviceId, ct);
        return await readings.FindReading(deviceId, readingId, ct) ?? throw new ApiFailure(404, "READING_NOT_FOUND", "Reading was not found.");
    }
    public async Task<(DateTime from, DateTime to, double minimumMoisture, List<SensorReading> readings)> History(int id, int days, CancellationToken ct)
    {
        if (days != 7 && days != 30) throw new ApiFailure(400, "INVALID_RANGE", "Days must be 7 or 30.");
        var device = await Device(id, ct);
        var field = await fields.FindByIdAsync(device.FieldId.Value, ct) ?? throw new ApiFailure(404, "FIELD_NOT_FOUND", "Field was not found.");
        var profile = await profiles.FindByIdAsync(field.ProfileId.Value, ct) ?? throw new ApiFailure(404, "PROFILE_NOT_FOUND", "Profile was not found.");
        var to = clock.GetUtcNow().UtcDateTime; var from = to.AddDays(-days);
        return (from, to, profile.Thresholds.Moisture, await readings.History(id, from, to, ct));
    }
    private async Task<Device> Device(int id, CancellationToken ct) => await devices.FindByIdAsync(id, ct) ?? throw new ApiFailure(404, "DEVICE_NOT_FOUND", "Device was not found.");
}
