using Amsberg_digital_universe.Domain.Comment;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Comments.CreateCommentDraft;

public class CreateCommentDraftHandler(AppDbContext context, TimeProvider time):IRequestHandler<CreateCommentDraftCommand, Guid>
{
    public async Task<Guid> Handle(CreateCommentDraftCommand command, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profileId = await context.Profiles
            .Where(x => x.UserId == command.AuthorUserId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct);
        if (profileId == Guid.Empty)
            throw new KeyNotFoundException("profile or post not found");
        
        var comment = Comment.CreateDraft(command.IdempotencyKey, profileId, command.PostId, command.ParentId);
        context.Comments.Add(comment);
        try
        {
            await context.SaveChangesAsync(ct);
            return comment.Id;
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sE && sE.Number is 2627 or 2601)
        {
            return comment.Id;
        }
      
    }
}