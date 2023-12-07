using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TaskList.DataAccess.DataContext;

namespace TaskList.DataAccess.Repository;

internal class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class
{

    private readonly CheckListDataContext dbContext;

    public Repository(CheckListDataContext dbContext) 
    {
        this.dbContext = dbContext;
    }

    public Task<TEntity?> FindOneOrNone(Expression<Func<TEntity, bool>> predicate) 
    {
        return this.dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }

    public Task<TEntity> FindOne(Expression<Func<TEntity, bool>> predicate) 
    {
        return this.dbContext.Set<TEntity>().SingleAsync(predicate);
    }

    public Task<bool> Any(Expression<Func<TEntity, bool>> predicate) 
    {
        return this.dbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public Task<TEntity[]> Where(Expression<Func<TEntity, bool>> predicate) 
    {
        return this.dbContext.Set<TEntity>().Where(predicate).ToArrayAsync();
    }

    public void Add(TEntity entity)
    {
        this.dbContext.Set<TEntity>().Add(entity);
    }

    public Task SaveChanges()
    {
        return this.dbContext.SaveChangesAsync();
    }
}