using Core;
using Core.Models;
using Infrastructure.Persistence.Converters;

namespace Infrastructure.Persistence;

public class ShippingAcknowledgementRepository(ShippingAcknowledgementContext shippingAcknowledgementContext)
    : IShippingAcknowledgementRepository
{
    //TODO Add integration tests
    public async Task SaveBoxes(List<Box> boxes)
    {
        var boxEntities = boxes.Select(box => box.ToBoxEntity());

        //TODO investigate if this is the best option for bulk insertions
        await shippingAcknowledgementContext.Boxes.AddRangeAsync(boxEntities);
        await shippingAcknowledgementContext.SaveChangesAsync();
    }
}