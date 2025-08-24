using System.ComponentModel.DataAnnotations;

namespace ShippingAcknowledgementWorker;

public class AcknowledgementScanningOptions
{
    public const string SectionName = "AcknowledgementScanningOptions";

    [Range(1, int.MaxValue)]
    public int ScanIntervalInSeconds { get; init; } = 10;
}