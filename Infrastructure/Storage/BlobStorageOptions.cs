namespace Amsberg_digital_universe.Infrastructure.Storage;

public class BlobStorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string? PublicUrl { get; set; }
    public string ProfilePicsKey { get; set; } = string.Empty;
}