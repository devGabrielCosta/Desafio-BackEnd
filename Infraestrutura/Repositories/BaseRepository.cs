using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public T? Get(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return _context.Set<T>();
        }

        public async Task InsertAsync(T value)
        {
           await _context.Set<T>().AddAsync(value);
           _context.SaveChanges();
        }
        public void Update(T value)
        {
            _context.Set<T>().Update(value);
            _context.SaveChanges();
        }

        public void Delete(T value)
        {
            _context.Set<T>().Remove(value);
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {   
            T value = Get(id);
            Delete(value);
        }
    }
}
