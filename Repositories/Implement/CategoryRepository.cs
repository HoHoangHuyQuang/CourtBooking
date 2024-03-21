using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;

namespace CourtBooking.Repositories.Implement
{
    public class CategoryRepository : BaseRepository<Category, int?>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
