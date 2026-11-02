using Amsberg_digital_universe.Domain.Common;
using Amsberg_digital_universe.Domain.User;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Infrastructure.Auth;

public class JwtTokenProvider(IOptions<AuthOptions> options):ITokenProvider
{
    private readonly AuthOptions _options = options.Value;

    public string GetToken(User user)
    {
        if(_options.Secret == null)
            throw new Exception("Secret is null. Check your configuration.");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_options.LifetimeInMinutes),
            SigningCredentials = credentials,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(descriptor);
        return token;
    }
}