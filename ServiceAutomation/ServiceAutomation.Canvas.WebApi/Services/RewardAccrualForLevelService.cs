using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class RewardAccrualForLevelService : IRewardAccrualForLevelService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILevelBonusCalculatorService _levelBonusCalculatorService;
        private readonly IPackagesService _packagesService;

        public RewardAccrualForLevelService(AppDbContext dbContext,
                                            ILevelBonusCalculatorService levelBonusCalculatorService,
                                            IPackagesService packagesService)
        {
            _dbContext = dbContext;
            _levelBonusCalculatorService = levelBonusCalculatorService;
            _packagesService = packagesService;
        }

        public async Task AccrueRewardForBasicLevelAsync(Guid userId)
        {
            var userPackage = await _packagesService.GetUserPackageByIdAsync(userId);
            if (userPackage == null)
                return;

            var userBasicLevelId = await _dbContext.Users.Where(u => u.Id == userId).Select(u => u.BasicLevelId).FirstOrDefaultAsync();

            if (userBasicLevelId == null)
                return;

            var rewardInfo = await _levelBonusCalculatorService.CalculateLevelBonusRewardAsync(userBasicLevelId.Value, userPackage.Id);

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
    }
}
