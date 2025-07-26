using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.Repositories
{
    public interface ITaskCommandRepository : ICommandRepository<TaskItem>
    {
    }
}
