using CourtBooking.Models;
using CourtBooking.ViewModels;
using CourtBooking.Repositories.Interfaces;
using CourtBooking.Utils;
using Microsoft.EntityFrameworkCore;
using CourtBooking.Data;

namespace CourtBooking.Repositories.Implement
{
    public class BookingRepository : BaseRepository<Booking, Guid?>, IBookingRepository
    {
        public BookingRepository(DatabaseContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Booking>?> GetAll()
        {
            return await _dbSet.Include(b => b.Users).Include(b => b.Court).OrderByDescending(b=>b.CreatedAt).ToListAsync();
        }
        public override async Task<Booking?> GetById(Guid? entityId)
        {
            return await _dbSet.Include(b => b.Users).Include(b => b.Court).FirstOrDefaultAsync(p => p.BookingId == entityId);
        }
        public async Task<IEnumerable<Booking>> GetBookingsForCourt(int courtId)
        {
            return await _context.Bookings.Include(b => b.Users).Include(b => b.Court).Where(b => b.CourtId == courtId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsForUser(string userId)
        {
            return await _context.Bookings.Include(b => b.Users).Include(b => b.Court).Where(b => b.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetUpcomingBookings()
        {
            return await _context.Bookings.Include(b => b.Users).Include(b => b.Court).Where(b => b.StartTime > DateTime.Now).ToListAsync();
        }
        public async Task<IEnumerable<Court>> GetAllCourtAvailable(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                throw new ArgumentException("Timeline conflict! sta");
            }
            var bookedCourtIds = _context.Bookings.Include(b => b.Users).Include(b => b.Court).Where(b =>
                                                        b.Status != -1 &&
                                                      ((startTime >= b.StartTime && startTime < b.EndTime) ||
                                                        (endTime > b.StartTime && endTime <= b.EndTime)) )
                                                    .Select(b => b.CourtId)
                                                    .ToList();
            return await _context.Courts.Where(c => !bookedCourtIds.Contains(c.CourtId)).ToListAsync();
        }
        public bool IsCourtAvailable(int courtId, DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                throw new ArgumentException("Timeline conflict! sta");
            }
            
            return !_context.Bookings.Any(b =>
                                    b.CourtId == courtId && b.Status != -1 &&
                                    (startTime >= b.StartTime && startTime < b.EndTime ||
                                            endTime > b.StartTime && endTime <= b.EndTime));
        }
        public async Task<bool> AcceptBooking(Guid? entityId)
        {
            var bking = await _dbSet.Include(b => b.Users).Include(b => b.Court).FirstOrDefaultAsync(p => p.BookingId == entityId);
            if(bking == null || bking.Status != 0)
            {
                return false;
            }
            bking.Status = 1;
            bking.Checksum = AppUtils.MD5Enscrypt(bking.BookingId.ToString() + bking.CourtId.ToString() + bking.CreatedAt.ToString() + bking.StartTime.ToString() + bking.DurationInHour.ToString() + bking.TotalAmount.ToString() + bking.Status.ToString());

            await Update(bking);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelBooking(Guid? entityId)
        {
            var bking = await _dbSet.Include(b => b.Users).Include(b => b.Court).FirstOrDefaultAsync(p => p.BookingId == entityId);
            if (bking == null || bking.Status == -1)
            {
                return false;
            }
            bking.Status = -1;
            bking.Checksum = AppUtils.MD5Enscrypt(bking.BookingId.ToString() + bking.CourtId.ToString() + bking.CreatedAt.ToString() + bking.StartTime.ToString() + bking.DurationInHour.ToString() + bking.TotalAmount.ToString() + bking.Status.ToString());

            await Update(bking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
