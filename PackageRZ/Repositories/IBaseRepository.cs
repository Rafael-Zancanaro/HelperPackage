namespace PackageRZ.Repositories
{
    public interface IBaseRepository<T, TPK>
    {
        Task<T> FindByIdAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}