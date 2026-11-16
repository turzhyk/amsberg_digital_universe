using MediatR;

namespace Amsberg_digital_universe.Features.Auth.GetUserData;

public record GetUserDataQuery(Guid UserId):IRequest<GetUserDataResponse>;