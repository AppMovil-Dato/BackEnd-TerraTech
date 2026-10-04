using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity) =>
        new(
            entity.Id, 
            entity.UserId, 
            entity.FundoNameString,
            entity.ContactPhoneString, 
            entity.MoistureThresholdValue, 
            entity.TempThresholdValue, entity.Location, entity.SizeM2
        );
}