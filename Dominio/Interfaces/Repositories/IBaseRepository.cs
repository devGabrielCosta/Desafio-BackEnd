using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        T Get(Guid id);
        IEnumerable<T> Get();
        Task InsertAsync(T value);
        Task UpdateAsync(T value);
        Task DeleteAsync(T value);
        Task DeleteAsync(Guid id);
    }
}
