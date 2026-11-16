namespace Amsberg_digital_universe.Domain.Common.Storage;

public interface IBlobStorage
{
    Task<Uri> GetUploadLink(string container, string key, string contentType, TimeSpan ttl);
}