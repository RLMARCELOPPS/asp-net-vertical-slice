using AutoMapper;
using ecommerse_api.Features.Orders.Command.CheckoutOrder;
using ecommerse_api.Features.Orders.Command.DeleteOrder;
using ecommerse_api.Features.Orders.Command.UpdateOrder;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Queries.GetOrder;
using ecommerse_api.Features.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ecommerse_api.Features.Orders.Controller
{
    [Route("api/v{version:apiVersion}/api/orders")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class OrderController : ControllerBase 
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public OrderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOrdersQuery());
            return Ok(result);
        }

        [HttpGet("{orderId:Guid}")]
        public async Task<IActionResult> GetById(Guid orderId)
        {
            var result = await _mediator.Send(new GetOrderQuery { OrderId = orderId });
            return Ok(result);
        }

        [HttpPut("checkout")]

        public async Task<IActionResult> Chekout(Guid id)
        {
            var result = await _mediator.Send( new CheckoutOrderCommand { OrderId = id });

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateO(Guid orderId, [FromBody] UpdateOrderCommand request)
        {

            var command = request;
            command.OrderId = orderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]

        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteOrderCommand { Id = id });

            return NoContent();
        }
    }
}
