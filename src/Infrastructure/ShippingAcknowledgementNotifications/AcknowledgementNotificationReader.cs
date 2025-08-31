using Core;

namespace Infrastructure.ShippingAcknowledgementNotifications;

public class AcknowledgementNotificationReader : IAcknowledgementNotificationReader
{
    public async IAsyncEnumerable<string> ReadNotificationLinesAsync(string notificationPath)
    {
        using var reader = new StreamReader(notificationPath);

        while (await reader.ReadLineAsync() is { } line)
        {
            yield return line;
        }
    }
}