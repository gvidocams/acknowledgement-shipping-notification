using Microsoft.Extensions.Logging;

namespace Core.ShippingAcknowledgements;

public class ShippingAcknowledgementScanner(
    IShippingAcknowledgementProvider shippingAcknowledgementProvider,
    IShippingAcknowledgementProcessor shippingAcknowledgementProcessor,
    ILogger<ShippingAcknowledgementScanner> logger) : IShippingAcknowledgementScanner
{
    public async Task ScanAndDispatchAcknowledgements()
    {
        var fileNames = shippingAcknowledgementProvider.GetShippingAcknowledgementPaths();

        logger.LogInformation("Found {AcknowledgementCount} acknowledgements", fileNames.Count);

        foreach (var fileName in fileNames)
        {
            using var scope = logger.BeginScope("Acknowledgement notification: {FileName}", fileName);
            try
            {
                logger.LogInformation("Processing shipping acknowledgement notification {FileName}", fileName);

                await shippingAcknowledgementProcessor.ProcessShippingAcknowledgementNotification(fileName);

                logger.LogInformation("Finished processing acknowledgement notification {FileName}", fileName);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occured while processing acknowledgement notification {FileName}", fileName);
            }
        }
    }
}