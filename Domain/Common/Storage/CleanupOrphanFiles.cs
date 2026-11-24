using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Storage;

public class CleanupOrphanFiles(IServiceScopeFactory serviceFactory, IBlobStorage blobStorage, TimeProvider time)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var process = ProcessFiles(stoppingToken);
        await Task.WhenAll(process);
    }

    private async Task ProcessFiles(CancellationToken ct)
    {
        var timer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (await timer.WaitForNextTickAsync(ct))
        {
            var now = time.GetUtcNow().UtcDateTime;
            using var scope = serviceFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var fileToDelete = await context.StorageFiles
                    .FirstOrDefaultAsync(x => x.Status != StorageFileStatus.Confirmed, ct);
            if (fileToDelete is null)
                continue;
            if (fileToDelete.Status is StorageFileStatus.Pending &&
                fileToDelete.CreatedAt < now - TimeSpan.FromMinutes(15))
            { 
                context.StorageFiles.Remove(fileToDelete);
                var blobResult = await blobStorage.DeleteAsync("staging", fileToDelete.Key, ct);
            }
            if (fileToDelete.Status is StorageFileStatus.Rejected)
            {
                context.StorageFiles.Remove(fileToDelete);
                var blobResult = await blobStorage.DeleteAsync("staging", fileToDelete.Key, ct);
            }

            await context.SaveChangesAsync(ct);

        }
    }
}