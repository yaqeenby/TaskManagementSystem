using TaskManagementSystem.Shared.Models;

namespace TaskManagementSystem.Auth.Services
{
    public interface IAuthService
    {
        Task<JwtUserModel?> ValidateUserAsync(string email, string password);
    }
}