using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Shared.Repositories;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Repositories
{
    public class UserQueryRepository : GenericRepository<User>, IUserQueryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserQueryRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User?> GetByIdWithTasksAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> IsUserExistAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId && !u.IsDeleted);
        }

        public async Task<bool> IsEmailDuplicatedAsync(Guid userId, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != userId && !u.IsDeleted);
        }
    }
}
