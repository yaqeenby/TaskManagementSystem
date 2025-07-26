using TaskManagementSystem.Shared.Models;

namespace TaskManagementSystem.Shared.Services
{
    public interface IJwtAuthService
    {
        string GenerateToken(JwtUserModel user);
    }
}