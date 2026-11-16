using System.Security.Claims;
using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfile;

public static class GetProfileEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null)
                return Results.Unauthorized();
            var query = new GetProfileQuery(Guid.Parse(userId));
            var result  = await sender.Send(query, ct);
            return Results.Ok(result);
        });
    }
}