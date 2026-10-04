using Microsoft.AspNetCore.Mvc;
using NovaTech.TerraTech.Platform.NotificationManagement.Application.Errors;
using NovaTech.TerraTech.Platform.NotificationManagement.Application.Services;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Transform;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Notifications")]
public class NotificationsController(
    INotificationService notificationService,
    ILogger<NotificationsController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new notification", Description = "Creates a notification for a specific profile")]
    [SwaggerResponse(201, "Notification created", typeof(NotificationResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await notificationService.Handle(command, cancellationToken);
            
            if (result.IsSuccess && result.Value != null)
            {
                return CreatedAtAction(nameof(GetNotificationById), new { id = result.Value.Id }, 
                    NotificationResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }
            
            if (result.Error is NotificationError notificationError)
            {
                return notificationError switch
                {
                    NotificationError.InvalidTitle or NotificationError.InvalidMessage or NotificationError.InvalidProfileId 
                        => BadRequest("Invalid notification request"),
                    _ => Problem(title: "Unexpected server error", 
                        detail: "An unexpected error occurred while processing your request", statusCode: 500)
                };
            }
            
            return Problem(title: "Unexpected server error", 
                detail: "An unexpected error occurred while processing your request", statusCode: 500);
        }
        catch (ApiFailure) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating notification");
            return Problem(title: "Unexpected server error", 
                detail: "An unexpected error occurred while processing your request", statusCode: 500);
        }
    }

    [HttpPut("{notificationId}/reads")]
    [SwaggerOperation(Summary = "Marks a notification as read")]
    [SwaggerResponse(200, "Notification marked as read")]
    [SwaggerResponse(404, "Notification not found")]
    [SwaggerResponse(500, "Unexpected error")]
    public async Task<IActionResult> MarkAsRead(int notificationId, CancellationToken cancellationToken)
    {
        try
        {
            var command = new MarkAsReadCommand(notificationId);
            var result = await notificationService.Handle(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                return Ok(new { message = "Notification marked as read" });
            }
            
            if (result.Error is NotificationError notificationError && notificationError == NotificationError.NotFound)
            {
                return NotFound();
            }
            
            return Problem(title: "Unexpected server error", statusCode: 500);
        }
        catch (ApiFailure) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error marking notification {Id} as read", notificationId);
            return Problem(title: "Unexpected server error", statusCode: 500);
        }
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets notifications", Description = "Gets all notifications or filtered by profile")]
    [SwaggerResponse(200, "Notifications retrieved", typeof(IEnumerable<NotificationResource>))]
    public async Task<IActionResult> GetNotificationsByProfile(
        [FromQuery] int? profileId = null,  // Cambiado de string a int?
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Notification> notifications;
        
        if (!profileId.HasValue)
        {
            notifications = await notificationService.HandleGetAll(cancellationToken);
        }
        else
        {
            var query = new GetNotificationsByProfileQuery(profileId.Value);
            notifications = await notificationService.Handle(query, cancellationToken);
        }
        
        var resources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a notification by id")]
    [SwaggerResponse(200, "Notification found", typeof(NotificationResource))]
    [SwaggerResponse(404, "Notification not found")]
    public async Task<IActionResult> GetNotificationById(int id, CancellationToken cancellationToken)
    {
        var query = new GetNotificationByIdQuery(id);
        var notification = await notificationService.Handle(query, cancellationToken);
        
        if (notification == null)
            return NotFound();
        
        var resource = NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
        return Ok(resource);
    }
}