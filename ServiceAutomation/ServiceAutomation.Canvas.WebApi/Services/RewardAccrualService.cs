using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class RewardAccrualService : IRewardAccrualService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILevelBonusCalculatorService _levelBonusCalculatorService;
        private readonly IPackagesService _packagesService;
        private readonly ISaleBonusCalculationService _saleBonusCalculationService;
        private readonly ISalesService _salesService;
        private readonly ILevelStatisticService _levelStatisticService;
        private readonly IAutoBonusCalculatorService _autoBonusCalculatorService;

        public RewardAccrualService(AppDbContext dbContext,
                                            ILevelBonusCalculatorService levelBonusCalculatorService,
                                            ISaleBonusCalculationService saleBonusCalculationService,
                                            IPackagesService packagesService,
                                            ISalesService salesService,
                                            ILevelStatisticService levelStatisticService,
                                            IAutoBonusCalculatorService autoBonusCalculatorService)
        {
            _dbContext = dbContext;
            _levelBonusCalculatorService = levelBonusCalculatorService;
            _packagesService = packagesService;
            _saleBonusCalculationService = saleBonusCalculationService;
            _salesService = salesService;
            _levelStatisticService = levelStatisticService;
            _autoBonusCalculatorService = autoBonusCalculatorService;
        }

        public async Task AccrueRewardForBasicLevelAsync(Guid userId)
        {
            var userPackage = await _packagesService.GetUserPackageByIdAsync(userId);
            if (userPackage == null)
                return;

            var userBasicLevelId = await _dbContext.Users.Where(u => u.Id == userId).Select(u => u.BasicLevelId).FirstOrDefaultAsync();

            if (userBasicLevelId == null)
                return;

            await AccrueLevelBonusRewardAsync(userId, userBasicLevelId.Value, userPackage);
            await AccrueAutoBonusRewardAsync(userId, userBasicLevelId.Value, userPackage);
        }

        private async Task AccrueLevelBonusRewardAsync(Guid userId, Guid userBasicLevelId, UserPackageModel userPackage)
        {
            var rewardInfo = await _levelBonusCalculatorService.CalculateLevelBonusRewardAsync(userBasicLevelId, userPackage.Id);

            if (rewardInfo.Reward == 0)
                return;

            var basicLevel = await _dbContext.Users.AsNoTracking().Where(u => u.Id == userId).Select(u => u.BasicLevelId).FirstAsync();
            var levelBonusId = await _dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.LevelBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = levelBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Pending,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now
            };

            await _dbContext.Accruals.AddAsync(accrual);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AccrueRewardForSaleAsync(Guid userId, decimal sellingPrice)
        {
            var userPackage = await _packagesService.GetUserPackageByIdAsync(userId);
            if (userPackage == null)
                return;

            await AccrueRewardForStartBonusAsync(userId, sellingPrice, userPackage);
            await AccrueRewardForDynamicBonusAsync(userId, sellingPrice, userPackage);
        }

        private async Task AccrueRewardForStartBonusAsync(Guid userId, decimal sellingPrice, UserPackageModel userPackage)
        {
            var userSalesCount = await _salesService.GetUserSalesCountAsync(userId);

            var rewardInfo = await _saleBonusCalculationService.CalculateStartBonusRewardAsync(sellingPrice, userPackage, userSalesCount);

            if (rewardInfo.Reward == 0)
                return;

            var startBonusId = await _dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.StartBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = startBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Pending,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now
            };

            await _dbContext.Accruals.AddAsync(accrual);
            await _dbContext.SaveChangesAsync();
        }

        private async Task AccrueRewardForDynamicBonusAsync(Guid userId, decimal sellingPrice, UserPackageModel userPackage)
        {
            var userSalesCount = await _salesService.GerSalesCountInMonthAsync(userId);
            var rewardInfo = await _saleBonusCalculationService.CalculateDynamicBonusRewardAsync(sellingPrice, userPackage, userSalesCount);

            if (rewardInfo.Reward == 0)
                return;

            var startBonusId = await _dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.DynamicBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = startBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Pending,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now
            };

            await _dbContext.Accruals.AddAsync(accrual);
            await _dbContext.SaveChangesAsync();
        }
    
        private async Task AccrueAutoBonusRewardAsync(Guid userId, Guid userBasicLevelId, UserPackageModel userPackage)
        {
            var monthlyLevelInfo = await _levelStatisticService.GetMonthlyLevelInfoByUserIdAsync(userId);

            var rewardInfo = await _autoBonusCalculatorService.CalculateAutoBonusRewardAsync(userBasicLevelId, userPackage.Id, monthlyLevelInfo.CurrentTurnover);

            if (rewardInfo.Reward == 0)
                return;

            var autoBonusId = await _dbContext.Bonuses.AsNoTracking()
                                                      .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.AutoBonus)
                                                      .Select(b => b.Id)
                                                      .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = autoBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Pending,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now,
                ForBsicLevelId = userBasicLevelId
            };

            await _dbContext.Accruals.AddAsync(accrual);
            await _dbContext.SaveChangesAsync();
        }


    }
}
