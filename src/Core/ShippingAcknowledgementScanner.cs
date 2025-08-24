using Microsoft.Extensions.Logging;

namespace Core;

public class ShippingAcknowledgementScanner(
    IShippingAcknowledgementProvider shippingAcknowledgementProvider,
    IShippingAcknowledgementProcessor shippingAcknowledgementProcessor,
    ILogger<ShippingAcknowledgementScanner> logger) : IShippingAcknowledgementScanner
{
    public void ScanAndDispatchAcknowledgements()
    {
        var fileNames = shippingAcknowledgementProvider.GetShippingAcknowledgementPaths();

        logger.LogInformation("Found {AcknowledgementCount} acknowledgements", fileNames.Count);

        foreach (var fileName in fileNames)
        {
            shippingAcknowledgementProcessor.ProcessShippingAcknowledgementNotification(fileName);
        }
    }
}