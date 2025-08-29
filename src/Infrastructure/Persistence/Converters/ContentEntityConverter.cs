using Core.Models;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Converters;

public static class ContentEntityConverter
{
    public static ContentEntity ToContentEntity(this Content content) =>
        new()
        {
            PoNumber = content.PoNumber,
            Isbn = content.Isbn,
            Quantity = content.Quantity,
        };
}