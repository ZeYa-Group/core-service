﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeHandlers.DateTimeHandlers;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static ServiceAutomation.Canvas.WebApi.Constants.Requests;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ApiBaseController
    {
        private readonly IUserProfileService userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            this.userProfileService = userProfileService;
        }

        [Authorize]
        [HttpGet(Constants.Requests.UserProfile.GetProfileInfo)]
        public async Task<UserProfileResponseModel> GetProfileInfo(Guid userId)
        {
            return await userProfileService.GetUserInfo(userId);
        }

        [Authorize]
        [HttpPost(Constants.Requests.UserProfile.UploadProfilePhoto)]
        public async Task<ResultModel> UploadProfilePhoto([FromForm] UploadProfilePhotoRequestModel requestModel)
        {
            var photo = requestModel.ProfilePhoto;

            await using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);
            var photoData = memoryStream.ToArray();

            return await userProfileService.UploadProfilePhoto(requestModel.UserId, photoData);
        }

        [Authorize]
        [HttpPost(Constants.Requests.UserProfile.UploadProfileInfo)]
        public async Task<ResultModel> UploadProfileInfo([FromBody] UploadUserProfileRequestModel requestModel)
        {
            return await userProfileService.UploadProfileInfo(requestModel.UserId, requestModel.FirstName, requestModel.LastName, requestModel.Patronymic, requestModel.DateOfBirth);
        }

        [Authorize]
        [HttpPost(Constants.Requests.UserProfile.ChangePassword)]
        public async Task<ResultModel> ChangePassword([FromBody] ChangePasswordRequestModel requestModel)
        {
            if(requestModel.NewPassword != requestModel.ConfirmPassword)
            {
                return new ResultModel()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Password do not match"
                    }
                };
            }
            return await userProfileService.ChangePassword(requestModel.UserId, requestModel.OldPassword, requestModel.NewPassword);
        }

        [Authorize]
        [HttpPost(Constants.Requests.UserProfile.UploadPhoneNumber)]
        public async Task<ResultModel> UploadPhoneNumber([FromBody] ChangePhoneNumberRequestModel requestModel)
        {
            return await userProfileService.UploadPhoneNumber(requestModel.UserId, requestModel.NewPhoneNumber);
        }

        [Authorize]
        [HttpPost(Constants.Requests.UserProfile.ChangeEmailAdress)]
        public async Task<ResultModel> ChangeEmailAdress([FromBody] ChangeEmailRequestModel requestModel)
        {
            return await userProfileService.ChangeEmailAdress(requestModel.UserId, requestModel.NewEmailAdress);
        }
    }
}
