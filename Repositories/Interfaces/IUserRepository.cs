using CourtBooking.Models;

namespace CourtBooking.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, Guid?>
    {
        public Task<User?> GetByUsername(string username);
    }
   
}
