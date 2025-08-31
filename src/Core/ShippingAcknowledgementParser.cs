using System.Threading.Channels;
using Core.Converters;
using Core.Models;

namespace Core;

public class ShippingAcknowledgementParser : IShippingAcknowledgementParser
{
    private const string BoxIdentifier = "HDR";
    private const string ProductIdentifier = "LINE";
    
    public async Task ParseShippingAcknowledgementNotification(ChannelWriter<Box> writer, string filePath)
    {
        //TODO extract streamreader logic since it doesn't belong in Core
        using var reader = new StreamReader(filePath);
        Box? box = null;

        while (await reader.ReadLineAsync() is { } line && await writer.WaitToWriteAsync())
        {
            if (line.StartsWith(BoxIdentifier))
            {
                if (box is not null)
                {
                    await writer.WriteAsync(box);
                }

                // TODO Create error handling if couldn't parse the box
                box = BoxConverter.ToBox(line);
            }
            else if (line.StartsWith(ProductIdentifier))
            {
                // TODO Create error handling for cases when a line has been found before a box
                // TODO Create error handling if couldn't parse the line
                box?.Contents.Add(ContentConverter.ToContent(line));
            }
        }

        if (box is not null && await writer.WaitToWriteAsync())
        {
            await writer.WriteAsync(box);
        }

        writer.Complete();
    }
}