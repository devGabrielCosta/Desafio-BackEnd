using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().First(x => x.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return _context.Set<T>();
        }

        public Task InsertAsync(T value)
        {
            _context.Set<T>().AddAsync(value);
            return _context.SaveChangesAsync();
        }
        public Task UpdateAsync(T value)
        {
            _context.Set<T>().Update(value);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T value)
        {
            _context.Set<T>().Remove(value);
            return _context.SaveChangesAsync();
        }
    }
}
