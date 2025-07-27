using System.ComponentModel.DataAnnotations;
namespace TaskManagementSystem.Users.DTOs;

public class UpdateUserDto
{
    [Required]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}