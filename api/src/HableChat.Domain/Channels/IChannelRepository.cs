using System.Linq.Expressions;

namespace HableChat.Domain.Channels;

public interface IChannelRepository
{
    void Add(Channel entity);
    Task<Channel> GetByIdAsync(long id);
    IQueryable<Channel> Get(Expression<Func<Channel, bool>> filter = null);
    Task Delete(long id);
}
