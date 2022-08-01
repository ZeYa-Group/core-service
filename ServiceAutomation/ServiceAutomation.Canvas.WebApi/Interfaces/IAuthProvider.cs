using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IAuthProvider
    {
        string Generate(UserModel user);
        Task<AuthenticationResult> Authenticate(LoginRequestModel requestModel);
        Task<AuthenticationResult> Register(RegisterRequestModel requestModel);

    }
}
