using CourtBooking.Models;

namespace CourtBooking.Repositories.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking, Guid?>
    {
        Task<IEnumerable<Booking>> GetBookingsForUser(string userId);
        Task<IEnumerable<Booking>> GetBookingsForCourt(int courtId);
        Task<IEnumerable<Booking>> GetUpcomingBookings();
        bool IsCourtAvailable(int courtId, DateTime startTime, DateTime endTime);
        public Task<IEnumerable<Court>> GetAllCourtAvailable(DateTime startTime, DateTime endTime);
        Task<bool> CancelBooking(Guid? entityId);
        Task<bool> AcceptBooking(Guid? entityId);
    }
}
