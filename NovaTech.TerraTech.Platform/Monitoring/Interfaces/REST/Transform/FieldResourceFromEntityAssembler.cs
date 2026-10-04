using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="Field"/> entities to <see cref="FieldResource"/>.
/// </summary>
/// <remarks>
/// Author: Guillermo Howard Robles - u202222275
/// </remarks>
public static class FieldResourceFromEntityAssembler
{
    public static FieldResource ToResourceFromEntity(Field entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        // Safe extraction with null checks and default values
        var profileId = entity.ProfileId?.Value ?? 0;
        var name = entity.Name?.Value ?? string.Empty;
        var sizeM2 = entity.SizeM2?.Value ?? 0;
        var soilType = entity.SoilType?.Value ?? string.Empty;
        var latitude = entity.LocationLatLong?.Latitude ?? 0;
        var longitude = entity.LocationLatLong?.Longitude ?? 0;

        return new FieldResource(
            entity.Id,
            profileId,
            name,
            sizeM2,
            soilType,
            latitude,
            longitude,
            entity.CropName
        );
    }
}