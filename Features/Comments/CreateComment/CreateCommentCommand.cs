using MediatR;

namespace Amsberg_digital_universe.Features.Comments.CreateComment;

public record CreateCommentCommand(Guid IdempotencyKey, Guid PostId, string Content, Guid AuthorId) : IRequest;