using Amsberg_digital_universe.Infrastructure.Persistence.Seeders;

namespace Amsberg_digital_universe.Domain.Common.Extensions;

public static class DbSeederExtension
{
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services.AddScoped<UserSeeder>();
        return services;
    }

    public static async Task SeedDb(this IServiceProvider provider)
    {
        await using var scope = provider.CreateAsyncScope();
        var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
        await userSeeder.SeedUsers();
        
    }
}