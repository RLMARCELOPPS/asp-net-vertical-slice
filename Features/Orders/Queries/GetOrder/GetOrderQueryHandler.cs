using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Repository;
using MediatR;

namespace ecommerse_api.Features.Orders.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, IApiResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICartItemRepository _caartItemRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper, ICartItemRepository caartItemRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _caartItemRepository = caartItemRepository;
        }

        public async Task<IApiResponse> Handle(GetOrderQuery request, CancellationToken cancellation)
        {
            try
            {
                var orderData = await _orderRepository.GetByIdAsync(request.OrderId);
                var cartItemData = await _caartItemRepository.GetAllByOrderAsync(orderData.Id);

                orderData.CartItems = cartItemData.ToList();

                var order = _mapper.Map<OrderDto>(orderData);


                return new ApiResponse<OrderDto>
                {
                    Data = order,
                    Message = "Order fetched successfully"
                };

            }
            catch(Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "An error occurred while fetching the user.",
                    ErrorCode = "SERVER_ERROR",
                    Details = new List<string> { ex.Message },
                };
            }
        }
    }
}
