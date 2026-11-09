using Amsberg_digital_universe.Domain.Common.Post;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Posts.CreatePostDraft;

public class CreatePostDraftHandler(AppDbContext context, TimeProvider time) : IRequestHandler<CreatePostDraftCommand>
{
    public async Task Handle(CreatePostDraftCommand command, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profileId = await context.Profiles
            .AsNoTracking()
            .Where(x => x.UserId == command.UserId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct);
        if(profileId == Guid.Empty)
            throw new UnauthorizedAccessException();
            
        var draft = Post.CreateDraft(command.IdempotencyKey, profileId, now);
        context.Posts.Add(draft);
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sE && sE.Number is 2627 or 2601)
        {
            // throw new PostException("Duplicate draft");
        }
    }
}