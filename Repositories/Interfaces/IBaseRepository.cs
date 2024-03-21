namespace CourtBooking.Repositories.Interfaces
{
    public interface IBaseRepository<T, ID> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> AddRange(List<T> entityList);
        Task<bool> Update(T entity);
        bool Delete(T entity);
        Task<IEnumerable<T>?> GetAll();
        Task<T?> GetById(ID? entityId);
        Task<bool> IsExists(ID? entityId);
    }
}
