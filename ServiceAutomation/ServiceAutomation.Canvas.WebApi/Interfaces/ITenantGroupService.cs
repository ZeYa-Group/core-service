﻿using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface ITenantGroupService
    {
        Task<ReferralGroupModel> GetReferralTree(Guid userId);

        Task CreateTenantGroupForUserAsync(UserModel userModel);

        Task<ReferralGroupModel> GetReferralGroupByUserIdAsync(Guid userId);

        Task<ReferralGroupModel> GetReferralGroupAsync(Guid groupId);

        Task<ReferralGroupModel[]> GetPartnersReferralGroupsAsync(Guid groupId);
    }
}
