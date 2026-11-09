using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Posts.CreatePostDraft;

public static class CreatePostDraftEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/drafts", async (CreatePostDraftRequest request, ClaimsPrincipal user,ISender sender, CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                throw new UnauthorizedAccessException();
            
        });
    }
}