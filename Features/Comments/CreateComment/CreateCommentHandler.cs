using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Comments.CreateComment;

public class CreateCommentHandler(AppDbContext context, TimeProvider time) : IRequestHandler<CreateCommentCommand>
{
    public async Task Handle(CreateCommentCommand command, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var profileId = await context.Profiles
            .Where(x => x.UserId == command.AuthorId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct);
        if (profileId == Guid.Empty)
            throw new KeyNotFoundException("profile or post not found");

        var comment = await context.Comments
            .Where(x =>
                x.IsDraft
                && x.AuthorId == profileId
                && x.PostId == command.PostId
                && x.IdempotencyKey == command.IdempotencyKey).FirstOrDefaultAsync(ct);
        if (comment is null)
            throw new KeyNotFoundException("comment not found");
        comment.Build(now, command.Content);


        await context.SaveChangesAsync(ct);
    }
}