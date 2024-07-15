
//For global authorization
using ecommerse_api.Features.Users.Repository;

namespace ecommerse_api.Common.Middleware

{
    public class UserIdHeaderAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _relm;
        public UserIdHeaderAuthenticationMiddleware(RequestDelegate next, string relm)
        {
            _next = next;
            _relm = relm;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository service)
        {
            // List of endpoints that don't require the x-user-id header
            var userPathBase = new PathString("/api/user");

            // Check if the request path starts with the base user path
            if (context.Request.Path.StartsWithSegments(userPathBase))
            {
                // Bypass the middleware for user paths
                await _next(context);
                return;
            }

            // Existing authentication logic
            if (!context.Request.Headers.ContainsKey("x-user-id"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User Id was not provided");
                return;
            }

            if (!context.Request.Headers.TryGetValue("x-user-id", out var userIdString) ||
                !Guid.TryParse(userIdString, out var userId) ||
                !await service.UserExistAsync(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(context.Response.ToString());
                return;
            }

            // If we get here, the user is authenticated
            await _next(context);
        }
    }

    public static class UserIdHeaderAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdHeaderAuthenticationMiddleware>();
        }
    }
}
