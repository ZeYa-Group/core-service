using System;
using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.DataAccess.DbContexts;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserManager : IUserManager
    {
        private readonly PotgreSqlContext dbContext;

        public UserManager(PotgreSqlContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Guid> AddUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
