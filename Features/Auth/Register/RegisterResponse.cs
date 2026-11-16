namespace Amsberg_digital_universe.Features.Auth.Register;

public record RegisterResponse(Guid UserId, string JwtToken);