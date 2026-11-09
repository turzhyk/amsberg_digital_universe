using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", 
            async ([FromBody]LoginCommand loginCommand, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(loginCommand, ct);
            return Results.Ok(result); 
        });
    }
}