using System.Linq.Expressions;

using LuizaLabsApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T: class 
{
    protected DatabaseContext context;
    internal DbSet<T> dbSet;
    protected readonly ILogger _logger;

    public GenericRepository(DatabaseContext context, ILogger logger)
    {
        this.context = context;
        this.dbSet = context.Set<T>();
        this._logger = logger;
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Add(T entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        dbSet.Remove(await GetById(id));
        await context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> Update(T entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();
        return true;
    }


    public virtual async Task<bool> Upsert(Guid id, T entity)
    {
        var o = await GetById(id);
        if (o == null)
            return await Add(entity);
        else
            return await Update(entity);
    }

    public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.Where(predicate).ToListAsync();
    }
}