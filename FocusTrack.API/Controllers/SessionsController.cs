using FocusTrack.Application.Sessions.Commands.CreateSession;
using FocusTrack.Application.Sessions.Commands.DeleteSession;
using FocusTrack.Application.Sessions.Commands.UpdateSession;
using FocusTrack.Application.Sessions.Queries.GetSessionById;
using FocusTrack.Application.Sessions.Queries.GetSessionsPaged;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [AllowAnonymous]
    public class SessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetSessionByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetSessionsPagedQuery(page, pageSize));
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateSessionCommand cmd)
        {
            if (cmd.Id != id)
                return BadRequest("ID mismatch.");

            await _mediator.Send(cmd);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteSessionCommand(id));
            return NoContent();
        }
    }
}
