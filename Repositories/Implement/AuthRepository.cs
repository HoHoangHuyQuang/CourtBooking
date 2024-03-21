using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using CourtBooking.Utils;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Repositories.Implement
{
    public class AuthRepository : IAuthRepository
    {
        
        protected readonly DatabaseContext _context;

        public AuthRepository(DatabaseContext context)
        {
            _context = context;
          
        }

        public async Task<User?> LoginValidate(string Username, string Password)
        {
            var result = await _context.Users.Include(a => a.Role)
                                 .Where(a => a.UserName.Equals(Username) && 
                                 a.Password.Equals(AppUtils.HmacSha256Encrypt(Password)))
                                 .FirstOrDefaultAsync();

            return result;
        }     


    }
}
