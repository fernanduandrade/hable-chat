using System.Linq.Expressions;

namespace HableChat.Domain.Messages;

public interface IMessageRepository
{
    void Add(Message entity);
    Task Delete(long id, long channelId);
    IQueryable<Message> Get(Expression<Func<Message, bool>> filter = null);
}
