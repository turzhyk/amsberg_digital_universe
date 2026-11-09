namespace Amsberg_digital_universe.Domain.Common.Post;

public class Post
{
    public Guid Id { get; private set; }
    public Guid IdempotencyKey { get; private set; }
    public bool IsDraft { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public Guid ProfileId { get; private set; }
    public Profile.Profile Profile { get; init; } = null!;
    public string? Content { get; private set; }
    public bool Edited { get; private set; } = false;
    public int LikeCount { get; private set; }
    public List<string>? ImageUrls { get; private set; }
    public byte[] RowVersion { get; set; } = null!;

    public static Post CreateDraft(Guid idempotencyKey, Guid authorId, DateTime now) => new()
        { Id = Guid.CreateVersion7(), IdempotencyKey = idempotencyKey, ProfileId = authorId, CreatedAt = now};

    public void Publish(string? content, DateTime now, List<string>? images)
    {
        Content = content;
        PublishedAt = now;
        ImageUrls = images;
        IsDraft = false;
    }

    public static Post Create(Guid authorId, string? content, DateTime now, List<string>? images)
        => new()
        {
            Id = Guid.CreateVersion7(),
            ProfileId = authorId,
            Content = content,
            CreatedAt = now,
            ImageUrls = images
        };

    public void Edit(string newContent)
    {
        Content = newContent;
        Edited = true;
    }
}