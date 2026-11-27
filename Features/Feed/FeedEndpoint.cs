using Amsberg_digital_universe.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Feed;

public static class FeedEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("", async ([FromQuery] int page, [FromQuery] int pageSize, ISender sender, CancellationToken ct) =>
        {
            var query = new FeedQuery(page, pageSize);
            var result = await sender.Send(query, ct);
            return Results.Ok(result);
        });
    }
}