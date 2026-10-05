using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
public static class FieldResourceFromEntityAssembler
{
    public static FieldResource ToResourceFromEntity(Field entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var profileId = entity.ProfileId?.Value ?? 0;
        var name = entity.Name?.Value ?? string.Empty;
        var sizeM2 = entity.SizeM2?.Value ?? 0;
        var soilType = entity.SoilType?.Value ?? string.Empty;
        var latitude = entity.LocationLatLong?.Latitude ?? 0;
        var longitude = entity.LocationLatLong?.Longitude ?? 0;
        return new FieldResource(entity.Id, profileId, name, sizeM2, soilType, latitude, longitude, entity.CropName, entity.Boundary?.Select(p => new FieldVertexResource(p.Latitude, p.Longitude)).ToArray());
    }
}
