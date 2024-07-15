using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<IApiResponse>
    {
        public Guid OrderId { get; set; }
    }
}
