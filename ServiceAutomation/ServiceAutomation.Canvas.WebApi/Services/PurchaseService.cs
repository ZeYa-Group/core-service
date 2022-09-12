using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class PurchaseService: IPurchaseService
    {
        private readonly AppDbContext _dbContext;

        public PurchaseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BuyPackageAsync(PackageModel package, Guid userId)
        {
            var purchase = new PurchaseEntity
            {
                UserId = userId,
                PackageId = package.Id,
                Price = package.Price,
                PurchaseDate = DateTime.Today
            };

            await _dbContext.UsersPurchases.AddAsync(purchase);
            await _dbContext.SaveChangesAsync();
        }
    }
}
