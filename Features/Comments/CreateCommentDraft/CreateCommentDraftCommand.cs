using MediatR;

namespace Amsberg_digital_universe.Features.Comments.CreateCommentDraft;

public record CreateCommentDraftCommand(Guid IdempotencyKey, Guid PostId, Guid AuthorUserId, Guid? ParentId):IRequest<Guid>;