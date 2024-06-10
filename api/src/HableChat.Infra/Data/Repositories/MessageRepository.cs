using System.Linq.Expressions;
using HableChat.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace HableChat.Infra.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IRepository<Message> _baseRepository;
    private readonly DbSet<Message> _dbSet;

    public MessageRepository(IRepository<Message> repository, AppDbContext context)
    {
        _dbSet = context.Set<Message>();
        _baseRepository = repository;
    }

    public void Add(Message entity)
    {
        _baseRepository.Add(entity);
    }

    public async Task Delete(long id, long channelId)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.ChannelId == channelId && x.Id == id);
        _dbSet.Remove(entity);
    }

    public IQueryable<Message> Get(Expression<Func<Message, bool>> filter = null)
    {
        return _baseRepository.Get(filter);
    }
}
