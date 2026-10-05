using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Notification resource")]
public record NotificationResource([SwaggerParameter(Description = "Notification identifier")] int Id, [SwaggerParameter(Description = "Profile identifier")] int ProfileId, [SwaggerParameter(Description = "Notification title")] string Title, [SwaggerParameter(Description = "Notification message")] string Message, [SwaggerParameter(Description = "Whether the notification has been read")] bool IsRead, [SwaggerParameter(Description = "Whether this is an alert")] bool IsAlert, [SwaggerParameter(Description = "Creation timestamp")] DateTimeOffset? CreatedAt);
