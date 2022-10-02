using System.Threading.Tasks;
using System;
using ServiceAutomation.Canvas.WebApi.Models;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface ILevelBonusCalculatorService
    {
        Task<CalulatedLevelBonusRewardInfoModel> CalculateLevelBonusRewardAsync(Guid currentBasicLevel, Guid currentPackage);
    }
}
