using System.Linq.Expressions;

namespace WatchShop.DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(int id, string includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(string includeProperties = null);
        Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> filter, string includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}