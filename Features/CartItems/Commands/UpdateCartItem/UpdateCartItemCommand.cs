using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.CartItems.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<IApiResponse>
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
