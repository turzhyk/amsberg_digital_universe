using Amsberg_digital_universe.Domain.Comment;
using Amsberg_digital_universe.Domain.Post;
using Amsberg_digital_universe.Domain.Profile;
using Amsberg_digital_universe.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ConfirmToken> UserConfirmTokens => Set<ConfirmToken>();
    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Profile>(_builder =>
        {
            _builder.HasIndex(x => x.Username).IsUnique();
            _builder.HasKey(x => x.Id);
        });
        builder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Profile).WithOne(x => x.User).HasForeignKey<Profile>(p => p.UserId);
        });
        builder.Entity<Post>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Profile).WithMany(p => p.Posts)
                .HasForeignKey(x => x.ProfileId);
            b.HasIndex(x => x.IdempotencyKey).IsUnique();
            b.Property(x => x.RowVersion).IsRowVersion();
        });

        builder.Entity<Comment>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.AuthorId, x.IdempotencyKey }).IsUnique();
            b.Property(x => x.RowVersion).IsRowVersion();
        });
    }
}