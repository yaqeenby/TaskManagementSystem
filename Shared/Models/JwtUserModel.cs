using TaskManagementSystem.Shared.Base;

namespace TaskManagementSystem.Shared.Models
{
    public class JwtUserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}