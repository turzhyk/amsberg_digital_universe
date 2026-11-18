using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Profile.ConfirmProfilepic;

public class ConfirmProfilepicHandler(AppDbContext context, IBlobStorage blobStorage)
    : IRequestHandler<ConfirmProfilepicCommand, ConfirmProfilepicResult>
{
    public async Task<ConfirmProfilepicResult> Handle(ConfirmProfilepicCommand command, CancellationToken ct)
    {
        var profileId = await context.GetProfileId(command.UserId, ct);

        var storageFile = await context.StorageFiles
            .Where(x => x.AuthorId == profileId && x.Key == command.Key)
            .FirstOrDefaultAsync(ct);

        var blobProps = await blobStorage.GetPropertiesAsync("staging", command.Key, ct);
        if (blobProps is null)
            return ConfirmProfilepicResult.NotFound;

        var profile = await context.Profiles
            .FirstOrDefaultAsync(x => x.Id == profileId, ct);

        return ConfirmProfilepicResult.Success;
    }
    
}