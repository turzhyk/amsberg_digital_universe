using System.Diagnostics;
using Amsberg_digital_universe.Domain.Common.Post;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public class CreatePostHandler(AppDbContext context, TimeProvider time)
    : IRequestHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> Handle(CreatePostCommand command, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profile = await context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == command.UserId, ct);
        if (profile is null)
            throw new KeyNotFoundException("profile not found");

        var post = await context.Posts.FirstOrDefaultAsync(x => x.IdempotencyKey == command.IdempotencyKey, ct);
        
        if (post is null || post.ProfileId != profile.Id)
            throw new KeyNotFoundException("post not found");
        if (!post.IsDraft)
            return CreatePostResult.Success;

        post.Publish(command.Content, now, null);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException e)
        {
            return CreatePostResult.Success;
        }
        
        return CreatePostResult.Success;
    }
}