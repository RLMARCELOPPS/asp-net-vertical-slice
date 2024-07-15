using AutoMapper;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Features.Users.Repository;
using MediatR;

namespace ecommerse_api.Features.CartItems.Queries.GetCartItems
{
    public class GetICartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, IEnumerable<CartItemDto>>
    {

        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public GetICartItemsQueryHandler
        (
            ICartItemRepository cartItemRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>>Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var pendingCart = await _cartItemRepository.GetAllPendingAsync();
            return _mapper.Map<IEnumerable<CartItemDto>>(pendingCart);
        }
    }
}
