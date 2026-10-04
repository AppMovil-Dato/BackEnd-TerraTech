using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST;
public record UpsertMyProfileResource([Required, MinLength(2), MaxLength(150)] string FullName, [Required, MaxLength(100)] string FundoName, [Required, MaxLength(30)] string ContactPhone, [Required, MaxLength(250)] string Location, [Range(0.01, 999999999)] double SizeM2, [Range(0, 100)] double? MoistureThreshold = null, double? TempThreshold = null);
public record MyProfileResource(int Id, int UserId, string? FullName, string EmailAddress, string FundoName, string ContactPhone, string? Location, double? SizeM2, double MoistureThreshold, double TempThreshold);
[ApiController, Route("api/v1/profiles/me")]
public class MeProfileController(NovaTech.TerraTech.Platform.ProfileManagement.Application.Internal.CommandServices.SelfProfileService profiles) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MyProfileResource>> Get(CancellationToken ct)
    {
        var (profile, user) = await profiles.Get(User.UserId(), ct);
        return Resource(profile, user);
    }
    [HttpPut]
    public async Task<ActionResult<MyProfileResource>> Upsert(UpsertMyProfileResource r, CancellationToken ct)
    {
        var (profile, user) = await profiles.Upsert(User.UserId(), r.FullName, r.FundoName, r.ContactPhone, r.Location, r.SizeM2, r.MoistureThreshold, r.TempThreshold, ct);
        return Resource(profile, user);
    }
    private static MyProfileResource Resource(Profile p, User user) => new(p.Id, p.UserId, user.FullName, user.EmailAddress.Value, p.Name.Name, p.Phone.Number, p.Location, p.SizeM2, p.Thresholds.Moisture, p.Thresholds.Temperature);
}
