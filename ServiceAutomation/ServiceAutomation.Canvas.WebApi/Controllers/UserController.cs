using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Constants;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthProvider authProvider;
        public UserController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
        {
            var user = await authProvider.Authenticate(requestModel);

            if(user != null)
            {
                var token = authProvider.Generate(user);

                return Ok(token);
            }

            return NotFound("User not found.");
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Logout)]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Register)]
        public async Task<IActionResult> Register(RegisterRequestModel requestModel)
        {
            var response = await authProvider.Register(requestModel);
            return Ok(response);
        }

    }
}
