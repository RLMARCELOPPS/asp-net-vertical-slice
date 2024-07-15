using ecommerse_api.Features.Users.Command.CreateUser;
using ecommerse_api.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ecommerse_api.Features.Users.Controller
{

    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator) { 
            
            _mediator = mediator;
            
        }

        [HttpGet("{userId:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = userId });
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
