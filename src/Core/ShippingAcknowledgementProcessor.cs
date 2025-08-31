using System.Threading.Channels;
using Core.Models;

namespace Core;

public class ShippingAcknowledgementProcessor(
    IShippingAcknowledgementParser shippingAcknowledgementParser,
    IShippingAcknowledgementBoxProcessor shippingAcknowledgementBoxProcessor,
    IShippingAcknowledgementProvider shippingAcknowledgementProvider,
    int channelCapacity) : IShippingAcknowledgementProcessor
{
    public async Task ProcessShippingAcknowledgementNotification(string filePath)
    {
        var channel = Channel.CreateBounded<Box>(channelCapacity);

        var acknowledgementWriterTask = shippingAcknowledgementParser.ParseShippingAcknowledgementNotification(channel.Writer, filePath);
        var batchProcessorTask = shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxes(channel.Reader);

        await Task.WhenAll(acknowledgementWriterTask, batchProcessorTask);

        shippingAcknowledgementProvider.CompleteShippingAcknowledgementNotification(filePath);
    }
}