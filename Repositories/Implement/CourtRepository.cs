using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;

namespace CourtBooking.Repositories.Implement
{
    public class CourtRepository : BaseRepository<Court, int?>, ICourtRepository
    {
        public CourtRepository(DatabaseContext context) : base(context)
        {
        }

    }
}
