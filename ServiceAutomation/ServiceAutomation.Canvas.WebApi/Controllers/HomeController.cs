﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Constants;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class HomeController : ApiBaseController
    {
        private readonly IUserReferralService userReferralService;
        private readonly IPersonalDataService personalDataService;
        private readonly IBonusCalculatorService bonusCalculatorService;

        public HomeController(IUserReferralService userReferralService, IPersonalDataService personalDataService, IBonusCalculatorService bonusCalculatorService)
        {
            this.userReferralService = userReferralService;
            this.personalDataService = personalDataService;
            this.bonusCalculatorService = bonusCalculatorService;
        }

        [HttpGet(Constants.Requests.Home.GetReferralLink)]
        public async Task<string> GetUserReferral(Guid userId)
        {
            return await userReferralService.GetUserRefferal(userId);
        }

        [HttpGet(Constants.Requests.Home.GetPersonalPageInfo)]
        public async Task<IActionResult> GetPersonalPageInfo(Guid userId)
        {
            return Ok(await personalDataService.GetHomeUserData(userId));
        }
    }
}
