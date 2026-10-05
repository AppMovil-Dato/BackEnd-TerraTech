using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Interfaces.REST.Transform;
public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification entity)
    {
        return new NotificationResource(entity.Id, entity.ProfileId, entity.Title, entity.Message, entity.IsRead, entity.IsAlert, entity.CreatedAt);
    }
}
