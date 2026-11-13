using MediatR;

namespace Amsberg_digital_universe.Features.Comments.DeleteComment;

public record DeleteCommentCommand(Guid Id, Guid UserId):IRequest;