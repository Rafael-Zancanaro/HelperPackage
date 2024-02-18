using Microsoft.EntityFrameworkCore;
using PackageRZ.Domain.Entities;
using System.Linq.Expressions;

namespace PackageRZ.Repositories
{
    public abstract class BaseRepository<T, TPK> : IBaseRepository<T, TPK> where T : BaseEntity<TPK>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> FindByFilterAsync(Expression<Func<T, bool>> expression)
            => await _dbSet.FindAsync(expression).ConfigureAwait(false);
        public async Task<T> FindByIdAsync(T entity)
            => await _dbSet.FindAsync(entity.Id).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
            => await _dbSet.Where(expression).ToListAsync().ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync().ConfigureAwait(false);

        public async Task<T> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = await _dbSet.FindAsync(entity.Id).ConfigureAwait(false);
            if (result == null)
                return null;

            _dbContext.Entry(result).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

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
}