using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public class ShippingAcknowledgementBoxProcessor(IShippingAcknowledgementRepository shippingAcknowledgementRepository, int batchSize)
    : IShippingAcknowledgementBoxProcessor
{
    //TODO Rename all async methods with async suffix
    public async Task SaveShippingAcknowledgementBoxes(ChannelReader<Box> reader)
    {
        var batch = new List<Box>();

        await foreach (var box in reader.ReadAllAsync())
        {
            batch.Add(box);

            if (batch.Count < batchSize) continue;

            await shippingAcknowledgementRepository.SaveBoxes([..batch]);
            batch.Clear();
        }

        if (batch.Count != 0)
        {
            await shippingAcknowledgementRepository.SaveBoxes(batch);
        }
    }
}