using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NovaTech.TerraTech.Platform.ProfileManagement.Application.CommandServices;
using NovaTech.TerraTech.Platform.ProfileManagement.Application.QueryServices;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Transform;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST;
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(IProfileCommandService profileCommandService, IProfileQueryService profileQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create Profile", Description = "Create a new profile.", OperationId = "CreateProfile")]
    [SwaggerResponse(201, "The profile was created.", typeof(ProfileResource))]
    [SwaggerResponse(400, "The profile was not created.")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource)
    {
        var command = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var profile = await profileCommandService.Handle(command);
        if (profile == null)
            return BadRequest();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new { profileId = profileResource.Id }, profileResource);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Profiles", Description = "Get all profiles.", OperationId = "GetAllProfiles")]
    [SwaggerResponse(200, "The profiles were found and returned.", typeof(IEnumerable<ProfileResource>))]
    [SwaggerResponse(404, "The profiles were not found.")]
    public async Task<IActionResult> GetAllProfiles()
    {
        var profiles = await profileQueryService.Handle(new GetAllProfilesQuery());
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{profileId:int}")]
    [SwaggerOperation(Summary = "Get Profile by Id", Description = "Get a profile by its unique identifier.", OperationId = "GetProfileById")]
    [SwaggerResponse(200, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(profileId));
        if (profile == null)
            return NotFound();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }

    [HttpPut("{profileId:int}")]
    [SwaggerOperation(Summary = "Update Profile", Description = "Update an existing profile.", OperationId = "UpdateProfile")]
    [SwaggerResponse(200, "The profile was updated.", typeof(ProfileResource))]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> UpdateProfile(int profileId, [FromBody] UpdateProfileResource resource)
    {
        var command = new UpdateProfileCommand(profileId, resource.FundoName, resource.ContactPhone, resource.MoistureThreshold, resource.TempThreshold);
        var profile = await profileCommandService.Handle(command);
        if (profile == null)
            return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }

    [HttpDelete("{profileId:int}")]
    [SwaggerOperation(Summary = "Delete Profile", Description = "Delete an existing profile.", OperationId = "DeleteProfile")]
    [SwaggerResponse(204, "The profile was deleted successfully.")]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> DeleteProfile(int profileId)
    {
        var command = new DeleteProfileCommand(profileId);
        await profileCommandService.Handle(command);
        return NoContent();
    }
}
