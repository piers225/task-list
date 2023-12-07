
using System.Linq.Expressions;

namespace TaskList.DataAccess.Repository;

internal interface IRepository<TEntity> 
    where TEntity : class
{
    public Task<TEntity?> FindOneOrNone(Expression<Func<TEntity, bool>> predicate);

    public Task<TEntity> FindOne(Expression<Func<TEntity, bool>> predicate);

    public Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

    public Task<TEntity[]> Where(Expression<Func<TEntity, bool>> predicate);

    public void Add(TEntity entity);

    public Task SaveChanges();
}