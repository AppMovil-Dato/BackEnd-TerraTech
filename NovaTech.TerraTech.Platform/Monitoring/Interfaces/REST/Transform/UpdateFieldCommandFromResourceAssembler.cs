using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

public static class UpdateFieldCommandFromResourceAssembler
{
    public static UpdateFieldCommand ToCommandFromResource(int id, UpdateFieldResource resource)
    {
        return new UpdateFieldCommand(
            id,
            new FieldName(resource.Name),
            new SizeM2(resource.SizeM2),
            new SoilType(resource.SoilType),
            new LocationLatLong(resource.Latitude, resource.Longitude), resource.CropName
        );
    }
}