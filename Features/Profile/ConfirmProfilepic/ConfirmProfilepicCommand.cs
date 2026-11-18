using MediatR;

namespace Amsberg_digital_universe.Features.Profile.ConfirmProfilepic;

public record ConfirmProfilepicCommand(Guid UserId, string Key):IRequest<ConfirmProfilepicResult>;