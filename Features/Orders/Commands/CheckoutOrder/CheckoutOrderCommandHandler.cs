using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Orders.Repository;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.CheckoutOrder
{
   
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, IApiResponse>
    {
        private readonly IOrderRepository  _orderRepository;
        private readonly ICartItemRepository  _cartItemRepository;
        private readonly IMapper _mapper;

        public CheckoutOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ICartItemRepository cartItemRepository) 
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
        }
        
        public async Task<IApiResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new ApiErrorResponse()
                {
                    Message = "No request provided",
                    ErrorCode = ErrorCodes.NoRequestProvided
                };

            }

            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);

                if (order == null)
                {
                    return new ApiErrorResponse
                    {
                        Message = "Order not Found",
                        ErrorCode = ErrorCodes.EmptyRecord
                    };

                }

                var cartItems = await _cartItemRepository.GetAllByOrderAsync(order.Id);
                if (!cartItems.Any())
                {
                    return new ApiErrorResponse()
                    {
                        Message = "Cart items is Empty!",
                        ErrorCode = ErrorCodes.NotFound,
                        ResponseCode = ResponseCodes.BadRequest
                    };
                }
                order.CartItems = cartItems.ToList();
                order.Status = OrderStatus.Processed;
                await _orderRepository.UpdateAsync(order);

                return new ApiResponse<OrderDto>
                {
                    Data = _mapper.Map<OrderDto>(order),
                    Message = "Order processed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while creating order.",
                    ErrorCode = ErrorCodes.ServerError,
                    Details = new List<string> { ex.Message }   
                };
            }
        }
    }
}
