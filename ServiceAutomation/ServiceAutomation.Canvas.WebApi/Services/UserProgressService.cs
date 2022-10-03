using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
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
        private readonly ITenantGroupService tenantGroupService;

        public UserProgressService(ILevelsService levelsService,
                                   ILevelStatisticService levelStatisticService,
                                   ITravelBonusService travelBonusService,
                                   ITenantGroupService tenantGroupService)
        {
            this.levelsService = levelsService;
            this.levelStatisticService = levelStatisticService;
            this.travelBonusService = travelBonusService;
            this.tenantGroupService = tenantGroupService;
        }

        public async Task<ProgressResponseModel> GetUserProgress(Guid userId)
        {
            var monthlyLevelInfo = await levelStatisticService.GetMonthlyLevelInfoByUserIdAsync(userId);
            var basicLevelInfo = await levelStatisticService.GetBasicLevelInfoByUserIdAsync(userId);
            var nextMounthlyLevelRequirements = await levelsService.GetNextMonthlyLevelAsync(monthlyLevelInfo.CurrentLevel.Level);
            var nextBasicLevelRequirements = await levelsService.GetNextBasicLevelRequirementsAsync((Level)basicLevelInfo.CurrentLevel.Level);
            var travelBonusInfo = await travelBonusService.GetTravelBonusInfoByUserIdAsync(userId);
            var referralLevelsInfos = await tenantGroupService.GetLevelsInfoInReferralStructureByUserIdAsync(userId);

            var structuralLevelProgress = new StructuralLevelProgressInfoModel
            {
                CurrentLevel = monthlyLevelInfo.CurrentLevel,
                CurrentMonthlyTurnover = monthlyLevelInfo.CurrentTurnover,
                RequiredMonthlyTurnoverToNextLevel = nextMounthlyLevelRequirements.Turnover ?? 0
            };

            var baseLevelProgress = new BaseLevelProgressInfoModel
            {
                BaseLevel = basicLevelInfo.CurrentLevel,
                CurrentCommonTurnover = basicLevelInfo.CurrentTurnover,
                NextBasicLevelRequirements = nextBasicLevelRequirements,
                CountOfRefferralRequiredFoNextLevel = nextBasicLevelRequirements.PartnersRequirementLevel.HasValue 
                                                      && referralLevelsInfos.TryGetValue((Level)nextBasicLevelRequirements.PartnersRequirementLevel.Value, out int count)
                                                      ? count
                                                      : 0
            };

            var autoBonusProgress = new AutoBonusProgressInfoModel
            {
                BaseLevel = basicLevelInfo.CurrentLevel,
                CurrentMonthlyTurnover = monthlyLevelInfo.CurrentTurnover,
                RequiredMonthlyTurnoverToNextLevel = nextMounthlyLevelRequirements.Turnover ?? 0
            };

            var response = new ProgressResponseModel
            {
                AllTimeIncome = 0,
                AvailableForWithdrawal = 0,
                AwaitingAccrual = 0,
                BaseLevelProgress = baseLevelProgress,
                TravelBonusInfo = travelBonusInfo,
                StructuralLevelProgress = structuralLevelProgress,
                AutoBonusProgress = autoBonusProgress
            };

            return response;
        }
    }
}
