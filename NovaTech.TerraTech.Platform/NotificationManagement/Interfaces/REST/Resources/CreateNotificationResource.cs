using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create a notification")]
public record CreateNotificationResource([Required][SwaggerParameter(Description = "Profile identifier")] int ProfileId, [Required][SwaggerParameter(Description = "Notification title")] string Title, [Required][SwaggerParameter(Description = "Notification message")] string Message, [SwaggerParameter(Description = "Whether this is an alert notification")] bool IsAlert = false);
