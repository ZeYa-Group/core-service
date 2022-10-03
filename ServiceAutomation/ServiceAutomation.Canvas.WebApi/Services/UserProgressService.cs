using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserProgressService : IUserProgressService
    {
        private readonly ILevelsService levelsService;
        private readonly ILevelStatisticService levelStatisticService;
        private readonly ITravelBonusService travelBonusService;

        public UserProgressService(ILevelsService levelsService,
                                   ILevelStatisticService levelStatisticService,
                                   ITravelBonusService travelBonusService)
        {
            this.levelsService = levelsService;
            this.levelStatisticService = levelStatisticService;
            this.travelBonusService = travelBonusService;
        }

        public async Task<ProgressResponseModel> GetUserProgress(Guid userId)
        {
            var monthlyLevelInfo = await levelStatisticService.GetMonthlyLevelInfoByUserIdAsync(userId);
            var basicLevelInfo = await levelStatisticService.GetBasicLevelInfoByUserIdAsync(userId);
            var nextMounthlyLevelRequirements = await levelsService.GetNextMonthlyLevelAsync(monthlyLevelInfo.CurrentLevel.Level);
            var nextBasicLevelRequirements = await levelsService.GetNextBasicLevelRequirementsAsync((Level)basicLevelInfo.CurrentLevel.Level);
            var travelBonusInfo = await travelBonusService.GetTravelBonusInfoByUserIdAsync(userId);

            var response = new ProgressResponseModel
            {
                BaseLevelInfo = basicLevelInfo,
                MounthlyLevelInfo = monthlyLevelInfo,
                AllTimeIncome = 0,
                AvailableForWithdrawal = 0,
                AwaitingAccrual = 0,
                NextBasicLevelRequirements = nextBasicLevelRequirements,
                NextMounthlyLevelRequirement = nextMounthlyLevelRequirements?.Turnover,
                TravelBonusInfo = travelBonusInfo
                //PartnersCurrentLevelCount = 0,
            };

            return response;
        }
    }
}
