using System.Linq.Expressions;

namespace PackageRZ.Repositories
{
    public interface IBaseRepository<T, TPK>
    {
        Task<T> FindByFilterAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<T> FindByIdAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}