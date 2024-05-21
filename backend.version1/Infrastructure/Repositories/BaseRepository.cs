using backend.Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backend.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        public IQueryable<T> FindAll() => ApplicationDbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            ApplicationDbContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity) => ApplicationDbContext.Set<T>().Add(entity);

        public void Update(T entity) => ApplicationDbContext.Set<T>().Update(entity);

        public void Delete(T entity) => ApplicationDbContext.Set<T>().Remove(entity);

        public async Task<T?> GetByIdAsync(int id)
        {
            return await ApplicationDbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await ApplicationDbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await ApplicationDbContext.Set<T>().AddAsync(entity);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            ApplicationDbContext.Set<T>().Update(entity);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ApplicationDbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                ApplicationDbContext.Set<T>().Remove(entity);
                await ApplicationDbContext.SaveChangesAsync();
            }
        }
    }
}