using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using ServiceAutomation.Canvas.WebApi.Interfaces.CountryService;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocumentController : ApiBaseController
    {
        private readonly IDocumentVerificationService verificationService;

        public UserDocumentController(IDocumentVerificationService verificationService)
        {
            this.verificationService = verificationService;
        }

        [HttpPost(Constants.Requests.UserDocument.SendDataForVerification)]
        public async Task<IActionResult> SendDataForVerification(DocumentVerificationRequestModel requestModel)
        {
            await Task.Delay(1000);
            return Ok();
        }

        [HttpGet(Constants.Requests.UserDocument.GetVerifiedData)]
        public async Task<IActionResult> GetVerifiedData()
        {
            await Task.Delay(100);
            return Ok();
        }
    }
}
