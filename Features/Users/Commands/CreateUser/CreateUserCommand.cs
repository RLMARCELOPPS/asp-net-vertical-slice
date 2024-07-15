using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.Users.Command.CreateUser
{
    public class CreateUserCommand : IRequest<IApiResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
