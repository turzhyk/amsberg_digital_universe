namespace Amsberg_digital_universe.Domain.Post;

public record PostResponse(
    Guid Id,
    DateTime CreatedAt,
    string? Content,
    AuthorResponse Author
);