namespace Amsberg_digital_universe.Domain.Common;

public class ImageValidationOptions
{
    public int MaxFileSize { get; set; }
    public int MaxWidth { get; set; }
    public int MaxHeight { get; set; }
    public required HashSet<string> AllowedFormats { get; set; }
}