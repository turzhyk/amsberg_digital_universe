using MediatR;

namespace Amsberg_digital_universe.Features.Profile.Username;

public record SetUsernameCommand(Guid UserId, string Username) : IRequest<SetUsernameResult>;
