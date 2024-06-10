using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Messages;
using MediatR;

namespace HableChat.Application.Messages.Create;


public sealed record CreateMessageDto(string Content);

public sealed record CreateMessageCommand(string Content, string UserId, long ChannelId)
    : IRequest<Message> {}

public class CreateMessageCommandHandler(IMessageRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMessageCommand, Message>
{
    public async Task<Message> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = Message.Create(request.UserId, request.ChannelId, request.Content);
        repository.Add(message);
        await unitOfWork.Commit(cancellationToken);
        
        return message;
    }
}
