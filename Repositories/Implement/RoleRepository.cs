using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;

namespace CourtBooking.Repositories.Implement
{
    public class RoleRepository : BaseRepository<Role, int?>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
