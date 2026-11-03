using Amsberg_digital_universe.Domain.Common.Profile;

namespace Amsberg_digital_universe.Domain.User;

public class User
{
    public Guid Id { get; private set; }
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; init; } = null!;
    public string Email { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Confirmed { get; private set; } = false;

    public static User Register(string email, string login, string passwordHash, DateTime now)
        => new()
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            Login = login,
            PasswordHash = passwordHash,
            CreatedAt = now
        };

    public static User Seed(Guid id, string email, string login, string passwordHash, DateTime now)
        => new()
        {
            Id = id,
            Email = email,
            Login = login,
            PasswordHash = passwordHash,
            CreatedAt = now
        };
    public bool IsEmailConfirmed() => Confirmed;

    public void SetProfileId(Guid id)
    {
        ProfileId = id;
    }
    public void ConfirmEmail()
    {
        Confirmed = true;
    }
}