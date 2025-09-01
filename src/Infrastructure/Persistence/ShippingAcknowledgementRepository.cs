using Core;
using Core.Models;
using EFCore.BulkExtensions;
using Infrastructure.Persistence.Converters;

namespace Infrastructure.Persistence;

public class ShippingAcknowledgementRepository(ShippingAcknowledgementContext shippingAcknowledgementContext)
    : IShippingAcknowledgementRepository
{
    //TODO Add integration tests
    public async Task SaveBoxes(List<Box> boxes)
    {
        var boxEntities = boxes.Select(box => box.ToBoxEntity());

        await shippingAcknowledgementContext.BulkInsertAsync(boxEntities);
    }
}