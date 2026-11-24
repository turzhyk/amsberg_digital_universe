using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Azure.Storage.Blobs;
using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfilepicUploadLink;

public class GetProfilepicUploadLinkHandler(IBlobStorage blobStorage, AppDbContext context, IConfiguration config, TimeProvider time)
    : IRequestHandler<GetProfilepicUploadLinkCommand, GetProfilepicUploadLinkResponse>
{
    public async Task<GetProfilepicUploadLinkResponse> Handle(GetProfilepicUploadLinkCommand request,
        CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var container = "staging";
        var profileId = await context.GetProfileId(request.UserId, ct);
        var key = $"profilepics/{profileId}/{Guid.CreateVersion7()}";
        var url = await blobStorage.GetUploadLink(container, key, request.ContentType, TimeSpan.FromMinutes(5));

        var storageFile = StorageFile.Create(container, key, profileId, now);
        context.StorageFiles.Add(storageFile);
        await context.SaveChangesAsync(ct);

        return new GetProfilepicUploadLinkResponse(url.ToString(), "x-ms-blob-type: BlockBlob", key);
    }
}