using TaskManagementSystem.Shared.Base;

namespace TaskManagementSystem.Shared.Repositories
{
    public interface IQueryRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        IQueryable<T> Query(); // for more advanced queries
    }
}