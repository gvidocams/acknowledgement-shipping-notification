using System.Threading.Channels;
using Core.Models;

namespace Core;

public interface IShippingAcknowledgementParser
{
    Task ParseShippingAcknowledgementNotification(ChannelWriter<Box> writer, string filePath);
}