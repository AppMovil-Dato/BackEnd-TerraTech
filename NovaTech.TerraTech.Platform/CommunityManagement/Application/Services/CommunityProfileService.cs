using Microsoft.Extensions.Logging;
using NovaTech.TerraTech.Platform.CommunityManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Application.Services;
public class CommunityProfileService(ICommunityProfileRepository profileRepository, IUnitOfWork unitOfWork, ILogger<CommunityProfileService> logger) : ICommunityProfileService
{
    public async Task<Result<CommunityProfile>> Handle(CreateCommunityProfileCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var profile = new CommunityProfile(command);
            await profileRepository.AddAsync(profile, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<CommunityProfile>.Success(profile);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating community profile {ProfileId}", command.ProfileId);
            return Result<CommunityProfile>.Failure(CommunityError.InvalidProfileId, "The provided profile data is invalid");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating community profile {ProfileId}", command.ProfileId);
            return Result<CommunityProfile>.Failure(CommunityError.DatabaseError, "An unexpected error occurred while creating the community profile");
        }
    }

    public async Task<IEnumerable<CommunityProfile>> Handle(GetAllCommunityProfilesQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.ListAsync(cancellationToken);
    }

    public async Task<CommunityProfile?> Handle(GetCommunityProfileByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<CommunityProfile?> Handle(GetCommunityProfileByProfileIdQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.FindByProfileIdAsync(query.ProfileId, cancellationToken);
    }

    public async Task<Result<CommunityProfile>> Handle(UpdateCommunityProfileCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var profile = await profileRepository.FindByIdAsync(command.Id, cancellationToken);
            if (profile == null)
                return Result<CommunityProfile>.Failure(CommunityError.NotFound, "The profile was not found");
            profile.UpdateInformation(command.Nickname, command.ReputationScore, command.PublicBio, command.VisibilityStatus);
            profileRepository.Update(profile);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<CommunityProfile>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating community profile {Id}", command.Id);
            return Result<CommunityProfile>.Failure(CommunityError.DatabaseError, "An unexpected error occurred while updating the profile");
        }
    }

    public async Task<bool> Handle(DeleteCommunityProfileCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var profile = await profileRepository.FindByIdAsync(command.Id, cancellationToken);
            if (profile == null)
                return false;
            profileRepository.Remove(profile);
            await unitOfWork.CompleteAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting community profile {Id}", command.Id);
            return false;
        }
    }
}
