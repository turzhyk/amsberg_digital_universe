using System.Security.Cryptography;
using System.Text;

namespace Amsberg_digital_universe.Infrastructure.Auth;

public static class PasswordHasher
{
    public static string Hash(string password, byte[] secret)
    {
        var hash = HMACSHA256.HashData(secret, Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
}