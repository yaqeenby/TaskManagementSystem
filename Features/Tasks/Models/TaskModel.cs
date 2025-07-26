using TaskManagementSystem.Shared.Base;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Tasks.Models
{

    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public Guid AssignedUserId { get; set; }
        public User AssignedUser { get; set; } = null!;
    }
}