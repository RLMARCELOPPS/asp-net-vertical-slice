using ecommerse_api.Features.Users.Repository;
using Microsoft.AspNetCore.Authorization;


namespace VSAEcommerce.Api.Common.Middleware;

public class UserIdRequirement : IAuthorizationRequirement 
{
}

public class UserIdHeaderAuthorizationHandler : AuthorizationHandler<UserIdRequirement>
{
    private readonly IUserRepository _userRepository;

    public UserIdHeaderAuthorizationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdRequirement requirement)
    {
        var httpContext = (context.Resource as Microsoft.AspNetCore.Http.DefaultHttpContext);

        if (httpContext != null &&
            httpContext.Request.Headers.TryGetValue("x-user-id", out var userIdString) &&
            Guid.TryParse(userIdString, out var userId) &&
            await _userRepository.UserExistAsync(userId))
        {
            // User is valid
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, "Invalid user ID."));
        }
    }
}