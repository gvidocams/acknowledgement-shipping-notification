using System.Threading.Channels;
using Core.Models;

namespace Core;

public class ShippingAcknowledgementProcessor(
    IShippingAcknowledgementParser shippingAcknowledgementParser,
    IShippingAcknowledgementBoxProcessor shippingAcknowledgementBoxProcessor) : IShippingAcknowledgementProcessor
{
    public async Task ProcessShippingAcknowledgementNotification(string filePath)
    {
        var channel = Channel.CreateBounded<Box>(10); //TODO configure and optimize the capacity of the channel

        var acknowledgementWriterTask = shippingAcknowledgementParser.ParseShippingAcknowledgementNotification(channel.Writer, filePath);
        var batchProcessorTask = shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxes(channel.Reader);

        await Task.WhenAll(acknowledgementWriterTask, batchProcessorTask);
    }
}