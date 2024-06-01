using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("")]
    public class DefaultController : ControllerBase
    {
        [HttpGet("testSuccess")]
        public IActionResult TestSuccess()
        {
            return Ok("success");
        }

        [HttpGet("testNotFound")]
        public IActionResult TestNotFound()
        {
            return NotFound("not found");
        }

        [HttpGet("testError")]
        public IActionResult TestError()
        {
            return StatusCode(500, "error");
        }
        
        [HttpGet("testRole")]
        [Authorize(Roles = "ROLE_USER,ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult TestRole()
        {
            var currentUser = User?.Identity?.Name;
            var roles = User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new 
            { 
                Username = currentUser,
                Roles = roles
            });
        }
        
        [HttpGet("checkAdminRole")]
        [Authorize]
        public IActionResult CheckAdminRole()
        {
            if (User.IsInRole("ROLE_ADMIN"))
            {
                return Ok("User is an admin.");
            }
            else
            {
                return Ok("User is not an admin.");
            }
        }

        [HttpGet("checkOwnerRole")]
        [Authorize]
        public IActionResult CheckOwnerRole()
        {
            if (User.IsInRole("ROLE_OWNER"))
            {
                return Ok("User is an owner.");
            }

            return Ok("User is not an owner.");
        }
    }
}
