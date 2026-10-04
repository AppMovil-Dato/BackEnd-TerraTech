using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaTech.TerraTech.Platform.Monitoring.Application.Services;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
using NovaTech.TerraTech.Platform.Shared.Resources;
using Swashbuckle.AspNetCore.Annotations;
using NovaTech.TerraTech.Platform.Monitoring.Application.Errors;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Monitoring")]
public class DevicesController(
    IDeviceCommandService deviceCommandService,
    IDeviceQueryService deviceQueryService,
    IStringLocalizer<CommonMessages> localizer,
    ILogger<DevicesController> logger)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Device",
        Description = "Creates a new IoT device associated with a field",
        OperationId = "CreateDevice")]
    [SwaggerResponse(201, "The Device was created", typeof(DeviceResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(409, "A device with this MAC address already exists", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateDevice(
        [FromBody] CreateDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await deviceCommandService.Handle(command, cancellationToken);
            
            return ActionResultFromCreateDeviceResultAssembler.ToActionResultFromCreateDeviceResult(
                result, this, localizer, nameof(GetDeviceById));
        }
        catch (ApiFailure) { throw; }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating device with MAC {MacAddress}", resource.MacAddress);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating device with MAC {MacAddress}", resource.MacAddress);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingDevice"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all devices",
        Description = "Retrieves all available devices",
        OperationId = "GetAllDevices")]
    [SwaggerResponse(200, "List of all devices", typeof(IEnumerable<DeviceResource>))]
    public async Task<ActionResult<IEnumerable<DeviceResource>>> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            var resources = await deviceQueryService.GetAllDeviceResourcesAsync(cancellationToken);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving all devices");
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Gets a device by id",
        Description = "Retrieves a specific device by its identifier",
        OperationId = "GetDeviceById")]
    [SwaggerResponse(200, "The device was found", typeof(DeviceResource))]
    [SwaggerResponse(404, "The device was not found")]
    public async Task<ActionResult> GetDeviceById(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var device = await deviceQueryService.GetDeviceByIdAsync(id, cancellationToken);
            if (device is null) return NotFound();
            var resource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(device);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving device with id {DeviceId}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("~/api/v1/fields/{fieldId:int}/devices")]
    [SwaggerOperation(
        Summary = "Gets devices by field id",
        Description = "Retrieves all devices belonging to a specific field",
        OperationId = "GetDevicesByFieldId")]
    [SwaggerResponse(200, "List of devices for the field", typeof(IEnumerable<DeviceResource>))]
    [SwaggerResponse(404, "Field not found")]
    public async Task<ActionResult<IEnumerable<DeviceResource>>> GetDevicesByFieldId(
        int fieldId,
        CancellationToken cancellationToken)
    {
        try
        {
            var devices = await deviceQueryService.GetDevicesByFieldIdAsync(fieldId, cancellationToken);
            var resources = devices.Select(DeviceResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (ArgumentException)
        {
            return NotFound(new { error = localizer["FieldNotFound"].Value });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving devices for field {FieldId}", fieldId);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("~/api/v1/status/{status}/devices")]
    [SwaggerOperation(
        Summary = "Gets devices by status",
        Description = "Retrieves all devices with a specific status (ONLINE, OFFLINE, LOW_BATTERY)",
        OperationId = "GetDevicesByStatus")]
    [SwaggerResponse(200, "List of devices with the specified status", typeof(IEnumerable<DeviceResource>))]
    [SwaggerResponse(400, "Invalid status value")]
    public async Task<ActionResult<IEnumerable<DeviceResource>>> GetDevicesByStatus(
        [FromRoute] string status,
        CancellationToken cancellationToken)
    {
        try
        {
            var deviceStatus = DeviceStatus.Create(status);
            var devices = await deviceQueryService.GetDevicesByStatusAsync(deviceStatus, cancellationToken);
            var resources = devices.Select(DeviceResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (ApiFailure) { throw; }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid status value: {Status}", status);
            return BadRequest(new { error = localizer["InvalidDeviceStatus"].Value });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving devices with status {Status}", status);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Updates a device",
        Description = "Updates an existing device's MAC, status, and last sync time",
        OperationId = "UpdateDevice")]
    [SwaggerResponse(200, "The device was updated", typeof(DeviceResource))]
    [SwaggerResponse(400, "Invalid request data", typeof(string))]
    [SwaggerResponse(404, "Device not found", typeof(string))]
    [SwaggerResponse(409, "MAC address already in use", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateDevice(
        [FromRoute] int id,
        [FromBody] UpdateDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateDeviceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await deviceCommandService.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error switch
                {
                    CreateDeviceError.DeviceNotFound => NotFound(),
                    CreateDeviceError.InvalidData or CreateDeviceError.InvalidMacAddress => BadRequest(new { error = result.Message }),
                    CreateDeviceError.DuplicateDevice => Conflict(new { error = result.Message }),
                    _ => Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: result.Message,
                        statusCode: StatusCodes.Status500InternalServerError)
                };
            }

            var resourceResponse = DeviceResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
            return Ok(resourceResponse);
        }
        catch (ApiFailure) { throw; }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating device {DeviceId}", id);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating device {DeviceId}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingDevice"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Deletes a device",
        Description = "Deletes an existing device by its identifier",
        OperationId = "DeleteDevice")]
    [SwaggerResponse(204, "The device was deleted")]
    [SwaggerResponse(404, "Device not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteDevice(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteDeviceCommand(id);
            var result = await deviceCommandService.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error switch
                {
                    CreateDeviceError.DeviceNotFound => NotFound(new { error = localizer["DeviceNotFound"].Value }),
                    _ => Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: result.Message,
                        statusCode: StatusCodes.Status500InternalServerError)
                };
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting device with id {DeviceId}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingDevice"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}