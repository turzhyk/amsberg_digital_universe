using MediatR;

namespace Amsberg_digital_universe.Features.Profile.GetProfilepicUploadLink;

public record GetProfilepicUploadLinkCommand(Guid UserId, string FileName, string ContentType, int Size) : IRequest<GetProfilepicUploadLinkResponse>;