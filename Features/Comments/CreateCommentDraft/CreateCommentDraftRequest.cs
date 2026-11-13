namespace Amsberg_digital_universe.Features.Comments.CreateCommentDraft;

public record CreateCommentDraftRequest(Guid IdempotencyKey, Guid? ParentId);