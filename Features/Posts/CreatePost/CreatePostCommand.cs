using MediatR;

namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public record CreatePostCommand(Guid UserId, Guid IdempotencyKey, string Content) : IRequest<CreatePostResult>;