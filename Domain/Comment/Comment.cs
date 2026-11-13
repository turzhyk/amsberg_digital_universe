using System.ComponentModel.DataAnnotations;

namespace Amsberg_digital_universe.Domain.Comment;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid IdempotencyKey { get; private set; }
    public Guid PostId { get; private set; }
    public Post.Post Post { get; init; } = null!;
    public bool IsDraft { get; private set; } = true;
    public bool IsRoot { get; private set; } = true;
    public Guid AuthorId { get; private set; }
    public Guid? ParentId { get; private set; }
    public Comment? Parent { get; private set; }

    public string? Content { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;
    [Timestamp] public byte[] RowVersion { get; set; }

    public bool IsRemoved { get; private set; } = false;
    
    public static Comment CreateDraft(Guid idempotency, Guid author, Guid postId, Guid? parentId) => new()
    {
        Id = Guid.CreateVersion7(),
        IdempotencyKey = idempotency,
        AuthorId = author,
        PostId = postId,
        ParentId = parentId,
        IsRoot = parentId == null
    };

    public void Build(DateTime now, string content)
    {
        Content = content;
        IsDraft = false;
        Publish(now);
    }

    public void Publish(DateTime now)
    {
        PublishedAt = now;
    }

    public void Remove() => IsRemoved = true;
}