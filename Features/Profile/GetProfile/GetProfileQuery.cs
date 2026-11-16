using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfile;

public record GetProfileQuery(Guid UserId):IRequest<GetProfileResponse>;