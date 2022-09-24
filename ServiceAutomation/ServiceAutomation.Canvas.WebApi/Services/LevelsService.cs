using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class LevelsService : ILevelsService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITurnoverService _turnoverService;
        private readonly IMapper _mapper;

        public LevelsService(AppDbContext dbContext, ITurnoverService turnoverService, IMapper mapper)
        {
            _dbContext = dbContext;
            _turnoverService = turnoverService;
            _mapper = mapper;
        }

        public async Task<LevelModel> GetMonthlyLevelByUserIdAsync(Guid userId)
        {
            var monthlyTurnover = await _turnoverService.GetMonthlyTurnoverByUserIdAsync(userId);

            var monthlyLevel = await _dbContext.MonthlyLevels.Where(l => l.Level == _dbContext.MonthlyLevels
                                                                                        .Where( x => !x.Turnover.HasValue || x.Turnover.Value < monthlyTurnover)
                                                                                        .Max(x => x.Level))
                                                       .SingleOrDefaultAsync();

            return _mapper.Map<LevelModel>(monthlyLevel);
        }

        public async Task СalculatePartnersBasicLevels(Guid userId)
        {
            var tenantGroup = await _dbContext.TenantGroups
                                              .Include(t => t.Parent)
                                              .Include( t => t.OwnerUser)
                                              .FirstOrDefaultAsync(t => t.OwnerUserId == userId);
            if (tenantGroup.Parent == null)
            {
                return;
            }

            var parentGroup = tenantGroup.Parent;
            while (true)
            {
                var turnover = await _turnoverService.GetTurnoverByUserIdAsync(parentGroup.OwnerUserId);

                

                if (parentGroup.Parent == null)
                {
                    break;
                }

                parentGroup = parentGroup.Parent;
            }
        }
    }
}

