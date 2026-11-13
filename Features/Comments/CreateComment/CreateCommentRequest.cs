namespace Amsberg_digital_universe.Features.Comments.CreateComment;

public record CreateCommentRequest(Guid IdempotencyKey, string Content);