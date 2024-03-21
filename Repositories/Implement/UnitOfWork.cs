using CourtBooking.Data;
using CourtBooking.Repositories.Interfaces;

namespace CourtBooking.Repositories.Implement
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class UnitOfWork : IUnitOfWork
    {
       
        private readonly DatabaseContext _context;
        private IAuthRepository _authRepo;
        private IUserRepository _userRepo;
        private IBookingRepository _bookingRepo;
        private ICourtRepository _courtRepo;
        private IProductRepository _productRepo;
        private IRoleRepository _roleRepo;
        private ICategoryRepository _cateRepo;
        
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

        }

        public IAuthRepository AuthRepo
        {
            get
            {
                if (this._authRepo == null)
                {
                    this._authRepo = new AuthRepository(_context);
                }
                return _authRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                if (this._userRepo == null)
                {
                    this._userRepo = new UserRepository(_context);
                }
                return _userRepo;
            }
        }
        public IBookingRepository BookingRepo
        {
            get
            {
                if (this._bookingRepo == null)
                {
                    this._bookingRepo = new BookingRepository(_context);
                }
                return _bookingRepo;
            }
        }

        public ICourtRepository CourtRepo
        {
            get
            {
                if (this._courtRepo == null)
                {
                    this._courtRepo = new CourtRepository(_context);
                }
                return _courtRepo;
            }
        }

        public IProductRepository ProductRepo
        {
            get
            {
                if (this._productRepo == null)
                {
                    this._productRepo = new ProductRepository(_context);
                }
                return _productRepo;
            }
        }

        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (this._cateRepo == null)
                {
                    this._cateRepo = new CategoryRepository(_context);
                }
                return _cateRepo;
            }
        }

        public IRoleRepository RoleRepo
        {
            get
            {
                if (this._roleRepo == null)
                {
                    this._roleRepo = new RoleRepository(_context);
                }
                return _roleRepo;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
