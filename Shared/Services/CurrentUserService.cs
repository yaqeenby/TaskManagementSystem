using System.Security.Claims;

namespace TaskManagementSystem.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var parsedUserId))
                {
                    UserId = parsedUserId;
                }

                UserEmail = user.FindFirst(ClaimTypes.Email)?.Value;
                UserName = user.Identity?.Name;
                UserRole = user.FindFirst(ClaimTypes.Role)?.Value;
            }
        }

        public Guid? UserId { get; }
        public string? UserEmail { get; }
        public string? UserName { get; }
        public string? UserRole { get; }
    }
}