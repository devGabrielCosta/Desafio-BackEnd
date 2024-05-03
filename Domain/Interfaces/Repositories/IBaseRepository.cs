using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get(Guid id);
        IQueryable<T> Get();
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Guid entity);
    }
}
