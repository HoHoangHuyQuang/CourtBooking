using CourtBooking.Data;
using CourtBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Repositories.Implement
{
    public class BaseRepository<T, ID> : IBaseRepository<T, ID> where T : class
    {
        protected readonly DatabaseContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> AddRange(List<T> entityList)
        {
            await _dbSet.AddRangeAsync(entityList);
            return true;
        }

        public virtual bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>?> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetById(ID? entityId)
        {
            T? entity = await _dbSet.FindAsync(entityId);
            return entity;
        }

        public virtual async Task<bool> IsExists(ID? entityId)
        {
            T? entity = await _dbSet.FindAsync(entityId);
            if (entity != null)
            {
                return true;
            }
            return false;
        }

        public virtual Task<bool> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(true);
        }

    }
}
