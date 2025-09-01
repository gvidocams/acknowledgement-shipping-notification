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
        //TODO Add error handling if uploading the data fails
        var boxEntities = boxes.Select(box => box.ToBoxEntity());

        await shippingAcknowledgementContext.BulkInsertAsync(boxEntities, config => config.IncludeGraph = true);
    }
}