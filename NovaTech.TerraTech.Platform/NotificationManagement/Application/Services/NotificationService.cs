using NovaTech.TerraTech.Platform.NotificationManagement.Application.Errors;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Application.Services;
public class NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, ILogger<NotificationService> logger) : INotificationService
{
    public async Task<Result<Notification>> Handle(CreateNotificationCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = new Notification(command);
            await notificationRepository.AddAsync(notification, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Notification>.Success(notification);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating notification for ProfileId {ProfileId}", command.ProfileId);
            return Result<Notification>.Failure(NotificationError.InvalidTitle, "The provided title is invalid");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating notification for ProfileId {ProfileId}", command.ProfileId);
            return Result<Notification>.Failure(NotificationError.UnexpectedError, "An unexpected error occurred while creating the notification");
        }
    }

    public async Task<Result<bool>> Handle(MarkAsReadCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await notificationRepository.FindByIdAsync(command.Id, cancellationToken);
            if (notification == null)
                return Result<bool>.Failure(NotificationError.NotFound, "The notification was not found");
            notification.MarkAsRead();
            notificationRepository.Update(notification);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error marking notification {Id} as read", command.Id);
            return Result<bool>.Failure(NotificationError.UnexpectedError, "An unexpected error occurred while marking the notification as read");
        }
    }

    public async Task<IEnumerable<Notification>> Handle(GetNotificationsByProfileQuery query, CancellationToken cancellationToken = default)
    {
        return await notificationRepository.FindByProfileIdAsync(query.ProfileId, cancellationToken);
    }

    public async Task<IEnumerable<Notification>> HandleGetAll(CancellationToken cancellationToken = default)
    {
        return await notificationRepository.ListAsync(cancellationToken);
    }

    public async Task<Notification?> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await notificationRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}
