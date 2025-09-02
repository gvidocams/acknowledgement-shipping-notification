using System.Threading.Channels;
using Core.Converters;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public class ShippingAcknowledgementParser(IAcknowledgementNotificationReader acknowledgementNotificationReader)
    : IShippingAcknowledgementParser
{
    private const string BoxIdentifier = "HDR";
    private const string ProductIdentifier = "LINE";

    public async Task ParseShippingAcknowledgementNotificationAsync(ChannelWriter<Box> writer, string notificationLocation)
    {
        Box? box = null;

        await foreach (var line in acknowledgementNotificationReader.ReadNotificationLinesAsync(notificationLocation))
        {
            if (line.StartsWith(BoxIdentifier))
            {
                if (box is not null && await writer.WaitToWriteAsync())
                {
                    await writer.WriteAsync(box);
                }

                // TODO Create error handling if couldn't parse the box
                // TODO Create validations for box line
                box = BoxConverter.ToBox(line);
            }
            else if (line.StartsWith(ProductIdentifier))
            {
                // TODO Create error handling for cases when a line has been found before a box
                // TODO Create validations for content line
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