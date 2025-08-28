using Core.Models;

namespace Core;

public interface IShippingAcknowledgementRepository
{
    Task SaveBoxes(List<Box> boxes);
}