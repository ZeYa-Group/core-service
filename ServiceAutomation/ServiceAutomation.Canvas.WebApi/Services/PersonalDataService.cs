using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class PersonalDataService : IPersonalDataService
    {
        private readonly AppDbContext dbContext;
        public PersonalDataService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<HomePageResponseModel> GetHomeUserData(Guid userId)
        {
            var response = new HomePageResponseModel();

            var packageData = await dbContext.UsersPurchases
                .Include(x => x.Package).FirstOrDefaultAsync(x => x.UserId == userId);

            Random random = new Random();
            random.Next(1,10);

            if (packageData == null)
            {
                response.BaseLevel = 1;
                response.MounthlyLevel = 1;
                response.AllTimeIncome = 0;
                response.AvailableForWithdrawal = 0;
                response.AwaitingAccrual = 0;
                response.GroupTurnover = 0;
                response.MonthlyTurnover = 0;
                response.ReceivedPayoutPercentage = 0;
                response.ReuqiredAction = "test comment";
                response.ReuqiredGroupTurnover = 9000;
            }
            else
            {
                response.PackageName = packageData.Package.Name;
                response.BaseLevel = random.Next(1,9);
                response.MounthlyLevel = response.BaseLevel + 1;
                response.AllTimeIncome = 12560;
                response.AvailableForWithdrawal = 2500;
                response.AwaitingAccrual = 300;
                response.GroupTurnover = 500;
                response.MonthlyTurnover = 12220;
                response.ReceivedPayoutPercentage = 10;
                response.ReuqiredAction = "test comment";
                response.ReuqiredGroupTurnover = 0;
            }

            return response;
        }
    }
}
