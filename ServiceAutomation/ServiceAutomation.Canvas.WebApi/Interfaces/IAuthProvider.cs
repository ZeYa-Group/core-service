using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IAuthProvider
    {
        Task Login();
        Task Logout();
        Task Register();
    }
}
