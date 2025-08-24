using Core;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class ShippingAcknowledgementProvider(IOptions<AcknowledgementProviderOptions> acknowledgementProviderOptions) : IShippingAcknowledgementProvider
{
    private readonly AcknowledgementProviderOptions _acknowledgementProviderOptions = acknowledgementProviderOptions.Value;

    public List<string> GetShippingAcknowledgementPaths() =>
        Directory
            .GetFiles(_acknowledgementProviderOptions.FilePath)
            .ToList();
}