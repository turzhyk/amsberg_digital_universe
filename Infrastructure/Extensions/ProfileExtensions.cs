using Amsberg_digital_universe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Extensions;

public static class ProfileExtensions
{
     public static async Task<Guid> GetProfileId(this AppDbContext context, Guid userId, CancellationToken ct)
     {
          var profileId = await context.Profiles
               .Where(x => x.UserId == userId)
               .Select(x => x.Id)
               .FirstOrDefaultAsync(ct);
          if (profileId == Guid.Empty)
               throw new KeyNotFoundException("profile or post not found");
          return profileId;
     }
}