namespace Core;

public interface IShippingAcknowledgementProcessor
{
    Task ProcessShippingAcknowledgementNotification(string filePath);
}