using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementParser
{
    Task ParseShippingAcknowledgementNotificationAsync(ChannelWriter<Box> writer, string notificationLocation);
}