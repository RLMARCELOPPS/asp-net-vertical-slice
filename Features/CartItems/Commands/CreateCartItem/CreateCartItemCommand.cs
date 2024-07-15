using ecommerse_api.Common.Responses;
using ecommerse_api.Shared;
using MediatR;
namespace ecommerse_api.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommand : IRequest<IApiResponse>
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid UserId { get; set; }
    }
}
