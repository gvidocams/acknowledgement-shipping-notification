namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementProcessor
{
    Task ProcessShippingAcknowledgementNotification(string filePath);
}