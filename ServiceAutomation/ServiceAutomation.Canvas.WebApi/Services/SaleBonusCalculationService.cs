using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class SaleBonusCalculationService : ISaleBonusCalculationService
    {
        private readonly AppDbContext _dbContext;
        public SaleBonusCalculationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CalulatedRewardInfoModel> CalculateStartBonusRewardAsync(decimal sellingPrice, UserPackageModel userPackage, int userSalesCount)
        {
            var startBonusReward = await _dbContext.StartBonusRewards.AsNoTracking()
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
    }
}
