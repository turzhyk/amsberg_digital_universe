namespace Amsberg_digital_universe.Domain.Profile;

public class Profile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User.User User { get; init; } = null!;
    public string Username { get; private set; } = "";
    public string? IconUrl { get; private set; } 
    public string? AvatarKey { get; private set; }
    public ICollection<Domain.Post.Post> Posts { get; init; } = new List<Domain.Post.Post>();

    public static Profile Create(Guid userId) => new()
    {
        Id = Guid.CreateVersion7(),
        Username = Guid.CreateVersion7().ToString(),
        UserId = userId
    };

    public static Profile Seed(Guid id, User.User user, string username) => new()
    {
        Id = id,
        User = user,
        Username = username,
    };
    public void SetUsername(string username)
    {
        Username = username.Trim();
    }

    public string? SetProfilepicUrl(string key)
    {
        var init = AvatarKey;
        AvatarKey = key;
        return init;
    }
}