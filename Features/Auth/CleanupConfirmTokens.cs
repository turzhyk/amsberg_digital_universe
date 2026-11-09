using Amsberg_digital_universe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Features.Auth;

public class CleanupConfirmTokens(TimeProvider time, IServiceScopeFactory factory):BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var process = ProcessTokens(stoppingToken);
        await Task.WhenAll(process);
    }

    private async Task ProcessTokens(CancellationToken ct)
    {
        var timer  = new PeriodicTimer(TimeSpan.FromMinutes(2), time);
       
      
        while (await timer.WaitForNextTickAsync(ct))
        {   
            var now = time.GetUtcNow().UtcDateTime;
            using var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var inactiveTokensDeleted = await context.UserConfirmTokens.Where(x => x.Active == false).ExecuteDeleteAsync(ct);
            var expiredTokensDeleted = await context.UserConfirmTokens.Where(x => x.ExpiresAt < now).ExecuteDeleteAsync(ct);
        }
    }
}