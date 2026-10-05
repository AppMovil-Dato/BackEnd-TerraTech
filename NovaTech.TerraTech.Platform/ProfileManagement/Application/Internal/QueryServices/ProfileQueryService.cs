using NovaTech.TerraTech.Platform.ProfileManagement.Application.QueryServices;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Application.Internal.QueryServices;
public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        return await profileRepository.ListAsync();
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        return await profileRepository.FindByIdAsync(query.Id);
    }
}
