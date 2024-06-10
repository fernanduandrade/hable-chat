using HableChat.Domain.Channels;
using MediatR;

namespace HableChat.Application.Channels.GetById;

public sealed record GetChannelByIdQuery(long id) : IRequest<Channel> {}

public class GetChannelByIdQueryHandler(IChannelRepository repository) : IRequestHandler<GetChannelByIdQuery, Channel>
{
    public async Task<Channel> Handle(GetChannelByIdQuery request, CancellationToken cancellationToken)
    {
        var channel = await repository.GetByIdAsync(request.id);
        return channel;
    }
}
