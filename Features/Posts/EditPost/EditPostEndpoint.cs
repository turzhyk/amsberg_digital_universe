using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Posts.EditPost;

public static class EditPostEndpoint
{
    [Authorize]
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch("/posts/{id}",
            async ([FromBody] EditPostCommand command, ClaimsPrincipal user, ISender sender, [FromQuery] string id,
                CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var newCommand = new EditPostCommand(command.PostId, Guid.Parse(userId), command.Content);
                await sender.Send(command, ct);
            });
    }
}