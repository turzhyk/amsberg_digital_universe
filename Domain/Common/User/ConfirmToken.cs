using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Amsberg_digital_universe.Domain.Common.User;

public class ConfirmToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } 
    public string Token { get; set; } 
    public DateTime ExpiresAt { get; set; }
    private ConfirmToken(){} 

    public static ConfirmToken Create(Guid userId, DateTime now, TimeSpan lifetime) => new()
    {
        Id = Guid.CreateVersion7(),
        UserId = userId,
        ExpiresAt = now.Add(lifetime),
        Token = RandomNumberGenerator.GetInt32(100000, 1000000).ToString() // switch to hash later
    };

}