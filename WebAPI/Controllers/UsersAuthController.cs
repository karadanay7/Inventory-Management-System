using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request.Identity;
using Application.Interface.Identity;
using Application.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UserAuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Single Login method
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequestDTO loginModel)
        {
            // Ensure login logic is implemented here
            var result = await _accountService.LoginAsync(loginModel); // Assuming you have a LoginAsync method in IAccountService

            if (result.Flag)
            {
                return Ok(result); // Return success response
            }
            else
            {
                return Unauthorized(result.Message); // Return error response
            }
           
        }
    }
}
