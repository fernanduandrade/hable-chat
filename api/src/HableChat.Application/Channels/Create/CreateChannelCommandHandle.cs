using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Channels;
using MediatR;

namespace HableChat.Application.Channels.Create;

public sealed record CreateChannelDto(string Name);

public sealed record CreateChannelCommand(string Name, long GuildId) : IRequest<Channel> {}

public class CreateChannelCommandHandle(IChannelRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateChannelCommand, Channel>
{
    public async Task<Channel> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = Channel.Create(request.Name, request.GuildId);
        repository.Add(channel);
        await unitOfWork.Commit(cancellationToken);
        return channel;
    }
}
