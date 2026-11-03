using System.Text;

using Amsberg_digital_universe.Domain.Common.User;
using Amsberg_digital_universe.Domain.User;
using Amsberg_digital_universe.Infrastructure.Auth;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Features.Auth.Register;

public class RegisterHandler(AppDbContext db, TimeProvider time, IOptions<HasherOptions> hasherOptions)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly HasherOptions _hasherOptions = hasherOptions.Value;

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var email = request.Email.ToLowerInvariant();
        var exists = await db.Users.AnyAsync(x => x.Email == request.Email, ct);
        if (exists)
            throw new Exception($"User with email {request.Email} already exists");
        var passwordHash = PasswordHasher.Hash(request.Password, Encoding.ASCII.GetBytes(_hasherOptions.Secret));
        var user = User.Register(request.Email, request.Email, passwordHash, now);
        var confirmToken = ConfirmToken.Create(user.Id, now, TimeSpan.FromMinutes(2));
        var profile = Domain.Common.Profile.Profile.Create();
        user.SetProfileId(profile.Id);

        await db.Profiles.AddAsync(profile, ct);
        await db.UserConfirmTokens.AddAsync(confirmToken, ct);
        await db.Users.AddAsync(user, ct);
        await db.SaveChangesAsync(ct);
        return new RegisterResponse(user.Id);
    }
}