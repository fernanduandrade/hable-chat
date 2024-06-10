using HableChat.Domain.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HableChat.Application.Messages.GetChannelMesssages;

public sealed record GetChannelMessagesQuery(long id, int Limit): IRequest<IEnumerable<Message>> {}

public class GetChannelMessagesQueryHandler(IMessageRepository repository) : IRequestHandler<GetChannelMessagesQuery, IEnumerable<Message>>
{
    public Task<IEnumerable<Message>> Handle(GetChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        var records = repository
            .Get(x => x.ChannelId == request.id)
            .Include(x => x.User)
            .Take(request.Limit)
            .OrderByDescending(x => x.Created);
    
        return Task.FromResult(records.AsEnumerable());
    }
}
