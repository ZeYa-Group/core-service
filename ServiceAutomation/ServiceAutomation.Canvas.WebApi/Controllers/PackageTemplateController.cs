﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTemplateController : ControllerBase
    {
        public PackageTemplateController()
        {

        }

        [HttpGet(Constants.Requests.PackageTemplate.GetPackTemplates)]
        public async Task<int> GetPackTemplates()
        {
            return 0;
        }

        [HttpPost(Constants.Requests.PackageTemplate.BuyPackTemplate)]
        public async Task<int> BuyPackTemplate()
        {
            return 0;
        }
    }
}
