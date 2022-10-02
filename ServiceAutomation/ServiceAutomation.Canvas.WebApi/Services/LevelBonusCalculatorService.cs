﻿using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class LevelBonusCalculatorService : ILevelBonusCalculatorService
    {
        private readonly IPackagesService _packagesService;
        private readonly AppDbContext _dbContext;

        public LevelBonusCalculatorService(AppDbContext dbContext, IPackagesService packagesService)
        {
            _dbContext = dbContext;
            _packagesService = packagesService;
        }
        public async Task<CalulatedLevelBonusRewardInfoModel> CalculateLevelBonusRewardAsync(Guid currentBasicLevel, Guid currentPackage)
        {
            var basicLevelReward = await _dbContext.LevelBonusRewards
                                                      .AsNoTracking()
                                                      .Where(r => r.LevelId == currentBasicLevel)
                                                      .FirstOrDefaultAsync();
            if (basicLevelReward == null)
                return new CalulatedLevelBonusRewardInfoModel
                {
                    InitialReward = 0,
                    Percent = 0,
                    Reward = 0
                };

            var rewardPercent = await _dbContext.LevelBonusRewardPercents
                                                .AsNoTracking()
                                                .Where(p => p.PackageId == currentPackage)
                                                .Select(p => p.Percent)
                                                .SingleAsync();

            var rewardForBasicLevel = basicLevelReward.Reward;
            var reward = (rewardForBasicLevel * rewardPercent) / 100;

            return new CalulatedLevelBonusRewardInfoModel
            {
                InitialReward = rewardForBasicLevel,
                Percent = rewardPercent,
                Reward = reward
            };
        }
    }
}
