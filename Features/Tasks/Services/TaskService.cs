using TaskManagementSystem.Shared.Enums;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Shared.Services;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Tasks.Repositories;

namespace TaskManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskQueryRepository _queryRepository;
        private readonly ITaskCommandRepository _commandRepository;
        private readonly ICurrentUserService _currentUserService;


        public TaskService(ITaskQueryRepository queryRepository, ITaskCommandRepository commandRepository, ICurrentUserService currentUserService)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<IEnumerable<TaskItem>>> GetAllTasksAsync()
        {
            try
            {
                var tasksData = await _queryRepository.GetAllAsync();
                return ApiResponse<IEnumerable<TaskItem>>.SuccessResponse(tasksData, "Tasks Retrieved Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<TaskItem>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskItem?>> GetTaskByIdAsync(Guid id)
        {
            try
            {
                var taskData = await _queryRepository.GetByIdAsync(id);
                Console.WriteLine(taskData?.AssignedUserId); // Should not be null
                Console.WriteLine(taskData?.AssignedUser?.Name);

                if (taskData != null)
                {
                    var role = _currentUserService?.UserRole;
                    if (role == "Admin" || (role == "User" && _currentUserService?.UserId == taskData.AssignedUserId))
                    {
                        return ApiResponse<TaskItem?>.SuccessResponse(taskData, "Task Retrieved Successfully");
                    }
                    else
                    {
                        return ApiResponse<TaskItem?>.FailResponse("Unauthorize to view task", ErrorCode.Unauthorize);
                    }
                }
                else
                {
                    return ApiResponse<TaskItem?>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskItem?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskItem>> CreateTaskAsync(CreateTaskDto task)
        {
            try
            {
                var taskData = new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    DueDate = task.DueDate,
                    AssignedUserId = task.AssignedUserId,
                    CreatedBy = this._currentUserService?.UserId
                };

                await _commandRepository.AddAsync(taskData);
                return ApiResponse<TaskItem>.SuccessResponse(taskData, "Task Updated Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskItem>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskItem>> UpdateTaskAsync(TaskItem task)
        {
            try
            {
                var taskData = await _queryRepository.GetByIdAsync(task.Id);
                if (taskData != null)
                {
                    taskData.Title = task.Title;
                    taskData.Description = task.Description;
                    taskData.Status = task.Status;
                    taskData.DueDate = task.DueDate;
                    taskData.AssignedUserId = task.AssignedUserId;
                    task.UpdatedBy = _currentUserService?.UserId;
                    task.UpdatedAt = DateTime.UtcNow;

                    await _commandRepository.Update(taskData);
                    return ApiResponse<TaskItem>.SuccessResponse(task, "Task Updated Successfully");
                }
                else
                {
                    return ApiResponse<TaskItem>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskItem>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskItem>> UpdateTaskStatusAsync(Guid id, string status)
        {
            try
            {
                var taskData = await _queryRepository.GetByIdAsync(id);
                if (taskData != null)
                {
                    var role = _currentUserService?.UserRole;
                    if (role == "Admin" || (role == "User" && _currentUserService?.UserId == taskData.AssignedUserId))
                    {
                        taskData.Status = status;
                        taskData.UpdatedBy = _currentUserService?.UserId;
                        taskData.UpdatedAt = DateTime.UtcNow;

                        await _commandRepository.Update(taskData);
                        return ApiResponse<TaskItem>.SuccessResponse(taskData, "Task Updated Successfully");

                    }
                    else
                    {
                        return ApiResponse<TaskItem>.FailResponse("Unauthorize to view task", ErrorCode.Unauthorize);
                    }
                }
                else
                {
                    return ApiResponse<TaskItem>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskItem>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<Guid>> DeleteTaskAsync(Guid id)
        {
            try
            {
                var taskData = await _queryRepository.GetByIdAsync(id);
                if (taskData != null)
                {
                    await _commandRepository.DeleteByIdAsync(id);
                    return ApiResponse<Guid>.SuccessResponse(id, "Task Deleted Successfully");
                }
                else
                {
                    return ApiResponse<Guid>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}