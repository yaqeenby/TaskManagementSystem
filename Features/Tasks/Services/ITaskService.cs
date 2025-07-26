using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<IEnumerable<TaskItem>>> GetAllTasksAsync();
        Task<ApiResponse<TaskItem?>> GetTaskByIdAsync(Guid id);
        Task<ApiResponse<TaskItem>> CreateTaskAsync(CreateTaskDto task);
        Task<ApiResponse<TaskItem>> UpdateTaskAsync(TaskItem task);
        Task<ApiResponse<TaskItem>> UpdateTaskStatusAsync(Guid id, string status);
        Task<ApiResponse<Guid>> DeleteTaskAsync(Guid id);
    }
}