using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public class UserQueryRepository : GenericRepository<User>, IUserQueryRepository
    {
        private readonly AppDbContext _context;

        public UserQueryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdWithTasksAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
