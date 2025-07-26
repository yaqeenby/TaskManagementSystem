using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;
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
        public async Task<ApiResponse<IEnumerable<User>>> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<User?>> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user;
        }

        [HttpGet("tasks/{id}")]
        public async Task<ApiResponse<User?>> GetUserWithTasks(Guid id)
        {
            var user = await _service.GetUserByIdWithTasksAsync(id);
            return user;
        }

        [HttpPost]
        public async Task<ApiResponse<User>> CreateUser(CreateUserDto user)
        {
            var response = await _service.CreateUserAsync(user);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<User>> UpdateUser(User user)
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
