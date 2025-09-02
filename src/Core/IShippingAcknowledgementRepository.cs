using Core.Models;

namespace Core;

public interface IShippingAcknowledgementRepository
{
    Task SaveBoxesAsync(List<Box> boxes);
}