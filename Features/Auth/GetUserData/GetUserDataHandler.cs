using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Auth.GetUserData;

public class GetUserDataHandler(AppDbContext context):IRequestHandler<GetUserDataQuery, GetUserDataResponse>
{
    public async Task<GetUserDataResponse> Handle(GetUserDataQuery query, CancellationToken ct)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == query.UserId,ct);
        if(user is null)
            throw new UnauthorizedAccessException();
        
        var response = new GetUserDataResponse(user.Login, user.Email);
        return response;
    }
}