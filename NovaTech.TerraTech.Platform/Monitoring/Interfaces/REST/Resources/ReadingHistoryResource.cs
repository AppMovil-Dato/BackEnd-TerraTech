namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

public record ReadingHistoryResource(
    int DeviceId,
    DateTime FromUtc,
    DateTime ToUtc,
    double MinimumMoisturePercent,
    IReadOnlyList<ReadingResource> Readings
);
