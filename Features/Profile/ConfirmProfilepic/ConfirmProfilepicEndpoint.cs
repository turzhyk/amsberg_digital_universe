using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Profile.ConfirmProfilepic;

public static class ConfirmProfilepicEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/avatar/confirm", async ([FromBody] ConfirmProfilepicRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Results.Unauthorized();
            var command = new ConfirmProfilepicCommand(Guid.Parse(userId), request.Key);
            Console.WriteLine("Profile Picture Command send");
            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}