using TaskManagementSystem.Shared.Models;
using TaskManagementSystem.Tasks.DTOs;

namespace TaskManagementSystem.Users.DTOs;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = UserRoles.User;
    public ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();

}
