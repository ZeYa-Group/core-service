﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Constants;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthProvider authProvider;
        private readonly IUserManager userManager;

        public AuthenticationController(IAuthProvider authProvider, IUserManager userManager)
        {
            this.authProvider = authProvider;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            var response = await authProvider.AuthenticateAsync(requestModel);
            
            return Ok(response);
        }

        [Authorize]
        [HttpPost(Requests.User.Logout)]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await authProvider.RefreshAsync(requestModel));
        }

        [AllowAnonymous]
        [HttpPost(Requests.User.Register)]
        public async Task<IActionResult> Register(RegisterRequestModel requestModel)
        {
            if (await userManager.IsUserAlreadyExistsAsync(requestModel.Email))
            {
                return BadRequest("User already exists");
            }

            var response = await authProvider.RegisterAsync(requestModel);

            return Ok(response);
        }
    }
}
