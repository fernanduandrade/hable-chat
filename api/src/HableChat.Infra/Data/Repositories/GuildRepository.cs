using System.Linq.Expressions;
using HableChat.Domain.Guilds;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace HableChat.Infra.Data.Repositories;

public class GuildRepository : IGuildRepository
{
    private readonly IRepository<Guild> _baseRepo;
    private readonly DbSet<Guild> _dbSet;

    public GuildRepository(IRepository<Guild> repository, AppDbContext context)
    {
        _baseRepo = repository;
        _dbSet = context.Guilds;
    }
    public void Add(Guild entity)
    {
        _baseRepo.Add(entity);
    }

    public async Task Delete(long id)
    {
        await _baseRepo.Delete(id);
    }

    public IQueryable<Guild> Get(Expression<Func<Guild, bool>> filter = null)
    {
        return _baseRepo.Get(filter);
    }

    public async Task<Guild> GetByIdAsync(long id)
      => await _dbSet.FindAsync(id);
}
