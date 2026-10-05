using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Repositories;
public interface ICommunityProfileRepository : IBaseRepository<CommunityProfile>
{
    Task<CommunityProfile?> FindByProfileIdAsync(int profileId, CancellationToken cancellationToken = default);
}
