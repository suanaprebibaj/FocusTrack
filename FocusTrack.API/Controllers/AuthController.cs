using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FocusTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok(new { message = "Logged out successfully" });
        }
    }

}
