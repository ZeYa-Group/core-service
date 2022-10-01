using ServiceAutomation.Canvas.WebApi.Models.AdministratorResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IAdministratorService
    {
        Task<ICollection<UserVerificationResponseModel>> GetVerificationRequest();
        Task AcceptVerificationRequest(Guid requestId, Guid userId);
        Task RejectVerificationRequest(Guid requestId, Guid userId);
    }
}
