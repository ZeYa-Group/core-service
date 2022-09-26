using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        public ProgressController()
        {

        }

        //public async Task<IActionResult> GetUserProgress(Guid userId)
        //{
        //    return Ok();
        //}
    }
}
