using Microsoft.AspNetCore.Mvc;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesService _packagesService;

        private readonly IPurchaseService _purchaseService;
        public PackagesController(IPackagesService packagesService, IPurchaseService purchaseService)
        {
            _packagesService = packagesService;
            _purchaseService = purchaseService;
        }

        [HttpGet(Constants.Requests.Package.GetPackages)]
        public async Task<IEnumerable<PackageModel>> GetPackagesAsync()
        {
            return await _packagesService.GetPackagesAsync();
        }

        [HttpPost(Constants.Requests.Package.BuyPackage)]
        public async Task<IActionResult> BuyPackageAsync([FromBody] BuyPackageRequestModel buyPackageRequest)
        {
            var package = await _packagesService.GetPackageByIdAsync(buyPackageRequest.PackageId);
            if (package == null)
            {
                return BadRequest();
            }

            var userId = new Guid(User?.Identity?.Name);

            await _purchaseService.BuyPackageAsync(package, userId);

            return Ok();
        }

        [HttpGet(Constants.Requests.Package.GetUserPackage)]
        public async Task<PackageModel> GetUserPackageAsync(Guid userId)
        {
            return await _packagesService.GetUserPackageAsync(userId);
        }

    }
}
