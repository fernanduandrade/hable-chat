using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Channels;
using MediatR;

namespace HableChat.Application.Channels.Delete;

public sealed record DeleteChannelCommand(long id) : IRequest<Unit> {}
public class DeleteChannelCommandHandler(IChannelRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteChannelCommand, Unit>
{
    public async Task<Unit> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        await repository.Delete(request.id);
        await unitOfWork.Commit(cancellationToken);
        return Unit.Value;
    }
}
