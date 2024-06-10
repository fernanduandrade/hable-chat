using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Messages;
using MediatR;

namespace HableChat.Application.Messages.Delete;

public sealed record DeleteMessageCommand(long id, long messageId) : IRequest<Unit> {}
public class DeleteMessageCommandHandler(IMessageRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        await repository.Delete(request.messageId, request.id);
        await unitOfWork.Commit(cancellationToken);
        return Unit.Value;
    }
}
