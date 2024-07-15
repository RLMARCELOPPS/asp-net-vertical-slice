using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Features.Users.Dto;
using ecommerse_api.Features.Users.Models;
using ecommerse_api.Features.Users.Repository;
using MediatR;

namespace ecommerse_api.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IApiResponse>
    {
        private  readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public GetUserByIdQueryHandler(IUserRepository user, IMapper mapper, IOrderRepository orderRepository, ICartItemRepository cartItemRepository)
        {
            _userRepository = user;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<IApiResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userExist = await _userRepository.UserExistAsync(request.Id);

                if (!userExist)
                {
                    return new ApiErrorResponse
                    {
                        Message = "User not found.",
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var user = await _userRepository.GetByIdAsync(request.Id);
                var orders = await _orderRepository.GetOrdersByUserAsync(request.Id);

                foreach (var order in orders)
                {
                    var cartItems = await _cartItemRepository.GetAllByOrderAsync(order.Id);
                    order.CartItems = cartItems.ToList();
                }

                user.Orders = orders;

                UserDto userDto = _mapper.Map<UserDto>(user);

                return new ApiResponse<UserDto>
                {
                    Data = userDto,
                    Message = "User fetch successfully.",
                    ResponseCode = ResponseCodes.Ok

                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while fetching user",
                    ErrorCode = ErrorCodes.NotFound,
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = new List<string> { ex.Message }

                };
            }
        }
    }
}
