using MediatR;

namespace Amsberg_digital_universe.Features.Posts.EditPost;

public record EditPostCommand (Guid PostId, Guid UserId, string Content):IRequest;