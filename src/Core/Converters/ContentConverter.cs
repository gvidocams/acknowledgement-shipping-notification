using Core.Models;

namespace Core.Converters;

public static class ContentConverter
{
    public static Content ToContent(string contentInput)
    {
        var contentParts = contentInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return new Content
        {
            PoNumber = contentParts[1],
            Isbn = contentParts[2],
            Quantity = int.Parse(contentParts[3])
        };
    }
}