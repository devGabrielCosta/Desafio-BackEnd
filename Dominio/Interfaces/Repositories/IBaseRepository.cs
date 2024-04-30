using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        T? Get(Guid id);
        IEnumerable<T> Get();
        Task InsertAsync(T value);
        void Update(T value);
        void Delete(T value);
        void Delete(Guid id);
    }
}
