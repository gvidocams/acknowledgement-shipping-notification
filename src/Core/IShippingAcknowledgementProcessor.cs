namespace Core;

public interface IShippingAcknowledgementProcessor
{
    void ProcessShippingAcknowledgementNotification(string filePath);
}