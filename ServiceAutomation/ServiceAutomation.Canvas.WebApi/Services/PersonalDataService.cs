﻿using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class PersonalDataService : IPersonalDataService
    {
        private readonly IPackagesService packagesService;
        private readonly ILevelsService levelsService;
        private readonly ILevelStatisticService levelStatisticService;
        private readonly AppDbContext dbContext;

        public PersonalDataService(IPackagesService packagesService, ILevelsService levelsService, ILevelStatisticService levelStatisticService, AppDbContext dbContext)
        {
            this.packagesService = packagesService;
            this.levelsService = levelsService;
            this.levelStatisticService = levelStatisticService;
            this.dbContext = dbContext;
        }

        public async Task<HomePageResponseModel> GetHomeUserData(Guid userId)
        {
            var package = await packagesService.GetUserPackageByIdAsync(userId);
            var monthlyLevelInfo = await levelStatisticService.GetMonthlyLevelInfoByUserIdAsync(userId);
            var basicLevelInfo = await levelStatisticService.GetBasicLevelInfoByUserIdAsync(userId);
            var nextBasicLevelRequirements = await levelsService.GetNextBasicLevelRequirementsAsync((Level)basicLevelInfo.CurrentLevel.Level);
            var allTimeIncome = await dbContext.Accruals.Where(x => x.UserId == userId).ToListAsync();
            //var awaitingAccural = dbContext.UserAccuralsVerifications.Where(x => x.UserId == userId).Select(x => x.Accurals);

            var response = new HomePageResponseModel
            {
                Package = package,
                BaseLevelInfo = basicLevelInfo,
                MounthlyLevelInfo = monthlyLevelInfo,
                AllTimeIncome = allTimeIncome.Sum(x => x.AccuralAmount),
                AvailableForWithdrawal = 0,
                AwaitingAccrual = 0,
                ReceivedPayoutPercentage = 0,
                ReuqiredAction = "test comment",
                NextBasicLevelRequirements = nextBasicLevelRequirements,
            };

            return response;
        }
    }
}
