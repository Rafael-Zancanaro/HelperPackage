using Microsoft.EntityFrameworkCore;
using PackageRZ.Domain.Entities;
using System.Linq.Expressions;

namespace PackageRZ.Repositories;

/// <summary>
/// Provides a base implementation for a repository offering standard CRUD operations using Entity Framework Core.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TPK">The type of the primary key of the entity.</typeparam>
public abstract class BaseRepository<T, TPK>(DbContext dbContext) : IBaseRepository<T, TPK> where T : BaseEntity<TPK>
{
    protected readonly DbContext _dbContext = dbContext;
    protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

    /// <inheritdoc/>
    public async Task<T> FindByFilterAsync(Expression<Func<T, bool>> expression)
        => await _dbSet.FirstOrDefaultAsync(expression).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<T> FindByIdAsync(T entity)
        => await _dbSet.FindAsync(entity.Id).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        => await _dbSet.Where(expression).ToListAsync().ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync().ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<T> InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }

    /// <inheritdoc/>
    public async Task<T> UpdateAsync(T entity)
    {
        var result = await _dbSet.FindAsync(entity.Id).ConfigureAwait(false);
        if (result == null)
            return null;

        _dbContext.Entry(result).CurrentValues.SetValues(entity);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(T entity)
    {
        var result = await _dbSet.FindAsync(entity.Id).ConfigureAwait(false);
        if (result == null)
            return false;

        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }
}