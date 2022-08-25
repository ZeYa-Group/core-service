using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class GroupService : IGroupService
    {
        public GroupService()
        {

        }

        public async Task<IEnumerable<GroupResponse>> GetUserTree(Guid userId)
        {
            return null;
        }
    }
}
