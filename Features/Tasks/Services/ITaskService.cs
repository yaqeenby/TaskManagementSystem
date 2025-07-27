using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<IEnumerable<TaskDto>>> GetAllTasksAsync();
        Task<ApiResponse<TaskDetailsDto?>> GetTaskByIdAsync(Guid id);
        Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto task);
        Task<ApiResponse<TaskDto>> UpdateTaskAsync(UpdateTaskDto task);
        Task<ApiResponse<TaskDto>> UpdateTaskStatusAsync(Guid id, string status);
        Task<ApiResponse<Guid>> DeleteTaskAsync(Guid id);
    }
}