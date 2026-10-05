using NovaTech.TerraTech.Platform.NotificationManagement.Application.Errors;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Application.Services;
public interface INotificationService
{
    Task<Result<Notification>> Handle(CreateNotificationCommand command, CancellationToken cancellationToken = default);
    Task<Result<bool>> Handle(MarkAsReadCommand command, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> Handle(GetNotificationsByProfileQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> HandleGetAll(CancellationToken cancellationToken = default);
    Task<Notification?> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken = default);
}
