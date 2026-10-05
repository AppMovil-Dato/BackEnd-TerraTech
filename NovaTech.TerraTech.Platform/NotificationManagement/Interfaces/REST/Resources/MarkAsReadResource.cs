using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to mark a notification as read")]
public record MarkAsReadResource([Required][SwaggerParameter(Description = "Notification identifier")] int Id);
