using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Domain.Repository;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Application.Internal.CommandServices;
public class SelfProfileService(IProfileRepository profiles, IUserRepository users, IUnitOfWork unitOfWork)
{
    public async Task<(Profile profile, User user)> Get(int userId, CancellationToken ct)
    {
        var profile = (await profiles.ListAsync(ct)).SingleOrDefault(x => x.UserId == userId) ?? throw new ApiFailure(404, "PROFILE_NOT_FOUND", "Complete your profile first.");
        var user = await users.FindByIdAsync(userId, ct) ?? throw new ApiFailure(401, "UNKNOWN_USER", "Unknown user.");
        return (profile, user);
    }

    public async Task<(Profile profile, User user)> Upsert(int userId, string fullName, string fundoName, string phone, string location, double sizeM2, double? moisture, double? temperature, CancellationToken ct)
    {
        if (fullName.Trim().Length < 2 || string.IsNullOrWhiteSpace(location))
            throw new ApiFailure(400, "INVALID_INPUT", "Name and location are required.");
        var profile = (await profiles.ListAsync(ct)).SingleOrDefault(x => x.UserId == userId);
        if (profile is null)
        {
            profile = new Profile(userId, fundoName, phone, moisture ?? 30, temperature ?? 35);
            await profiles.AddAsync(profile, ct);
        }
        else
            profile.Update(new UpdateProfileCommand(profile.Id, fundoName, phone, moisture ?? profile.Thresholds.Moisture, temperature ?? profile.Thresholds.Temperature));
        profile.SetTerrain(location, sizeM2);
        var user = await users.FindByIdAsync(userId, ct) ?? throw new ApiFailure(401, "UNKNOWN_USER", "Unknown user.");
        user.SetFullName(fullName);
        await unitOfWork.CompleteAsync(ct);
        return (profile, user);
    }
}
