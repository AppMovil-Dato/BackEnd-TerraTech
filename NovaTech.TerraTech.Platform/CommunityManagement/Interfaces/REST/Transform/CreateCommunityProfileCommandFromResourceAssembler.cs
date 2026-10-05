using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Transform;
public static class CreateCommunityProfileCommandFromResourceAssembler
{
    public static CreateCommunityProfileCommand ToCommandFromResource(CreateCommunityProfileResource resource)
    {
        return new CreateCommunityProfileCommand(resource.ProfileId, resource.Nickname, resource.ReputationScore, resource.PublicBio, resource.VisibilityStatus);
    }
}
