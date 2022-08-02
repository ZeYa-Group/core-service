using System;
using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IUserManager
    {
        Task<UserModel> GetByEmail(string email);
        Task<UserModel> AddUser(UserModel user);
        Task<bool> IsUserAlreadyExists(string email);
    }
}
