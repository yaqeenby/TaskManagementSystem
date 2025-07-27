using AutoMapper;
using TaskManagementSystem.Tasks.DTOs;
using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;

namespace TaskManagementSystem.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, CreateUserDto>();
            CreateMap<User, UpdateUserDto>();
            CreateMap<User, UserDetailsDto>();

            CreateMap<CreateUserDto, User>();
            CreateMap<UserDetailsDto, User>();


            CreateMap<TaskItem, TaskDto>();
            CreateMap<TaskItem, CreateTaskDto>();
            CreateMap<TaskItem, UpdateTaskDto>();
            CreateMap<TaskItem, TaskDetailsDto>();

            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<TaskDetailsDto, TaskItem>();
        }
    }

}