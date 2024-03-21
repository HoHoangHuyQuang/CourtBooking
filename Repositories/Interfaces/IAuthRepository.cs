using CourtBooking.Models;
using System.Security.Claims;

namespace CourtBooking.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<User?> LoginValidate(string Username, string Password);
       
    }
}
