namespace Core;

public interface IAcknowledgementNotificationReader
{
    IAsyncEnumerable<string> ReadNotificationLinesAsync(string notificationPath);
}