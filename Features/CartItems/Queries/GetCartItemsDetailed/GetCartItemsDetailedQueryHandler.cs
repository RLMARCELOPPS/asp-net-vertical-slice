using AutoMapper;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Features.Users.Dto;
using ecommerse_api.Features.Users.Repository;
using MediatR;

namespace ecommerse_api.Features.CartItems.Queries.GetCartItemsDetailed
{
    public class GetCartItemsDetailedQueryHandler : IRequestHandler<GetCartItemsDetailedQuery, IEnumerable<CartItemOrderDto>>
    {

        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCartItemsDetailedQueryHandler
        (
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<CartItemOrderDto>> Handle(GetCartItemsDetailedQuery request, CancellationToken cancellationToken )
        {
            var pendingOrders = await _orderRepository.GetAllStatusOrderAsync(OrderStatus.Pending.ToString());
            var pendingOrdersDto = new List<CartItemOrderDto>();

            foreach (var item in pendingOrdersDto)
            {
                var user = await _userRepository.GetByIdAsync(item.UserId);
                var cartItems = await _cartItemRepository.GetAllByOrderAsync(item.Id);

                var orderDto = _mapper.Map<CartItemOrderDto>(item);
                orderDto.UserInfo = _mapper.Map<UserDto>(user);
                orderDto.CartItems = _mapper.Map<List<CartItemDto>>(cartItems);

                pendingOrdersDto.Add(orderDto);
            }

            return pendingOrdersDto;
        }

    }
}
