namespace Infrastructure;

public class AcknowledgementProcessingOptions
{
    public const string SectionName = "AcknowledgementProcessingOptions";
    public int BatchSize { get; set; }
    public int ChannelCapacitySize { get; set; }
}