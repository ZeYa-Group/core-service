using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ApiBaseController
    {
        public AdministratorController()
        {

        }

        [HttpGet(Constants.Requests.Administrator.GetDocumentVerificationList)]
        public async Task<IActionResult> GetDocumentRequestListAsync()
        {
            return Ok();
        }
    }
}
