using Core;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.ShippingAcknowledgementNotifications;

//TODO Add integration tests
public class ShippingAcknowledgementProvider(IOptions<AcknowledgementProviderOptions> acknowledgementProviderOptions) : IShippingAcknowledgementProvider
{
    private readonly AcknowledgementProviderOptions _acknowledgementProviderOptions = acknowledgementProviderOptions.Value;

    public List<string> GetShippingAcknowledgementNotificationLocations() =>
        Directory
            .GetFiles(_acknowledgementProviderOptions.FilePath)
            .ToList();

    public void CompleteShippingAcknowledgementNotification(string notificationLocation)
    {
        // TODO Add failure handling
        File.Move(notificationLocation, _acknowledgementProviderOptions.ProcessedFilePath + Path.GetFileName(notificationLocation));
    }
}