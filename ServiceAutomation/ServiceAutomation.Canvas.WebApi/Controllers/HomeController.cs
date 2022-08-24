using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Constants;
using System;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet(Constants.Requests.Home.GetReferral)]
        public IActionResult GetReferral(Guid id)
        {
            return Ok(Guid.NewGuid());
        }
    }
}
