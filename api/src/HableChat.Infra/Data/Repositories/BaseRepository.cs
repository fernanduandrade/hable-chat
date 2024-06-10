using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace HableChat.Infra.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T : Entity
{
    private readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> filter = null)
    {
        var query = _dbSet.AsQueryable();

        if(filter is not null)
            query = query
            .Where(filter);
            
        return query;
    }

    public async Task<T> GetByIdAsync(long id)
    {
        var record = await _dbSet.FindAsync(id);
        return record;
    }

    public async Task Delete(long id)
    {
        var record = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        _dbSet.Remove(record);
        
    }
}
