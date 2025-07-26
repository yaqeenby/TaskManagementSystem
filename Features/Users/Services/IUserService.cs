using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Services
{

    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<User>>> GetAllUsersAsync();
        Task<ApiResponse<User?>> GetUserByIdAsync(Guid id);
        Task<ApiResponse<User?>> GetUserByIdWithTasksAsync(Guid id);
        Task<ApiResponse<User>> CreateUserAsync(CreateUserDto user);
        Task<ApiResponse<User>> UpdateUserAsync(User user);
        Task<ApiResponse<Guid>> DeleteUserAsync(Guid id);
    }

}