namespace CourtBooking.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepo { get; }
        IUserRepository UserRepo { get; }
        IBookingRepository BookingRepo { get; }
        ICourtRepository CourtRepo { get; }
        IProductRepository ProductRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        IRoleRepository RoleRepo { get; }
        
        Task SaveChangesAsync();
    }
}
