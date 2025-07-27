using Moq;
using FluentAssertions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Users.Services;
using TaskManagementSystem.Users.Repositories;
using TaskManagementSystem.Shared.Services;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Models;
using TaskManagementSystem.Shared.Enums;
using TaskManagementSystem.Shared.Models;
using Microsoft.Extensions.Logging;

namespace TaskManagementSystem.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserQueryRepository> _queryRepoMock;
    private readonly Mock<IUserCommandRepository> _commandRepoMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly IMapper _mapper;
    private readonly UserService _userService;
    public UserServiceTests()
    {
        _queryRepoMock = new Mock<IUserQueryRepository>();
        _commandRepoMock = new Mock<IUserCommandRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserDto, User>();
            cfg.CreateMap<User, UserDto>();
        }, loggerFactory);

        _mapper = config.CreateMapper();

        _userService = new UserService(
            _queryRepoMock.Object,
            _commandRepoMock.Object,
            _passwordHasherMock.Object,
            _currentUserServiceMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnSuccess_WhenValidInput()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "John",
            Email = "john@example.com",
            Password = "Pass123!",
            Role = UserRoles.Admin
        };

        _queryRepoMock.Setup(q => q.IsEmailDuplicatedAsync(It.IsAny<Guid>(), createUserDto.Email))
                      .ReturnsAsync(false);

        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<User>(), createUserDto.Password))
                           .Returns("hashed-password");

        _commandRepoMock.Setup(c => c.AddAsync(It.IsAny<User>()))
                        .Returns(Task.CompletedTask);

        var result = await _userService.CreateUserAsync(createUserDto);

        result.ErrorCode.Should().Be(ErrorCode.None);
        result.Data.Should().NotBeNull();
        result.Data.Email.Should().Be(createUserDto.Email);
        result.Message.Should().Be("User Added Successfully");
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnValidationError_WhenInvalidRole()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Jane",
            Email = "jane@example.com",
            Password = "secret",
            Role = "InvalidRole"
        };

        var result = await _userService.CreateUserAsync(createUserDto);

        result.ErrorCode.Should().Be(ErrorCode.ValidationError);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnError_WhenEmailDuplicated()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Mark",
            Email = "mark@example.com",
            Password = "secret",
            Role = UserRoles.User
        };

        _queryRepoMock.Setup(q => q.IsEmailDuplicatedAsync(It.IsAny<Guid>(), createUserDto.Email))
                      .ReturnsAsync(true);

        var result = await _userService.CreateUserAsync(createUserDto);

        result.ErrorCode.Should().Be(ErrorCode.UserEmailDuplicated);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCatchException_AndReturnFailResponse()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Error",
            Email = "error@example.com",
            Password = "pass",
            Role = UserRoles.User
        };

        _queryRepoMock.Setup(q => q.IsEmailDuplicatedAsync(It.IsAny<Guid>(), createUserDto.Email))
                      .ThrowsAsync(new Exception("Database error"));

        var result = await _userService.CreateUserAsync(createUserDto);

        result.ErrorCode.Should().Be(ErrorCode.GeneralApiError);
    }
}

