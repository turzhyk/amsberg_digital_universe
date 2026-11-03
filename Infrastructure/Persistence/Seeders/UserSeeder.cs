using System.Text;
using Amsberg_digital_universe.Domain.User;
using Amsberg_digital_universe.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Infrastructure.Persistence.Seeders;

public class UserSeeder(AppDbContext context, TimeProvider time, IOptions<HasherOptions> hasherOptions)
{
    private readonly HasherOptions _hasherOptions = hasherOptions.Value;
    public async Task SeedUsers()
    {
        var now = time.GetUtcNow().UtcDateTime;
        var adminId = Guid.Parse("c1a6ce86 - e201 - 4a56 - 89d1 - 663661e4a31a");
        var admin = await context.Users.AnyAsync(x => x.Id == adminId);
        if (!admin)
        {
            var passwordHash = PasswordHasher.Hash("0000", Encoding.ASCII.GetBytes(_hasherOptions.Secret) );
            var adminUser = User.Seed(adminId, "admin@local", "Admin", passwordHash, now);
            await context.AddAsync(adminUser);
        }
    }
}