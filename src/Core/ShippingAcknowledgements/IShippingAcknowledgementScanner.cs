namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementScanner
{
    Task ScanAndDispatchAcknowledgements();
}