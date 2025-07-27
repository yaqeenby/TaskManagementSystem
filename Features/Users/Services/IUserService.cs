using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Users.Services
{

    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto?>> GetUserByIdAsync(Guid id);
        Task<ApiResponse<UserDetailsDto?>> GetUserByIdWithTasksAsync(Guid id);
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto user);
        Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateUserDto user);
        Task<ApiResponse<Guid>> DeleteUserAsync(Guid id);
        Task<ApiResponse<bool>> IsUserExistAsync(Guid id);
    }

}