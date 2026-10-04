using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;

public record UpdateFieldCommand(
    int Id,
    FieldName Name,
    SizeM2 SizeM2,
    SoilType SoilType,
    LocationLatLong LocationLatLong, string? CropName = null
);