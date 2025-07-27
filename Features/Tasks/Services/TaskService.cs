using System.Text.Json;
using AutoMapper;
using TaskManagementSystem.Shared.Enums;
using TaskManagementSystem.Shared.Models;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Shared.Services;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Tasks.Repositories;
using TaskManagementSystem.Users.Services;

namespace TaskManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskQueryRepository _queryRepository;
        private readonly ITaskCommandRepository _commandRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TaskService(ITaskQueryRepository queryRepository, ITaskCommandRepository commandRepository, ICurrentUserService currentUserService, IMapper mapper, IUserService userService)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _currentUserService = currentUserService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<TaskDto>>> GetAllTasksAsync()
        {
            try
            {
                var tasksData = await _queryRepository.GetAllAsync();
                return ApiResponse<IEnumerable<TaskDto>>.SuccessResponse(_mapper.Map<IEnumerable<TaskDto>>(tasksData), "Tasks Retrieved Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<TaskDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskDetailsDto?>> GetTaskByIdAsync(Guid id)
        {
            try
            {
                var taskData = await _queryRepository.GetByIdAsync(id);

                if (taskData != null)
                {
                    var role = _currentUserService?.UserRole;
                    if (role == UserRoles.Admin || (role == UserRoles.User && _currentUserService?.UserId == taskData.AssignedUserId))
                    {
                        return ApiResponse<TaskDetailsDto?>.SuccessResponse(_mapper.Map<TaskDetailsDto>(taskData), "Task Retrieved Successfully");
                    }
                    else
                    {
                        return ApiResponse<TaskDetailsDto?>.FailResponse("Unauthorize to view task", ErrorCode.Unauthorize);
                    }
                }
                else
                {
                    return ApiResponse<TaskDetailsDto?>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDetailsDto?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto task)
        {
            try
            {
                var isUserExist = await _userService.IsUserExistAsync(task.AssignedUserId);
                if (isUserExist.ErrorCode != ErrorCode.None)
                {
                    return ApiResponse<TaskDto>.FailResponse(isUserExist.Message!, isUserExist.ErrorCode);
                }

                var taskData = _mapper.Map<TaskItem>(task);
                taskData.Id = Guid.NewGuid();
                taskData.Status = TaskItemStatus.New;
                taskData.CreatedBy = this._currentUserService?.UserId;

                await _commandRepository.AddAsync(taskData);
                return ApiResponse<TaskDto>.SuccessResponse(_mapper.Map<TaskDto>(taskData), "Task Updated Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDto>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskDto>> UpdateTaskAsync(UpdateTaskDto task)
        {
            try
            {
                if (task.Status != null && !TaskItemStatus.All.Contains(task.Status))
                {
                    return ApiResponse<TaskDto>.FailResponse("Invalid Status value, must be one of " + JsonSerializer.Serialize(TaskItemStatus.All), ErrorCode.ValidationError);
                }
                var taskData = await _queryRepository.GetByIdAsync(task.Id);
                if (taskData != null)
                {

                    if (task.AssignedUserId != Guid.Empty)
                    {
                        var isUserExist = await _userService.IsUserExistAsync(task.AssignedUserId);
                        if (isUserExist.ErrorCode != ErrorCode.None)
                        {
                            return ApiResponse<TaskDto>.FailResponse(isUserExist.Message!, isUserExist.ErrorCode);
                        }

                        taskData.AssignedUserId = task.AssignedUserId;
                    }

                    if (task.Title != null) taskData.Title = task.Title;
                    if (task.Description != null) taskData.Description = task.Description;
                    if (task.Status != null) taskData.Status = task.Status;
                    if (task.DueDate != null) taskData.DueDate = task.DueDate;

                    taskData.UpdatedBy = _currentUserService?.UserId;
                    taskData.UpdatedAt = DateTime.UtcNow;

                    await _commandRepository.Update(taskData);
                    return ApiResponse<TaskDto>.SuccessResponse(_mapper.Map<TaskDto>(taskData), "Task Updated Successfully");
                }
                else
                {
                    return ApiResponse<TaskDto>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDto>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskDto>> UpdateTaskStatusAsync(Guid id, string status)
        {
            if (!TaskItemStatus.All.Contains(status))
            {
                return ApiResponse<TaskDto>.FailResponse("Invalid Status value, must be one of " + JsonSerializer.Serialize(TaskItemStatus.All), ErrorCode.ValidationError);
            }

            try
            {
                var taskData = await _queryRepository.GetByIdAsync(id);
                if (taskData != null)
                {
                    var role = _currentUserService?.UserRole;
                    if (role == UserRoles.Admin || (role == UserRoles.User && _currentUserService?.UserId == taskData.AssignedUserId))
                    {
                        taskData.Status = status;
                        taskData.UpdatedBy = _currentUserService?.UserId;
                        taskData.UpdatedAt = DateTime.UtcNow;

                        await _commandRepository.Update(taskData);
                        return ApiResponse<TaskDto>.SuccessResponse(_mapper.Map<TaskDto>(taskData), "Task Updated Successfully");

                    }
                    else
                    {
                        return ApiResponse<TaskDto>.FailResponse("Unauthorize to view task", ErrorCode.Unauthorize);
                    }
                }
                else
                {
                    return ApiResponse<TaskDto>.FailResponse("Task not found", ErrorCode.TaskNotFound);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDto>.FailResponse($"Error: {ex.Message}");
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