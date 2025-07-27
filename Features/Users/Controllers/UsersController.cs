using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Services;

namespace TaskManagementSystem.Users.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserDto?>> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user;
        }

        [HttpGet("{id}/full-details")]
        public async Task<ApiResponse<UserDetailsDto?>> GetUserWithTasks(Guid id)
        {
            var user = await _service.GetUserByIdWithTasksAsync(id);
            return user;
        }

        [HttpPost]
        public async Task<ApiResponse<UserDto>> CreateUser(CreateUserDto user)
        {
            var response = await _service.CreateUserAsync(user);
            return response;
        }

        [HttpPut]
        public async Task<ApiResponse<UserDto>> UpdateUser(UpdateUserDto user)
        {
            var response = await _service.UpdateUserAsync(user);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Guid>> DeleteUser(Guid id)
        {
            var response = await _service.DeleteUserAsync(id);
            return response;
        }
    }
}
