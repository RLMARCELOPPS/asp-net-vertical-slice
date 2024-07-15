using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ecommerse_api.Common.Middleware;

public class UserIdHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the endpoint has an Authorize attribute with the specific policy
        var hasUserIdHeaderPolicy = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any(a => a.Policy == "UserIdHeaderRequired");

        if (hasUserIdHeaderPolicy)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-user-id",
                In = ParameterLocation.Header,
                Description = "User ID",
                Required = true // Assuming it's required for endpoints with this policy
            });
        }
    }
}