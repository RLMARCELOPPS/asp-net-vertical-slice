using AutoMapper;
using ecommerse_api.Features.CartItems.Commands.CreateCartItem;
using ecommerse_api.Features.CartItems.Commands.DeleteCartItem;
using ecommerse_api.Features.CartItems.Commands.UpdateCartItem;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Queries.GetCartItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ecommerse_api.Features.CartItems.Controller
{
    [Route("api/v{version:apiVersion}/cart-items")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CartItemController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CartItemController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCartItemsQuery());
            //Log.Information("Cart item List = {@result}", result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCartItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid Id, [FromBody] UpdateCartItemDto request)
        {
            var command = _mapper.Map<UpdateCartItemCommand>(request);
            command.Id = Id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteCartItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


    }
}
