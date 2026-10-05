using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Transform;
public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource resource)
    {
        return new CreateNotificationCommand(resource.ProfileId, resource.Title, resource.Message, resource.IsAlert);
    }
}
