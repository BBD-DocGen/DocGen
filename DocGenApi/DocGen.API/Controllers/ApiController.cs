using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DesignDocGen.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        [HttpGet("registeredUser")]
        [Authorize(Policy = "RegisteredUser")]
        public IActionResult Registered()
        {
            {
                return Ok(new { Message = "Hello you are registered and are allowed to enter" });
            }
        }

        [HttpGet("private")]
        [Authorize(Policy = "RegisteredUser")]
        public IActionResult Private()
        {
            try{
                return Ok(new
                {
                    Message = "Hello from a private endpoint! You need to be authenticated to see this."
                });
            }
            catch(Exception ex){
                return Message = ex;
            }
        }

        // View Claims
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            return Ok(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }
    }
}