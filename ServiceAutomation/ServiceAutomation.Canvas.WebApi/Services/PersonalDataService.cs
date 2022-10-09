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
            var availableForWithdraw = await dbContext.Accruals.Where(x => x.UserId == userId && x.TransactionStatus == DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw).ToListAsync();
            var awaitingAccural = await dbContext.UserAccuralsVerifications.Include(x => x.Accurals).Where(x => x.UserId == userId).ToListAsync();
            double receivedPayoutPercentage = 0;

            decimal awaitin = 0;
            foreach(var accural in awaitingAccural)
            {
                awaitin += accural.Accurals.Sum(x => x.AccuralAmount);
            }

            switch (monthlyLevelInfo.CurrentLevel.Level)
            {
                case 2:
                    receivedPayoutPercentage = 12.5;
                    break;
                case 3:
                    receivedPayoutPercentage = 15;
                    break;
                case 4:
                    receivedPayoutPercentage = 17;
                    break;
                case 5:
                    receivedPayoutPercentage = 19;
                    break;
                case 6:
                    receivedPayoutPercentage = 20.5;
                    break;
                case 7:
                    receivedPayoutPercentage = 22;
                    break;
                case 8:
                    receivedPayoutPercentage = 23;
                    break;
                case 9:
                    receivedPayoutPercentage = 24;
                    break;
                case 10:
                    receivedPayoutPercentage = 25;
                    break;
            }

            var response = new HomePageResponseModel
            {
                Package = package,
                BaseLevelInfo = basicLevelInfo,
                MounthlyLevelInfo = monthlyLevelInfo,
                AllTimeIncome = allTimeIncome.Sum(x => x.AccuralAmount),
                AvailableForWithdrawal = availableForWithdraw.Sum(x => x.AccuralAmount),
                AwaitingAccrual = awaitin,
                ReceivedPayoutPercentage = receivedPayoutPercentage,
                ReuqiredAction = "test comment",
                NextBasicLevelRequirements = nextBasicLevelRequirements,
            };

            return response;
        }
    }
}
