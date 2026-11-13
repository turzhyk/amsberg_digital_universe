using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Comments.CreateComment;

public static class CreateCommentEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("{postId}/comments/",
            async (Guid postId, [FromBody] CreateCommentRequest request, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    throw new UnauthorizedAccessException("user not found");
                var command = new CreateCommentCommand(request.IdempotencyKey,postId, request.Content, Guid.Parse(userId));
                await sender.Send(command, ct);

            });
    }
}