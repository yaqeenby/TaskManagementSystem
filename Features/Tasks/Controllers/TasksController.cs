using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;
using TaskManagementSystem.Shared.Models;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet]
    public async Task<ApiResponse<IEnumerable<TaskDto>>> GetTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return tasks;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ApiResponse<TaskDetailsDto?>> GetTask(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        return task;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost]
    public async Task<ApiResponse<TaskDto>> CreateTask(CreateTaskDto task)
    {
        var response = await _taskService.CreateTaskAsync(task);
        return response;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut]
    public async Task<ApiResponse<TaskDto>> UpdateTask(UpdateTaskDto task)
    {
        var response = await _taskService.UpdateTaskAsync(task);
        return response;
    }

    [Authorize]
    [HttpPut("UpdateStatus/{id}")]
    public async Task<ApiResponse<TaskDto>> UpdateTaskStatus(Guid id, string status)
    {
        var response = await _taskService.UpdateTaskStatusAsync(id, status);
        return response;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<Guid>> DeleteTask(Guid id)
    {
        var response = await _taskService.DeleteTaskAsync(id);
        return response;
    }
}
