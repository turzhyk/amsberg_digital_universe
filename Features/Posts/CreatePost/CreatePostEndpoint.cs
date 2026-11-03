using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public static class CreatePostEndpoint
{
   public static void AddRoute(IEndpointRouteBuilder app)
   {
      app.MapPost("/posts", async ([FromBody] CreatePostCommand command,ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
      {
         var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
         
      });
   }
}