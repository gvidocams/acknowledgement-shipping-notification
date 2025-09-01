using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public class ShippingAcknowledgementProcessor(
    IShippingAcknowledgementParser shippingAcknowledgementParser,
    IShippingAcknowledgementBoxProcessor shippingAcknowledgementBoxProcessor,
    IShippingAcknowledgementProvider shippingAcknowledgementProvider,
    int channelCapacity) : IShippingAcknowledgementProcessor
{
    public async Task ProcessShippingAcknowledgementNotification(string notificationLocation)
    {
        var channel = Channel.CreateBounded<Box>(channelCapacity);

        var acknowledgementWriterTask = shippingAcknowledgementParser.ParseShippingAcknowledgementNotificationAsync(channel.Writer, notificationLocation);
        var batchProcessorTask = shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxesAsync(channel.Reader);

        await Task.WhenAll(acknowledgementWriterTask, batchProcessorTask);

        shippingAcknowledgementProvider.CompleteShippingAcknowledgementNotification(notificationLocation);
    }
}