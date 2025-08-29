namespace Core;

public interface IAcknowledgementProcessingOptions
{
    public int BatchSize { get; }
    public int ChannelCapacitySize { get; }
}