using Core;
using Microsoft.Extensions.Options;

namespace Infrastructure;

//TODO Add integration tests
public class ShippingAcknowledgementProvider(IOptions<AcknowledgementProviderOptions> acknowledgementProviderOptions) : IShippingAcknowledgementProvider
{
    private readonly AcknowledgementProviderOptions _acknowledgementProviderOptions = acknowledgementProviderOptions.Value;

    public List<string> GetShippingAcknowledgementPaths() =>
        Directory
            .GetFiles(_acknowledgementProviderOptions.FilePath)
            .ToList();

    public void CompleteShippingAcknowledgementNotification(string filePath)
    {
        // TODO Add failure handling
        File.Move(filePath, _acknowledgementProviderOptions.ProcessedFilePath + Path.GetFileName(filePath));
    }
}