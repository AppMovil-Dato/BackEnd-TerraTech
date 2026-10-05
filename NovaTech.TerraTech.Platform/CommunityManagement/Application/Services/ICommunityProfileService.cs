using NovaTech.TerraTech.Platform.CommunityManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Application.Services;
public interface ICommunityProfileService
{
    Task<Result<CommunityProfile>> Handle(CreateCommunityProfileCommand command, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommunityProfile>> Handle(GetAllCommunityProfilesQuery query, CancellationToken cancellationToken = default);
    Task<CommunityProfile?> Handle(GetCommunityProfileByIdQuery query, CancellationToken cancellationToken = default);
    Task<CommunityProfile?> Handle(GetCommunityProfileByProfileIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<CommunityProfile>> Handle(UpdateCommunityProfileCommand command, CancellationToken cancellationToken = default);
    Task<bool> Handle(DeleteCommunityProfileCommand command, CancellationToken cancellationToken = default);
}
