using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Domain.Repositories;
public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> FindByProfileIdAsync(int profileId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> FindUnreadByProfileIdAsync(int profileId, CancellationToken cancellationToken = default);
}
