using Amsberg_digital_universe.Features.Auth.Register;
using Amsberg_digital_universe.Features.Posts.CreatePost;
using Amsberg_digital_universe.Features.Posts.CreatePostDraft;
using Amsberg_digital_universe.Features.Posts.EditPost;

namespace Amsberg_digital_universe.Features.Posts;

public static class PostModule
{
    public static void MapPosts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/posts");
        group.RequireAuthorization();
        CreatePostEndpoint.AddRoute(group);
        CreatePostDraftEndpoint.AddRoute(group);
        EditPostEndpoint.AddRoute(group);
    }
}