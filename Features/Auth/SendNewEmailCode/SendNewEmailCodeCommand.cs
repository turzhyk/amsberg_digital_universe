using MediatR;

namespace Amsberg_digital_universe.Features.Auth.SendNewEmailCode;

public record SendNewEmailCodeCommand(Guid userId) : IRequest;
