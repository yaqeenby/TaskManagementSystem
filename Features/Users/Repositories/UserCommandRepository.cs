using TaskManagementSystem.Data;
using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public class UserCommandRepository : GenericRepository<User>, IUserCommandRepository
    {
        private readonly AppDbContext _context;

        public UserCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
