using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.Orders.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<IApiResponse>
    {
        public Guid OrderId { get; set; }
    }
}
