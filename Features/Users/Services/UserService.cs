using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Shared.Enums;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Shared.Services;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;
using TaskManagementSystem.Users.Repositories;

namespace TaskManagementSystem.Users.Services
{

    public class UserService : IUserService
    {
        private readonly IUserQueryRepository _queryRepository;
        private readonly IUserCommandRepository _commandRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;

        public UserService(IUserQueryRepository queryRepository, IUserCommandRepository commandRepository, IPasswordHasher<User> passwordHasher, ICurrentUserService currentUserService)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var usersData = await _queryRepository.GetAllAsync();
                return ApiResponse<IEnumerable<User>>.SuccessResponse(usersData, "Users Retrieved Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<User>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<User?>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdAsync(id);
                if (userData != null)
                {
                    return ApiResponse<User?>.SuccessResponse(userData, "User Retrieved Successfully");
                }
                else
                {
                    return ApiResponse<User?>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<User?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<User?>> GetUserByIdWithTasksAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdWithTasksAsync(id);
                if (userData != null)
                {
                    return ApiResponse<User?>.SuccessResponse(userData, "User Retrieved Successfully");
                }
                else
                {
                    return ApiResponse<User?>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<User?>.FailResponse($"Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<User>> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                var userData = new User
                {
                    Id = Guid.NewGuid(),
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Password = user.Password,
                    CreatedBy = _currentUserService?.UserId
                };

                userData.Password = _passwordHasher.HashPassword(userData, user.Password);
                await _commandRepository.AddAsync(userData);
                return ApiResponse<User>.SuccessResponse(userData, "User Updated Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.FailResponse($"Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<User>> UpdateUserAsync(User user)
        {
            try
            {
                var userData = await _queryRepository.GetByIdAsync(user.Id);
                if (userData != null)
                {
                    if (user.Name != null) userData.Name = user.Name;
                    if (user.Email != null) userData.Email = user.Email;
                    if (user.Role != null) userData.Role = user.Role;

                    if (user.Password != null) userData.Password = _passwordHasher.HashPassword(user, user.Password);

                    user.UpdatedAt = DateTime.UtcNow;
                    user.UpdatedBy = _currentUserService?.UserId;

                    await _commandRepository.Update(userData);
                    return ApiResponse<User>.SuccessResponse(user, "User Updated Successfully");
                }
                else
                {
                    return ApiResponse<User>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Guid>> DeleteUserAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdAsync(id);
                if (userData != null)
                {
                    await _commandRepository.DeleteByIdAsync(id);
                    return ApiResponse<Guid>.SuccessResponse(id, "User Deleted Successfully");
                }
                else
                {
                    return ApiResponse<Guid>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.FailResponse($"Error: {ex.Message}");
            }
        }
    }

}