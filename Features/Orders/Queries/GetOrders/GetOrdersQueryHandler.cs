using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Repository;
using MediatR;


namespace ecommerse_api.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IApiResponse>
    {

        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        public GetOrdersQueryHandler(IOrderRepository orderRepository, ICartItemRepository cartItemRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task<IApiResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrderAsync();

                foreach (var order in orders)
                {
                    var cartItems =  await _cartItemRepository.GetAllByOrderAsync(order.Id);
                    order.CartItems = cartItems.ToList();
                }
                var orderDto = _mapper.Map<List<OrderDto>>(orders);

                return new ApiResponse<List<OrderDto>>
                {
                    Data = orderDto,
                    Message = "Order fetched successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while fetching Orders",
                    ErrorCode = "SERVER_ERROR",
                    Details = new List<string> { ex.Message }
                };
            }
        }

    }
}
