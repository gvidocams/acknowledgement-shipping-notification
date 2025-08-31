using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementBoxProcessor
{
    Task SaveShippingAcknowledgementBoxes(ChannelReader<Box> reader);
}