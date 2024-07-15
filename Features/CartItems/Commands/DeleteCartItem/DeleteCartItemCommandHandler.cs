using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Repository;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace ecommerse_api.Features.CartItems.Commands.DeleteCartItem
{
    public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand, IApiResponse>
    {

        private readonly ICartItemRepository _cartItemRepository;
        public DeleteCartItemCommandHandler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<IApiResponse> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var cartItem = await _cartItemRepository.DeleteAysnc(request.Id);

                if (cartItem == null)
                {
                    return new ApiErrorResponse
                    {
                        Message = "Cart Item Not Found",
                        ErrorCode = ErrorCodes.NotFound,
                        ResponseCode = ResponseCodes.NotFound,

                    };
                }
                return new ApiResponse<Empty>
                {
                    Message = "Cart item Deleted succesfully",
                    ResponseCode = ResponseCodes.NoContent
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while deleting Cart Item.",
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = new List<string> { ex.Message }
                };
            }
           

        }
    }
}
