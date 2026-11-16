using System.Security.Claims;
using MediatR;

namespace Amsberg_digital_universe.Features.Auth.GetUserData;

public static class GetUserDataEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/data", async (ClaimsPrincipal user, ISender sender,CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return Results.Unauthorized();
            var query = new GetUserDataQuery(Guid.Parse(userId));
            var result = await sender.Send(query, ct);
            return Results.Ok(result);
        });
    }
}