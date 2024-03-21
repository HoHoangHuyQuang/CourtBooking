using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Repositories.Implement
{
    public class UserRepository : BaseRepository<User, Guid?>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<User>?> GetAll()
        {
            var all = await _dbSet.Include(u => u.Role)
                
                .ToListAsync();
            return all;
        }
        public override async Task<User?> GetById(Guid? entityId)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .Where(u => u.Id == entityId)
                .FirstOrDefaultAsync();
        }
        public  async Task<User?> GetByUsername(string username)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .Where(u => u.UserName.Equals(username))
                .FirstOrDefaultAsync();
        }
       
    }
}
