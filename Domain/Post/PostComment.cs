namespace Amsberg_digital_universe.Domain.Common.Post;

public class PostComment
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    private Domain.User.User User { get; init; } = null!;
    public Guid PostId { get; private set; }
    private Domain.Post.Post Post { get; init; } = null!;

    public DateTime CreatedAt { get; private set; }
}