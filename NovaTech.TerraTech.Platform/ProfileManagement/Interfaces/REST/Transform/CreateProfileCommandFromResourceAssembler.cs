using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Transform;
public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource) => new(resource.UserId, resource.FundoName, resource.ContactPhone, resource.MoistureThreshold, resource.TempThreshold);
}
