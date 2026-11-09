using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public static class CreatePostEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/posts",
            async ([FromBody] CreatePostRequest request, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    throw new UnauthorizedAccessException();
                var command = new CreatePostCommand(Guid.Parse(userId), request.IdempotencyKey, request.Content);
                await sender.Send(command, ct);
            });
    }
}