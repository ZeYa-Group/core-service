using ServiceAutomation.Canvas.WebApi.Models;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface ILevelsService
    {
        Task<LevelModel> GetMonthlyLevelByUserIdAsync(Guid userId);

        Task СalculatePartnersBasicLevels(Guid userId);
    }
}
