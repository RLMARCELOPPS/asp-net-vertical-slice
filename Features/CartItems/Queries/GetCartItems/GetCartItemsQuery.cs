using ecommerse_api.Features.CartItems.Dto;
using MediatR;

namespace ecommerse_api.Features.CartItems.Queries.GetCartItems
{

    public class GetCartItemsQuery : IRequest<IEnumerable<CartItemDto>>
    {

    }
}
