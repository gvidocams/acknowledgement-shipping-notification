using System.Threading.Channels;
using Core.Models;

namespace Core;

public class ShippingAcknowledgementProcessor(
    IShippingAcknowledgementParser shippingAcknowledgementParser,
    IShippingAcknowledgementBoxProcessor shippingAcknowledgementBoxProcessor,
    int channelCapacity) : IShippingAcknowledgementProcessor
{
    public async Task ProcessShippingAcknowledgementNotification(string filePath)
    {
        // TODO Create the channel capacity as configurable and validate it against the batch size
        var channel = Channel.CreateBounded<Box>(channelCapacity);

        var acknowledgementWriterTask = shippingAcknowledgementParser.ParseShippingAcknowledgementNotification(channel.Writer, filePath);
        var batchProcessorTask = shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxes(channel.Reader);

        await Task.WhenAll(acknowledgementWriterTask, batchProcessorTask);
    }
}