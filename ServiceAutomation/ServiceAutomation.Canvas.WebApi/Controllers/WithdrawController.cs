using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        public WithdrawController()
        {

        }

        [Authorize]
        [HttpGet(Constants.Requests.Withdraw.GetWithdrawHistory)]
        public async Task<IEnumerable<WithdrawResponseModel>> GetWithdrawHistory(Guid id)
        {
            return null;
        }
    }
}
