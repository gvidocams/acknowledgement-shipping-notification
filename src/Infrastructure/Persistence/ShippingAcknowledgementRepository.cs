using Core;
using Core.Models;
using Infrastructure.Persistence.Converters;

namespace Infrastructure.Persistence;

public class ShippingAcknowledgementRepository(ShippingAcknowledgementContext shippingAcknowledgementContext)
    : IShippingAcknowledgementRepository
{
    //TODO Add integration tests
    public async Task SaveBoxesAsync(List<Box> boxes)
    {
        //TODO Add error handling if uploading the data fails
        var boxEntities = boxes.Select(box => box.ToBoxEntity()).ToList();

        await using var transaction = await shippingAcknowledgementContext.Database.BeginTransactionAsync();

        await shippingAcknowledgementContext.AddRangeAsync(boxEntities);
        await shippingAcknowledgementContext.SaveChangesAsync();

        shippingAcknowledgementContext.ChangeTracker.Clear();
        await transaction.CommitAsync();
    }
}