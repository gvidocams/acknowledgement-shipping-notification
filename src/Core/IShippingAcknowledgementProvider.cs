namespace Core;

public interface IShippingAcknowledgementProvider
{
    List<string> GetShippingAcknowledgementPaths();
    void CompleteShippingAcknowledgementNotification(string filePath);
}