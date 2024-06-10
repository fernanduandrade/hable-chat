using System.Linq.Expressions;

namespace HableChat.Domain.Guilds;

public interface IGuildRepository
{
    void Add(Guild entity);
    Task Delete(long id);
    Task<Guild> GetByIdAsync(long id);
    IQueryable<Guild> Get(Expression<Func<Guild, bool>> filter = null);
}
