using HableChat.Domain.Guilds;
using MediatR;

namespace HableChat.Application.Guilds.GetById;

public sealed record GetGuildByIdQuery(long Id) : IRequest<Guild> {}

public class GetGuildByIdQueryHandler(IGuildRepository repository)
    : IRequestHandler<GetGuildByIdQuery, Guild>
{
    public async Task<Guild> Handle(GetGuildByIdQuery request, CancellationToken cancellationToken)
    {
        var guild = await repository.GetByIdAsync(request.Id);
        return guild;
    }
}
