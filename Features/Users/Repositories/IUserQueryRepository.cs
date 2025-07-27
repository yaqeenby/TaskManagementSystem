using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public interface IUserQueryRepository : IQueryRepository<User>
    {
        Task<User?> GetByIdWithTasksAsync(Guid id);
        Task<bool> IsUserExistAsync(Guid id);
        Task<bool> IsEmailDuplicatedAsync(Guid userId, string email);
    }
}