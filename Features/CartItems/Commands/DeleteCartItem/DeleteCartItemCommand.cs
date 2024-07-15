using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.CartItems.Commands.DeleteCartItem
{
    public class DeleteCartItemCommand : IRequest<IApiResponse>
    {
        public Guid Id { get; set; }
    }
}
