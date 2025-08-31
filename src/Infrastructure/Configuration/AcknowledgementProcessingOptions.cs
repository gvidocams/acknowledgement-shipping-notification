namespace Infrastructure.Configuration;

public class AcknowledgementProcessingOptions
{
    public const string SectionName = "ShippingAcknowledgement:Processing";
    public int BatchSize { get; set; }
    public int ChannelCapacitySize { get; set; }
}