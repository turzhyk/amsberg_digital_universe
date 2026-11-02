using Amsberg_digital_universe.Domain.User;

namespace Amsberg_digital_universe.Infrastructure.Auth;

public interface ITokenProvider
{
    public string GetToken(User user);
}