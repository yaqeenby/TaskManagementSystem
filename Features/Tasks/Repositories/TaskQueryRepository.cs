using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.Repositories
{
    public class TaskQueryRepository : GenericRepository<TaskItem>, ITaskQueryRepository
    {
        private readonly AppDbContext _context;

        public TaskQueryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task;
        }
    }
}