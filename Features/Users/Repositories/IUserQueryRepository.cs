using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public interface IUserQueryRepository : IQueryRepository<User>
    {
        Task<User?> GetByIdWithTasksAsync(Guid id);
    }
}