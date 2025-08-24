using System.ComponentModel.DataAnnotations;

namespace ShippingAcknowledgementWorker;

public class FileScanningOptions
{
    public const string SectionName = "FileScanningOptions";

    [Required]
    public required string FilePath { get; init; }

    [Range(1, int.MaxValue)]
    public int FilePathScanIntervalInSeconds { get; init; } = 10;
}