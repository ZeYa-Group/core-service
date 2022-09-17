using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReferralStructureController : ApiBaseController
    {
        private readonly ITenantGroupService groupService;

        public ReferralStructureController(ITenantGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet(Constants.Requests.Structure.GetTree)]
        public async Task<IActionResult> GetTree(Guid userId)
        {
            return Ok(await groupService.GetReferralTree(userId));
        }

        [HttpGet(Constants.Requests.Structure.GetUserReferralGroup)]
        public async Task<IActionResult> GetUserRefferalGroupAsync()
        {
            Guid userId = GetCurrentUserId();
            var referralGroup = await groupService.GetReferralGroupByUserIdAsync(userId);
            return Ok(referralGroup);
        }

        [HttpGet(Constants.Requests.Structure.GetReferralGroup)]
        public async Task<IActionResult> GetRefferalGroupAsync(Guid groupId)
        {
            var referralGroup = await groupService.GetReferralGroupAsync(groupId);
            return Ok(referralGroup);
        }

        [HttpGet(Constants.Requests.Structure.GetPartnersReferralGroups)]
        public async Task<IActionResult> GetPartnersRefferalGroupaAsync(Guid groupId)
        {
            var referralGroup = await groupService.GetPartnersReferralGroupsAsync(groupId);
            return Ok(referralGroup);
        }

    }
}
