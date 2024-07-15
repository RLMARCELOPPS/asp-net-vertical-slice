using ecommerse_api.Shared;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<Result <Guid>>
    {
        public Guid Id { get; set; }
    }
}
