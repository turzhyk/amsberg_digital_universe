using System.Diagnostics;
using Amsberg_digital_universe.Domain.Common.Post;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public class CreatePostHandler(AppDbContext context, TimeProvider time)
    : IRequestHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> Handle(CreatePostCommand request, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profile = await context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, ct);
        Console.WriteLine(request.UserId);
        if (profile is null)
            throw new KeyNotFoundException("profile not found");

        var lastPostDate = await context.Posts
            .Where(x => x.ProfileId == profile.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (DateTime?)x.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (lastPostDate.HasValue && (lastPostDate.Value > now - TimeSpan.FromSeconds(10)))
            return CreatePostResult.Timeout;


        var post = Post.Create(profile.Id, request.Content, now, null);
        context.Posts.Add(post);
        await context.SaveChangesAsync(ct);
        return CreatePostResult.Success;
    }
}