using System.Security.Authentication;
using Amsberg_digital_universe.Domain.Common.Exceptions;
using Amsberg_digital_universe.Domain.Common.User;
using Amsberg_digital_universe.Domain.User;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Amsberg_digital_universe.Features.Auth.ConfirmRegistration;

public class ConfirmRegistrationHandler(TimeProvider timeProvider, AppDbContext context) : IRequestHandler
    <ConfirmRegistrationCommand>
{
    public async Task Handle(ConfirmRegistrationCommand request, CancellationToken ct)
    {
        var user = await context.Users.FirstAsync(x => x.Id == request.UserId, ct);
        if(user.IsEmailConfirmed())
            return;
        
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var userCode = await context.UserConfirmTokens
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.ExpiresAt)
            .FirstOrDefaultAsync(ct);
        if (userCode is null || userCode.IsExpired(now))
        {
            await ReleaseNewCode(user.Id, now);
            throw new ConfirmTokenInvalidException();
        }
        if (!userCode.Active)
        {
            // release after delay
            await ReleaseNewCode(user.Id, now);
            return;
        }
        
        var codeResult = userCode.TryUse(request.Code, now);
        await context.SaveChangesAsync(ct);
        if (codeResult != ConfirmToken.EmailConfirmResult.Success)
            throw new ConfirmTokenInvalidException("Invalid code");
        
        user.ConfirmEmail();
        await context.SaveChangesAsync(ct);
    }

    public async Task ReleaseNewCode(Guid userId, DateTime now)
    {
        var newCode = ConfirmToken.Create(userId, now, TimeSpan.FromMinutes(2));
        await context.UserConfirmTokens.AddAsync(newCode);
    }
}