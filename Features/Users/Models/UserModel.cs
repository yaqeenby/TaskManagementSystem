using TaskManagementSystem.Shared.Base;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Users.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
        public bool IsDeleted { get; set; } = false;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}