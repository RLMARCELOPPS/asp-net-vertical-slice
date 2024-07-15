using ecommerse_api.Features.CartItems.Dto;
using MediatR;

namespace ecommerse_api.Features.CartItems.Queries.GetCartItemsDetailed
{
    public class GetCartItemsDetailedQuery : IRequest<IEnumerable<CartItemOrderDto>>
    {

    }
}
