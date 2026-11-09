using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace Amsberg_digital_universe.Domain.Common.User;

public class ConfirmToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public int MaxAttempts { get; private set; } = 3;
    public int RetryCount { get; private set; } = 0;
    public bool Active { get; private set; } = true;
    public bool Sent { get; private set; } = false;
    public int SendAttempts { get; private set; } = 0;

    private ConfirmToken()
    {
    }

    public static ConfirmToken Create(Guid userId, DateTime now, TimeSpan lifetime) => new()
    {
        Id = Guid.CreateVersion7(),
        UserId = userId,
        ExpiresAt = now.Add(lifetime),
        Token = RandomNumberGenerator.GetInt32(100000, 1000000).ToString() // switch to hash later
    };

    public bool IsExpired(DateTime now) => ExpiresAt < now;

    public EmailConfirmResult TryUse(string code, DateTime now)
    {
        if (ExpiresAt < now || !Active)
            return EmailConfirmResult.Expired;
        if (Token != code)
        {
            RetryCount++;
            if (RetryCount >= MaxAttempts - 1)
                Active = false;
            return EmailConfirmResult.Wrong;
        }

        Active = false;
        return EmailConfirmResult.Success;
    }

    public enum EmailConfirmResult
    {
        Success,
        Expired,
        Wrong
    }
}