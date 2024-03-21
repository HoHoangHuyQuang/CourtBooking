using CourtBooking.Models;

namespace CourtBooking.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product, Guid?>
    {
        public Task<IEnumerable<Product>?> GetAllByCategory(int? cateid);
    }
}
