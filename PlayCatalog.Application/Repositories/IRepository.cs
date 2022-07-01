
using System.Linq.Expressions;
using PlayCatalog.Model;

namespace PlayCatalog.Application.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T,bool>>filter);
        Task<T> GetItem(Guid id);
        Task<T> GetItem(Expression<Func<T, bool>> filter);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteItem(Guid id);
    }
}
