using System.ComponentModel.DataAnnotations;

namespace Infrastructure;

public class AcknowledgementProviderOptions
{
    public const string SectionName = "AcknowledgementProviderOptions";
    
    [Required]
    public required string FilePath { get; set; }
}