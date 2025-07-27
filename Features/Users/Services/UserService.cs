using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Shared.Enums;
using TaskManagementSystem.Shared.Models;
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
        private readonly IMapper _mapper;
        public UserService(IUserQueryRepository queryRepository, IUserCommandRepository commandRepository, IPasswordHasher<User> passwordHasher, ICurrentUserService currentUserService, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var usersData = await _queryRepository.GetAllAsync();
                return ApiResponse<IEnumerable<UserDto>>.SuccessResponse(_mapper.Map<IEnumerable<UserDto>>(usersData), "Users Retrieved Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto?>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdAsync(id);
                if (userData != null)
                {
                    return ApiResponse<UserDto?>.SuccessResponse(_mapper.Map<UserDto>(userData), "User Retrieved Successfully");
                }
                else
                {
                    return ApiResponse<UserDto?>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDetailsDto?>> GetUserByIdWithTasksAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdWithTasksAsync(id);
                if (userData != null)
                {
                    return ApiResponse<UserDetailsDto?>.SuccessResponse(_mapper.Map<UserDetailsDto>(userData), "User Retrieved Successfully");
                }
                else
                {
                    return ApiResponse<UserDetailsDto?>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailsDto?>.FailResponse($"Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                if (!UserRoles.All.Contains(user.Role))
                {
                    return ApiResponse<UserDto>.FailResponse("Invalid Role value, must be one of " + JsonSerializer.Serialize(UserRoles.All), ErrorCode.ValidationError);
                }

                var isEmailDuplicated = await _queryRepository.IsEmailDuplicatedAsync(Guid.Empty, user.Email);

                if (isEmailDuplicated)
                {
                    return ApiResponse<UserDto>.FailResponse("Duplicated User Email", ErrorCode.UserEmailDuplicated);
                }

                var userData = _mapper.Map<User>(user);
                userData.Id = Guid.NewGuid();
                userData.CreatedBy = _currentUserService?.UserId;
                userData.Password = _passwordHasher.HashPassword(userData, user.Password);

                await _commandRepository.AddAsync(userData);

                return ApiResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(userData), "User Added Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.FailResponse($"Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateUserDto user)
        {
            try
            {
                if (user.Role != null && !UserRoles.All.Contains(user.Role))
                {
                    return ApiResponse<UserDto>.FailResponse("Invalid Role value, must be one of " + JsonSerializer.Serialize(UserRoles.All), ErrorCode.ValidationError);
                }

                if (user.Email != null)
                {
                    var isEmailDuplicated = await _queryRepository.IsEmailDuplicatedAsync(user.Id, user.Email);

                    if (isEmailDuplicated)
                    {
                        return ApiResponse<UserDto>.FailResponse("Duplicated User Email", ErrorCode.UserEmailDuplicated);
                    }
                }

                var userData = await _queryRepository.GetByIdAsync(user.Id);
                if (userData != null)
                {
                    if (user.Name != null) userData.Name = user.Name;
                    if (user.Email != null) userData.Email = user.Email;
                    if (user.Role != null) userData.Role = user.Role;

                    if (user.Password != null)
                    {
                        userData.Password = user.Password;
                        userData.Password = _passwordHasher.HashPassword(userData, user.Password);
                    }

                    userData.UpdatedAt = DateTime.UtcNow;
                    userData.UpdatedBy = _currentUserService?.UserId;

                    await _commandRepository.Update(userData);
                    return ApiResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(userData), "User Updated Successfully");
                }
                else
                {
                    return ApiResponse<UserDto>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Guid>> DeleteUserAsync(Guid id)
        {
            try
            {
                var userData = await _queryRepository.GetByIdWithTasksAsync(id);
                if (userData != null)
                {
                    if (userData.Tasks != null && userData.Tasks.Count > 0)
                    {
                        return ApiResponse<Guid>.FailResponse("Can't delete user since it has assigned tasks", ErrorCode.UserHasAssignedTasks);
                    }
                    else
                    {
                        userData.IsDeleted = true;

                        await _commandRepository.Update(userData);
                        return ApiResponse<Guid>.SuccessResponse(id, "User Deleted Successfully");
                    }

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

        public async Task<ApiResponse<bool>> IsUserExistAsync(Guid id)
        {
            try
            {
                var isExist = await _queryRepository.IsUserExistAsync(id);
                if (isExist)
                {
                    return ApiResponse<bool>.SuccessResponse(isExist, "User Exist");
                }
                else
                {
                    return ApiResponse<bool>.FailResponse("User not found", ErrorCode.UserNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }
    }

}