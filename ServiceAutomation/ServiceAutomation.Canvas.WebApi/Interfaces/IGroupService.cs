using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupResponse>> GetUserTree(Guid userId);
    }
}
