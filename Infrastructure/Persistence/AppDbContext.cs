using Amsberg_digital_universe.Domain.Common.User;
using Amsberg_digital_universe.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Amsberg_digital_universe.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ConfirmToken> UserConfirmTokens => Set<ConfirmToken>();
}