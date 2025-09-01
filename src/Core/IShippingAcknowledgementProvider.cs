namespace Core;

public interface IShippingAcknowledgementProvider
{
    List<string> GetShippingAcknowledgementNotificationLocations();
    void CompleteShippingAcknowledgementNotification(string notificationLocation);
}