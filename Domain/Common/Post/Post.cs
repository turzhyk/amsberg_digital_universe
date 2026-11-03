namespace Amsberg_digital_universe.Domain.Common.Post;

public class Post
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    private Domain.User.User User { get; init; } = null!;
    public string? Content { get; private set; }
    public bool Edited { get; private set; } = false;
    public int LikeCount { get; private set; }
    public List<string>? ImageUrls { get; private set; }

    public Post Create(Guid authorId, string? content, DateTime now, List<string>? images)
        => new()
        {
            Id = Guid.CreateVersion7(),
            UserId = authorId,
            Content = content,
            CreatedAt = now,
            ImageUrls = images
        };
}