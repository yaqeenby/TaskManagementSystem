using TaskManagementSystem.Users.Models;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            if (!context.Users.Any())
            {
                // Create users
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "admin",
                    Email = "admin@example.com",
                    Role = "Admin",
                };

                admin.Password = passwordHasher.HashPassword(admin, "Admin123!");

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "user",
                    Email = "user@example.com",
                    Role = "User",
                };
                user.Password = passwordHasher.HashPassword(user, "User123!");


                context.Users.AddRange(admin, user);

                // Create tasks
                var tasks = new List<TaskItem>
                {
                    new() { Id = Guid.NewGuid(), Title = "Task 1", Description = "First task", AssignedUserId = user.Id, Status = TaskItemStatus.New, DueDate = DateTime.Now },
                    new() { Id = Guid.NewGuid(), Title = "Task 2", Description = "Second task", AssignedUserId = user.Id, Status = TaskItemStatus.New, DueDate = DateTime.Now },
                    new() { Id = Guid.NewGuid(), Title = "Task 3", Description = "Third task", AssignedUserId = admin.Id, Status = TaskItemStatus.New, DueDate = DateTime.Now },
                };

                context.Tasks.AddRange(tasks);

                context.SaveChanges();
            }
        }
    }
}