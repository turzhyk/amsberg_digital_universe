using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Comments.CreateCommentDraft;

public static class CreateCommentDraftEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("{postId}/comments/draft",
            async ([FromRoute] Guid postId, [FromBody] CreateCommentDraftRequest request, ISender sender, ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    throw new UnauthorizedAccessException();
                var command =
                    new CreateCommentDraftCommand(request.IdempotencyKey,postId, Guid.Parse(userId), request.ParentId);
                var result = await sender.Send(command, ct);
                return result;
            });
    }
}