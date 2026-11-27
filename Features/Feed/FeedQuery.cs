using Amsberg_digital_universe.Domain;
using Amsberg_digital_universe.Domain.Post;
using MediatR;

namespace Amsberg_digital_universe.Features.Feed;

public record FeedQuery(int Page, int PageSize) : IRequest<PagedResponse<PostResponse>>;
