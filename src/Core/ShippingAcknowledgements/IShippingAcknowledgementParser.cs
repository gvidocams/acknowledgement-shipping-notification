using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementParser
{
    Task ParseShippingAcknowledgementNotification(ChannelWriter<Box> writer, string filePath);
}