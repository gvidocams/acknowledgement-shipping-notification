using Microsoft.Extensions.Logging;

namespace Core.ShippingAcknowledgements;

public class ShippingAcknowledgementScanner(
    IShippingAcknowledgementProvider shippingAcknowledgementProvider,
    IShippingAcknowledgementProcessor shippingAcknowledgementProcessor,
    ILogger<ShippingAcknowledgementScanner> logger) : IShippingAcknowledgementScanner
{
    public async Task ScanAndDispatchAcknowledgements()
    {
        //TODO Add error handling and logging for cases when getting the notifications fails
        var notificationLocations = shippingAcknowledgementProvider.GetShippingAcknowledgementNotificationLocations();

        logger.LogInformation("Found {NotificationCount} acknowledgement notifications", notificationLocations.Count);

        foreach (var notificationLocation in notificationLocations)
        {
            using var scope = logger.BeginScope("Acknowledgement notification: {NotificationLocation}", notificationLocation);
            try
            {
                logger.LogInformation("Processing shipping acknowledgement notification {NotificationLocation}", notificationLocation);

                await shippingAcknowledgementProcessor.ProcessShippingAcknowledgementNotification(notificationLocation);

                logger.LogInformation("Finished processing acknowledgement notification {NotificationLocation}", notificationLocation);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occured while processing acknowledgement notification {NotificationLocation}", notificationLocation);
            }
        }
    }
}