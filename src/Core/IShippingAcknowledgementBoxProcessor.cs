using System.Threading.Channels;
using Core.Models;

namespace Core;

public interface IShippingAcknowledgementBoxProcessor
{
    Task SaveShippingAcknowledgementBoxes(ChannelReader<Box> reader);
}