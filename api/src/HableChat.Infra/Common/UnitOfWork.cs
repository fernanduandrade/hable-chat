using HableChat.Application.Common.Interfaces;
using HableChat.Infra.Data;

namespace HableChat.Infra.Common;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<bool> Commit(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
