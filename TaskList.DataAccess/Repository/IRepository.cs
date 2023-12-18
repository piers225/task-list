
using System.Linq.Expressions;

namespace TaskList.DataAccess.Repository;

internal interface IRepository<TEntity> 
    where TEntity : class
{
    public Task<TEntity?> FindOneOrNone(int id);
    
    public Task<TEntity> FindOne(int id);
    
    public Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

    public Task<TEntity[]> Where(Expression<Func<TEntity, bool>> predicate);

    public void Add(TEntity entity);

    public Task SaveChanges();
}