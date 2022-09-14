using ServiceAutomation.Canvas.WebApi.Models;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IPurchaseService
    {
        Task BuyPackageAsync(PackageModel package, Guid userId);
    }
}
