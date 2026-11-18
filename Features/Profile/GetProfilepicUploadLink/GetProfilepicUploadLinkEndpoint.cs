using System.Security.Claims;
using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfilepicUploadLink;

public static class GetProfilepicUploadLinkEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/avatar/upload", async (ClaimsPrincipal user,ISender sender, CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Results.Unauthorized();
            var command = new GetProfilepicUploadLinkCommand(Guid.Parse(userId), "", "", 1);
            var result = await sender.Send(command, ct);
            return Results.Ok(result);
        });
    }
}