using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class LevelStatisticService : ILevelStatisticService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public LevelStatisticService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<LevelModel> GetMonthlyLevelByUserIdAsync(Guid userId)
        {
            var monthlyLevel = await dbContext.MonthlyLevelStatistics.AsNoTracking()
                                                                  .Where(m => m.UserId == userId)
                                                                  .OrderByDescending(s => s.ReachingLevelDate)
                                                                  .Select(m => m.Level)
                                                                  .FirstAsync();
            return mapper.Map<LevelModel>(monthlyLevel);
        }

        public async Task<LevelInfoModel> GetMonthlyLevelInfoByUserIdAsync(Guid userId)
        {
            var monthlyLevelStatistic = await dbContext.MonthlyLevelStatistics.AsNoTracking()
                                                                               .Where(m => m.UserId == userId)
                                                                               .Include(m => m.Level)
                                                                               .OrderByDescending(s => s.ReachingLevelDate)
                                                                               .FirstAsync();
            LevelEntity currentMonthlyLevel;
            decimal currentTurnover;

            var currentDate = DateTime.Today;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            if (monthlyLevelStatistic.ReachingLevelDate < firstDayOfMonth)
            {
                var secondLevel = await dbContext.MonthlyLevels.FirstAsync(l => l.Level == Level.SecondLevel);
                var statistic = new MonthlyLevelStatisticEntity
                {
                    UserId = userId,
                    LevelId = secondLevel.Id,
                    ReachingLevelDate = firstDayOfMonth
                };

                var monthlyLevelStatisticEntity = await dbContext.MonthlyLevelStatistics.AddAsync(statistic);
                await dbContext.SaveChangesAsync();

                currentMonthlyLevel = secondLevel;
                currentTurnover = 0;
            }
            else
            {
                currentMonthlyLevel = monthlyLevelStatistic.Level;
                currentTurnover = monthlyLevelStatistic.Turnover ?? 0;
            }

            var monthlyLevelInfo = new LevelInfoModel
            {
                CurrentLevel = mapper.Map<LevelModel>(currentMonthlyLevel),
                CurrentTurnover = currentTurnover
            };

            return monthlyLevelInfo;
        }

        public async Task<LevelInfoModel> GetBasicLevelInfoByUserIdAsync(Guid userId)
        {
            var basicLevelStatistic = await dbContext.BasicLevelStatistics
                                                      .AsNoTracking()
                                                      .Where(u => u.UserId == userId)
                                                      .Include(b => b.Level)
                                                      .OrderByDescending(b => b.ReachingLevelDate)
                                                      .FirstOrDefaultAsync();

            var basicLevelInfo = new LevelInfoModel
            {
                CurrentLevel = mapper.Map<LevelModel>(basicLevelStatistic.Level),
                CurrentTurnover = basicLevelStatistic.Turnover ?? 0
            };

            return basicLevelInfo;
        }

        public async Task AddMonthlyLevelInfoAsync(Guid userId, Guid newLevelId, decimal currentMonthlyTurnover)
        {
            var monthlyStatisticRecord = new MonthlyLevelStatisticEntity
            {
                UserId = userId,
                LevelId = newLevelId,
                ReachingLevelDate = DateTime.Now,
                Turnover = currentMonthlyTurnover
            };

            await dbContext.MonthlyLevelStatistics.AddAsync(monthlyStatisticRecord);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateMonthlyLevelInfoAsync(Guid userId, Guid ForLevelId, decimal newMonthTurnover)
        {
            var basicLevelStatistic = await dbContext.MonthlyLevelStatistics.FirstAsync(s => s.UserId == userId && s.LevelId == ForLevelId);
            basicLevelStatistic.Turnover = newMonthTurnover;
            await dbContext.SaveChangesAsync();
        }

        public async Task AddBasicLevelInfoAsync(Guid userId, Guid newLevelId, decimal currentTurnover)
        {
            var basicLevelStatistic = new BasicLevelStatisticEntity
            {
                UserId = userId,
                LevelId = newLevelId,
                ReachingLevelDate = DateTime.Now,
                Turnover = currentTurnover
            };

            await dbContext.BasicLevelStatistics.AddAsync(basicLevelStatistic);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateBasicLevelInfoAsync(Guid userId, Guid ForLevelId, decimal newTurnover)
        {
            var basicLevelStatistic = await dbContext.BasicLevelStatistics.FirstAsync(s => s.UserId == userId && s.LevelId == ForLevelId);
            basicLevelStatistic.Turnover = newTurnover;
            await dbContext.SaveChangesAsync();
        }

        public async Task AddLevelsInfoForNewUserAsync(Guid userId)
        {
            var secondMonthlyLevel = await dbContext.MonthlyLevels.FirstAsync(l => l.Level == Level.FirstLevel);
            await AddMonthlyLevelInfoAsync(userId, secondMonthlyLevel.Id, 0);

            var firstBasicLevel = await dbContext.BasicLevels.FirstAsync(b => b.Level == Level.FirstLevel);
            await AddBasicLevelInfoAsync(userId, firstBasicLevel.Id, 0);
        }

        public async Task<double> GetPayoutPercentageAsync(LevelInfoModel monthlyLevelInfo)
        {
            var userLevel = monthlyLevelInfo.CurrentLevel.Id;

            var payoutPercentage = await dbContext.TeamBonusRewards
                                                      .AsNoTracking()
                                                      .Where(u => u.MonthlyLevelId == userLevel)
                                                      .FirstAsync();

            return payoutPercentage.Percent;
        }
    }
}
