using FocusTrack.Application.Admin.Queries.FilterSessions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FocusTrack.API.Controllers.Admin
{
    [ApiController]
    [Route("admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminSessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminSessionsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] FilterSessionsQuery query)
        {
            var result = await _mediator.Send(query);

            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }
    }
}
