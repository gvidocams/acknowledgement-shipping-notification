using Core;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class AcknowledgementProcessingOptionsAdapter(IOptions<AcknowledgementProcessingOptions> options) : IAcknowledgementProcessingOptions
{
    private readonly AcknowledgementProcessingOptions _options = options.Value;

    public int BatchSize => _options.BatchSize;
    public int ChannelCapacitySize => _options.ChannelCapacitySize;
}