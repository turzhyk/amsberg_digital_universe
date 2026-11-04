namespace Amsberg_digital_universe.Domain.Common.Post;

public class Post
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid ProfileId { get; private set; }
    public  Profile.Profile Profile { get; init; } = null!;
    public string? Content { get; private set; }
    public bool Edited { get; private set; } = false;
    public int LikeCount { get; private set; }
    public List<string>? ImageUrls { get; private set; }

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