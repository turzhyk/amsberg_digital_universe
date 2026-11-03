using MediatR;

namespace Amsberg_digital_universe.Features.Auth.ConfirmRegistration;

public record ConfirmRegistrationCommand(string Code, Guid UserId):IRequest;