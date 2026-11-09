using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Profile.Username;

public class SetUsernameHandler(AppDbContext context) : IRequestHandler<SetUsernameCommand, SetUsernameResult>
{
    public async Task<SetUsernameResult> Handle(SetUsernameCommand request, CancellationToken ct)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        var userNameExists = await context.Profiles
            .AnyAsync(x => x.Username == username, ct);
        if (userNameExists)
            return SetUsernameResult.Taken;
        var user = await context.Users
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, ct);
        var profile = user?.Profile;
        if (profile == null)
            throw new Exception("TEMP EX");
        
        profile.SetUsername(username);
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlE && (sqlE.Number == 2601 || sqlE.Number == 2627))
        {
            return SetUsernameResult.Taken;
        }

        return SetUsernameResult.Success;
    }
}