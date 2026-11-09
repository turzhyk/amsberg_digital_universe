using MediatR;

namespace Amsberg_digital_universe.Features.Posts.CreatePostDraft;

public record CreatePostDraftCommand(Guid UserId, Guid IdempotencyKey):IRequest;