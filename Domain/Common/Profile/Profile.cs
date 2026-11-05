namespace Amsberg_digital_universe.Domain.Common.Profile;

public class Profile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Domain.User.User User { get; init; } = null!;
    public string Username { get; private set; } = "";
    public string? IconUrl { get; private set; } 
    public ICollection<Post.Post> Posts { get; init; } = new List<Post.Post>();

    public static Profile Create() => new()
    {
        Id = Guid.CreateVersion7(),
        Username = Guid.CreateVersion7().ToString()
    };

    public static Profile Seed(Guid id, Guid userId, string username) => new()
    {
        Id = id,
        UserId = userId,
        Username = username,
    };
    public void SetUsername(string username)
    {
        Username = username.Trim();
    }
}