namespace TaskManagementSystem.Tasks.DTOs
{

    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public Guid AssignedUserId { get; set; }
    }

}