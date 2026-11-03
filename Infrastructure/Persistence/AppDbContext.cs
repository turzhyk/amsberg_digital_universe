using Amsberg_digital_universe.Domain.Common.Profile;
using Amsberg_digital_universe.Domain.Common.User;
using Amsberg_digital_universe.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ConfirmToken> UserConfirmTokens => Set<ConfirmToken>();
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Profile>(_builder =>
        {
            _builder.HasIndex(x => x.Username).IsUnique();
            _builder.HasIndex(x => x.Id).IsUnique();
        });
        builder.Entity<User>(_builder =>
        {
            _builder.HasIndex(x => x.Id).IsUnique();
        });
    }
}