using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Azure.Storage.Blobs;
using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfilepicUploadLink;

public class GetProfilepicUploadLinkHandler(IBlobStorage blobStorage, AppDbContext context, IConfiguration config):IRequestHandler<GetProfilepicUploadLinkCommand,GetProfilepicUploadLinkResponse>
{
    public async Task<GetProfilepicUploadLinkResponse> Handle(GetProfilepicUploadLinkCommand request, CancellationToken ct)
    {
        var profileId = await context.GetProfileId(request.UserId, ct);
        var key = $"profilepics/{profileId}/{Guid.CreateVersion7()}";
        var url = blobStorage.GetUploadLink("staging",key, request.ContentType, TimeSpan.FromMinutes(5));
        return new GetProfilepicUploadLinkResponse(url.ToString(), "x-ms-blob-type: BlockBlob", key);
    }
}