namespace Core;

public interface IShippingAcknowledgementScanner
{
    Task ScanAndDispatchAcknowledgements();
}