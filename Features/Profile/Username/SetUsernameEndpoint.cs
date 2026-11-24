using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Profile.Username;

public static class SetUsernameEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut("/username",
            async ([FromQuery] string targetUsername, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    return Results.Unauthorized();
                var command = new SetUsernameCommand(Guid.Parse(userId), targetUsername);
                var result = await sender.Send(command, ct);
                return Results.Ok(result);
            });
    }
}