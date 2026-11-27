using Amsberg_digital_universe.Domain;
using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Domain.Post;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Feed;

public class FeedHandler(AppDbContext context, IBlobStorage blobStorage)
    : IRequestHandler<FeedQuery, PagedResponse<PostResponse>>
{
    public async Task<PagedResponse<PostResponse>> Handle(FeedQuery request, CancellationToken ct)
    {
        var skip = request.Page * request.PageSize;
        var posts = await context.Posts
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip).Take(request.PageSize)
            .Select(x => new
            {
                x.Id,
                x.CreatedAt,
                x.Content,
                Author = new
                {
                    x.Profile.Username,
                    x.Profile.AvatarKey
                }
            })
            .ToListAsync(ct);


        List<PostResponse> result = new List<PostResponse>(request.PageSize);
        foreach (var p in posts)
        {
            var authorAvatarUrl = p.Author.AvatarKey != null
                ? await blobStorage.GetPublicLink("staging", p.Author.AvatarKey, TimeSpan.FromMinutes(10))
                : null;
            result.Add(new PostResponse
            (
                p.Id, p.CreatedAt,
                p.Content,
                new AuthorResponse(p.Author.Username, p.Author.Username, authorAvatarUrl)
            ));
        }

        return new PagedResponse<PostResponse>
        {
            Items = result,
            PageSize = request.PageSize,
            Page = request.Page,
            HasNext = true,
            HasPrevious = true
        };
    }
}