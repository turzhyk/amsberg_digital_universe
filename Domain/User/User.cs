namespace Amsberg_digital_universe.Domain.User;

public class User
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Confirmed { get; set; } = false;

    public static User Register(string email, string login, string passwordHash, DateTime now)
        => new()
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            Login = login,
            PasswordHash = passwordHash,
            CreatedAt = now
        };
}