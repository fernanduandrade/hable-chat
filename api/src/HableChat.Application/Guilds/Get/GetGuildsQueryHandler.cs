using HableChat.Domain.Guilds;
using MediatR;

namespace HableChat.Application.Guilds.Get;

public sealed record GetGuildsQuery() : IRequest<IEnumerable<Guild>> {}

public class GetGuildsQueryHandler(IGuildRepository repository)
    : IRequestHandler<GetGuildsQuery, IEnumerable<Guild>>
{
    public Task<IEnumerable<Guild>> Handle(GetGuildsQuery request, CancellationToken cancellationToken)
    {
        var records = repository.Get();

        return Task.FromResult(records.AsEnumerable());
    }
}
