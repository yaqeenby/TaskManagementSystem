using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Auth.Models;
using TaskManagementSystem.Auth.Services;
using TaskManagementSystem.Shared.Services;

namespace TaskManagementSystem.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtAuthService _jwtAuthService;

        public AuthController(IAuthService authService, IJwtAuthService jwtAuthService)
        {
            _authService = authService;
            _jwtAuthService = jwtAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _jwtAuthService.GenerateToken(user);
            return Ok(new { token });
        }
    }

}
