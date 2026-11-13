using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Posts.EditPost;

public class EditPostHandler(AppDbContext context, TimeProvider time):IRequestHandler<EditPostCommand>
{
    public async Task Handle(EditPostCommand command, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profileId = await context.Profiles
            .Where(x => x.UserId == command.UserId)
            .Select(x=>x.Id)
            .FirstOrDefaultAsync(ct);
        var post = await context.Posts
            .FirstOrDefaultAsync(x => x.Id == command.PostId,ct);
        
        post.Edit(command.Content, null, now);

        await context.SaveChangesAsync(ct);
    }
}