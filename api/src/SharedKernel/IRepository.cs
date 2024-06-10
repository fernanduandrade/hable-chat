using System.Linq.Expressions;

namespace SharedKernel;

public interface IRepository<T> where T : class
{
    IQueryable<T> Get(Expression<Func<T, bool>> filter = null);
    Task<T> GetByIdAsync(long id);
    void Add(T entity);
    Task Delete(long id);
    
}