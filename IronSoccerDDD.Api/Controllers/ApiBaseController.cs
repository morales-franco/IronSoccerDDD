using IronSoccerDDD.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IronSoccerDDD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult BadRequest(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected new IActionResult CreatedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
        {
            return base.CreatedAtAction(actionName, routeValues, Envelope.Ok(value));
        }
    }
}