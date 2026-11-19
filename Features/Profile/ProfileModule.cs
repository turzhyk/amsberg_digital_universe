using Amsberg_digital_universe.Features.Profile.GetProfile;
using Amsberg_digital_universe.Features.Profile.GetProfilepicUploadLink;
using Amsberg_digital_universe.Features.Profile.Username;

namespace Amsberg_digital_universe.Features.Profile;

public static class ProfileModule
{
    public static void MapProfiles(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/profile")
            .WithTags("Profiles'");;
        group.RequireAuthorization();
        SetUsernameEndpoint.AddRoute(group);
        GetProfileEndpoint.AddRoutes(group);
        GetProfilepicUploadLinkEndpoint.AddRoutes(group);
        
    }
}