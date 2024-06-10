using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Guilds;
using MediatR;

namespace HableChat.Application.Guilds.Create;

public sealed record CreateGuildCommand(string Name) : IRequest<Guild> {}

public class CreateCommandHandler(IUserManager userManager, IUnitOfWork unitOfWork, IGuildRepository repository)
    : IRequestHandler<CreateGuildCommand, Guild>
{
    public async Task<Guild> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
    {
        var userId = userManager.GetId();
        var guild = Guild.Create(userId, request.Name);
        
        repository.Add(guild);

        await unitOfWork.Commit(cancellationToken);
        return guild;
    }
}
