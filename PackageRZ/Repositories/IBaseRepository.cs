using System.Linq.Expressions;

namespace PackageRZ.Repositories;

/// <summary>
/// Defines a standard repository interface for basic CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TPK">The type of the primary key of the entity.</typeparam>
public interface IBaseRepository<T, TPK>
{
    /// <summary>
    /// Finds a single entity that matches the specified filter expression asynchronously.
    /// </summary>
    /// <param name="expression">The filter expression.</param>
    /// <returns>A task returning the matched entity or null if not found.</returns>
    Task<T> FindByFilterAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Retrieves all entities that match the specified filter expression asynchronously.
    /// </summary>
    /// <param name="expression">The filter expression.</param>
    /// <returns>A task returning an enumerable collection of matching entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Finds an entity by its primary key asynchronously.
    /// </summary>
    /// <param name="entity">An entity containing the primary key value.</param>
    /// <returns>A task returning the matched entity or null if not found.</returns>
    Task<T> FindByIdAsync(T entity);

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task returning an enumerable collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Inserts a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task returning the inserted entity.</returns>
    Task<T> InsertAsync(T entity);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity containing updated values.</param>
    /// <returns>A task returning the updated entity or null if it was not found.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task returning true if deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAsync(T entity);
}