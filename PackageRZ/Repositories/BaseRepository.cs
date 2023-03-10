using Microsoft.EntityFrameworkCore;
using PackageRZ.Domain.Entities;

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

        public async Task<T> FindByIdAsync(T entity)
            => await _dbSet.FindAsync(entity.Id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = await _dbSet.FindAsync(entity.Id);
            if (result == null)
                return null;

            _dbContext.Entry(result).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            var result = await _dbSet.FindAsync(entity.Id);
            if (result == null)
                return false;

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}