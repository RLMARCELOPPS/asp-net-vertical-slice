using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.Users.Dto;
using ecommerse_api.Features.Users.Models;
using ecommerse_api.Features.Users.Repository;
using FluentValidation;
using MediatR;

namespace ecommerse_api.Features.Users.Command.CreateUser
{

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IApiResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserCommand> validator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<IApiResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) {
                var errorMessages = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return new ApiErrorResponse
                {
                    Message = "Validation Failed",
                    ErrorCode = ErrorCodes.ValidationError,
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = errorMessages

                };
            }

            try
            {
                var user = _mapper.Map<User>(request);
                await _userRepository.CreateAsync(user, cancellationToken);

                var userData = _mapper.Map<UserDto>(user);

                return new ApiResponse<UserDto>
                {
                    Data = userData,
                    Message = "User Successfully Created.",
                    ResponseCode = ResponseCodes.Created
                };



            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                     Message = "Error while creating User.",
                     ResponseCode = ResponseCodes.BadRequest,
                     Details = new List<string> { ex.Message }
                };
            }
            
        }
    }
}
