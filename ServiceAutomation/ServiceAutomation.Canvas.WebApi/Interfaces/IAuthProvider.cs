using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IAuthProvider
    {
        string Generate(UserModel user);
        Task<UserModel> Authenticate(LoginRequestModel requestModel);
        Task<UserModel> Register(RegisterRequestModel requestModel);

    }
}
