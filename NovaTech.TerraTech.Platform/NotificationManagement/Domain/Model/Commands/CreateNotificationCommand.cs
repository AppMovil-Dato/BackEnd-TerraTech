namespace NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;
public record CreateNotificationCommand(int ProfileId, string Title, string Message, bool IsAlert = false);
