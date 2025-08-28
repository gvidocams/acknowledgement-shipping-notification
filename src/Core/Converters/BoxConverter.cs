using Core.Models;

namespace Core.Converters;

public static class BoxConverter
{
    public static Box ToBox(string boxInput)
    {
        var boxParts = boxInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return new Box
        {
            SupplierIdentifier = boxParts[1],
            Identifier = boxParts[2],
            Contents = []
        };
    }
}