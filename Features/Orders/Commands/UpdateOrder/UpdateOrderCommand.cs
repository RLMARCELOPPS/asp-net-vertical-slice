using ecommerse_api.Common.Responses;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Shared;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<IApiResponse>
    {
        public OrderStatus Status { get; set; }
        internal Guid OrderId { get; set; }

    }
}
