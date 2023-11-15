using AuctionAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace AuctionAPI.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(
    Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            var dbEntityEntry = _context.Entry(entity);

            if (updatedProperties.Any())
            {
                // Sadece belirli özellikleri güncelle
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                // Tüm özellikleri güncelle
                dbEntityEntry.State = EntityState.Modified;
            }

            return Task.CompletedTask;
        }
        public async Task UpdateFieldAsync<TField>(object id, Expression<Func<T, TField>> fieldSelector, TField newValue)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity with the specified ID not found.");
            }

            var propertyInfo = ((MemberExpression)fieldSelector.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The fieldSelector expression does not refer to a valid property.");
            }

            propertyInfo.SetValue(entity, newValue, null);
            _context.Entry(entity).Property(propertyInfo.Name).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {

            await _context.SaveChangesAsync();
        }
    }
}
