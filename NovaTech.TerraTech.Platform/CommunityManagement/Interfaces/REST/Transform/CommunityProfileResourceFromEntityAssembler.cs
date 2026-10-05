using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Transform;
public static class CommunityProfileResourceFromEntityAssembler
{
    public static CommunityProfileResource ToResourceFromEntity(CommunityProfile entity)
    {
        return new CommunityProfileResource(entity.Id, entity.ProfileId, entity.Nickname.Nickname, entity.ReputationScore.Score, entity.PublicBio.Bio, entity.VisibilityStatus);
    }
}
