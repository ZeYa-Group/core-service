using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocumentController : ApiBaseController
    {
        public UserDocumentController()
        {

        }

        [HttpGet(Constants.Requests.UserDocument.GetUserLegalNotice)]
        public async Task<IActionResult> GetUserLegalNotice(Guid UserId)
        {
            return Ok();
        }

        [HttpPost(Constants.Requests.UserDocument.UploadUserLegalNotice)]
        public async Task<IActionResult> UploadUserLegalNotice(Guid UserId)
        {
            return Ok();
        }

        [HttpGet(Constants.Requests.UserDocument.GetUserEvidenceData)]
        public async Task<IActionResult> GetUserEvidenceData(Guid UserId)
        {
            return Ok();
        }

        [HttpPost(Constants.Requests.UserDocument.UploadUserEvidenceData)]
        public async Task<IActionResult> UploadUserEvidenceData(Guid UserId)
        {
            return Ok();
        }

        [HttpGet(Constants.Requests.UserDocument.GetUserBankRequecitations)]
        public async Task<IActionResult> GetUserBankRequecitations(Guid UserId)
        {
            return Ok();
        }

        [HttpPost(Constants.Requests.UserDocument.UploadUserBankRequecitations)]
        public async Task<IActionResult> UploadUserBankRequecitations(Guid UserId)
        {
            return Ok();
        }

        [HttpGet(Constants.Requests.UserDocument.GetUserLegalAddress)]
        public async Task<IActionResult> GetUserLegalAddress(Guid UserId)
        {
            return Ok();
        }

        [HttpPost(Constants.Requests.UserDocument.UploadUserLegalAddress)]
        public async Task<IActionResult> UploadUserLegalAddress(Guid UserId)
        {
            return Ok();
        }
    }
}
