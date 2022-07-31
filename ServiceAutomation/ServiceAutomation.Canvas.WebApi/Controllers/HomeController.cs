using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Constants;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [AllowAnonymous]
        [HttpGet(Requests.Home.GetAction)]
        public IActionResult GetAction()
        {
            return Ok("Hello from WebApi");
        }

        [Authorize]
        [HttpGet(Requests.Home.GetAuthAction)]
        public IActionResult GetAuthAction()
        {
            var user = User.Identity.Name;
            return Ok($"Hello {user}");
        }
    }
}
