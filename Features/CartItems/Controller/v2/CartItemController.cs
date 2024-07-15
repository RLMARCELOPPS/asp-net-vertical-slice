using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Expr;

namespace ecommerse_api.Features.CartItems.Controller.v2
{
    [Route("api/v{version:apiVersion}/test")]
    [ApiVersion("2")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        [HttpGet]
        
        public IActionResult TestApiV1()
        {
            return Ok("Test api V2 Controller ");
        }

    }
}
