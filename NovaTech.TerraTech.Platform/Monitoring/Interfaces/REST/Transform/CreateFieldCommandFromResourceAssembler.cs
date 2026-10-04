using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

public class CreateFieldCommandFromResourceAssembler
{
    public static CreateFieldCommand ToCommandFromResource(CreateFieldResource resource)
    {
        return new CreateFieldCommand(
            new ProfileId(resource.ProfileId),
            new FieldName(resource.Name),
            new SizeM2(resource.SizeM2),
            new SoilType(resource.SoilType),
            new LocationLatLong(resource.Latitude, resource.Longitude), resource.CropName
        );
    }
}