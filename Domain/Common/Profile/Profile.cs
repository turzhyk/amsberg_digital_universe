namespace Amsberg_digital_universe.Domain.Common.Profile;

public class Profile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Domain.User.User User { get; init; } = null!;
    public string Username { get; private set; } = "";
    public string IconUrl { get; private set; }
    public ICollection<Post.Post> Posts { get; init; } = null!;

    public static Profile Create() => new()
    {
        Id = Guid.CreateVersion7(),
        Username =""
    };

    public void SetUsername(string username)
    {
        Username = username.Trim();
    }
}