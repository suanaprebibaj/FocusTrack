using FocusTrack.Application.Admin.Commands.ChangeUserStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FocusTrack.API.Controllers.Admin
{
    [ApiController]
    [Route("admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminUsersController(IMediator mediator) => _mediator = mediator;

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, ChangeUserStatusCommand cmd)
        {
            if (id != cmd.UserId)
                return BadRequest("ID mismatch.");

            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
