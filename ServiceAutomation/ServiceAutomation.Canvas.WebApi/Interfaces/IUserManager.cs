using System;
using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Models;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IUserManager
    {
        Task<UserModel> GetByEmail(string email);
        Task<UserModel> GetByUsername(string username);
        Task<UserModel> AddUser(UserModel user);
    }
}
