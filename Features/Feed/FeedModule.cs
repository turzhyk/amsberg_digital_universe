namespace Amsberg_digital_universe.Features.Feed;

public static class FeedModule
{
    public static void MapFeed(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/feed").WithTags("Feed");
        FeedEndpoint.AddRoute(group);
    }
}