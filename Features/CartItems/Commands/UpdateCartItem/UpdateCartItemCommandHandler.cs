using AutoMapper;
using ecommerse_api.Common.Repository.Validation;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.CartItems.Repository;
using FluentValidation;
using MediatR;

namespace ecommerse_api.Features.CartItems.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, IApiResponse>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IValidator<UpdateCartItemCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateCartItemCommandHandler
        (
            ICartItemRepository cartItemRepository,
            IValidator<UpdateCartItemCommand> validor,
            IMapper mapper
        )
        {
            _cartItemRepository = cartItemRepository;
            _validator = validor;
            _mapper = mapper;
        }

        public async Task<IApiResponse> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
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
                var cartItem = await _cartItemRepository.GetByIdAsync(request.Id);

                if(cartItem == null)
                {
                    return new ApiErrorResponse
                    {
                        Message = "Item Cart Not Found.",
                        ErrorCode = ErrorCodes.EmptyRecord,
                        ResponseCode = ResponseCodes.NotFound,
                    };
                }

                cartItem.ProductName = request.ProductName;
                cartItem.Quantity = request.Quantity;
                cartItem.UnitPrice = request.UnitPrice;

                await _cartItemRepository.UpdateAynsc(cartItem);
                var cart = _mapper.Map<CartItemDto>(cartItem);

                return new ApiResponse<CartItemDto>
                {
                    Data = cart,
                    Message = "Cart Item Successfully Updated.",
                    ResponseCode = ResponseCodes.Created
                };

            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while updating Cart Item.",
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = new List<string> { ex.Message }
                };
            }

        }
    }
}
