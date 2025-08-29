using Core;
using Core.Models;
using Infrastructure.Persistence.Converters;

namespace Infrastructure.Persistence;

public class ShippingAcknowledgementRepository(ShippingAcknowledgementContext shippingAcknowledgementContext)
    : IShippingAcknowledgementRepository
{
    public async Task SaveBoxes(List<Box> boxes)
    {
        var boxEntities = boxes.Select(box => box.ToBoxEntity());

        await shippingAcknowledgementContext.Boxes.AddRangeAsync(boxEntities);
        await shippingAcknowledgementContext.SaveChangesAsync();
    }
}