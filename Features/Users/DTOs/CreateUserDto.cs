
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Users.DTOs;

public class CreateUserDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Role { get; set; } = null!;
}
