using Xunit;
using Moq;
using TaskManagementSystem.Users.Controllers;
using TaskManagementSystem.Users.DTOs;
using TaskManagementSystem.Users.Services;
using TaskManagementSystem.Shared.Responses;
using TaskManagementSystem.Shared.Models;
using TaskManagementSystem.Shared.Enums;

namespace TaskManagementSystem.Tests;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Role = UserRoles.Admin,
            Password = "Admin123!"
        };

        var expectedUser = new UserDto
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane@example.com",
            Role = UserRoles.Admin
        };

        var expectedResponse = new ApiResponse<UserDto>
        {
            ErrorCode = ErrorCode.None,
            Data = expectedUser
        };

        _mockUserService
            .Setup(s => s.CreateUserAsync(createUserDto))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.CreateUser(createUserDto);

        Assert.Equal(ErrorCode.None, result.ErrorCode);
        Assert.NotNull(result.Data);
        Assert.Equal(expectedUser.Email, result.Data.Email);
        _mockUserService.Verify(s => s.CreateUserAsync(createUserDto), Times.Once);
    }
}

