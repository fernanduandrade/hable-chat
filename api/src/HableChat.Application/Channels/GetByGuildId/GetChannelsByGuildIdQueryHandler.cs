using HableChat.Domain.Channels;
using MediatR;

namespace HableChat.Application.Channels.GetByGuildId;


public sealed record GetChannelsByGuildIdQuery(long id) : IRequest<IEnumerable<Channel>> {}
public class GetChannelsByGuildIdQueryHandler(IChannelRepository repository)
    : IRequestHandler<GetChannelsByGuildIdQuery, IEnumerable<Channel>>
{
    public Task<IEnumerable<Channel>> Handle(GetChannelsByGuildIdQuery request, CancellationToken cancellationToken)
    {
        var records = repository.Get(x => x.GuildId == request.id);

        return Task.FromResult(records.AsEnumerable());
    }
}
