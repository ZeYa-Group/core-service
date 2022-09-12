using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PackagesService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PackageModel> GetPackageByIdAsync(Guid packageId)
        {
            var package = await _dbContext.Packages.FirstAsync(x => x.Id == packageId);
            return _mapper.Map<PackageModel>(package);
        }

        public async Task<IEnumerable<PackageModel>> GetPackagesAsync()
        {
            var packagesEntities = await _dbContext.Packages
                .Include(x => x.PackageBonuses)
                .ThenInclude(x => x.Bonus)
                .ToListAsync();

            return packagesEntities.Select(x => _mapper.Map<PackageModel>(x));
        }

        public async Task<PackageModel> GetUserPackageAsync(Guid userId)
        {
            var package = await _dbContext.UsersPurchases
                                .AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .Include(x=> x.Package)
                                .ThenInclude(x => x.PackageBonuses)
                                .ThenInclude(x => x.Bonus)
                                .Select(x => x.Package)
                                .FirstOrDefaultAsync();

            return _mapper.Map<PackageModel>(package);
        }

    }
}
