using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using JWT.Extensions.AspNetCore.Factories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Profile.GetProfile;

public class GetProfileHandler(AppDbContext context, IBlobStorage blobStorage):IRequestHandler<GetProfileQuery, GetProfileResponse>
{
    public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var profile = await context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.UserId == request.UserId, ct);
        if(profile == null)
            throw new UnauthorizedAccessException();

        Uri? avatarLink = null;
        if (profile.AvatarKey != null)
        {
            avatarLink = await blobStorage.GetPublicLink("staging", profile.AvatarKey, TimeSpan.FromHours(1));
        }
        var response = new GetProfileResponse(profile.Username, avatarLink);
        return response;
    }
}