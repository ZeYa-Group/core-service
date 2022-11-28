using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class PurchaseService: IPurchaseService
    {
        private readonly AppDbContext dbContext;
        private readonly IPackagesService packagesService;
        private readonly ILevelCalculationService levelCalculationService;
        private readonly IRewardAccrualService rewardAccrualService;

        public PurchaseService(AppDbContext dbContext,
                              IPackagesService packagesService,
                              ILevelCalculationService levelCalculationService,
                              IRewardAccrualService rewardAccrualService)
        {
            this.dbContext = dbContext;
            this.packagesService = packagesService;
            this.levelCalculationService = levelCalculationService;
            this.rewardAccrualService = rewardAccrualService;
        }

        public async Task BuyPackageAsync(PackageModel package, Guid userId)
        {
            var currentUserPackage = await packagesService.GetUserPackageByIdAsync(userId);
            if (currentUserPackage != null && currentUserPackage.Price >= package.Price)
            {
                throw new Exception("The purchased package must be larger than the current one!");
            }

            await AddNewPurchaseAsync(package, userId);
        }

        public async Task BuyPackageByPackageTypeAsync(PackageType packageType, Guid userId)
        {
            var purchasedPackage = await packagesService.GetPackageByTypeAsync(packageType);
            var currentUserPackage = await packagesService.GetUserPackageByIdAsync(userId);

            if (currentUserPackage != null && currentUserPackage.Price >= purchasedPackage.Price)
            {
                throw new Exception("The purchased package must be larger than the current one!");
            }

            await AddNewPurchaseAsync(purchasedPackage, userId);
        }    
       

        private async Task AddNewPurchaseAsync(PackageModel package, Guid userId)
        {
            var purchasePrice = package.Price;

            var purchase = new PurchaseEntity
            {
                UserId = userId,
                PackageId = package.Id,
                Price = purchasePrice,
                PurchaseDate = DateTime.Now
            };

            await dbContext.UsersPurchases.AddAsync(purchase);
            await dbContext.SaveChangesAsync();

            await levelCalculationService.СalculateUserLevelsAsync(userId);
            await levelCalculationService.СalculateParentPartnersLevelsAsync(userId);

            var inviteReferral = await dbContext.Users.AsNoTracking()
                                           .Where(u => u.Id == userId)
                                           .Select(u => u.InviteReferral)
                                           .FirstOrDefaultAsync();
            if (inviteReferral == null)
                return;

            var referralUserId = await dbContext.Users.AsNoTracking()
                                                       .Where(u => u.PersonalReferral == inviteReferral)
                                                       .Select(u => u.Id)
                                                       .FirstOrDefaultAsync();

            await rewardAccrualService.AccrueRewardForSaleAsync(referralUserId, userId, purchasePrice);
        }
    }
}
