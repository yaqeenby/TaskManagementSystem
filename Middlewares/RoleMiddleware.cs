using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Services;
using TaskManagementSystem.Shared.Enums;

namespace TaskManagementSystem.Middlewares
{
    public class RoleBasedAccessMiddleware : IMiddleware
    {
        public RoleBasedAccessMiddleware()
        {
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

            await next(context);
        }
    }
}
