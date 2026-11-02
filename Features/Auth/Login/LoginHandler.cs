using System.Text;
using Amsberg_digital_universe.Infrastructure.Auth;
using Amsberg_digital_universe.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Features.Auth.Login;

public class LoginHandler(AppDbContext context, IOptions<HasherOptions> hasherOptions)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly HasherOptions _hasherOptions = hasherOptions.Value;

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var email = request.Login.Trim().ToLowerInvariant();
        var user = await context.Users.Where(x => x.Email == request.Login).AsNoTracking().FirstOrDefaultAsync(ct);
        if (user is null)
            throw new UnauthorizedAccessException($"Email or password is incorrect");
        var hashedPassword = PasswordHasher.Hash(request.Password, Encoding.ASCII.GetBytes(_hasherOptions.Secret));
        if (hashedPassword != user.PasswordHash)
            throw new UnauthorizedAccessException($"Email or password is incorrect");
        return new LoginResponse { Token = "token", RefreshToken = "refresh" };
    }
}