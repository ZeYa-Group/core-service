using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeHandlers.DateTimeHandlers;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        public UserProfileController()
        {

        }

        [HttpGet(Constants.Requests.UserProfile.GetProfileInfo)]
        public async Task GetProfileInfo(Guid userId)
        {
            throw new NotImplementedException();
        }

        [HttpPost(Constants.Requests.UserProfile.UploadProfilePhoto)]
        public async Task UploadProfilePhoto([FromForm] UploadProfilePhotoRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
