using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Amsberg_digital_universe.Features.Auth.SendNewEmailCode;

public static class SendNewEmailCodeEndpoint
{
    [Authorize]
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/code/new", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Results.Unauthorized();
            await sender.Send(new SendNewEmailCodeCommand(Guid.Parse(userId)), ct);
            return Results.Ok();
        });
    }
}