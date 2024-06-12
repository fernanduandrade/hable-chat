using System.Linq.Expressions;
using HableChat.Domain.Channels;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace HableChat.Infra.Data.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly IRepository<Channel> _baseRepository;
    private readonly DbSet<Channel> _dbSet;
    public ChannelRepository(IRepository<Channel> repository, AppDbContext context)
    {
        _dbSet = context.Set<Channel>();
        _baseRepository = repository;
    }

    public void Add(Channel entity)
    {
        _baseRepository.Add(entity);
    }

    public async Task Delete(long id)
    {
        await _baseRepository.Delete(id);
    }

    public IQueryable<Channel> Get(Expression<Func<Channel, bool>> filter = null)
    {
        return _baseRepository.Get(filter);
    }

    public async Task<Channel> GetByIdAsync(long id)
    {

        return await _baseRepository.GetByIdAsync(id);
    }
    
}
