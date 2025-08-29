using Core.Models;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Converters;

public static class BoxEntityConverter
{
    public static BoxEntity ToBoxEntity(this Box box) =>
        new()
        {
            SupplierIdentifier = box.SupplierIdentifier,
            Identifier = box.Identifier,
            Contents = box.Contents?
                .Select(content => content.ToContentEntity())
                .ToList() ?? [] 
        };
}