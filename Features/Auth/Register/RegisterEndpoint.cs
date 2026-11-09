using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register",
            async ([FromBody] RegisterCommand request, ISender sender, CancellationToken ct) =>
            {
                var id = await sender.Send(request, ct);
                return Results.Ok(id);
            });
    }
}