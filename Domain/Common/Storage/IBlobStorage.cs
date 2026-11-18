using Azure.Storage.Blobs.Models;

namespace Amsberg_digital_universe.Domain.Common.Storage;

public interface IBlobStorage
{
    Task<Uri> GetUploadLink(string container, string key, string contentType, TimeSpan ttl);
    Task<BlobProperties?> GetPropertiesAsync(string container, string key, CancellationToken ct);
}