using Amsberg_digital_universe.Features.Auth.ConfirmRegistration;
using Amsberg_digital_universe.Features.Auth.Login;
using Amsberg_digital_universe.Features.Auth.Register;
using Amsberg_digital_universe.Features.Auth.SendNewEmailCode;

namespace Amsberg_digital_universe.Features.Auth;

public static class AuthModule
{
    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth");
        RegisterEndpoint.AddRoutes(group);
        LoginEndpoint.AddRoutes(group);
        ConfirmRegistrationEndpoint.AddRoutes(group);
        SendNewEmailCodeEndpoint.AddRoutes(group);
    }
}