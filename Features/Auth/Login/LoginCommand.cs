using MediatR;

namespace Amsberg_digital_universe.Features.Auth.Login;

public record LoginCommand(string Login, string Password) : IRequest<LoginResponse>;