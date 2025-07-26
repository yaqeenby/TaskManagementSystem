using TaskManagementSystem.Data;
using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Tasks.Repositories;

namespace TaskManagementSystem.Tasks.Repositories
{
    public class TaskCommandRepository : GenericRepository<TaskItem>, ITaskCommandRepository
    {
        private readonly AppDbContext _context;

        public TaskCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}