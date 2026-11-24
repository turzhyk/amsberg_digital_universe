using Amsberg_digital_universe.Domain.Common.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Infrastructure.Storage;

public class AzureBlobStorage(BlobServiceClient client, IOptions<BlobStorageOptions> options):IBlobStorage
{
    private readonly BlobStorageOptions _options = options.Value;
    public async Task<Uri> GetUploadLink(string container, string key, string contentType, TimeSpan ttl)
    {
        var containerClient = client.GetBlobContainerClient(container);
        await containerClient.CreateIfNotExistsAsync();
        
        var blobClient = containerClient.GetBlobClient(key);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = container,
            BlobName = key,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(ttl),
            ContentType = contentType
        };
        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read | BlobContainerSasPermissions.Write);
        var localLink = blobClient.GenerateSasUri(sasBuilder);
        if(_options.PublicUrl is null)
            throw new ArgumentException("PublicUrl is null, check your configuration");
        
        var publicUrl = new Uri(_options.PublicUrl);
        var response = new UriBuilder(localLink)
        {
            Scheme = publicUrl.Scheme,
            Host = publicUrl.Host,
            Port = publicUrl.Port,
        }.Uri;
        return response;
    }

    public async Task<Uri> GetPublicLink(string container, string key, TimeSpan ttl)
    {
        var blobClient = client.GetBlobContainerClient(container).GetBlobClient(key);
        var sasUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(ttl));
        
        var publicUrl = new Uri(_options.PublicUrl);
        var response = new UriBuilder(sasUrl)
        {
            Scheme = publicUrl.Scheme,
            Host = publicUrl.Host,
            Port = publicUrl.Port,
        }.Uri;
        return response;
    }
    public async Task<BlobProperties?> GetPropertiesAsync(string container, string key, CancellationToken ct)
    {
        try
        {
            var containerClient = client.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(key);
            var properties = await blobClient.GetPropertiesAsync(cancellationToken: ct);

            return properties;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
     
    }
}