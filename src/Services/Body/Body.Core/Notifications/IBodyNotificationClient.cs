namespace Sas.Body.Core.Notifications;

public interface IBodyNotificationClient
{
    Task SendBodyDatabaseChangedNotification(string? senderId, CancellationToken cancellationToken);
}
