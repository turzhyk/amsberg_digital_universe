using MediatR;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public record CreatePostCommand(Guid UserId, string Content) : IRequest;