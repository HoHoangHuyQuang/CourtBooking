using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Repositories.Implement
{
    public class ProductRepository : BaseRepository<Product, Guid?>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Product>?> GetAll()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }
        public override async Task<Product?> GetById(Guid? entityId)
        {
            if (entityId == null)
            {
                return null;
            }
            return await _dbSet.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == entityId);
        }
        public async Task<IEnumerable<Product>?> GetAllByCategory(int? cateid)
        {
            var cate = await _context.Categories.FirstOrDefaultAsync(c => c.Id == cateid);
            if (cate == null)
            {
                return null;
            }
            return cate.Products;
        }
    }
}
