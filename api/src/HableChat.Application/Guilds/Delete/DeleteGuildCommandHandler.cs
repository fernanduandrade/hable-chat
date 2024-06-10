using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Guilds;
using MediatR;

namespace HableChat.Application.Guilds.Delete;

public sealed record DeleteGuildCommand(long id) : IRequest<Unit> {}
public class DeleteGuildCommandHandler(IGuildRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteGuildCommand, Unit>
{
    public async Task<Unit> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        await repository.Delete(request.id);
        await unitOfWork.Commit(cancellationToken);
        return Unit.Value;
    }
}
