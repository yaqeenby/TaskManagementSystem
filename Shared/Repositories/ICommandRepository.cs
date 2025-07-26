using TaskManagementSystem.Shared.Base;

namespace TaskManagementSystem.Shared.Repositories
{
    public interface ICommandRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task Update(T entity);
        Task<bool> DeleteByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}