using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class CommunityProfileRepository(AppDbContext context) : BaseRepository<CommunityProfile>(context), ICommunityProfileRepository
{
    public async Task<IEnumerable<CommunityProfile>> ListAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<CommunityProfile>().ToListAsync(cancellationToken);
    }

    public async Task<CommunityProfile?> FindByProfileIdAsync(int profileId, CancellationToken cancellationToken)
    {
        return await Context.Set<CommunityProfile>().FirstOrDefaultAsync(p => p.ProfileId == profileId, cancellationToken);
    }
}
