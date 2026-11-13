using Amsberg_digital_universe.Infrastructure.Extensions;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Comments.DeleteComment;

public class DeleteCommentHandler(AppDbContext context) : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand command, CancellationToken ct)
    {
        var profileId = await context.GetProfileId(command.UserId, ct);

        var comment = await context.Comments
            .Where(x => x.AuthorId == profileId && x.Id == command.Id)
            .FirstOrDefaultAsync(ct);
        if (comment is not null)
            comment.Remove();
    }
}