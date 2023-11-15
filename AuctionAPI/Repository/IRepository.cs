using System.Linq.Expressions;

namespace AuctionAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludesAsync(Func<IQueryable<T>, IQueryable<T>> include = null);

        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
        Task DeleteAsync(T entity);
        Task SaveAsync();
        Task UpdateFieldAsync<TField> (object id, Expression<Func<T, TField>> fieldSelector, TField newValue);
    }
}
