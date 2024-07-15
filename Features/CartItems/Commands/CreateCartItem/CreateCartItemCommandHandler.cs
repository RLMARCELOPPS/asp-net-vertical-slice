using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Features.Users.Dto;
using ecommerse_api.Shared;
using FluentValidation;
using MediatR;

namespace ecommerse_api.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand, IApiResponse>
    {

        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<CreateCartItemCommand> _validator;
        private readonly IMapper _mapper;

        public CreateCartItemCommandHandler
        (
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository,
            IValidator<CreateCartItemCommand> validator,
            IMapper mapper
        )
        {
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<IApiResponse> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
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
                var orderId = await _orderRepository.GetOrCreatePendingOrderIdAync(request.UserId);
                var cartItem = _mapper.Map<CartItem>(request);
                cartItem.OrderId = orderId;
                await _cartItemRepository.CreateAysnc(cartItem);
                var cart = _mapper.Map<CartItemDto>(cartItem);

                return new ApiResponse<CartItemDto>
                {
                    Data = cart,
                    Message = "Cart Item Successfully Created.",
                    ResponseCode = ResponseCodes.Created
                };

            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while creating Cart Item.",
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = new List<string> { ex.Message }
                };
            }


        }
    }
}
