using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaTech.TerraTech.Platform.CommunityManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommunityManagement.Application.Services;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST;
[ApiController]
[Route("api/v1/community-profiles")]
[Produces("application/json")]
[Tags("Communities")]
public class CommunityProfilesController(ICommunityProfileService profileService, ILogger<CommunityProfilesController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new community profile", Description = "Creates a profile for a community member")]
    [SwaggerResponse(201, "Profile created", typeof(CommunityProfileResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateProfile([FromBody] CreateCommunityProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateCommunityProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await profileService.Handle(command, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProfileByProfileId), new { profileId = result.Value.ProfileId }, CommunityProfileResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }

            return (CommunityError)result.Error switch
            {
                CommunityError.InvalidProfileId => BadRequest("Invalid profile request data"),
                _ => Problem(title: "Unexpected server error", detail: "An unexpected error occurred", statusCode: 500)};
        }
        catch (ApiFailure)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating community profile");
            return Problem(title: "Unexpected server error", detail: "An unexpected error occurred", statusCode: 500);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation(Summary = "Gets all community profiles")]
    [SwaggerResponse(200, "Profiles retrieved", typeof(IEnumerable<CommunityProfileResource>))]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var query = new GetAllCommunityProfilesQuery();
        var profiles = await profileService.Handle(query, cancellationToken);
        var resources = profiles.Select(CommunityProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [AllowAnonymous]
    [HttpGet("{profileId}")]
    [SwaggerOperation(Summary = "Gets a profile by its Profile ID")]
    [SwaggerResponse(200, "Profile found", typeof(CommunityProfileResource))]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> GetProfileByProfileId(int profileId, CancellationToken cancellationToken)
    {
        var query = new GetCommunityProfileByProfileIdQuery(profileId);
        var profile = await profileService.Handle(query, cancellationToken);
        if (profile == null)
            return NotFound();
        var resource = CommunityProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Updates a community profile", Description = "Updates nickname, bio, and visibility")]
    [SwaggerResponse(200, "Profile updated", typeof(CommunityProfileResource))]
    [SwaggerResponse(404, "Profile not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateCommunityProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateCommunityProfileCommand(id, resource.Nickname, resource.ReputationScore, resource.PublicBio, resource.VisibilityStatus);
            var result = await profileService.Handle(command, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(CommunityProfileResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }

            return (CommunityError)result.Error == CommunityError.NotFound ? NotFound("Profile not found") : Problem(title: "Unexpected server error", statusCode: 500);
        }
        catch (ApiFailure)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating profile {Id}", id);
            return Problem(title: "Unexpected server error", statusCode: 500);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a community profile", Description = "Deletes a community profile by its Profile ID")]
    [SwaggerResponse(204, "Profile deleted")]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> DeleteProfile(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCommunityProfileCommand(id);
        var result = await profileService.Handle(command, cancellationToken);
        if (!result)
            return NotFound("Profile not found");
        return NoContent();
    }
}
