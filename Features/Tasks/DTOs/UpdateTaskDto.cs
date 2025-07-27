using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.DTOs
{

    public class UpdateTaskDto
    {
        [Required]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid AssignedUserId { get; set; }
    }

}