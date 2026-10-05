using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
public partial class Notification
{
    protected Notification()
    {
    }

    public Notification(CreateNotificationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ProfileId = command.ProfileId;
        Title = command.Title;
        Message = command.Message;
        IsRead = false;
        IsAlert = command.IsAlert;
    }

    public int Id { get; private set; }
    public int ProfileId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }
    public bool IsAlert { get; private set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
