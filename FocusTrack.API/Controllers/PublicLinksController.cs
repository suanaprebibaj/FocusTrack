using FocusTrack.Application.Sessions.Commands.CreatePublicLink;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FocusTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    [AllowAnonymous]
    public class PublicLinksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublicLinksController(IMediator mediator) => _mediator = mediator;

        [HttpPost("{sessionId:guid}")]
        public async Task<IActionResult> CreatePublicLink(Guid sessionId)
        {
            var url = await _mediator.Send(new CreatePublicLinkCommand(sessionId));
            return Ok(new { url });
        }
    }
}
