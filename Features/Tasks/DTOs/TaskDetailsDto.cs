using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Users.DTOs;

namespace TaskManagementSystem.Tasks.DTOs
{

    public class TaskDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = TaskItemStatus.New;
        public DateTime? DueDate { get; set; }
        public Guid AssignedUserId { get; set; }
        public UserDto AssignedUser { get; set; }
    }

}