using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Services;
using TaskManagementSystem.Shared.Enums;

namespace TaskManagementSystem.Middlewares
{
    public class RoleBasedAccessMiddleware : IMiddleware
    {
        private readonly ITaskService _taskService;

        public RoleBasedAccessMiddleware(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpoint = context.GetEndpoint();
            var authorizeAttr = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (authorizeAttr == null)
            {
                await next(context);
                return;
            }

            var user = context.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            if (string.IsNullOrEmpty(authorizeAttr.Roles))
            {
                await next(context);
                return;
            }


            var requiredRoles = authorizeAttr.Roles.Split(',');
            var userHasRole = requiredRoles.Any(role => context.User.IsInRole(role.Trim()));

            if (!userHasRole)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: You don't have access to this resource.");
                return;
            }


            // var path = context.Request.Path.Value?.ToLower();

            // if (path != null && path.StartsWith("/api/tasks") && context.Request.Method == "GET")
            // {
            //     var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //     var segments = path.Split('/');

            //     if (segments.Length >= 4 && Guid.TryParse(segments[3], out Guid taskId))
            //     {
            //         var task = await _taskService.GetTaskByIdAsync(taskId);
            //         if (task.ErrorCode != ErrorCode.None || task.Data.AssignedUserId.ToString() != userId)
            //         {
            //             context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //             await context.Response.WriteAsync("Forbidden: You cannot access this task.");
            //             return;
            //         }
            //     }
            // }

            await next(context);
        }
    }
}
