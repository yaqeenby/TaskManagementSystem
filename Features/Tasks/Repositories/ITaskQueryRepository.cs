using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.Repositories
{
    public interface ITaskQueryRepository : IQueryRepository<TaskItem>
    {
    }
}
