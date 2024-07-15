using ecommerse_api.Features.Users.Command.CreateUser;
using ecommerse_api.Features.Users.Repository;
using FluentValidation;

namespace ecommerse_api.Features.Users.Validator
{
    public class UserCreateValidator : AbstractValidator<CreateUserCommand>
    {

        private readonly IUserRepository _userRepository;
        public UserCreateValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid")
                .MustAsync(BeUniqueEmail)
                .WithMessage("Email already exists");

            RuleFor(x => x.PhoneNumber).NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^\+?[0-9]\d{1,14}$")
                .WithMessage("Phone number is not valid");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 character");

        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.IsEmailUniqueAsync(email);
        }
    }
}
