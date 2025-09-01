using System.Threading.Channels;
using Core.Models;

namespace Core.ShippingAcknowledgements;

public interface IShippingAcknowledgementBoxProcessor
{
    Task SaveShippingAcknowledgementBoxesAsync(ChannelReader<Box> reader);
}