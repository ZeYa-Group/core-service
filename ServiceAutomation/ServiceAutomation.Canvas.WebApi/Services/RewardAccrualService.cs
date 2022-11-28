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
        private readonly AppDbContext dbContext;
        private readonly ILevelBonusCalculatorService levelBonusCalculatorService;
        private readonly IPackagesService packagesService;
        private readonly ISaleBonusCalculationService saleBonusCalculationService;
        private readonly ISalesService salesService;
        private readonly ILevelStatisticService levelStatisticService;
        private readonly IAutoBonusCalculatorService autoBonusCalculatorService;
        private readonly ITeamBonusService teamBonusService;

        public RewardAccrualService(AppDbContext dbContext,
                                            ILevelBonusCalculatorService levelBonusCalculatorService,
                                            ISaleBonusCalculationService saleBonusCalculationService,
                                            IPackagesService packagesService,
                                            ISalesService salesService,
                                            ILevelStatisticService levelStatisticService,
                                            IAutoBonusCalculatorService autoBonusCalculatorService,
                                            ITeamBonusService teamBonusService)
        {
            this.dbContext = dbContext;
            this.levelBonusCalculatorService = levelBonusCalculatorService;
            this.packagesService = packagesService;
            this.saleBonusCalculationService = saleBonusCalculationService;
            this.salesService = salesService;
            this.levelStatisticService = levelStatisticService;
            this.autoBonusCalculatorService = autoBonusCalculatorService;
            this.teamBonusService = teamBonusService;
        }

        public async Task AccrueRewardForBasicLevelAsync(Guid userId)
        {
            var userPackage = await packagesService.GetUserPackageByIdAsync(userId);
            if (userPackage == null)
                return;

            var userBasicLevelId = await dbContext.Users.Where(u => u.Id == userId).Select(u => u.BasicLevelId).FirstOrDefaultAsync();

            if (userBasicLevelId == null)
                return;

            await AccrueLevelBonusRewardAsync(userId, userBasicLevelId.Value, userPackage);
            await AccrueAutoBonusRewardAsync(userId, userBasicLevelId.Value, userPackage);
        }

        private async Task AccrueLevelBonusRewardAsync(Guid userId, Guid userBasicLevelId, UserPackageModel userPackage)
        {
            var rewardInfo = await levelBonusCalculatorService.CalculateLevelBonusRewardAsync(userBasicLevelId, userPackage.Id);

            if (rewardInfo.Reward == 0)
                return;

            var basicLevel = await dbContext.Users.AsNoTracking().Where(u => u.Id == userId).Select(u => u.BasicLevelId).FirstAsync();
            var levelBonusId = await dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.LevelBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = levelBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now
            };

            await dbContext.Accruals.AddAsync(accrual);
            await dbContext.SaveChangesAsync();
        }

        public async Task AccrueRewardForSaleAsync(Guid whoSoldId, Guid whoBoughtId, decimal sellingPrice)
        {
            var userPackage = await packagesService.GetUserPackageByIdAsync(whoSoldId);
            if (userPackage == null)
                return;

            await AccrueRewardForStartBonusAsync(whoSoldId, whoBoughtId, sellingPrice, userPackage);
            await AccrueRewardForDynamicBonusAsync(whoSoldId, whoBoughtId, sellingPrice, userPackage);
            await AccrualTeamBonusRewardsAsync(whoSoldId, whoBoughtId, sellingPrice);
        }

        private async Task AccrueRewardForStartBonusAsync(Guid whoSoldId, Guid whoBoughtId, decimal sellingPrice, UserPackageModel userPackage)
        {
            var userSalesCount = await salesService.GetUserSalesCountAsync(whoSoldId);

            var rewardInfo = await saleBonusCalculationService.CalculateStartBonusRewardAsync(sellingPrice, userPackage, userSalesCount);

            if (rewardInfo.Reward == 0)
                return;

            var startBonusId = await dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.StartBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = whoSoldId,
                BonusId = startBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now,
                ForWhomId = whoBoughtId
            };

            await dbContext.Accruals.AddAsync(accrual);
            await dbContext.SaveChangesAsync();
        }

        private async Task AccrueRewardForDynamicBonusAsync(Guid whoSoldId, Guid whoBoughtId, decimal sellingPrice, UserPackageModel userPackage)
        {
            var userSalesCount = await salesService.GerSalesCountInMonthAsync(whoSoldId);
            var rewardInfo = await saleBonusCalculationService.CalculateDynamicBonusRewardAsync(sellingPrice, userPackage, userSalesCount);

            if (rewardInfo.Reward == 0)
                return;

            var startBonusId = await dbContext.Bonuses.AsNoTracking()
                                                       .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.DynamicBonus)
                                                       .Select(b => b.Id)
                                                       .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = whoSoldId,
                BonusId = startBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now,
                ForWhomId = whoBoughtId
            };

            await dbContext.Accruals.AddAsync(accrual);
            await dbContext.SaveChangesAsync();
        }
    
        private async Task AccrueAutoBonusRewardAsync(Guid userId, Guid userBasicLevelId, UserPackageModel userPackage)
        {
            var monthlyLevelInfo = await levelStatisticService.GetMonthlyLevelInfoByUserIdAsync(userId);

            var rewardInfo = await autoBonusCalculatorService.CalculateAutoBonusRewardAsync(userBasicLevelId, userPackage.Id, monthlyLevelInfo.CurrentTurnover);

            if (rewardInfo.Reward == 0)
                return;

            var autoBonusId = await dbContext.Bonuses.AsNoTracking()
                                                      .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.AutoBonus)
                                                      .Select(b => b.Id)
                                                      .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = autoBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now,
                ForBsicLevelId = userBasicLevelId
            };

            await dbContext.Accruals.AddAsync(accrual);
            await dbContext.SaveChangesAsync();
        }

        private async Task AccrualTeamBonusRewardsAsync(Guid whoSoldId, Guid whoBoughtId, decimal sellingPrice)
        {
            var userMonthlyLevel = await levelStatisticService.GetMonthlyLevelByUserIdAsync(whoSoldId);
            var userBasicLevelInfo = await levelStatisticService.GetBasicLevelInfoByUserIdAsync(whoSoldId);

            var userRewardInfo = await teamBonusService.CalculateTeamBonusRewardAsync(sellingPrice, userMonthlyLevel, userBasicLevelInfo.CurrentTurnover);

            if (userRewardInfo.Reward == 0)
                return;

            await AccrualTeamBonusRewardsAsync(whoSoldId, whoBoughtId, userRewardInfo);

            decimal tempReward = userRewardInfo.Reward;
            LevelModel tempMonthlyLevel = userMonthlyLevel;

            while (true)
            {
                var parentGroup = await dbContext.Users.AsNoTracking()
                                                       .Where(u => u.Id == whoSoldId)
                                                       .Select(u => u.Group.Parent)
                                                       .FirstOrDefaultAsync();
                if (parentGroup == null)
                    break;

                var parentUser = parentGroup.OwnerUserId;
                var monthlyLevel = await levelStatisticService.GetMonthlyLevelByUserIdAsync(parentUser);
                var basicLevelInfo = await levelStatisticService.GetBasicLevelInfoByUserIdAsync(parentUser);

                var rewardInfo = await teamBonusService.CalculateTeamBonusRewardAsync(tempReward, monthlyLevel, tempMonthlyLevel, basicLevelInfo.CurrentTurnover);

                if (rewardInfo.Reward == 0)
                    break;

                await AccrualTeamBonusRewardsAsync(parentUser, whoBoughtId, rewardInfo);

                tempReward = rewardInfo.Reward;
                tempMonthlyLevel = monthlyLevel;
            }
        }


        private async Task AccrualTeamBonusRewardsAsync(Guid userId, Guid whoBoughtId, CalulatedRewardInfoModel rewardInfo)
        {
            var autoBonusId = await dbContext.Bonuses.AsNoTracking()
                                                      .Where(b => b.Type == DataAccess.Schemas.Enums.BonusType.TeamBonus)
                                                      .Select(b => b.Id)
                                                      .FirstAsync();
            var accrual = new AccrualsEntity
            {
                UserId = userId,
                BonusId = autoBonusId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.ReadyForWithdraw,
                AccuralPercent = rewardInfo.Percent,
                InitialAmount = rewardInfo.InitialReward,
                AccuralAmount = rewardInfo.Reward,
                AccuralDate = DateTime.Now,
                ForWhomId = whoBoughtId
            };

            await dbContext.Accruals.AddAsync(accrual);
            await dbContext.SaveChangesAsync();
        }
    }
}
