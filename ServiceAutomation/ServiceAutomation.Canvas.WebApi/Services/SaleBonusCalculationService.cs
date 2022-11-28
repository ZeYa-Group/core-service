﻿using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class SaleBonusCalculationService : ISaleBonusCalculationService
    {
        private readonly AppDbContext dbContext;
        public SaleBonusCalculationService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CalulatedRewardInfoModel> CalculateStartBonusRewardAsync(decimal sellingPrice, UserPackageModel userPackage, int userSalesCount)
        {
            var startBonusReward = await dbContext.StartBonusRewards.AsNoTracking()
                                                                     .FirstAsync(s => s.PackageId == userPackage.Id);

            var countOfDaysAfterPurchase = (DateTime.Today - userPackage.PurchaseDate).TotalDays;

            if (startBonusReward.DurationOfDays < countOfDaysAfterPurchase || startBonusReward.CountOfSale < userSalesCount)
                return new CalulatedRewardInfoModel();

            var percent = startBonusReward.Percent;
            var reward = (sellingPrice * percent) / 100;

            return new CalulatedRewardInfoModel
            {
                InitialReward = sellingPrice,
                Percent = percent,
                Reward = reward
            };
        }

        public async Task<CalulatedRewardInfoModel> CalculateDynamicBonusRewardAsync(decimal sellingPrice, UserPackageModel userPackage, int saleNumber)
        {
            var dynamicBonusReward = await dbContext.DynamicBonusRewards.AsNoTracking()
                                                                         .Where(r => r.PackageId == userPackage.Id && r.SalesNumber == saleNumber)
                                                                         .FirstOrDefaultAsync();
            if (dynamicBonusReward == null)
                return new CalulatedRewardInfoModel();

            if (dynamicBonusReward.HasRestriction)
            {
                var countOfDaysAfterPurchase = (DateTime.Today - userPackage.PurchaseDate).TotalDays;

                if (dynamicBonusReward.DurationOfDays < countOfDaysAfterPurchase)
                    return new CalulatedRewardInfoModel();
            }

            var percent = dynamicBonusReward.Percent;
            var reward = (sellingPrice * percent) / 100;

            return new CalulatedRewardInfoModel
            {
                InitialReward = sellingPrice,
                Percent = percent,
                Reward = reward
            };
        }
    }
}
