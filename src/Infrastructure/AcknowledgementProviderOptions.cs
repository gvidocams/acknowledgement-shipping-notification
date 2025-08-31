using System.ComponentModel.DataAnnotations;

namespace Infrastructure;

public class AcknowledgementProviderOptions
{
    public const string SectionName = "ShippingAcknowledgement:Provider";

    [Required] 
    public required string FilePath { get; set; }

    [Required] 
    public required string ProcessedFilePath { get; set; }
}