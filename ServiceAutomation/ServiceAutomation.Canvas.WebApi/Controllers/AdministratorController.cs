﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ApiBaseController
    {
        private readonly IAdministratorService administratorService;
        public AdministratorController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        [HttpGet(Constants.Requests.Administrator.GetDocumentVerificationList)]
        public async Task<IActionResult> GetDocumentRequestListAsync()
        {
            return Ok(await administratorService.GetVerificationRequest());
        }

        [HttpGet(Constants.Requests.Administrator.AcceptUserVerification)]
        public async Task AcceptUserVerification(Guid requestId, Guid userId)
        {
            await administratorService.AcceptVerificationRequest(requestId, userId);
        }

        [HttpGet(Constants.Requests.Administrator.RejectUserVerification)]
        public async Task RejectUserVerification(Guid requestId, Guid userId)
        {
            await administratorService.RejectVerificationRequest(requestId, userId);
        }
    }
}
