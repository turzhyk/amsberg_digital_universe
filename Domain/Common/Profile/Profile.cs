namespace Amsberg_digital_universe.Domain.Common.Profile;

public class Profile
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = "";
    public string IconUrl { get; private set; }

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