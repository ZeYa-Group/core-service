using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IRewardAccrualForLevelService
    {
        Task AccrueRewardForBasicLevelAsync(Guid userId);
    }
}
