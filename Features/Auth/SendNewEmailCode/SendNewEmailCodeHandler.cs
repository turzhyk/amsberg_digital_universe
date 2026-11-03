using Amsberg_digital_universe.Domain.Common.User;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Auth.SendNewEmailCode;

public class SendNewEmailCodeHandler(TimeProvider time, AppDbContext context):IRequestHandler<SendNewEmailCodeCommand>
{
    public async Task Handle(SendNewEmailCodeCommand request, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.userId, ct);
        
        if (user is null || user.IsEmailConfirmed())
            return;
        
        var activeCode = await context.UserConfirmTokens
            .AsNoTracking()
            .Where(x => x.UserId == request.userId)
            .OrderByDescending(x=>x.ExpiresAt)
            .FirstOrDefaultAsync(ct);
        
        if(activeCode != null || activeCode.Active || !activeCode.IsExpired(now))
            return;

        var newCode = ConfirmToken.Create(request.userId, now, TimeSpan.FromMinutes(2));
        await context.UserConfirmTokens.AddAsync(newCode, ct);
        await context.SaveChangesAsync(ct);
    }
}