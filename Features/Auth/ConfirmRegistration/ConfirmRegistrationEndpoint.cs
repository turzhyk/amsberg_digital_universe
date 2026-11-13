using System.Security.Claims;
using Amsberg_digital_universe.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Amsberg_digital_universe.Features.Auth.ConfirmRegistration;

public static class ConfirmRegistrationEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/confirm/{code}",
            async ([FromQuery] string code, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Results.Unauthorized();

                var command = new ConfirmRegistrationCommand(code, Guid.Parse(userId));
                await sender.Send(command, ct);
                return Results.Ok();
            }).RequireAuthorization();
    }
}