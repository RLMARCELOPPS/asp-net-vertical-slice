using AutoMapper;
using ecommerse_api.Common.Responses;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Shared;
using FluentValidation;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, IApiResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<UpdateOrderCommand> _validator;
        private readonly IMapper _mapper;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IValidator<UpdateOrderCommand> validator, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<IApiResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return new ApiErrorResponse
                {
                    Message = "Validation Failed",
                    ErrorCode = ErrorCodes.ValidationError,
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = errorMessages

                };
            }

            try
            {

                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                if(order == null)
                {
                    return new ApiErrorResponse
                    {
                        Message = "Item Cart Not Found.",
                        ErrorCode = ErrorCodes.EmptyRecord,
                        ResponseCode = ResponseCodes.NotFound,
                    };
                }

                order.Status = request.Status;
                await _orderRepository.UpdateAsync(order);
                var mappedOrder = _mapper.Map<OrderDto>(order);
                return new ApiResponse<OrderDto>
                {
                    Data = mappedOrder,
                    Message = "Order Successfully Updated.",
                    ResponseCode = ResponseCodes.Created
                };

            }
            catch (Exception ex)
            {
                return new ApiErrorResponse
                {
                    Message = "Error while updating Order.",
                    ResponseCode = ResponseCodes.BadRequest,
                    Details = new List<string> { ex.Message }
                };
            }
        }
    }
}
