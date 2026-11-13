using Amsberg_digital_universe.Domain.Common.Profile;
using Amsberg_digital_universe.Domain.Post;

namespace Amsberg_digital_universe.Domain.Post;

public class Post
{
    public Guid Id { get; private set; }
    public Guid IdempotencyKey { get; private set; }
    public bool IsDraft { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public bool IsDelayed { get; private set; } = false;
    public DateTime? DelayedPublishDate { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; init; } = null!;
    public string? Content { get; private set; }
    public bool Edited { get; private set; } = false;
    public int LikeCount { get; private set; }
    public int CommentsCount { get; private set; }
    public List<string>? ImageUrls { get; private set; }
    public byte[] RowVersion { get; set; } = null!;

    public static Post CreateDraft(Guid idempotencyKey, Guid authorId, DateTime now) => new()
        { Id = Guid.CreateVersion7(), IdempotencyKey = idempotencyKey, ProfileId = authorId, CreatedAt = now };

    public bool IsPublic() => PublishedAt != null;

    public static Post Create(Guid authorId, DateTime now)
        => new()
        {
            Id = Guid.CreateVersion7(),
            ProfileId = authorId,
            CreatedAt = now,
        };

    public BuildResult Build(string? content, DateTime now, List<string>? images, DateTime? delayedDate)
    {
        Content = content;
        ImageUrls = images;
        IsDraft = false;
        if (delayedDate <= now)
            return BuildResult.InvalidContent;
        DelayedPublishDate = delayedDate;
        IsDelayed = delayedDate != null;
        if (!IsDelayed)
            Publish(now);
        return BuildResult.Success;
    }

    public void Publish(DateTime now)
    {
        PublishedAt = now;
    }

    public void Edit(string? newContent, DateTime? newPublishDate, DateTime now)
    {
        if (newContent != null)
            Content = newContent;
        if (!IsPublic() && newPublishDate != null)
            DelayedPublishDate = newPublishDate;
        if (IsDelayed && now > DelayedPublishDate)
            Edited = true;
    }
}