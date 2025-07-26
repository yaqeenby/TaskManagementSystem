namespace TaskManagementSystem.Shared.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserEmail { get; }
        string? UserName { get; }
        string? UserRole { get; }
    }
}