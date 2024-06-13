using System.Linq.Expressions;

namespace CraftShop.API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? Filter = null, string? includeProperties = null,int pageSize = 3,int pageNumber = 1);
        Task<List<T>> GetAllNoPagination(Expression<Func<T, bool>>? Filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? Filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}
