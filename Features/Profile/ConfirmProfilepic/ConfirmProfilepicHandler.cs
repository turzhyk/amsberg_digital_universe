using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Profile.ConfirmProfilepic;

public class ConfirmProfilepicHandler(AppDbContext context, IBlobStorage blobStorage, IImageValidator valiador)
    : IRequestHandler<ConfirmProfilepicCommand, ConfirmProfilepicResult>
{
    public async Task<ConfirmProfilepicResult> Handle(ConfirmProfilepicCommand command, CancellationToken ct)
    {
        var profileId = await context.GetProfileId(command.UserId, ct);
        var profile = await context.Profiles
            .FirstOrDefaultAsync(x => x.Id == profileId, ct);


        var storageFile = await context.StorageFiles
            .Where(x => x.AuthorId == profileId && x.Key == command.Key)
            .FirstOrDefaultAsync(ct);
        if (profile is null || storageFile is null)
            return ConfirmProfilepicResult.NotFound;


        var blobProps = await blobStorage.GetPropertiesAsync("staging", command.Key, ct);
        if (blobProps is null)
        {
            storageFile.Reject();
            return ConfirmProfilepicResult.NotFound;
        }

        var download = await blobStorage.GetFileAsync("staging", command.Key, ct);
        var validationResult = valiador.IsValid(download, nameof(ImageValidationProperty.Avatar));

        if (validationResult is not ImageValidationResult.Valid)
        {
            storageFile.Reject();
            return validationResult switch
            {
                ImageValidationResult.SizeTooBig => ConfirmProfilepicResult.TooBig,
                ImageValidationResult.ResolutionTooBig => ConfirmProfilepicResult.TooBig,
                _ => ConfirmProfilepicResult.WrongType
            };
        }


        profile.SetProfilepicUrl(command.Key);
        storageFile.ConfirmUpload();
        await context.SaveChangesAsync(ct);
        return ConfirmProfilepicResult.Success;
    }
}