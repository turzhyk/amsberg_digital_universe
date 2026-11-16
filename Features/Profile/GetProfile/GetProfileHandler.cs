using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Profile.GetProfile;

public class GetProfileHandler(AppDbContext context):IRequestHandler<GetProfileQuery, GetProfileResponse>
{
    public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var profile = await context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.UserId == request.UserId, ct);
        if(profile == null)
            throw new UnauthorizedAccessException();
        var response = new GetProfileResponse(profile.Username, null);
        return response;
    }
}