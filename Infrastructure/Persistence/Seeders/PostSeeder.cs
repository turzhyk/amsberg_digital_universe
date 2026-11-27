using Amsberg_digital_universe.Domain.Common;
using Amsberg_digital_universe.Domain.Post;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Persistence.Seeders;

public class PostSeeder(AppDbContext context, TimeProvider time)
{
    public async Task SeedDb()
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profile = await context.Profiles.FirstOrDefaultAsync();
        if (profile is null)
            throw new SeederException("no profile found for posts");
        var idempotency = Guid.CreateVersion7();
        var postDraft = Post.CreateDraft(idempotency, profile.Id, now);
        postDraft.Build("post 1", now, null, null);
        context.Posts.Add(postDraft);
        await context.SaveChangesAsync();
    }
}