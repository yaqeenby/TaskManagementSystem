using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public interface IUserCommandRepository : ICommandRepository<User>
    {
    }
}