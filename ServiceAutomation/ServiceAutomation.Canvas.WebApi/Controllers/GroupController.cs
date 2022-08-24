using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public GroupController()
        {

        }

        [HttpGet(Constants.Requests.Group.GetTree)]
        public async Task<IEnumerable<int>> GetTree(Guid userId)
        {
            return null;
        }
    }
}
