using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get(Guid id);
        IQueryable<T> Get();
        Task InsertAsync(T value);
        void Update(T value);
        void Delete(T value);
        void Delete(Guid id);
    }
}
