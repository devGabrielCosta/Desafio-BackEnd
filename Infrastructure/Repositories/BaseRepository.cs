using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get(Guid id)
        {
            return _context.Set<T>().Where(x => x.Id == id);
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public async Task InsertAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
           _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {   
            T value = Get(id).FirstOrDefault();
            Delete(value);
        }
    }
}
